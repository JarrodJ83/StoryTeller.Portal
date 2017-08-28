using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.Middleware;
using StoryTeller.ResultAggregation.CommandHandlers;
using StoryTeller.ResultAggregation.QueryHandlers;
using StoryTeller.ResultAggregation.RequestHandlers;
using StoryTeller.ResultAggregation.Settings;

namespace StoryTeller.Portal
{
    public class Startup
    {
        private Container container = new Container();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            IntegrateSimpleInjector(services);
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
            

            container.Register<ISqlSettings, SqlSettings>();
            container.Register<IApiContext, ApiContext>();
            container.Register<ApiAuthenticationMiddleware>();

            container.Register(typeof(ICommandHandler<>), new []{ typeof(AddRunForApplicationViaSql).Assembly });
            container.Register(typeof(ICommandHandler<,>), new[] {typeof(AddRunForApplicationViaSql).Assembly});

            container.Register(typeof(StoryTeller.Portal.CQRS.IRequestHandler<>), new[] { typeof(AddRunRequestHandler).Assembly });
            container.Register(typeof(StoryTeller.Portal.CQRS.IRequestHandler<,>), new[] { typeof(AddRunRequestHandler).Assembly });
            
            container.Register(typeof(IQueryHandler<,>), new[] { typeof(GetAllSpecsViaSql).Assembly });

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

        IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(IMediator).GetTypeInfo().Assembly;
        }

        void RegisterMediatr(Container container)
        {
            container.RegisterSingleton<IMediator, Mediator>();

            var assemblies = GetAssemblies().ToArray();
           
            container.RegisterCollection(typeof(INotificationHandler<>), assemblies);
            container.RegisterCollection(typeof(IAsyncNotificationHandler<>), assemblies);
            container.RegisterCollection(typeof(ICancellableAsyncNotificationHandler<>), assemblies);

            container.RegisterSingleton(Console.Out);

            container.RegisterSingleton(new SingleInstanceFactory(container.GetInstance));
            container.RegisterSingleton(new MultiInstanceFactory(container.GetAllInstances));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitializeContainer(app);

            container.Verify();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    ReactHotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.Use((c, next) => container.GetInstance<ApiAuthenticationMiddleware>().Invoke(c, next));

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
