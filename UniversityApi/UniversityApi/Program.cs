using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using UniversityApi.Data;
using UniversityApi.Dtos;
using Services.Services.Implementations;
using Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Services.Exceptions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("~/wwwroot/logs/myapp.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();




builder.Services.AddDbContext<UniDatabase>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
        sqlOptions => sqlOptions.MigrationsAssembly("UniversityApi"));
});




builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateDto>();

builder.Services.AddFluentValidationRulesToSwagger();


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IStudentService, StudentService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<SerilogMiddleware>();

app.Run();
