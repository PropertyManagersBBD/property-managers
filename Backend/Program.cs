using Backend.Database.Context;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Threading.RateLimiting;

namespace Backend
{
	public class Program
	{
		private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
		{
			builder
			.AddConsole()
			.AddFilter((category, level) =>
			category == DbLoggerCategory.Database.Command.Name && level >= LogLevel.Warning);
		});

		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllers();

			// Register the service(s)
			builder.Services.AddScoped<IPropertyManagerService, PropertyManagerService>();

			// Add the DB and connection string
			var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
			builder.Services.AddDbContext<PropertyManagerContext>(options =>
				options.UseLoggerFactory(_loggerFactory).UseSqlServer(connectionString));
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(c =>
				{
					c.SwaggerDoc("v1", new OpenApiInfo
					{
						Version = "v1",
						Title = "Property Manager API",
						Description = "Documentation for the Property Manager API",
					});
					//c.DocumentFilter<SwaggerBaseUrlDocumentFilter>("https://localhost:7147");

					var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
					var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
					c.IncludeXmlComments(xmlPath);
				});

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin()
                                            .WithMethods("GET", "POST") // Only allows GET and POST methods
                                            .AllowAnyHeader();
                                  });
            });

			// Rate limiting
			var rateLimitingPolicyName = "fixed";
			builder.Services.AddRateLimiter(_ => _
			.AddFixedWindowLimiter(policyName: rateLimitingPolicyName, options =>
			{
				options.PermitLimit = 25;
				options.Window = TimeSpan.FromSeconds(1);
				options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
				options.QueueLimit = 50;
			}));

			// Authentication
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.Authority = Environment.GetEnvironmentVariable("COGNITO_PROPMAN_Authority");

					options.Audience = Environment.GetEnvironmentVariable("COGNITO_PROPMAN_ClientId");

					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidIssuer = Environment.GetEnvironmentVariable("COGNITO_PROPMAN_Authority"),

						ValidateAudience = true,
						ValidAudience = Environment.GetEnvironmentVariable("COGNITO_PROPMAN_ClientId"),

						ValidateIssuerSigningKey = true,
						IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
						{
							// Fetch the JSON Web Key Set (JWKS) from the authority and find the matching key.
							var jwks = GetJsonWebKeySetAsync().GetAwaiter().GetResult();
							return jwks.Keys.Where(k => k.KeyId == kid);
						},
						ValidateLifetime = true
					};
				});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if(app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();

			app.UseAuthentication(); // Ensure this comes before UseAuthorization
			app.UseAuthorization();
			// Rate limiting
			app.UseRateLimiter();

			app.MapControllers().RequireRateLimiting(rateLimitingPolicyName);

			app.Run();

			async Task<JsonWebKeySet> GetJsonWebKeySetAsync()
			{
				var authority = Environment.GetEnvironmentVariable("COGNITO_PROPMAN_Authority");
				using(var httpClient = new HttpClient())
				{
					var response = await httpClient.GetStringAsync($"{authority}/.well-known/jwks.json");
					return new JsonWebKeySet(response);
				}
			}
		}
	}
}
