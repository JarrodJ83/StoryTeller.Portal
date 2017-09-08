using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using DotNetify;
using MediatR;
using Microsoft.Extensions.Configuration;
using storyteller.portal.dotnetify;
using storyteller.portal.dotnetify.view_models;
using Serilog;
using StoryTeller.Portal;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.CQRS.Sql;
using StoryTeller.Portal.Middleware;
using StoryTeller.Portal.Models;
using StoryTeller.Portal.Models.Views;
using StoryTeller.Portal.Queries;
using StoryTeller.Portal.QueryHandlers;
using StoryTeller.ResultAggregation.CommandHandlers;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.ResultAggregation.Events;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.ResultAggregation.Queries;
using StoryTeller.ResultAggregation.QueryHandlers;
using StoryTeller.ResultAggregation.RequestHandlers;
using StoryTeller.ResultAggregation.Requests;
using CQRS = StoryTeller.Portal.CQRS;

namespace helloworld
{
    public class Startup
    {
        //private Container container = new Container();
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

            //IntegrateSimpleInjector(services);
            RegisterMicrosoftDependencyInjection(services);
        }

       

        public void Configure(IApplicationBuilder app)
        {
            //InitializeContainer(app);
            //container.Verify();
            //app.Use((c, next) => container.GetInstance<ApiAuthenticationMiddleware>().Invoke(c, next));
            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Seq("http://localhost:5341").CreateLogger();

            app.UseWebSockets();
            app.UseSignalR();
            app.UseDotNetify();
            app.UseDotNetify(config =>
            {
                
                //config.UseDeveloperLogging(log => logger.Verbose(log));
                //config.SetFactoryMethod((type, args) =>
                //{
                //    return container.GetInstance(type);
                //});
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

        //private void IntegrateSimpleInjector(IServiceCollection services)
        //{
        //    container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

        //    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //    services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));
        //    services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(container));

        //    //services.AddSingleton<Container>();


        //    //services.EnableSimpleInjectorCrossWiring(container);
        //    //services.UseSimpleInjectorAspNetRequestScoping(container);
        //    services.AddMiddlewareAnalysis();
        //}

        private void RegisterMicrosoftDependencyInjection(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var eventsHub = new EventsHub();
            services.AddSingleton<IEventsHub>(eventsHub);
            services.AddSingleton<ISqlSettings, SqlSettings>();
            services.AddTransient<IApiContext, ApiContext>(); 
            services.AddTransient<ApiAuthenticationMiddleware>();

            services.AddTransient<ICommandHandler<AddRunForApplication>, AddRunForApplicationViaSql>();
            services.AddTransient<ICommandHandler<AddRunResult>, AddRunResultViaSql>();
            services.AddTransient<ICommandHandler<AddSpecToRun>, AddSpecToRunViaSql>();
            services.AddTransient<ICommandHandler<AddSpec, int>, AddSpecViaSql>();
            services.AddTransient<ICommandHandler<UpdateRunSpec>, UpdateRunSpecViaSql>();
            services.AddTransient<ICommandHandler<UpdateRun>, UpdateRunViaSql>();

            services.AddTransient<IQueryHandler<AllApps, List<App>>, AllAppsViaSql>();
            services.AddTransient<IQueryHandler<LatestRunSumarries, List<RunSummary>>, LatestRunSummariesViaSql>();
            services.AddTransient<IQueryHandler<SummaryOfRun, RunSummary>, SummaryForRunViaSql>();
            services.AddTransient<IQueryHandler<SpecsByApplication, List<Spec>>, GetAllSpecsViaSql>();
            services.AddTransient<IQueryHandler<LatestRunByApplication, Run>, LatestRunByApplicationViaSql>();

            services.AddTransient<CQRS.IRequestHandler<AddRunRequest, Run>, AddRunRequestHandler>();
            services.AddTransient<CQRS.IRequestHandler<AddSpecBatchToRunRequest, List<RunSpec>>, AddSpecBatchToRunRequestHandler>();
            services.AddTransient<CQRS.IRequestHandler<AddSpecRequest, Spec>, AddSpecRequestHandler>();
            services.AddTransient<CQRS.IRequestHandler<AddSpecToRunRequest, RunSpec>, AddSpecToRunRequestHandler>();
            services.AddTransient<CQRS.IRequestHandler<GetLatestRunRequest, Run>, GetLatestRunRequestHandler>();
            services.AddTransient<CQRS.IRequestHandler<GetAllSpecs, List<Spec>>, GetSpecsRequestHandler>();
            services.AddTransient<CQRS.IRequestHandler<PostRunResultRequest>, PostRunResultRequestHandler>();
            services.AddTransient<CQRS.IRequestHandler<PutRunRequest>, PutRunRequestHandler>();
            services.AddTransient<CQRS.IRequestHandler<PutRunSpecRequest>, PutRunSpecRequestHandler>();

            services.AddSingleton<MediatR.IMediator, MediatR.Mediator>();
            //services.AddTransient<MediatR.IMediator, MediatorExceptionHandlerDecorator>();

            services.AddTransient<INotificationHandler<RunCreated>>(provider => eventsHub);
            services.AddTransient<INotificationHandler<RunSpecUpdated>>(provider => eventsHub);
            services.AddTransient<INotificationHandler<RunCompleted>>(provider => eventsHub);
            
            services.AddSingleton(p => new SingleInstanceFactory(p.GetService));
            services.AddSingleton(p => new MultiInstanceFactory(p.GetServices));

            services.AddTransient<RunFeed>();
            services.AddTransient<NavHeader>();
            services.AddTransient<RunChart>();
        }

        //private void InitializeContainer(IApplicationBuilder app)
        //{
        //    // Add application presentation components:
        //    container.RegisterMvcControllers(app);
        //    container.RegisterMvcViewComponents(app);

        //    var sqlSettings = new SqlSettings();;
           
        //    container.Register<IEventsHub, EventsHub>(Lifestyle.Singleton);

        //    container.Register<ISqlSettings>(() => sqlSettings, Lifestyle.Singleton);
        //    container.Register<IApiContext, ApiContext>();
        //    container.Register<ApiAuthenticationMiddleware>();

        //    container.RegisterDisposableTransient<RunFeed, RunFeed>();
        //    container.RegisterDisposableTransient<NavHeader, NavHeader>();
        //    container.RegisterDisposableTransient<RunChart, RunChart>();

        //    //container.Register(() => new RunFeed(new LatestRunSummariesViaSql(sqlSettings), new SummaryForRunViaSql(sqlSettings)));
        //    //container.Register(() => new NavHeader(new AllAppsViaSql(sqlSettings)));
        //    //container.Register(() => new RunFeedRow(new SummaryForRunViaSql(sqlSettings), broadcaster));
        //    //container.Register(() => new RunChart(new LatestRunSummariesViaSql(sqlSettings)));

        //    RegisterCQRSHandlers();

        //    //RegisterMediatr(container);

        //    // Need to wait for core 2.1 release as there is a bug using transaction scope currently
        //    //container.RegisterDecorator(typeof(IRequestHandler<>), typeof(TransactionScopeRequestDecorator<>));
        //    //container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(TransactionScopeRequestResponseDecorator<,>));

        //    // Cross-wire ASP.NET services (if any). For instance:
        //    container.CrossWire<ILoggerFactory>(app);
        //    container.CrossWire<IHttpContextAccessor>(app);

        //    // NOTE: Do prevent cross-wired instances as much as possible.
        //    // See: https://simpleinjector.org/blog/2016/07/
        //}

       

        //private void RegisterCQRSHandlers()
        //{
        //    container.Register(typeof(ICommandHandler<>), new[] {typeof(AddRunForApplicationViaSql).Assembly});
        //    container.Register(typeof(ICommandHandler<,>), new[] {typeof(AddRunForApplicationViaSql).Assembly});

        //    container.Register(typeof(StoryTeller.Portal.CQRS.IRequestHandler<>),
        //        new[] {typeof(AddRunRequestHandler).Assembly});
        //    container.Register(typeof(StoryTeller.Portal.CQRS.IRequestHandler<,>),
        //        new[] {typeof(AddRunRequestHandler).Assembly, typeof(AllAppsRequestHandler).Assembly});

        //    container.Register(typeof(IQueryHandler<,>),
        //        new[] {typeof(GetAllSpecsViaSql).Assembly, typeof(AllAppsViaSql).Assembly});
        //}
        

        //void RegisterMediatr(Container container)
        //{
        //    container.RegisterSingleton<IMediator, MediatR.Mediator>();
        //    container.RegisterDecorator<IMediator, MediatorExceptionHandlerDecorator>();

        //    var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        //    container.RegisterCollection(typeof(INotificationHandler<>), new[]{ typeof(IEventsHub) });
        //    container.RegisterCollection(typeof(IAsyncNotificationHandler<>), assemblies);
        //    container.RegisterCollection(typeof(ICancellableAsyncNotificationHandler<>), assemblies);

        //    container.RegisterSingleton(Console.Out);

        //    container.RegisterSingleton(new SingleInstanceFactory(container.GetInstance));
        //    container.RegisterSingleton(new MultiInstanceFactory(container.GetAllInstances));
        //}

    }

    //static class Ext
    //{
    //    public static void RegisterDisposableTransient<TService, TImplementation>(this Container c) where TImplementation : class, IDisposable, TService
    //        where TService : class
    //    {
    //        var scoped = Lifestyle.Scoped;
    //        var r = Lifestyle.Transient.CreateRegistration<TService, TImplementation>(c);
    //        r.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "ignore");
    //        c.AddRegistration(typeof(TService), r);
    //        c.RegisterInitializer<TImplementation>(o => scoped.RegisterForDisposal(c, o));
    //    }
    //}
}