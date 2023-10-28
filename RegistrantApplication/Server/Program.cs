using System.Reflection;
using Mapster;
using MapsterMapper;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<LiteContext>();
builder.Services.AddSwaggerGen();
builder.Services.AddMapster();

var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
// scans the assembly and gets the IRegister, adding the registration to the TypeAdapterConfig
typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());
// register the mapper as Singleton service for my application
var mapperConfig = new Mapper(typeAdapterConfig);

builder.Services.AddSingleton<IMapper>(mapperConfig);

/*builder.Configuration.AddJsonFile("appsettings.json");*/
//builder.WebHost.UseUrls("https://*:8080", "http://*:8080");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    
    app.UseSwagger();
    app.UseSwaggerUI(option =>
    {
        //option.RoutePrefix = "api/developer";
    });
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

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
