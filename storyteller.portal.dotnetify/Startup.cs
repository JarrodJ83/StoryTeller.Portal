using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using DotNetify;
using MediatR;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using storyteller.portal.dotnetify;
using storyteller.portal.dotnetify.view_models;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using StoryTeller.Portal;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.CQRS.Sql;
using StoryTeller.Portal.Middleware;
using StoryTeller.Portal.QueryHandlers;
using StoryTeller.Portal.RequestHandlers;
using StoryTeller.ResultAggregation.CommandHandlers;
using StoryTeller.ResultAggregation.QueryHandlers;
using StoryTeller.ResultAggregation.RequestHandlers;
using IMediator = MediatR.IMediator;

namespace helloworld
{
    public class Startup
    {
        private Container container = new Container();
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSignalR();
            services.AddDotNetify();
            services.AddMvc();
            IntegrateSimpleInjector(services);
        }

        public void Configure(IApplicationBuilder app)
        {
            InitializeContainer(app);
            container.Verify();
            app.Use((c, next) => container.GetInstance<ApiAuthenticationMiddleware>().Invoke(c, next));

            app.UseWebSockets();
            app.UseSignalR();
            app.UseDotNetify(config => {
                config.SetFactoryMethod((type, args) =>
                {
                    return container.GetInstance(type);
                });
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                //routes.MapSpaFallbackRoute(
                //    name: "spa-fallback",
                //    defaults: new { controller = "Home", action = "Index" });
            });
          
        }

        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(container));

            services.EnableSimpleInjectorCrossWiring(container);
            services.UseSimpleInjectorAspNetRequestScoping(container);
            services.AddMiddlewareAnalysis();
        }

        private void InitializeContainer(IApplicationBuilder app)
        {
            // Add application presentation components:
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);

            var sqlSettings = new SqlSettings();;

            container.Register<ISqlSettings>(() => sqlSettings, Lifestyle.Singleton);
            container.Register<IApiContext, ApiContext>(Lifestyle.Scoped);
            container.Register<ApiAuthenticationMiddleware>(Lifestyle.Scoped);
            container.Register(() => new RunFeed(new LatestRunSummariesViaSql(sqlSettings), new SummaryForRunViaSql(sqlSettings)) , Lifestyle.Singleton);
            
            RegisterCQRSHandlers();

            RegisterMediatr(container);

            // Need to wait for core 2.1 release as there is a bug using transaction scope currently
            //container.RegisterDecorator(typeof(IRequestHandler<>), typeof(TransactionScopeRequestDecorator<>));
            //container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(TransactionScopeRequestResponseDecorator<,>));

            // Cross-wire ASP.NET services (if any). For instance:
            container.CrossWire<ILoggerFactory>(app);
            container.CrossWire<IHttpContextAccessor>(app);

            // NOTE: Do prevent cross-wired instances as much as possible.
            // See: https://simpleinjector.org/blog/2016/07/
        }

        private void RegisterCQRSHandlers()
        {
            container.Register(typeof(ICommandHandler<>), new[] {typeof(AddRunForApplicationViaSql).Assembly}, Lifestyle.Scoped);
            container.Register(typeof(ICommandHandler<,>), new[] {typeof(AddRunForApplicationViaSql).Assembly}, Lifestyle.Scoped);

            container.Register(typeof(StoryTeller.Portal.CQRS.IRequestHandler<>),
                new[] {typeof(AddRunRequestHandler).Assembly}, Lifestyle.Scoped);
            container.Register(typeof(StoryTeller.Portal.CQRS.IRequestHandler<,>),
                new[] {typeof(AddRunRequestHandler).Assembly, typeof(AllAppsRequestHandler).Assembly}, Lifestyle.Scoped);

            container.Register(typeof(IQueryHandler<,>),
                new[] {typeof(GetAllSpecsViaSql).Assembly, typeof(AllAppsViaSql).Assembly}, Lifestyle.Scoped);
        }
        void RegisterMediatr(Container container)
        {
            container.RegisterSingleton<IMediator, MediatR.Mediator>();
            container.RegisterDecorator<IMediator, MediatorExceptionHandlerDecorator>(Lifestyle.Scoped);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            container.RegisterCollection(typeof(INotificationHandler<>), assemblies);
            container.RegisterCollection(typeof(IAsyncNotificationHandler<>), assemblies);
            container.RegisterCollection(typeof(ICancellableAsyncNotificationHandler<>), assemblies);

            container.RegisterSingleton(Console.Out);

            container.RegisterSingleton(new SingleInstanceFactory(container.GetInstance));
            container.RegisterSingleton(new MultiInstanceFactory(container.GetAllInstances));
        }

    }
}