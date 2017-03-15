using System;
using Partition.Model;

namespace Partition.Entities
{
    public class ProductStoreResponse
    {
        public string Dimension { get; set; }

        public decimal DataPoint { get; set; }
    }

    public class ProductStoreRequest
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DataDefinitions.DimensionType DimensionType { get; set; }

        public int DimensionTypeValue { get; set; }

        public DataDefinitions.DataPointType DataPointType { get; set; }

        public DataDefinitions.AggregateType AggregateType { get; set; }

        public DataDefinitions.DataOrder DataOrder { get; set; }

        public bool IsValid()
        {
            return Enum.IsDefined(typeof(DataDefinitions.DataOrder), DataOrder) &&
                     Enum.IsDefined(typeof(DataDefinitions.DimensionType), DimensionType) &&
                     Enum.IsDefined(typeof(DataDefinitions.DataPointType), DataPointType) &&
                     Enum.IsDefined(typeof(DataDefinitions.AggregateType), AggregateType) &&
                     FromDate >= new DateTime(2009, 1, 1) &&
                     ToDate <= new DateTime(2017, 1, 1);
        }
    }

}
