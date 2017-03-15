using System;

namespace Load
{
    class Program
    {
        static void Main()
        {
            new BulkLoader().Load(new DateTime(2009, 1, 1), new DateTime(2016, 12, 31));
        }
    }
}
