using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.API.Data;
using PersonalFinanceManagement.BLL.Services;
using PersonalFinanceManagement.DAL.Context;
using PersonalFinanceManagement.DAL.Repositories;
using PersonalFinanceManagement.DAL.Repositories.Base;
using PersonalFinanceManagement.Domain.Interfaces;
using PersonalFinanceManagement.Interfaces.Base.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<PFMDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("PersonalFinanceManagement.DAL.SqlServer")));
builder.Services.AddTransient<PFMDbInitializer>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(DbRepositoryBase<>));
builder.Services.AddScoped<ITransactionRepository, DbTransactionRepository>();
builder.Services.AddScoped<IReportsService, ReportsService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PFMDbInitializer>();
    db.Initialize();
}

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
