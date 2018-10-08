using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrainerTracks.Data.Context;
using TrainerTracks.Data.Model;

namespace TrainerTracks
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            // Build the configuration reading from appsettings.{environment}.json
            // Pretty much lifted from https://stackoverflow.com/questions/31453495/how-to-read-appsettings-values-from-config-json-in-asp-net-core#answer-31453663
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region controller setup
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            #endregion controller setup

            #region config file setup
            // Add functionality to inject IOptions<T>
            services.AddOptions();
            // Add our Config object so it can be injected. Reads from the TrainerTracksConfig section of the appsettings.json file.
            services.Configure<TrainerTracksConfig>(Configuration.GetSection("TrainerTracksConfig"));
            #endregion config file setup

            #region DB context setup

            var connection = Configuration.GetConnectionString("TrainerTracks");
            services.AddDbContext<TrainerTracksContext>(options => options.UseNpgsql(connection));

            //services.AddScoped<IDataAccessProvider, DataAccessPostgreSqlProvider.DataAccessPostgreSqlProvider>();

            #endregion DB context setup
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
