using CrudDapperGraphQL.AppStart;
using CrudDapperGraphQL.Auth;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container => Custom services

ServiceRegistration.AddDependentServices(builder.Services);

string apiUserJsonConfig = Environment.GetEnvironmentVariable("BookLibraryServiceAccount");
var apiUserConfiguration = ApiUserConfigBuilder.Build(apiUserJsonConfig);
builder.Services.Configure<ApiUser>(apiUserConfiguration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var app = builder.Build();

await ApplicationSetup.InitializeOptionsAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGraphQL("/graphql");

app.Run();
