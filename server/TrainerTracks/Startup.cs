using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TrainerTracks.Data.Context;
using TrainerTracks.Data.Model;
using TrainerTracks.Data.Repository;
using TrainerTracks.Web.Services;

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
            #region Services interface coupling

            services.AddScoped<IAccountServices, AccountServices>();
            services.AddScoped<ITrainerRepository, TrainerRepository>();
            services.AddScoped<ITrainerCredentialsRepository, TrainerCredentialsRepository>();

            #endregion Services interface coupling

            #region JWT setup

            string jwtKey = Configuration.GetSection("TrainerTracksConfig").GetValue<string>("JwtKey");
            byte[] key = Encoding.ASCII.GetBytes(jwtKey);
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

            #endregion JWT setup

            #region Controller setup

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #endregion Controller setup

            #region Config file setup

            // Add functionality to inject IOptions<T>
            services.AddOptions();
            // Add our Config object so it can be injected. Reads from the TrainerTracksConfig section of the appsettings.json file.
            services.Configure<TrainerTracksConfig>(Configuration.GetSection("TrainerTracksConfig"));

            #endregion Config file setup

            #region DB context setup

            string connection = Configuration.GetConnectionString("TrainerTracks");
            services.AddDbContext<AccountContext>(options => options.UseNpgsql(connection));

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

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            
            app.UseHttpsRedirection();
            app.UseCookiePolicy();

            // Make sure UseAuthentication comes before UseMvc to ensure authentication occurs properly.
            // See: https://stackoverflow.com/questions/48720518/asp-net-core-2-401-error-with-bearer-token
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
