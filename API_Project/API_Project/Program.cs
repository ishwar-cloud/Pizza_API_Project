using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using API_Project.DataCollection;
using API_Project.Services;
using System.Text;
using Microsoft.OpenApi.Models;
using API_Project.Models;
using API_Project.Interface;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

//adding Dependancy Scope
builder.Services.AddScoped<IJwtToken, JwtToken>(); // for JWT Interface and Class

builder.Services.AddScoped<IUserService, UserService>(); // for User interface and its class

builder.Services.AddScoped<IManagerService, ManagerService>(); // For manager Interface and its class

builder.Services.AddTransient<IMongoDbContext, MongoDbContext>(); // For MongoDb interface and Its class 

builder.Services.AddTransient<IInserDataToDB, InsertDataToDb>(); // for Insertdata interface and its class




//configuring a MongoDB setting
builder.Services.Configure<MongoDBSetting>(

        builder.Configuration.GetSection("MongoDB")
);


// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{

    // define the API version and metadata.   
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "API_Project", Version = "v1" });
    
    // Adding Security Defination
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

    //Adding  security requirements 
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        //Create new object
         {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    } 
                },
                 new string[]{}
                    
         }
    });

});

// Authentication of JWT Token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
var app = builder.Build();

//call InsertDataToMenu method
var insertData = app.Services.GetRequiredService<IInserDataToDB>();
insertData.InsertDataToMenu();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API_Project v1"));
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
