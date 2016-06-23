using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Nancy.ModelBinding;

namespace Nancy.Linker.Demo.Features.ChangePricingLevel
{
    public class Module : NancyModule
    {
        public static readonly Route Route = new Route("ChangePricingLevel",
            "/demo/customers/{CustomerId:int}/changepricinglevel");

        public Module(IMediator mediator)
        {
            Put[Route.Name, Route.Path, true] =
                async (parameters, token) =>
                {
                    var cmd = this.Bind<Command>();
                    cmd.CustomerId = (int) parameters.CustomerId;

                    await mediator.SendAsync(cmd);

                    return HttpStatusCode.NoContent;
                };
        }
    }

    public static class RouteLinkerExtension
    {
        public static string ChangePricingLevel(this RouteLinker routeLinker, int customerId)
        {
            return routeLinker.BuildAbsoluteRoute(Module.Route, new {CustomerId = customerId});
        }
    }

    public class Command : IAsyncRequest
    {
        public int CustomerId { get; set; }
        public Customer.PricingLevel PricingLevel { get; }

        public Command(int customerId, Customer.PricingLevel pricingLevel)
        {
            CustomerId = customerId;
            PricingLevel = pricingLevel;
        }
    }

    public class Handler : IAsyncRequestHandler<Command, Unit>
    {
        private readonly DbFactory _dbFactory;

        public Handler(DbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<Unit> Handle(Command message)
        {
            using (var db = _dbFactory())
            {
                var customer = db.Customers.Single(x => x.CustomerId == message.CustomerId);
                customer.ChangePricingLevel(message.PricingLevel);
                await db.SaveAsync();
            }

            return Unit.Value;
        }
    }
}
