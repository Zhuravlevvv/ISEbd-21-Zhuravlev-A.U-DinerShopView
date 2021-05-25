using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DinerBusinessLogic.BusinessLogics;
using DinerBusinessLogic.Interfaces;
using DinerViewDatabaseImplement.Implements;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DinerBusinessLogic.HelperModels;

namespace DinerViewRestApi
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
            services.AddTransient<IClientStorage, ClientStorage>();
            services.AddTransient<IOrderStorage, OrderStorage>();
            services.AddTransient<IFoodStorage, FoodStorage>();
            services.AddTransient<ISnackStorage, SnackStorage>();
            services.AddTransient<IStoreHouseStorage, StoreHouseStorage>();
            services.AddTransient<IMessageInfoStorage, MessageInfoStorage>();
            services.AddTransient<OrderLogic>();
            services.AddTransient<ClientLogic>();
            services.AddTransient<SnackLogic>();
            services.AddTransient<MailLogic>();
            services.AddTransient<StoreHouseLogic>();
            MailLogic.MailConfig(new MailConfig
            {
                SmtpClientHost = "smtp.yandex.ru",
                SmtpClientPort = 587,
                MailLogin = "zhuravlev1337.73@yandex.ru",
                MailPassword = "zhura1337228",
            });
            services.AddTransient<FoodLogic>();
            services.AddControllers().AddNewtonsoftJson();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
