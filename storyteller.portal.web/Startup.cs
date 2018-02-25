using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoryTeller.Portal.Db;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;
using StoryTeller.ResultAggregation.CommandHandlers;
using StoryTeller.ResultAggregation.Commands;
using StoryTeller.Portal.CQRS;
using StoryTeller.Portal.Web;
using Microsoft.AspNetCore.Http;
using StoryTeller.Portal.CQRS.Sql;
using StoryTeller.Portal;
using StoryTeller.Portal.Middleware;
using StoryTeller.ResultAggregation.Requests;
using System.Collections.Generic;
using StoryTeller.Portal.Queries;
using StoryTeller.Portal.QueryHandlers;
using StoryTeller.ResultAggregation.Models;
using StoryTeller.Portal.Models.Views;
using StoryTeller.ResultAggregation.Queries;
using StoryTeller.ResultAggregation.QueryHandlers;
using StoryTeller.ResultAggregation.RequestHandlers;
using StoryTeller.Portal.Models;
using StoryTeller.ResultAggregation.Events;

namespace storyteller.portal.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var connection = @"Server=(localdb)\mssqllocaldb;Database=Results;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<ResultsDbContext>(options => options.UseSqlite($"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "results.db")}"));
            RegisterMicrosoftDependencyInjection(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

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

            services.AddTransient<IRequestHandler<AddRunRequest, Run>, AddRunRequestHandler>();
            services.AddTransient<IRequestHandler<AddSpecBatchToRunRequest, List<RunSpec>>, AddSpecBatchToRunRequestHandler>();
            services.AddTransient<IRequestHandler<AddSpecRequest, Spec>, AddSpecRequestHandler>();
            services.AddTransient<IRequestHandler<AddSpecToRunRequest, RunSpec>, AddSpecToRunRequestHandler>();
            services.AddTransient<IRequestHandler<GetLatestRunRequest, Run>, GetLatestRunRequestHandler>();
            services.AddTransient<IRequestHandler<GetAllSpecs, List<Spec>>, GetSpecsRequestHandler>();
            services.AddTransient<IRequestHandler<PostRunResultRequest>, PostRunResultRequestHandler>();
            services.AddTransient<IRequestHandler<PutRunRequest>, PutRunRequestHandler>();
            services.AddTransient<IRequestHandler<PutRunSpecRequest>, PutRunSpecRequestHandler>();

            services.AddSingleton<MediatR.IMediator, MediatR.Mediator>();
            //services.AddTransient<MediatR.IMediator, MediatorExceptionHandlerDecorator>();

            services.AddTransient<MediatR.INotificationHandler<RunCreated>>(provider => eventsHub);
            services.AddTransient<MediatR.INotificationHandler<RunSpecUpdated>>(provider => eventsHub);
            services.AddTransient<MediatR.INotificationHandler<RunCompleted>>(provider => eventsHub);

            services.AddSingleton(p => new MediatR.SingleInstanceFactory(p.GetService));
            services.AddSingleton(p => new MediatR.MultiInstanceFactory(p.GetServices));            
        }
    }
}
