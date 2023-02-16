using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Radzen;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;
using CloudDevOpsProject1.Server.Data;
using Microsoft.AspNetCore.Identity;
using CloudDevOpsProject1.Server.Models;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddSingleton(sp =>
{
    // Get the address that the app is currently running at
    var server = sp.GetRequiredService<IServer>();
    var addressFeature = server.Features.Get<IServerAddressesFeature>();
    string baseAddress = addressFeature.Addresses.First();
    return new HttpClient{BaseAddress = new Uri(baseAddress)};
});
builder.Services.AddScoped<CloudDevOpsProject1.Server.DevOps_Proj_DatabaseService>();
builder.Services.AddDbContext<CloudDevOpsProject1.Server.Data.DevOps_Proj_DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevOps_Proj_DatabaseConnection"));
});
builder.Services.AddControllers().AddOData(opt =>
{
    var oDataBuilderDevOps_Proj_Database = new ODataConventionModelBuilder();
    oDataBuilderDevOps_Proj_Database.EntitySet<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee>("Employees");
    oDataBuilderDevOps_Proj_Database.EntitySet<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory>("Inventories");
    oDataBuilderDevOps_Proj_Database.EntitySet<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager>("Managers");
    oDataBuilderDevOps_Proj_Database.EntitySet<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part>("Parts");
    oDataBuilderDevOps_Proj_Database.EntitySet<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant>("Plants");
    oDataBuilderDevOps_Proj_Database.EntitySet<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position>("Positions");
    oDataBuilderDevOps_Proj_Database.EntitySet<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable>("TestTables");
    oDataBuilderDevOps_Proj_Database.EntitySet<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2>("TestTable2S");
    oDataBuilderDevOps_Proj_Database.EntitySet<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor>("Vendors");
    opt.AddRouteComponents("odata/DevOps_Proj_Database", oDataBuilderDevOps_Proj_Database.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddScoped<CloudDevOpsProject1.Client.DevOps_Proj_DatabaseService>();
builder.Services.AddHttpClient("CloudDevOpsProject1.Server").AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddScoped<CloudDevOpsProject1.Client.SecurityService>();
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevOps_Proj_DatabaseConnection"));
});
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationIdentityDbContext>().AddDefaultTokenProviders();
builder.Services.AddControllers().AddOData(o =>
{
    var oDataBuilder = new ODataConventionModelBuilder();
    oDataBuilder.EntitySet<ApplicationUser>("ApplicationUsers");
    var usersType = oDataBuilder.StructuralTypes.First(x => x.ClrType == typeof(ApplicationUser));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.Password)));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.ConfirmPassword)));
    oDataBuilder.EntitySet<ApplicationRole>("ApplicationRoles");
    o.AddRouteComponents("odata/Identity", oDataBuilder.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddScoped<AuthenticationStateProvider, CloudDevOpsProject1.Client.ApplicationAuthenticationStateProvider>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseHeaderPropagation();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToPage("/_Host");
app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>().Database.Migrate();
app.Run();