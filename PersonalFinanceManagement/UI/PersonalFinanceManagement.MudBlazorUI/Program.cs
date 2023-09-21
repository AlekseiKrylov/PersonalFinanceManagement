global using Microsoft.AspNetCore.Components.Authorization;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;
using PersonalFinanceManagement.Interfaces.WebApiClients;
using PersonalFinanceManagement.MudBlazorUI;
using PersonalFinanceManagement.MudBlazorUI.Infrastructure.Extensions;
using PersonalFinanceManagement.MudBlazorUI.Servises;
using PersonalFinanceManagement.WebAPIClients.Clients;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<AuthorizationMessageHandler>();
builder.Services.AddAuthorizationCore();
builder.Services.AddApi<IUsersWebApiClient, UsersWebApiClient>("api/users/");
builder.Services.AddApi<IEntitiesWebApiClient<WalletDTO, WalletCreateDTO>, WalletsWebApiClient>("api/wallets/")
    .AddHttpMessageHandler<AuthorizationMessageHandler>();
builder.Services.AddApi<ICategoriesWebApiClient, CategoriesWebApiClient>("api/categories/")
    .AddHttpMessageHandler<AuthorizationMessageHandler>();
builder.Services.AddApi<ITransactionsWebApiClient, TransactionsWebApiClient>("api/transactions/")
    .AddHttpMessageHandler<AuthorizationMessageHandler>();
builder.Services.AddApi<IReportsWebApiClient, ReportsWebApiClient>("api/reports/")
    .AddHttpMessageHandler<AuthorizationMessageHandler>();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
});

await builder.Build().RunAsync();
