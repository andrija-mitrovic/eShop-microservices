namespace Catalog.API
{
    public class Constants
    {
        public const string MONGO_DB_CONNECTION_STRING = "DatabaseSettings:ConnectionString";
        public const string MONGO_DB_DATABASE_NAME = "DatabaseSettings:DatabaseName";
        public const string MONGO_DB_COLLECTION_NAME = "DatabaseSettings:CollectionName";
        public const string ENVIRONMENT = "Environment";
        public const string SERILOG_MINIMUM_LOGGING_LEVEL = "Serilog:MinimumLevel";
        public const string SERILOG_ELASTIC_SEARCH_CONFIGURATION = "Serilog:ElasticSearch";
    }
}
