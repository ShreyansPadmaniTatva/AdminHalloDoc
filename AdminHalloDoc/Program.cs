using AdminHalloDoc.Entities;
using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Repositories.Admin.Repository;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(
options =>
{
    options.IOTimeout = TimeSpan.FromSeconds(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

});

var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();

builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IViewActionRepository, ViewActionRepository>();
builder.Services.AddScoped<IViewNotesRepository, ViewNotesRepository>();
builder.Services.AddScoped<IMyProfileRepository, MyProfileRepository>();
builder.Services.AddScoped<IPhysicianRepository, PhysicianRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseSession();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AdminLogin}/{action=Index}/{id?}");

app.Run();
