using AdminHalloDoc.Entities;
using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Repositories.Admin.Repository;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using AdminHalloDoc.Repositories.Patient.Repository;
using AdminHalloDoc.Repositories.Patient.Repository.Interface;
using Rotativa.AspNetCore;
using System.Net;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

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

var smsConfig = builder.Configuration
        .GetSection("SmsConfiguration")
        .Get<SmsConfiguration>();

builder.Services.AddSingleton(emailConfig);
builder.Services.AddSingleton(smsConfig);
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
//Admin Repository
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IViewActionRepository, ViewActionRepository>();
builder.Services.AddScoped<IViewNotesRepository, ViewNotesRepository>();
builder.Services.AddScoped<IMyProfileRepository, MyProfileRepository>();
builder.Services.AddScoped<IPhysicianRepository, PhysicianRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRoleAccessRepository, RoleAccessRepository>();
builder.Services.AddScoped<ISchedulingRepository, SchedulingRepository>();
builder.Services.AddScoped<IRecordsRepository, RecordsRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

//Patient Repository
builder.Services.AddScoped<IPatientDashboardRepository, PatientDashboardRepository>();
builder.Services.AddScoped<IPatientRequestRepository, PatientRequestRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePages(context => {
    var request = context.HttpContext.Request;
    var response = context.HttpContext.Response;

    if (response.StatusCode == 404)
    {
        response.Redirect("/PageNoteFound");
    }
   
    return Task.CompletedTask;
});

//app.UseHttpsRedirection();
app.UseSession();
app.UseStaticFiles();
app.UseRotativa();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
