using CrudDapperGraphQL.AppStart;
using CrudDapperGraphQL.Auth;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container => Custom services

ServiceRegistration.AddDependentServices(builder.Services);

ServiceRegistration.AddGlobalErrorHandling(builder.Services);

string apiAuthJsonConfig = Environment.GetEnvironmentVariable("BookLibraryServiceAccount") ?? string.Empty;
var apiAuthConfiguration = ApiAuthConfigBuilder.Build(apiAuthJsonConfig);
builder.Services.Configure<ApiAuthModel>(apiAuthConfiguration);



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var app = builder.Build();


app.InitializeAuthOptions();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseGlobalErrorHandling();
// app.UseHsts();

// app.UseHttpsRedirection();

app.UseRouting();
// app.UseCors();

app.UseHealthChecks("/_health", new HealthCheckOptions
{
    ResponseWriter = app.Environment.IsDevelopment() ? UIResponseWriter.WriteHealthCheckUIResponse : UIResponseWriter.WriteHealthCheckUIResponseNoExceptionDetails
});

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.MapGraphQL("/graphql");

app.Run();
