using Backend.Database.Context;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

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
					
					var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
					var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
					c.IncludeXmlComments(xmlPath);
				});


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if(app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
