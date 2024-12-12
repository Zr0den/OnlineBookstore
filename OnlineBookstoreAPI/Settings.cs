namespace OnlineBookstoreAPI
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public class MySqlSettings
    {
        public string ConnectionString { get; set; }
    }

    public class RedisSettings
    {
        public string ConnectionString { get; set; }
    }
}
