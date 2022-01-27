using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ParkiAPI
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {

        readonly IApiVersionDescriptionProvider provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            this.provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var desc in provider.ApiVersionDescriptions)
            {

                options.SwaggerDoc(desc.GroupName,
                new OpenApiInfo
                {
                    Title = $"ParkiAPI {desc.ApiVersion}",
                    Version = desc.ApiVersion.ToString(),
                    Description = "Parki WebApi",
                    Contact = new OpenApiContact()
                    {
                        Email = "test@mail.com",
                        Name = "Param",
                        Url = new Uri("http://somewebaddres.co.uk")
                    }
                });

            }


            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
              "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
              "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
              "Example: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
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
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

            //use the VS XML generation file to describe each endpoint
            //to access rest path in project properties under build XML Documentation File
            var XMLCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
            var XMLCommentFullPath = Path.Combine(AppContext.BaseDirectory, XMLCommentFile);
            options.IncludeXmlComments(XMLCommentFullPath);
        }
    }
}
