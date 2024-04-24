using WhiteBlackList.Web.Filters;
using WhiteBlackList.Web.Midlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// IPList Sýnýfý ile appsettings.json içerisindeki IPList objesini birleþtir.
builder.Services.Configure<IPList>(builder.Configuration.GetSection("IPList"));


builder.Services.AddScoped<CheckWhiteList>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


//app.UseMiddleware<IPSafeMiddleware>();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
