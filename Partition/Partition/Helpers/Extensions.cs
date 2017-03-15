using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Partition.Model;

namespace Partition.Helpers
{
    public static class Extensions
    {
        public static string DataPointToField(this DataDefinitions.DataPointType dataPointType)
        {
            return dataPointType == DataDefinitions.DataPointType.Profit
                ? DataDefinitions.DataPointType.SalePrice + "-" + DataDefinitions.DataPointType.CostPrice
                : dataPointType.ToString();
        }
    }
}
