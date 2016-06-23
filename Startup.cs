using Owin;

namespace Nancy.Linker.Demo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy(options =>
            {
                options.Bootstrapper = new MyNancyBootstrapper();
            });
        }
    }
}
