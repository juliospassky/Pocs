using Application.Configs;
using Application.Validators.Filters;
using Domain.Services;
using Domain.Services.Interfaces;
using FluentValidation.AspNetCore;
using Infrastructure.Database.Config;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(o => o.SuppressMapClientErrors = true)
    .AddJsonOptions(jsonOpts =>
    {
        jsonOpts.JsonSerializerOptions.PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance;
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddMvc(options =>
//{
//    options.Filters.Add<ValidationFilter>();
//})
//    .AddFluentValidation(options =>
//    {
//        // Validate child properties and root collection elements
//        options.ImplicitlyValidateChildProperties = true;
//        options.ImplicitlyValidateRootCollectionElements = true;
//        // Automatic registration of validators in assembly
//        options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
//    });

builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection("MongoDb"));
builder.Services.AddSingleton<ITodoService, TodoService>();
builder.Services.AddMvc(o =>
    {
        o.EnableEndpointRouting = false;
        o.Filters.Add<ValidationFilter>();
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    })
    .AddFluentValidation(options =>
    {
        // Validate child properties and root collection elements
        options.ImplicitlyValidateChildProperties = true;
        options.ImplicitlyValidateRootCollectionElements = true;
        // Automatic registration of validators in assembly
        options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
