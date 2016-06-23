namespace Nancy.Linker.Demo
{
    public class Route
    {
        public string Name { get; }
        public string Path { get; }

        public Route(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }

    public class RouteLinker
    {
        private readonly IResourceLinker _linker;
        private readonly NancyContext _context;

        public RouteLinker(IResourceLinker linker, NancyContext context)
        {
            _linker = linker;
            _context = context;
        }

        public string BuildAbsoluteRoute(Route route, dynamic parameters = null)
        {
            return _linker.BuildAbsoluteUri(_context, route.Name, parameters).ToString();
        }

        public string BuildRelativeRoute(Route route, dynamic parameters = null)
        {
            return _linker.BuildRelativeUri(_context, route.Name, parameters).ToString();
        }
    }
}
