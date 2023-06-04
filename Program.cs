using EduSpaceAPI.Controllers;
using EduSpaceAPI.Helpers;
using EduSpaceAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<JWTGenerator>();  
builder.Services.AddTransient<FileManager>();  
builder.Services.AddTransient<UserRepository>();  
builder.Services.AddTransient<DepartmentRepository>();  
builder.Services.AddTransient<ProgramRepository>();  
builder.Services.AddTransient<StudentRepository>();  
builder.Services.AddTransient<SessionRepository>();  

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("https://localhost:7282")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

///////////////////////////////////////////////////////////////////
// Configure JWT authentication
var tokenKey = Encoding.ASCII.GetBytes(builder.Configuration["JWT:SecretKey"]); // Replace with your own secret key
var Issuer = builder.Configuration["JWT:ValidIssuer"]; // Replace with your own secret key
var Audience = builder.Configuration["JWT:ValidAudience"]; // Replace with your own secret key
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true; // Set to true for production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = Audience,
        ValidIssuer = Issuer
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication(); // Add this before UseAuthorization()
app.UseAuthorization();

app.MapControllers();

app.Run();
