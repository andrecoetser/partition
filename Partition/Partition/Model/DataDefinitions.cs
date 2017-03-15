namespace Partition.Model
{
    public class DataDefinitions
    {
        public enum DateType { Year, Month }

        public enum DimensionType { Product, Store }

        public enum DataPointType { SalePrice, CostPrice, Profit }

        public enum AggregateType { Sum, Avg, Max, Min }

        public enum DataOrder { Asc, Desc }
    }
}
