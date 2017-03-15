using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Partition.Entities;
using Partition.Helpers;

namespace Partition.Model
{
    public class DataContext
    {
        private const string DateFormat = "dd MMM yyyy";
        private readonly string _connectionString;

        public DataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<TimeRangeResponse>> TimeRangeQuery(TimeRangeRequest request)
        {
            return await GetData("dbo.[TimeRangeQuery]",
                new List<SqlParameter>
                {
                    new SqlParameter("@DateType", SqlDbType.VarChar) {Value = request.DateType.ToString()},
                    new SqlParameter("@FromDate", SqlDbType.VarChar) {Value = request.FromDate.ToString(DateFormat)},
                    new SqlParameter("@ToDate", SqlDbType.VarChar) {Value = request.ToDate.ToString(DateFormat)},
                    new SqlParameter("@DimensionType", SqlDbType.VarChar) {Value = request.DimensionType.ToString()},
                    new SqlParameter("@DimensionTypeValue", SqlDbType.Int) {Value = request.DimensionTypeValue},
                    new SqlParameter("@AggregateType", SqlDbType.VarChar) {Value = request.AggregateType.ToString()},
                    new SqlParameter("@DataPointType", SqlDbType.VarChar) { Value = request.DataPointType.DataPointToField()}
                },
                reader =>
                {
                    var mapped = new List<TimeRangeResponse>();
                    while (reader.Read())
                    {

                        mapped.Add(new TimeRangeResponse
                        {
                            DateType = (int) reader["DateType"],
                            DataPoint = (decimal) reader["DataPoint"]
                        });
                    }

                    return mapped;
                }
            );
        }

        public async Task<IEnumerable<ProductStoreResponse>> ProductStoreQuery(ProductStoreRequest request)
        {
            return await GetData("dbo.[ProductStoreQuery]",
                new List<SqlParameter>
                {
                    new SqlParameter("@DataOrder", SqlDbType.VarChar) {Value = request.DataOrder.ToString()},
                    new SqlParameter("@FromDate", SqlDbType.VarChar) {Value = request.FromDate.ToString(DateFormat)},
                    new SqlParameter("@ToDate", SqlDbType.VarChar) {Value = request.ToDate.ToString(DateFormat)},
                    new SqlParameter("@DimensionType", SqlDbType.VarChar) {Value = request.DimensionType.ToString()},
                    new SqlParameter("@DimensionTypeValue", SqlDbType.Int) {Value = request.DimensionTypeValue},
                    new SqlParameter("@AggregateType", SqlDbType.VarChar) {Value = request.AggregateType.ToString()},
                    new SqlParameter("@DataPointType", SqlDbType.VarChar) { Value = request.DataPointType.DataPointToField()}
                },
                reader =>
                {
                    var mapped = new List<ProductStoreResponse>();
                    while (reader.Read())
                    {

                        mapped.Add(new ProductStoreResponse
                        {
                            Dimension = reader["Dimension"].ToString(),
                            DataPoint = (decimal) reader["DataPoint"]
                        });
                    }

                    return mapped;
                }
            );

        }

        private async Task<IEnumerable<T>> GetData<T>(string storedProc, IEnumerable<SqlParameter> parameters,
            Func<SqlDataReader, IEnumerable<T>> loader)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();

                try
                {

                    using (var queryCommand = con.CreateCommand())
                    {
                        queryCommand.CommandType = CommandType.StoredProcedure;
                        queryCommand.CommandText = storedProc;
                        queryCommand.CommandTimeout = 600;

                        foreach (var parameter in parameters)
                        {
                            queryCommand.Parameters.Add(parameter);
                        }

                        return loader(await queryCommand.ExecuteReaderAsync());
                    }
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}
