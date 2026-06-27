using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Session1;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");//bring in the connection string from appsettings.json
//TESTINGGGGGGG PLS WORK
Console.WriteLine("Connection String:");
Console.WriteLine(connectionString);

//means that we are configuring the DbContext to use MySQL as the database provider
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.Parse("8.0.36-mysql")));


//means whenever a controller is created, the BooksAuthorizationConvention will be applied to it. 

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new BooksAuthorizationConvention());
});
builder.Services.AddEndpointsApiExplorer();//discovers all api routes

//this tells swagger that the API uses JWT authentication

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    //so when calling some protected endpoint swagger should send the JWT token in the Authorization header

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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,//who created the token
            ValidateAudience = true,  //who the token is intended for (matlab issi api ke liye hai na?)
            ValidateLifetime = true, //check if the token is expired
            ValidateIssuerSigningKey = true, //check if the token signature is valid, agar kisi ne token ko modify kiya hai to signature invalid ho jayega
            ValidIssuer = builder.Configuration["Jwt:Issuer"], 
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

//as maam said first authentication is checked and then authorization is checked

builder.Services.AddAuthorization(options =>
{
    //fallback policy means ke protect evry endpoint by default, unless explicitly specified otherwise
    // so if a controller or action does not have any authorization attributes this fallback policy will be applied to it
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddScoped<IBookService, BookService>();//dependency injection for ibookservies

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<BookNotFoundExceptionHandler>();

var app = builder.Build();

//second part : middleware and seeding

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
