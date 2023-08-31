global using Microsoft.AspNetCore.Components.Authorization;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
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
builder.Services.AddApi<IUsersWebApiClient, UsersWebApiClient>("api/Users/");
builder.Services.AddApi<IEntitiesWebApiClient<WalletDTO, WalletCreateDTO>, WalletsWebApiClient>("api/Wallets/")
    .AddHttpMessageHandler<AuthorizationMessageHandler>();
builder.Services.AddApi<ICategoriesWebApiClient, CategoriesWebApiClient>("api/Categories/")
    .AddHttpMessageHandler<AuthorizationMessageHandler>();
builder.Services.AddApi<ITransactionsWebApiClient, TransactionsWebApiClient>("api/Transactions/")
    .AddHttpMessageHandler<AuthorizationMessageHandler>();
builder.Services.AddApi<IReportsWebApiClient, ReportsWebApiClient>("api/Reports/")
    .AddHttpMessageHandler<AuthorizationMessageHandler>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();
