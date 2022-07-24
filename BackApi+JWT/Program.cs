using BackApi_JWT;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Aqui estamos obteniendo la información de nuestra secretkey en appsettings
builder.Configuration.AddJsonFile("appsettings.json");
var secretkey = builder.Configuration.GetSection("settings").GetSection("secretkey").ToString();
var keyBytes = Encoding.UTF8.GetBytes(secretkey);

//Configuración de nuetro servicio para JWT
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false; //por el momento false ya que no tenemos certificado en el hosting
    config.SaveToken = true; // ¡Importante!

    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, //aqui estamos haciendo una validación por usuario cada vez que esta hago login
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false, //false debido que el usuario esta obteniendo la informacion solo con sus credenciales en nuestro ejemplo de api
        ValidateAudience = false
    };

});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//Agregamos el servicio de autorización a nuestro swagger, de este modo con el token podemos hacer recursos protegidos
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});



//servicio StringConnection
builder.Services.AddDbContext<CoreWebApiContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

//servicio para ignorar la referencia ciclica 
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

//servicio configuracion de los cors
//añadimos una politica como servicio en el builder que nos permite cualquier orige, cabecera y método que haga una solicitud a nuestra api
var misReglarCors = "CorsRules";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: misReglarCors, builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();



// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

//}

app.UseSwagger();
app.UseSwaggerUI();

//Aquí activamos nuestra politica cors
app.UseCors(misReglarCors);

//Aquí activamos nuestra servicio JWT
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
