using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParksAPI.Models;
using ParksAPI.Models.Mapper;
using ParksAPI.Models.Repository;
using ParksAPI.Models.Repository.IRepository;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ParksAPI
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
            services.AddControllers();
            // 2. Part 2
            // -----------------------
            services.AddDbContextPool<AppDbContext>(optionsAction => optionsAction.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // -----------------------

            // 3. Part 2
            // ---------------------

            services.AddScoped<INationalParkRepository, NationalParkRepository>();

            services.AddScoped<ITrailRepository, TrailRepository>(); // 6. Part 3

            // ---------------------

            // 12. Part 4
            // ---------------------

            services.AddScoped<IUserRepository, UserRepository>();

            // ---------------------

            // 12. Part 5
            // ---------------------

            // That step of binding AppSettings class to AppSettings, can be neglected.

            // bind AppSettings class to AppSettings section in appsettings.json file.
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // ---------------------

            // 12. Part 6
            // ---------------------

            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            var secretKey = Encoding.ASCII.GetBytes(appSettings.Secret);

            // Add Bearer token support to our api.
            services.AddAuthentication(options => {

                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                // Configure JWt Bearer
            }).AddJwtBearer(options => {

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // Here we are validating the issuer signing key, so that if the key is invalid the request won't be authorized or authenticated.
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey), // We set the issuer signing key to our secret key (stored in appsettings.json)

                    // once we deploy our application or when we using a production environment,
                    // we can set 'ValidateIssuer' to true, and we can set the domain name of the issuer that we want to validate. The same thing for 'ValidateAudience' and the domain name of the audience.

                    
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                // There are many more properties are options available with JWT Bearer token. But here we are going for the basics.
            });

            //services.AddCors();

            // ---------------------

            // 3. Part 5
            // -----------------------------

            // Add the AutoMapper and passing to it the mapping that we created.
            services.AddAutoMapper(typeof(ParksMapping));

            // -----------------------------

            // 7. Part 2
            // ------------------------------

            // Add service Api versioning to the services collection and configure options in it. (Microsoft.AspNetCore.Mvc.Versioning Nuget Package)
            services.AddApiVersioning(options => {

                // This means that if you don't specify an api version, it will load the default version for you and will not give an error.
                options.AssumeDefaultVersionWhenUnspecified = true;

                // set the default version.
                options.DefaultApiVersion = new ApiVersion(1, 0);

                // sets a value indicating whether requests report the current API version in the responses.
                options.ReportApiVersions = true;

            });

            // ------------------------------

            // 7. Part 3
            // ------------------------------

            // ApiExplorer belong to (Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer Nuget Package)

            // set version format of the ApiExplorer, we set it to be small 'v' letter and then the version number(1.5)
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");

            // add the Swagger Generator Configuration file
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            // Register or add the swagger Generator
            services.AddSwaggerGen();

            // ------------------------------

            // 5. Part 2
            // -----------------------------

            // Register the Swagger Generator
            #region AddSwaggerGen
            /*
            services.AddSwaggerGen(options => {

                // 1- The first thing to do, is to add a Swagger document (that is an open API specification).
                //    The first parameter will be the name of the URI on which the "Open API specification" (Swagger document) can be found.
                //    The second parameter will be an object of Microsoft.OpenApi.Models.OpenApiInfo, which will contain informations about that "Open API specification document" (Swagger document) 

                options.SwaggerDoc("ParksOpenAPISpec", new Microsoft.OpenApi.Models.OpenApiInfo()
                {

                    Title = "Parks API",
                    Version = "1",

                    // 5. Part 7
                    // --------------

                    Description = "Parks API Project",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "application jobfinder",
                        Email = "applicationjobfinder@gmail.com",
                        Url = new Uri("https://www.google.com")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }

                    // --------------

                });
            */
                #region Add Two documents 7. Part 1
                /*
                options.SwaggerDoc("ParksOpenAPISpecNP", new Microsoft.OpenApi.Models.OpenApiInfo() {

                    Title = "Parks API-National Parks",
                    Version = "1",
                    Description = "Parks API Project - National Parks.",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "application jobfinder",
                        Email = "applicationjobfinder@gmail.com",
                        Url = new Uri("https://www.google.com")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }

                });

                options.SwaggerDoc("ParksOpenAPISpecTrails", new Microsoft.OpenApi.Models.OpenApiInfo()
                {

                    Title = "Parks API-Trails",
                    Version = "1",
                    Description = "Parks API Project - Trails.",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "application jobfinder",
                        Email = "applicationjobfinder@gmail.com",
                        Url = new Uri("https://www.google.com")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
                */
                #endregion
            /*

                // 5. Part 5
                // -----------------

                // Tell Swashbuckle where it can find this comments so it can incorporate them into the OpenAPI specification document.
                options.IncludeXmlComments("ParksAPI.xml");

                // -----------------

            });
            */
            #endregion

            // -----------------------------

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {                                                                       // 7. Part 3
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // 5. Part 2
            // -----------------------------

            // 2- Enable middleware to serve generated Swagger as a JSON endpoint.
            //    The Instructor prefers to add this after UseHttpsRedirection() as that ensures that any call to a non-encrypted OpenAPI (Swagger) endpoint will be redirected to the encrypted version (Https protocol).

            app.UseSwagger();

            // -----------------------------

            // 7. Part 4
            // ------------------------------

            // Configure the Swagger UI Endpoints dynamically using the apiVersionDescriptionProvider.
            app.UseSwaggerUI(options => {

                foreach (var desc in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    // Add Swagger Json Endpoints.
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
                }

                // if we set that, we have to remove launchUrl in launchSettings.json file, Or comment the below line and set "launchUrl": "swagger" in launchSettings.json file.
                //options.RoutePrefix = "";

            });

            // ------------------------------

            // 5. Part 4
            // -----------------------------
            #region app.UseSwaggerUI(options)
            /*
            // To Create Documentation User Interface for the OpenAPI sepecification document, we use Swagger UI.

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), and also in the options we specify the Swagger JSON endpoint.
            app.UseSwaggerUI(options => {

                // specify the url of the Swagger documentation which is here "/swagger/ParksOpenAPISpec/swagger.json".
                options.SwaggerEndpoint("/swagger/ParksOpenAPISpec/swagger.json", "Parks API");
            */
                #region MultipleOAS 7. Part1
                /*
                options.SwaggerEndpoint("/swagger/ParksOpenAPISpecNP/swagger.json", "Parks API-NP");
                options.SwaggerEndpoint("/swagger/ParksOpenAPISpecTrails/swagger.json", "Parks API-Trails");
                */
                #endregion
            /*

                // if we set that, we have to remove launchUrl in launchSettings.json file, Or comment the below line and set "launchUrl": "swagger" in launchSettings.json file.
                //options.RoutePrefix = "";

            });
            */
            #endregion
            // -----------------------------

            app.UseRouting();

            // 12. Part 6
            // ---------------------

            #region CORS
            // Add support for cross origin resource sharing (CORS, also known as cross domain requests)
            // It is a mechanism that uses additional Http headers to tell a browser to give an application running at one origin access to resources from a different origin.
            // And we do that because the URLs for our Web API (Backend) and Web Application (Frontend) are different (they have different origins).
            // That Error "Cross-Origin Request Blocked" is happen when a request is send from the client (browser) directly to the Web API. That error doesn't happen if the browser call the web api through HttpClient.
            /*
            app.UseCors(configurePolicy =>
            {

                configurePolicy.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
            });
            */
            #endregion

            app.UseAuthentication(); // must before UseAuthorization().

            // ---------------------

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Call MapControllers inside UseEndpoints to map attribute routed controllers.
                endpoints.MapControllers();
            });
        }
    }
}
