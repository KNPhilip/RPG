global using System.Text.Json.Serialization;
global using Microsoft.AspNetCore.Mvc;
global using dotNET7.Models;
global using dotNET7.Dtos;
global using dotNET7.Dtos.Character;
global using dotNET7.Dtos.User;
global using dotNET7.Services.CharacterService;
global using dotNET7.Services.AuthService;
global using AutoMapper;
global using Microsoft.EntityFrameworkCore;
global using dotNET7.Data;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using Microsoft.IdentityModel.Tokens;
global using System.Text;
global using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<RPGContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();