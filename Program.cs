using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using EduGuide_Backend.Models;
using EduGuide_Backend.Services;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
namespace EduGuide_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            var builder = WebApplication.CreateBuilder(args);

            // Controllers
            builder.Services.AddControllers();

            // OpenAPI  
            builder.Services.AddOpenApi();

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(
                builder.Configuration.GetConnectionString("constr"));
            dataSourceBuilder.EnableUnmappedTypes();
            dataSourceBuilder.MapEnum<UserRole>("UserRole");
            dataSourceBuilder.MapEnum<SubscriptionTier>("SubscriptionTier");
            dataSourceBuilder.MapEnum<Gender>("Gender");
            dataSourceBuilder.MapEnum<AcademicYear>("AcademicYear");
            var dataSource = dataSourceBuilder.Build();

            builder.Services.AddDbContext<EgaidbContext>(options =>
                options.UseNpgsql(dataSource, o =>
                {
                    o.MapEnum<UserRole>("UserRole");
                    o.MapEnum<SubscriptionTier>("SubscriptionTier");
                    o.MapEnum<Gender>("Gender");
                    o.MapEnum<AcademicYear>("AcademicYear");
                }));

            builder.Services.AddScoped<IPasswordService, PasswordService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IProfileService, ProfileService>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
                    ValidAudience = builder.Configuration["JwtConfig:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Secret"]!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true

                };

            });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            try
            {
                using (var conn = new Npgsql.NpgsqlConnection(builder.Configuration.GetConnectionString("constr")))
                {
                    conn.Open();
                    using (var cmd = new Npgsql.NpgsqlCommand("SELECT enumlabel FROM pg_enum WHERE enumtypid = '\"UserRole\"'::regtype;", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"[DIAGNOSTIC] UserRole value: {reader.GetString(0)}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DIAGNOSTIC] Failed to query pg_enum: {ex.Message}");
            }

            app.Run();
        }
    }
}
