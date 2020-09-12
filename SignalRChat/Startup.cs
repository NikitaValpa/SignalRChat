using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
using SignalRChat.Hubs;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SignalRChat.Data;

namespace SignalRChat
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // ���� ����� ���������� �� ����� ����������. ����������� ���� ����� ��� ���������� �������� � ��������� �����.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
                //o.KeepAliveInterval = TimeSpan.FromMinutes(1);
            });

                // ���������� ��������� ��� �������������� � ����� ������ Entity Framework
            services.AddDbContext<SignalRChatContext>(options =>
                    options.UseMySql(Configuration.GetConnectionString("SignalRChatContext"), optionsMySql => optionsMySql.EnableRetryOnFailure()));

            // ���������� ��������� ��� �������������� � ����� ������ Identity Framework
            services.AddDbContext<SignalRChatContextIdentity>(options =>
                    options.UseMySql(Configuration.GetConnectionString("SignalRChatContext"), optionsMySql => optionsMySql.EnableRetryOnFailure()));

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
             {   options.Password.RequiredLength = 5;
                 options.Password.RequireNonAlphanumeric = false;
                 options.Password.RequireLowercase = false;
                 options.Password.RequireUppercase = false;
                 options.Password.RequireDigit = false;
             }).AddEntityFrameworkStores<SignalRChatContextIdentity>();

            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });
        }

        // ���� ����� ���������� �� ����� ����������. ����������� ���� ����� ��� ��������� ��������� HTTP-�������.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // �������� HSTS �� ��������� ���������� 30 ����. �� ������ �������� ��� ��� ���������������� ���������, ��. Https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            var loggingOptions = Configuration.GetSection("Log4NetCore").Get<Log4NetProviderOptions>();
            loggerFactory.AddLog4Net(loggingOptions);

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<ChatHub>("/Chater");
            });
        }
    }
}