using core.repository;

namespace core
{
    public class Container
    {
        private static Container container = null;

        // Clients

        //Repositories
        public IRepository feedRepo { get; set; }
        public IRepository profileRepo { get; set; }
        public IRepository userRepo { get; set; }

        private static Container instance = null;

        private static readonly object padlock = new object();

        private Container()
        {
        }

        public static Container Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new Container();
                        }
                    }
                }
                return instance;
            }
        }
    }
}