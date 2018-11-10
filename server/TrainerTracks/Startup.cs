using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
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

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddAuthentication().AddCookie("Cookies");

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNumber"));
            //});

            var key = Encoding.ASCII.GetBytes("this is my custom Secret key for authnetication");
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

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

            app.UseCookiePolicy();
            app.UseAuthentication();
        }
    }
}
