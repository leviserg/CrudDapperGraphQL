using CrudDapperGraphQL.AppStart;
using CrudDapperGraphQL.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container => Custom services

ServiceRegistration.AddDependentServices(builder.Services);

builder.Services.Configure<ApiUser>(builder.Configuration.GetSection("ApiServiceAccount"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var app = builder.Build();

await ApplicationSetup.InitializeOptionsAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGraphQL("/graphql");

app.Run();
