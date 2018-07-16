using core.repository;

namespace core
{
    public class Container
    {
        private static Container container = null;

        private Container(IRepository repo)
        {
            this.repo = repo;
        }
        public IRepository repo  { get; }

        public static void CreateContainer(IRepository repo)
        {
            Container.container = new Container(repo);
        }

        public static Container FromContainer()
        {
            return container;
        }

    }
}