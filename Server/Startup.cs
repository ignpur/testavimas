using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Adapter;
using Server.Bridge;
using Server.Proxy;

namespace Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();

			services.AddSingleton<Game>();
            
            services.AddSingleton<ConsoleLogger>();
            services.AddSingleton(new FileLogger("logs/game_log.txt"));
            
            services.AddSingleton(provider =>
                new PlayerActionLogger(provider.GetRequiredService<ConsoleLogger>()));
            services.AddSingleton(provider =>
                new GameEventLogger(provider.GetRequiredService<FileLogger>()));
            
            services.AddSingleton(provider =>
                new LoggerAdapter(provider.GetRequiredService<PlayerActionLogger>()));
            services.AddSingleton(provider =>
                new LoggerAdapter(provider.GetRequiredService<GameEventLogger>()));

			services.AddScoped<GameFacade>(provider =>
			{
				var game = provider.GetService<IGame>();
				var hubContext = provider.GetRequiredService<IHubContext<GameHub>>();
				return new GameFacade(game, hubContext);
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<GameHub>("/hubs/battleship");
            });
        }
    }
}
