using DatingApplication.Data;
using DatingApplication.Entities;
using DatingApplication.Helper;
using DatingApplication.Interfaces;
using DatingApplication.Middleware;
using DatingApplication.SignalR;
using DatingApplication.TokenServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApplicationDbContext>(opt=>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddScoped<ITokenService, TokenServices>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<LogUserActivity>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSignalR(options => options.EnableDetailedErrors = true);
builder.Services.AddSingleton<PresenceTracker>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySetting"));
builder.Services.AddScoped<IPhotoServices,PhotoServices>();
builder.Services.AddIdentityCore<AppUser>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
})
    .AddRoles<AppRole>()
    .AddRoleManager<RoleManager<AppRole>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("RequiredAdminRole", policy => policy.RequireRole("Admin"));
    opt.AddPolicy("ModeratorPhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt => {
                    opt.TokenValidationParameters = new TokenValidationParameters() 
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };

                    opt.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.


app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(builder=> builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200"));
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManger = services.GetRequiredService<UserManager<AppUser>>();
    var roleManger = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [Connections]");
    await Seed.SeedUsers(userManger, roleManger);
}
catch (Exception ex)
{

    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occurrd during migration");
}

app.Run();
