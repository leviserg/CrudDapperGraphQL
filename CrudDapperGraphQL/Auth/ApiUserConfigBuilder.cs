namespace CrudDapperGraphQL.Auth
{
    public static class ApiUserConfigBuilder
    {
        public static IConfiguration Build(string apiUserJsonConfig)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(apiUserJsonConfig);
            writer.Flush();
            stream.Position = 0;
            IConfiguration configuration = new ConfigurationBuilder().AddJsonStream(stream).Build();
            stream.Dispose();
            return configuration;
        }
    }
}
