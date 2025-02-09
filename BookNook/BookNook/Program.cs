using System.Text;
using BookNook.Client.Pages;
using BookNook.Client.services;
using BookNook.Components;
using BookNook.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();


builder.Services.AddScoped(sp =>
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    var client = new HttpClient
    {
        BaseAddress = new Uri(navigationManager.BaseUri)
    };
    return client;
});



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login"; 
        options.AccessDeniedPath = "/access-denied"; 
    });


//identity
builder.Services.AddDbContext<BookNookDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("BookNookContext") ?? throw new InvalidOperationException("Connection string 'BookNookContext' not found."));
});
builder.Services.AddDataProtection();
builder.Services.AddIdentityCore<BookNookUser>(opt =>
{
    opt.Password.RequiredLength = 6;
    opt.Password.RequireDigit = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = true;
    opt.Tokens.PasswordResetTokenProvider=TokenOptions.DefaultEmailProvider;
    opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
});
IdentityBuilder identityBuilder = new IdentityBuilder(typeof(BookNookUser), typeof(BookNookRole),builder.Services);
identityBuilder.AddEntityFrameworkStores<BookNookDbContext>().AddDefaultTokenProviders().AddUserManager<UserManager<BookNookUser>>()
    .AddRoleManager<RoleManager<BookNookRole>>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<UserStateService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseRouting();


 app.UseAuthentication();
 app.UseAuthorization();


app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BookNook.Client._Imports).Assembly);

app.Run();
