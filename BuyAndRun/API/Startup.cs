using API.Errors;
using API.Mapping;
using API.Policies;
using AutoMapper;
using BLL.Identity;
using BLL.Interfaces;
using BLL.Services;
using DAL;
using DAL.Contexts;
using DAL.Entities.Identity;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace API
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
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddDbContext<AuctionDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("DAL")));

            services
                .AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AuctionDbContext>()
                .AddUserManager<AuctionUserManager>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.Authority = "http://localhost:5000";
                o.Audience = "auction.api";
                o.RequireHttpsMetadata = false;
            });

            services.AddAuthorization(options =>
            {
                var authPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
                authPolicyBuilder = authPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = authPolicyBuilder.Build();

                options.AddPolicy("ModeratorOfAuction",
                    policy => policy.Requirements.Add(new ModeratorOfAuctionRequirement())
                );

                options.AddPolicy("EditPost",
                    policy => policy.Requirements.Add(new EditLotRequirement())
                );

                options.AddPolicy("EditThread",
                    policy => policy.Requirements.Add(new EditCategoryRequirement())
                );
            });

            #region uncomment for disabling model state filter
            //services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
            //{
            //    options.SuppressModelStateInvalidFilter = true;
            //});
            #endregion
            #region uncomment for SwaggerFluentValidation
            //services.AddMvc().AddFluentValidation(fv =>
            //{
            //    fv.RegisterValidatorsFromAssemblyContaining<AuctionResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<CreateAuctionResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<UpdateAuctionResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<RoleResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<CreateUserResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<UpdateUserResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<UserResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<CreatePostResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<PostResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<UpdatePostResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<CreateThreadResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<ThreadResourceValidator>();
            //    fv.RegisterValidatorsFromAssemblyContaining<UpdateThreadResourceValidator>();
            //}); 
            #endregion

            services.AddSwaggerGen(x =>
            {
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new string[] {}
                    }
                });
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "AuctionApp API" });
                //x.AddFluentValidationRules();
                //x.MapType<Microsoft.AspNetCore.Mvc.ProblemDetails>(() => new OpenApiSchema { });
            });

            services.AddAutoMapper(
                typeof(BLL.Mapping.MappingProfile),
                typeof(MappingProfile)
            );

            services.AddSingleton<IAuthorizationHandler, ModeratorOfAuctionHandler>();
            services.AddScoped<IAuthorizationHandler, EditLotHandler>();
            services.AddScoped<IAuthorizationHandler, EditCategoryHandler>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserService, UserSerivce>();
            services.AddTransient<ILotService, LotService>();
            services.AddTransient<IAuctionService, AuctionService>();
            services.AddTransient<ICategoryService, CategoryService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(options => options.AllowAnyHeader()
                                              .AllowAnyMethod()
                                              .AllowAnyOrigin());
            }
            else
            {
                //app.UseMiddleware<ExceptionHandler>();
                app.UseCors(options => options.AllowAnyHeader()
                                              .AllowAnyMethod()
                                              .WithOrigins("http://localhost:4200/")); // (e.g. https://mydomain.com)
            }

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuctionApp API");
            });

            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
