using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ParkiAPI.Data;
using ParkiAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ParkiAPI.Mapper;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ParkiAPI
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


            //inject CORS
            services.AddCors();
            //inject datacontect class and provide it with DB conn string
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //inject trail respository
            services.AddScoped<ITrailRepository, TrailRepository>();
            //inject Nationap Park reporsitory
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            //Add Automapper
            services.AddAutoMapper(typeof(ParkiMapping));

            //Adding versioning
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;

            });

            //Moving out the API Documentation insted of repeating AddSwaggerGen
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            //Lets Add or inject the swagger documentation object 
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            //Lets call swagger documentaiton generation
            services.AddSwaggerGen();

           var _appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(_appSettingsSection);

            var _appSettings = _appSettingsSection.Get<AppSettings>();
            var _SecretKey = Encoding.ASCII.GetBytes(_appSettings.Secret);

            services.AddAuthentication(options=> 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(B=> 
                {
                    B.RequireHttpsMetadata = false;
                    B.SaveToken = true;
                    B.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey= new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(_SecretKey),
                        ValidateIssuer=false,
                        ValidateAudience=false
                    
                    };
                
                });

            //https://github.com/microsoft/aspnet-api-versioning/blob/master/samples/webapi/SwaggerWebApiSample/Startup.cs

            //Setup Api documentation
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo
            //    {
            //        Title = "ParkiAPI",
            //        Version = "v1",
            //        Description = "Parki WebApi",
            //        Contact = new OpenApiContact()
            //        {
            //            Email = "test@mail.com",
            //            Name = "Param",
            //            Url = new Uri("http://somewebaddres.co.uk")
            //        }
            //    });

            //    //use the VS XML generation file to describe each endpoint
            //    //to access rest path in project properties under build XML Documentation File
            //    var XMLCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
            //    var XMLCommentFullPath = Path.Combine(AppContext.BaseDirectory, XMLCommentFile);
            //    c.IncludeXmlComments(XMLCommentFullPath);
            //});


            //https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-5.0&tabs=visual-studio
            //https://docs.microsoft.com/en-us/azure/active-directory-b2c/secure-rest-api?tabs=windows&pivots=b2c-user-flow
            //Configure JSON serialization options
            //services.AddControllers()
            //            .AddNewtonsoftJson(options => options.UseMemberCasing());  ///////////GOING TO IT TO TEST ONCE ALL TEST I WANT TO CARRY ON ARE DONE
            //https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-5-0
            //Example use
            //With the preceding change, property names in the web API's
            //serialized JSON response match their corresponding property
            //names in the CLR object type. For example, the Book class's Author
            //property serializes as Author.
            // In Models/ Book.cs, annotate the BookName property with the following[JsonProperty] attribute:

            //[BsonElement("Name")]
            //[JsonProperty("Name")]
            //public string BookName { get; set; }
            services.AddControllers();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                //Configure route for dinnferent versions and its documentations
                app.UseSwaggerUI(c =>
                {
                    foreach (var desc in provider.ApiVersionDescriptions)
                        c.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());

                    //we are setting this 
                    //so we can define and 
                    //control version and route.
                    //c.RoutePrefix = "";
                });
                // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ParkiAPI v1"));

                //Vershoning
                //app.UseSwaggerUI(options=> {
                //    options.SwaggerEndpoint("/swagger/ParkyOpenAPISpec/swagger.json", "Parky API");
                //    //options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecTrails/swagger.json", "Parky API Trails");
                //    options.RoutePrefix = "";
                //});
            }



            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());                
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
