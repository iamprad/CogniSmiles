using Microsoft.EntityFrameworkCore;
using CogniSmiles.Data;
using CogniSmiles.Interfaces;
using CogniSmiles.Services;
using CogniSmiles.Models;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
// Add services to the container.

builder.Services.AddRazorPages();
builder.Services.AddDbContext<CogniSmilesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CogniSmilesContext") ?? throw new InvalidOperationException("Connection string 'CogniSmilesContext' not found.")));

builder.Services.AddTransient<IFileUploadService, FileUploadLocalService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(15);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 629145600;
    options.Limits.MaxRequestBufferSize= 629145600;
});

builder.Services.Configure<FormOptions>(options => options.MultipartBodyLengthLimit = 629145600);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();
app.MapDefaultControllerRoute();
app.Run();
