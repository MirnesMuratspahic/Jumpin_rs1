using Jumpin.Context;
using Jumpin.Services;
using Jumpin.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AlarmSystem.Services.Interfaces;
using AlarmSystem.Services;
using Sentry.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IFlatService, FlatService>();
builder.Services.AddScoped<IErrorProviderService, ErrorProviderService>();
builder.Services.AddScoped<ITokenHelperService, TokenHelperService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IArtificialIntelligenceService, ArtificialIntelligenceService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<StripeService>();
builder.Services.AddScoped<PdfService>();
builder.Services.AddScoped<SmsService>();



builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UsersApiConnectionString")));

builder.Services.AddSingleton<Stripe.StripeClient>(provider =>
       new Stripe.StripeClient(builder.Configuration.GetValue<string>("Stripe:SecretKey")));

builder.Services.AddCors(options => options.AddPolicy("AllowAnyOrigin",
        builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));

builder.WebHost.UseSentry(options =>
{
    options.Dsn = "https://9db8fac7d0c46e7b529756eb33b15442@o4508585794338816.ingest.de.sentry.io/4508585797353552";
    options.Debug = true;
    options.TracesSampleRate = 1.0;
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!))
            };
        });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
    options.AddPolicy("DevicePolicy", policy => policy.RequireRole("Device"));
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSentryTracing();

app.MapDefaultControllerRoute();

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
