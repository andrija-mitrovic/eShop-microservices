namespace Basket.API
{
    public class Constants
    {
        public const string REDIS_CONNECTION_STRING = "CacheSettings:ConnectionString";
        public const string GRPC_DISCOUNT_SETTINGS = "GrpcSettings:DiscountUrl";
        public const string EVENT_BUS_HOST_ADDRESS = "EventBusSettings:HostAddress";
        public const string ENVIRONMENT = "Environment";
        public const string SERILOG_MINIMUM_LOGGING_LEVEL = "Serilog:MinimumLevel";
        public const string SERILOG_ELASTIC_SEARCH_CONFIGURATION = "Serilog:ElasticSearch";
    }
}
