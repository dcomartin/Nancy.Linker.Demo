using MediatR;
using Nancy.Routing;
using Nancy.TinyIoc;

namespace Nancy.Linker.Demo
{
    public class MyNancyBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            container.Register<IResourceLinker>(
                (x, overloads) =>
                    new ResourceLinker(x.Resolve<IRouteCacheProvider>(),
                        x.Resolve<IRouteSegmentExtractor>(), x.Resolve<IUriFilter>()));

            container.Register<IMediator>((x, overloads) => new Mediator(x.Resolve, x.ResolveAll));

            container.Register<IAsyncRequestHandler<Features.ChangePricingLevel.Command, Unit>>(
                (x, overloads) => new Features.ChangePricingLevel.Handler(() => new FakeDb()));
            container.Register<IAsyncRequestHandler<Features.GetCustomer.Query, Features.GetCustomer.ViewModel>>(
                (x, overloads) => new Features.GetCustomer.Handler((() => new FakeDb()), x.Resolve<RouteLinker>()));
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            container.Register<RouteLinker>((x, overloads) => new RouteLinker(x.Resolve<IResourceLinker>(), context));
        }
    }
}