using Backend_API;
using Backend_API.DBContext;
using Backend_API.Repositories;
using Backend_API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the database context with dependency injection
builder.Services.AddDbContext<INotesDBContext, NotesDBContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

// Register repositories and services as scoped for each HTTP request
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFolderRepository, FolderRepository>();
builder.Services.AddScoped<IFolderService, FolderService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    // Sets the default authentication scheme to JWT Bearer
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Compares the issuer in the token with the configured issuer
        ValidateAudience = true, // Compares the audience in the token with the configured audience
        ValidateLifetime = true, // Checks if the token is expired
        ValidateIssuerSigningKey = true, // Ensures the signature is valid by recomputing it with the signing key

        // What the API will validate against
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],

        // Signing key to validate
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ClockSkew = TimeSpan.FromMinutes(1) // A buffer for token expiration. This means if a device clock is behind real time, the token may still be considered valid and count 5 minutes for that
    };
});

// Configure swagger authentication
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header, // Specifies that the token should be passed in the header of the request
        Description = "Please enter a valid token",
        Name = "Authorization", // Name of the header
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT", // Bearer type should be JWT
        Scheme = "bearer" // Scheme should be bearer token
    });

    // Adds the security requirement to the swagger documentation
    // This means that swagger documents that all endpoints require authentication
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(cfg =>
    {
        // Setup localhost 5173 too for future development. Just remember to add it in compose file as well
        cfg.WithOrigins("https://notes.filipalberg.dk", "http://localhost:5173")
           .AllowAnyMethod()
           .AllowAnyHeader();
    });
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(); // Apply CORS policy

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
