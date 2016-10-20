namespace SecuritiesPopulater
{
    public class Program
    {
        private static readonly int NUMBER_OF_SECURITIES_TO_CREATE = 100000;

        public static void Main(string[] args)
        {
            int startingSecurityId = 0;
            int numberOfSecurities = NUMBER_OF_SECURITIES_TO_CREATE;
            if (args != null && args.Length > 0)
            {
                int.TryParse(args[0], out numberOfSecurities);
                if (args.Length > 1)
                    int.TryParse(args[1], out startingSecurityId);
            }
            IStore store = new RedisStore();
            store.Populate(numberOfSecurities, startingSecurityId);
        }
    }
}
