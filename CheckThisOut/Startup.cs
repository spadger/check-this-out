using System;
using System.IO;
using System.Reflection;
using JonBates.CheckThisOut.Core;
using JonBates.CheckThisOut.Core.BankClient;
using JonBates.CheckThisOut.Core.PaymentStore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace JonBates.CheckThisOut
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
            services.AddSingleton<IPaymentProcess, PaymentProcess>();
            services.AddSingleton<IBankClient, FakeBankClient>();
            services.AddSingleton<IPaymentStore, InMemoryPaymentStore>();

            services.AddControllers();
            services.AddSwaggerGen(
                x =>
                {
                    x.SwaggerDoc("v1",
                        new OpenApiInfo
                        {
                            Title = "Check-This-Out",
                            Version = "v1",
                            Contact = new OpenApiContact
                            {
                                Email = "jonmbates@gmail.com",
                                Name = "Jon Bates",
                                Url = new Uri("https://github.com/spadger")
                            }
                        });

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    x.IncludeXmlComments(xmlPath);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CheckThisOut V1");
            });

        }
    }
}
