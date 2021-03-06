﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core;
using core.repository.azureCosmos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MainService
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
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddControllersAsServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Initialize();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors(
                options => options.WithOrigins("https://funta.azurewebsites.net", "http://funta.azurewebsites.net", "http://localhost:4200").AllowAnyHeader().AllowAnyMethod()
            );
            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void Initialize()
        {
            var token = authorization.GetToken().Result;
            Container.Instance.feedRepo = AzureCosmosDocRepository.CreateAzureCosmosDocRepository("FeedCollection", token).Result;
            Container.Instance.userRepo = AzureCosmosDocRepository.CreateAzureCosmosDocRepository("UserCollection", token).Result;
        }
    }
}
