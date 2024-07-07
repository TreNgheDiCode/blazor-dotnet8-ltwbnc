using BaseLibrary.Helpers;
using BaseLibrary.Helpers.Client;
using Blazored.LocalStorage;
using ClientUser;
using ClientAdminLibrary.Helpers;
using ClientUserLibrary.Services.Contracts;
using ClientUserLibrary.Services.Implementations;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;
using ClientUserLibrary.Helpers;
using ClientUserLibrary.Services.CartService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddTransient<CustomHttpHandler>();
builder.Services.AddHttpClient("SystemApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7016");
    client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*"); // Thêm header này n?u c?n thi?t
}).AddHttpMessageHandler<CustomHttpHandler>();

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<GetHttpClient>();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<CartService>();


// Syncfusion
builder.Services.Configure<SyncfusionSection>(builder.Configuration.GetSection("Syncfusion"));
var syncfusionSection = builder.Configuration.GetSection(nameof(SyncfusionSection)).Get<SyncfusionSection>();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXlfdnRWRmNeWEB2X0U=");
builder.Services.AddSyncfusionBlazor();
builder.Services.AddScoped<SfDialogService>();

await builder.Build().RunAsync();
