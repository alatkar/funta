﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core;
using core.repository;
using core.repository.azureCosmos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FeedService
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
            services.AddCors();

            //services.AddTransient<IGetDocClient, Class1>();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Initialize();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(
                options => options.WithOrigins("https://funta.azurewebsites.net", "http://funta.azurewebsites.net", "http://localhost:4200").AllowAnyHeader().AllowAnyMethod()
            );
            app.UseMvc();
        }

        private void Initialize()
        {
            var token = authorization.GetToken().Result;
            Container.Instance.feedRepo = AzureCosmosDocRepository.CreateAzureCosmosDocRepository("FeedCollection", token).Result;
        }
    }
}
