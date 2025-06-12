using System.Text;
using ConsoleApp1.Data;
using ConsoleApp1.Data.Interfaces;
using ConsoleApp1.Data.Repositories;
using MediaApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var jwtKey = "MediaApiTokenGeneration123456789@";
var jwtIssuer = "AuthAPI";
var jwtAudience = "BlazorApp";

var builder = WebApplication.CreateBuilder(args);

// Configuration de la chaine de connexion à la base de données
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Injection des dépendances
builder.Services.AddSingleton<IDbConnectionFactory>(new SqlConnectionFactory(connectionString!));
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IMediaRepository, MediaRepository>();
builder.Services.AddScoped<IFolderRepository, FolderRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IFolderService, FolderService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, SmtpEmailService>();

builder.Services.AddSwaggerGen(options =>
{
    // Informations sur l'API
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Media API With Authentication",
        Version = "v1",
        Description = "Api for Media Management and Group"
    });

    // Ajout du schéma JWT Bearer
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Entrez le token JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configurer JWT Authentification
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});
builder.Services.AddAuthorization();


builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressInferBindingSourcesForParameters = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "uploads")
    ),
    RequestPath = "/uploads"
});

app.MapGet("/", () => "Bienvenue sur l'API de gestion de médias.");

app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthentication();
app.UseAuthorization();

// Ajout des routes des contrôleurs
app.MapControllers();

app.Run();

