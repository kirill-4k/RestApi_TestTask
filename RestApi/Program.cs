using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PatientDB;
using RestApi.Contracts;
using RestApi.DataAccess;
using RestApi.Filters;
using RestApi.Service;
using System.Configuration;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
	options.Filters.Add<HttpResponseExceptionFilter>();
})
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
		options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
	});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = ".Net Test Api",
		Description = "Test api for patients"
	});
	options.EnableAnnotations();
});
builder.Services.AddScoped<IPatientMapping, DomainDtoMapping>();
builder.Services.AddScoped<IService, Service>();
builder.Services.AddScoped<IDataAccess, DataAccess>();

var connectionString = builder.Configuration.GetConnectionString("PersonDB");
builder.Services.AddDbContext<PatientContext>(options => options.UseSqlServer(connectionString)) ;





var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<PatientContext>();
	context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
	});
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
