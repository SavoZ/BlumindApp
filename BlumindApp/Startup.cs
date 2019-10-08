using BlumindApp.Filters;
using Entities;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace BlumindApp {

    public class Startup {
        private readonly ILogger<DefaultCorsPolicyService> _logger;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _logger = loggerFactory.CreateLogger<DefaultCorsPolicyService>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BlumindBase"))
            );

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddSigningCredential(new RsaSecurityKey(RSA.Create(4096)))
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(IdentityConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityConfig.GetApiResources())
                .AddInMemoryClients(IdentityConfig.GetClients())
                .AddAspNetIdentity<ApplicationUser>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddIdentityServerAuthentication(options =>
            {
                options.Authority = Configuration.GetValue<string>("IdSrvAuth");
                options.ApiName = IdentityConfig.ApiResourceName;

#if !DEBUG
				options.RequireHttpsMetadata = true; 
#else
                options.RequireHttpsMetadata = false;
#endif
            });


            var cors = new DefaultCorsPolicyService(_logger)
            {
                AllowAll = true
            };

            services.AddSingleton<ICorsPolicyService>(cors);

            services.AddCors(options =>
            {
                options.AddPolicy("JtCors", m =>
                {
                    //m.WithOrigins("http://localhost:4200", "https://localhost:4200");
                    m.AllowAnyOrigin();
                    m.AllowAnyMethod();
                    m.AllowAnyHeader();
                    m.AllowCredentials();
                });
                var or = options.GetPolicy("JtCors").Origins;
            });

            services.AddMvc(options =>
            {
                //Required for Authorization attribute. This will apply 'Bearer' scheme to authorization.
                var policyBuilder = new AuthorizationPolicyBuilder
                {
                    AuthenticationSchemes = new List<string> { IdentityServerAuthenticationDefaults.AuthenticationScheme }
                };
                policyBuilder.RequireAuthenticatedUser();

                var policy = policyBuilder.Build();

                options.Filters.Add(new AuthorizeFilter(policy));
                options.Filters.Add(new GlobalErrorFilter());
            });

            services.AddAuthorization();


            services.AddMvc(options =>
            {
                var policyBuilder = new AuthorizationPolicyBuilder
                {
                    AuthenticationSchemes = new List<string> { IdentityServerAuthenticationDefaults.AuthenticationScheme }
                };

                policyBuilder.RequireAuthenticatedUser();
                var policy = policyBuilder.Build();

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
                    {
                        configuration.RootPath = "ClientApp/dist";
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();


            app.UseCors("JtCors");
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseIdentityServer();
            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSpa(spa => {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
