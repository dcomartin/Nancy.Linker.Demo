using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Nancy.Linker.Demo.Features.ChangePricingLevel;

namespace Nancy.Linker.Demo.Features.GetCustomer
{
    public class Module : NancyModule
    {
        public static Route Route = new Route("GetCustomer", "/demo/customers/{CustomerId:int}");

        public Module(IMediator mediator)
        {
            Get[Route.Name, Route.Path, true] = async (parameters, token) =>
            {
                var query = new Query((int) parameters.CustomerId);
                return await mediator.SendAsync(query);
            };
        }
    }

    public static class RouteLinkerExtension
    {
        public static string GetCustomer(this RouteLinker routeLinker, int customerId)
        {
            return routeLinker.BuildAbsoluteRoute(Module.Route, new { CustomerId = customerId });
        }
    }

    public class Query : IAsyncRequest<ViewModel>
    {
        public int CustomerId { get; }

        public Query(int customerId)
        {
            CustomerId = customerId;
        }
    }

    public class ViewModel
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public Links _links { get; set; }

        public class Links
        {
            public Link Self { get; }
            public Link ChangePricingLevel { get; }

            public Links(Link self, Link changePricingLevel)
            {
                Self = self;
                ChangePricingLevel = changePricingLevel;
            }
        }
    }

    public class Handler : IAsyncRequestHandler<Query, ViewModel>
    {
        private readonly DbFactory _dbFactory;
        private readonly RouteLinker _routeLinker;

        public Handler(DbFactory dbFactory, RouteLinker routeLinker)
        {
            _dbFactory = dbFactory;
            _routeLinker = routeLinker;
        }

        public async Task<ViewModel> Handle(Query message)
        {
            using (var db = _dbFactory())
            {
                var customer = db.Customers.Single(x => x.CustomerId == message.CustomerId);

                return new ViewModel
                {
                    CustomerId = customer.CustomerId,
                    Name = customer.Name,
                    _links =
                        new ViewModel.Links(
                            new Link(_routeLinker.GetCustomer(customer.CustomerId)),
                            new Link(_routeLinker.ChangePricingLevel(customer.CustomerId)))
                };
            }
        }
    }
}
