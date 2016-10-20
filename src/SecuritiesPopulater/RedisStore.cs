using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SecuritiesPopulater
{
    public class RedisStore : IStore
    {
        private static readonly string REDIS_ALIAS = "REDIS";
        private static readonly string REDIS_ADDRESS_ENV_PROPERTY_KEY = REDIS_ALIAS + "_PORT_6379_TCP_ADDR";
        private static readonly string REDIS_PORT_ENV_PROPERTY_KEY = REDIS_ALIAS + "_PORT_6379_TCP_PORT";

        public void Populate(int numberOfSecurities, int startingSecurityId)
        {
            Console.WriteLine("Creating REDIS Endpoints");
            Console.WriteLine("REDIS_ADDRESS_ENV_PROPERTY_KEY = {0}", Environment.GetEnvironmentVariable(REDIS_ADDRESS_ENV_PROPERTY_KEY));
            Console.WriteLine("REDIS_PORT_ENV_PROPERTY_KEY = {0}", Environment.GetEnvironmentVariable(REDIS_PORT_ENV_PROPERTY_KEY));
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints =
                {
                    { Environment.GetEnvironmentVariable(REDIS_ADDRESS_ENV_PROPERTY_KEY), int.Parse(Environment.GetEnvironmentVariable(REDIS_PORT_ENV_PROPERTY_KEY)) }
                }
            };
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configurationOptions);

            IDatabase db = redis.GetDatabase();
            IList<Security> securities = SecuritiesGenerator.GenerateRandomSecurities(numberOfSecurities, startingSecurityId);
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
