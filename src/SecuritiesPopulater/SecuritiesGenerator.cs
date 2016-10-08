using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuritiesPopulater
{
    public static class SecuritiesGenerator
    {
        private static readonly string[] CURRENCY_LIST = { "SGD", "USD", "GBP", "EUR", "SEK", "CNY", "JPY" };
        private static readonly string[] COUNTRY_LIST = { "Singapore", "United States", "United Kingdom", "Europe", "Sweden", "China", "Japan" };
        private static readonly Random random = new Random();

        public static IList<Security> GenerateRandomSecurities(int numberOfSecurities)
        {
            IList<Security> securities = new List<Security>();
            for (int i = 0; i < numberOfSecurities; i++)
            {
                securities.Add(new Security
                {
                    CountryOfOrigin = getRandomCountry(),
                    Owner = getRandomCountry(),
                    Currency = getRandomCurrency(),
                    SecurityId = createRandomSecurityId(i),
                    Type = getRandomSecurityType(),
                    Valuation = getRandomDouble()
                });
            }
            return securities;
        }

        private static double getRandomDouble()
        {
            return random.NextDouble();
        }

        private static SecurityType getRandomSecurityType()
        {
            int index = random.Next(Enum.GetNames(typeof(SecurityType)).Length);
            return (SecurityType)index;
        }

        private static string createRandomSecurityId(int i)
        {
            return "ISIN" + i;
        }

        private static string getRandomCurrency()
        {
            return CURRENCY_LIST[random.Next(CURRENCY_LIST.Length)];
        }

        private static string getRandomCountry()
        {
            return COUNTRY_LIST[random.Next(COUNTRY_LIST.Length)];
        }
    }
}
