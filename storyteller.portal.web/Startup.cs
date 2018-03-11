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
using CQRS = StoryTeller.Portal.CQRS;
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
using StoryTeller.Portal.RequestHandlers;
using StoryTeller.Portal.Requests;
using StoryTeller.Portal.Web.Hubs;
using MediatR;

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
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:4200");
                }));
            services.AddSignalR(config =>
            {
            });
            
            services.AddDbContext<ResultsDbContext>(options => 
                options.UseSqlServer($"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Results;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));
            RegisterMicrosoftDependencyInjection(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("CorsPolicy");
            app.UseSignalR(config =>
            {
                config.MapHub<DashboardHub>("/dashboard");                
            });
            
            
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

            services.AddSingleton<ISqlSettings, SqlSettings>();
            services.AddTransient<IApiContext, ApiContext>();
            services.AddTransient<ApiAuthenticationMiddleware>();

            services.AddTransient<INotificationHandler<RunCreated>, EventsBroadcaster>();
            services.AddTransient<CQRS.ICommandHandler<AddRunForApplication>, AddRunForApplicationViaSql>();
            services.AddTransient<CQRS.ICommandHandler<AddRunResult>, AddRunResultViaSql>();
            services.AddTransient<CQRS.ICommandHandler<AddSpecToRun>, AddSpecToRunViaSql>();
            services.AddTransient<CQRS.ICommandHandler<AddSpec, int>, AddSpecViaSql>();
            services.AddTransient<CQRS.ICommandHandler<UpdateRunSpec>, UpdateRunSpecViaSql>();
            services.AddTransient<CQRS.ICommandHandler<UpdateRun>, UpdateRunViaSql>();

            services.AddTransient<CQRS.IQueryHandler<AllApps, List<App>>, AllAppsViaSql>();
            services.AddTransient<CQRS.IQueryHandler<LatestRunSumarries, List<RunSummary>>, LatestRunSummariesViaSql>();
            services.AddTransient<CQRS.IQueryHandler<SummaryOfRun, RunSummary>, SummaryForRunViaSql>();
            services.AddTransient<CQRS.IQueryHandler<SpecsByApplication, List<Spec>>, GetAllSpecsViaSql>();
            services.AddTransient<CQRS.IQueryHandler<LatestRunByApplication, Run>, LatestRunByApplicationViaSql>();

            // controller handlers
            services.AddTransient<CQRS.IRequestHandler<AllAppsRequest, List<App>>, AllAppsRequestHandler>();

            // Results Aggregation Handlers
            services.AddTransient<CQRS.IRequestHandler<AddRunRequest, Run>, AddRunRequestHandler>();
            services.AddTransient<CQRS.IRequestHandler<AddSpecBatchToRunRequest, List<RunSpec>>, AddSpecBatchToRunRequestHandler>();
            services.AddTransient<CQRS.IRequestHandler<AddSpecRequest, Spec>, AddSpecRequestHandler>();
            services.AddTransient<CQRS.IRequestHandler<AddSpecToRunRequest, RunSpec>, AddSpecToRunRequestHandler>();
            services.AddTransient<CQRS.IRequestHandler<GetLatestRunRequest, Run>, GetLatestRunRequestHandler>();
            services.AddTransient<CQRS.IRequestHandler<GetAllSpecs, List<Spec>>, GetSpecsRequestHandler>();
            services.AddTransient<CQRS.IRequestHandler<PostRunResultRequest>, PostRunResultRequestHandler>();
            services.AddTransient<CQRS.IRequestHandler<PutRunRequest>, PutRunRequestHandler>();
            services.AddTransient<CQRS.IRequestHandler<PutRunSpecRequest>, PutRunSpecRequestHandler>();
            services.AddTransient<CQRS.IRequestHandler<StoryTeller.Portal.Requests.RunSummaries, List<RunSummary>>, StoryTeller.Portal.RequestHandlers.RunSummaries>();

            services.AddSingleton<MediatR.IMediator, MediatR.Mediator>();
            //services.AddTransient<MediatR.IMediator, MediatorExceptionHandlerDecorator>();

            services.AddMediatR(); 
            
            services.AddSingleton(p => new MediatR.SingleInstanceFactory(p.GetService));
            services.AddSingleton(p => new MediatR.MultiInstanceFactory(p.GetServices));            
        }
    }
}
