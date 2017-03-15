using System;
using Partition.Model;

namespace Partition.Entities
{
    public class TimeRangeResponse
    {
        public int DateType { get; set; }

        public decimal DataPoint { get; set; }
    }

    public class TimeRangeRequest
    {
        public DataDefinitions.DateType DateType { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DataDefinitions.DimensionType DimensionType { get; set; }

        public int DimensionTypeValue { get; set; }

        public DataDefinitions.DataPointType DataPointType { get; set; }

        public DataDefinitions.AggregateType AggregateType { get; set; }

        public bool IsValid()
        {
            return Enum.IsDefined(typeof(DataDefinitions.DateType), DateType) &&
                     Enum.IsDefined(typeof(DataDefinitions.DimensionType), DimensionType) &&
                     Enum.IsDefined(typeof(DataDefinitions.DataPointType), DataPointType) &&
                     Enum.IsDefined(typeof(DataDefinitions.AggregateType), AggregateType) &&
                     FromDate >= new DateTime(2009, 1, 1) &&
                     ToDate <= new DateTime(2017, 1, 1);
        }
    }
}
