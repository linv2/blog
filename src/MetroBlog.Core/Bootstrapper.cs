using Autofac;
using Nancy;
using Nancy.Authentication.Token;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Session;
using Nancy.TinyIoc;

namespace MetroBlog.Core
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            CookieBasedSessions.Enable(pipelines);
        }
        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            TokenAuthentication.Enable(pipelines, new TokenAuthenticationConfiguration(container.Resolve<ITokenizer>()));
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            nancyConventions.StaticContentsConventions.Clear();
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("public", "/public"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("themes", "/themes"));
        }
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
           
            base.ConfigureApplicationContainer(container);
         //   container.Register<ISerializer, CustomJsonSerializer>();
            CoreIoCContainer.Current = container;
        }

    }
}
