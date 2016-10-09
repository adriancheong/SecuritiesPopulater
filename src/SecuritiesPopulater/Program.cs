using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace SecuritiesPopulater
{
    public class Program
    {
        private static readonly string REDIS_ALIAS = "REDIS";
        private static readonly string REDIS_ADDRESS_ENV_PROPERTY_KEY = REDIS_ALIAS + "_PORT_6379_TCP_ADDR";
        private static readonly string REDIS_PORT_ENV_PROPERTY_KEY = REDIS_ALIAS + "_PORT_6379_TCP_PORT";
        private static readonly int NUMBER_OF_SECURITIES_TO_CREATE = 1000;

        public static void Main(string[] args)
        {
            int numberOfSecurities = NUMBER_OF_SECURITIES_TO_CREATE;
            if (args != null && args.Length == 1)
            {
                int.TryParse(args[0], out numberOfSecurities);
            }
            initAndPopulateRedisStore(numberOfSecurities);
        }

        private static void initAndPopulateRedisStore(int numberOfSecurities)
        {
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints =
                {
                    { Environment.GetEnvironmentVariable(REDIS_ADDRESS_ENV_PROPERTY_KEY), int.Parse(Environment.GetEnvironmentVariable(REDIS_PORT_ENV_PROPERTY_KEY)) }
                }
            };
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configurationOptions);

            IDatabase db = redis.GetDatabase();
            IList<Security> securities = SecuritiesGenerator.GenerateRandomSecurities(numberOfSecurities);
            foreach (Security security in securities)
            {
                db.HashSet(security.SecurityId, GenerateRedisHash<Security>(security));
                Console.WriteLine("Storing Security {0} to Redis", security.SecurityId);
            }
        }

        private static HashEntry[] GenerateRedisHash<T>(T obj)
        {
            var props = typeof(T).GetProperties();
            var hash = new HashEntry[props.Count()];
            for (int i = 0; i < props.Count(); i++)
                hash[i] = new HashEntry(props[i].Name, props[i].GetValue(obj).ToString());
            return hash;
        }
    }
}
