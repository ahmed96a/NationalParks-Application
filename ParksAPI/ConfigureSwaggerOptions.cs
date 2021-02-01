using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParksAPI
{
    // 7. Part 3
    // ----------------------------------

    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        // this interface is belong to Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer Nuget Package that we install
        // That instance gets the information that describe the discovered API versions information within our application.

        private readonly IApiVersionDescriptionProvider apiVersionDescriptionProvider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            this.apiVersionDescriptionProvider = apiVersionDescriptionProvider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            // apiVersionDescriptionProvider.ApiVersionDescriptions: Gets a read-only list of discovered API version descriptions.

            // Create the swagger documents dynamically based on the the current api versions that we have. (Currently we will have 2 versions, then we will have 2 documents)
            foreach (var desc in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(desc.GroupName, new OpenApiInfo() 
                {
                    Title = $"Parks API {desc.ApiVersion}",
                    Version = desc.ApiVersion.ToString()
                });

                // Tell Swashbuckle where it can find Xml Comments so it can incorporate them into the OpenAPI specification document.
                //options.IncludeXmlComments("ParksAPI.xml");
            }


            // 12. Part 10
            // ------------------------------------------

            // Add support for JWT token in swagger

            // That will be a pop up that we will see once we click on the authorization button.
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {

                Description = "JWT Authorization header using the Bearer scheme." +
                "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below." +
                "\r\n\r\n Example: \"Bearer 12345abcdf\"",

                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"

            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement() {

                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }

            });

            // ------------------------------------------

        }
    }

    // ----------------------------------
}
