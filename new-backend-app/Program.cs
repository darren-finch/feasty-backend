using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using new_backend.Data;
using new_backend.Middleware;
using new_backend.Repositories;
using new_backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDbContext<FeastyDbContext>(options => options.UseSqlite("Data Source=feasty.db"));
builder.Services.AddScoped<IAuthManager, AuthManager>(serviceProvider =>
{
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    return new AuthManager(httpContextAccessor);
});
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<IMealsRepository, MealsRepository>();
builder.Services.AddScoped<IMealsService, MealsService>();
builder.Services.AddScoped<IMealPlanMealsRepository, MealPlanMealsRepository>();
builder.Services.AddScoped<IMealPlanMealsService, MealPlanMealsService>();
builder.Services.AddScoped<IMealPlansRepository, MealPlansRepository>();
builder.Services.AddScoped<IMealPlansService, MealPlansService>();

// Add authentication and authorization
var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = domain;
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UsePathBase("/api");

app.UseAuthentication();
app.UseAuthorization();

app
  .MapControllers()
  .RequireAuthorization();

app.Run();

