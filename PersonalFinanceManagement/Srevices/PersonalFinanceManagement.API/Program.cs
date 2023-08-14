using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PersonalFinanceManagement.API.Data;
using PersonalFinanceManagement.API.Swagger;
using PersonalFinanceManagement.BLL.Services;
using PersonalFinanceManagement.DAL.Context;
using PersonalFinanceManagement.DAL.Repositories;
using PersonalFinanceManagement.DAL.Repositories.Base;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces;
using PersonalFinanceManagement.Domain.Interfaces.Repository;
using PersonalFinanceManagement.Domain.Interfaces.Services;
using PersonalFinanceManagement.Interfaces.Base.Repositories;
using PersonalFinanceManagement.Interfaces.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
    };
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<PFMDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        parametr => parametr.MigrationsAssembly("PersonalFinanceManagement.DAL.SqlServer")));
builder.Services.AddTransient<PFMDbInitializer>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReportsService, ReportsService>();
builder.Services.AddScoped<IEntityService<WalletDTO, WalletCreateDTO, Wallet>, WalletService>();
builder.Services.AddScoped<ICategoryServise, CategoryService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigSwaggerOptions>();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
