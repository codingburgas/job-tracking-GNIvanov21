using JobTracking.Application.Services;
using JobTracking.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// --- Add services to the container. ---

// 1. Add our custom services for dependency injection
builder.Services.AddSingleton<IDataService, FileDataService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();

// 2. Add controllers
builder.Services.AddControllers();

// 3. Configure CORS to allow our frontend to connect
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// 4. Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// --- Configure the HTTP request pipeline. ---

// Use Swagger for API documentation
app.UseSwagger();
app.UseSwaggerUI();

// Use the CORS policy we defined
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();