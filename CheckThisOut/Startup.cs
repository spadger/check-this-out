using System;
using System.Text;
using JonBates.CheckThisOut.Core;
using JonBates.CheckThisOut.Core.BankClient;
using JonBates.CheckThisOut.Core.PaymentStore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Prometheus;

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


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateActor = false,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = false,
                        ValidateTokenReplay = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });



            services.AddControllers();
            services.AddSwaggerGen(
                x =>
                {

                    OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
                    {
                        Name = "Bearer",
                        BearerFormat = "JWT",
                        Scheme = "bearer",
                        Description = "Specify the authorization token.",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                    };
                    x.AddSecurityDefinition("jwt_auth", securityDefinition);

                    // Make sure swagger UI requires a Bearer token specified
                    OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = "jwt_auth",
                            Type = ReferenceType.SecurityScheme
                        }
                    };
                    OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
                    {
                        {securityScheme, new string[] { }},
                    };
                    x.AddSecurityRequirement(securityRequirements);

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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMetricServer();

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
