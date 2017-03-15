using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Load
{
    public class BulkLoader
    {
        public const string ConnectionString =
            @"Server=localhost\sqlexpress;Database=Task;Integrated Security=true;Column Encryption Setting=disabled;Max Pool Size=250;";

        public const string BasePath = @"c:\data";
        public const string Associative = @"associative";
        public const string Transaction = @"transaction";

        public void Load(DateTime fromDate, DateTime toDate)
        {
            try
            {
                toDate = toDate.AddDays(1);

                Directory.CreateDirectory(BasePath);
                Directory.CreateDirectory($@"{BasePath}\{Associative}");
                Directory.CreateDirectory($@"{BasePath}\{Transaction}");

                LoadDataIntoFlatFiles(fromDate, toDate);
                BulkInsertFlatFiles(fromDate, toDate);

                Console.WriteLine("Done");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void BulkInsertFlatFiles(DateTime fromDate, DateTime toDate)
        {
            Console.WriteLine("Bulk insert:");
            
            var loopDate = fromDate;

            while (loopDate < toDate)
            {
                Console.WriteLine(loopDate.ToShortDateString());
                var watch = System.Diagnostics.Stopwatch.StartNew();

                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();

                    using (var insertCommand = con.CreateCommand())
                    {
                        insertCommand.CommandType = CommandType.StoredProcedure;
                        insertCommand.CommandText = "dbo.BulkInsert";
                        insertCommand.CommandTimeout = 600;

                        var fromDateParam = new SqlParameter("@FromDate", SqlDbType.DateTime)
                        {
                            Value = loopDate
                        };

                        loopDate = loopDate.AddDays(1);

                        insertCommand.Parameters.Add(fromDateParam);
                        var toDateParam = new SqlParameter("@ToDate", SqlDbType.DateTime)
                        {
                            Value = loopDate
                        };
                        insertCommand.Parameters.Add(toDateParam);

                        insertCommand.ExecuteNonQuery();
                    }

                    con.Close();
                }

                watch.Stop();
                Console.WriteLine("Elapsed ms: " + watch.ElapsedMilliseconds);
            }
        }

        private static void LoadDataIntoFlatFiles(DateTime fromDate, DateTime toDate)
        {
            Console.WriteLine("Load Data:");
            Console.WriteLine("Time is " + DateTime.Now.ToShortTimeString());
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var loopDate = fromDate;

            var threadCount = 0;
            var taskList = new List<Task>();

            while (loopDate < toDate)
            {
                var year = loopDate.Year;
                var month = loopDate.Month;
                var day = loopDate.Day;

                var lastTask = new Task(() => LoadDay(year, month, day));
                lastTask.Start();
                taskList.Add(lastTask);

                loopDate = loopDate.AddDays(1);
                threadCount = threadCount + 1;

                if (threadCount != 4 && loopDate != toDate) continue;

                Task.WaitAll(taskList.ToArray());
                taskList.Clear();
                threadCount = 0;
            }

            watch.Stop();
            Console.WriteLine("Time is " + DateTime.Now.ToShortTimeString() + ", elapsed ms: " + watch.ElapsedMilliseconds);
        }

        private static void LoadDay(int year, int month, int day)
        {
            var date = new DateTime(year, month, day);

            Console.WriteLine(date.ToShortDateString());
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                var transaction = new StringBuilder();
                var associative = new StringBuilder();

                //100 batches of 500 ie 50000 per day per thread
                for (var i = 0; i < 100; i++)
                {
                    using (var insertCommand = con.CreateCommand())
                    {
                        insertCommand.CommandType = CommandType.StoredProcedure;
                        insertCommand.CommandText = "dbo.BulkLoad";

                        var fromDate = new SqlParameter("@FromDate", SqlDbType.DateTime)
                        {
                            Value = date
                        };
                        insertCommand.Parameters.Add(fromDate);

                        var result = insertCommand.ExecuteScalar().ToString();
                        var split = result.Split(new[] { "#" }, StringSplitOptions.None);
                        var error = false;
                        if (!string.IsNullOrEmpty(result) && split.Length == 501)
                        {
                            for (var l = 0; l < 500; l++)
                            {
                                var row = split[l].Split(new[] { "|" }, StringSplitOptions.None);
                                if (!string.IsNullOrEmpty(split[l]) && row.Length == 2)
                                {
                                    transaction.AppendLine(row[0]);
                                    associative.AppendLine(row[1]);
                                }
                                else
                                {
                                    error = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            error = true;
                        }

                        if (!error) continue;

                        Console.WriteLine("Error at " + i);
                        i = i - 1;
                    }

                }

                var associativeFile = new StreamWriter($@"{BasePath}\{Associative}\{date:yyyyMMdd}.txt");
                associativeFile.WriteLine(associative.ToString());
                associativeFile.Close();

                var transactionFile = new StreamWriter($@"{BasePath}\{Transaction}\{date:yyyyMMdd}.txt");
                transactionFile.WriteLine(transaction.ToString());
                transactionFile.Close();

                con.Close();
            }
        }
    }
}
