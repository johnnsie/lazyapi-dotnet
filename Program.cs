var builder = WebApplication.CreateBuilder(args);

// Define a named CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()       // Allow any origin (use WithOrigins("http://localhost:3000") to restrict)
              .AllowAnyMethod()       // Allow GET, POST, etc.
              .AllowAnyHeader();      // Allow all headers
    });
});

builder.Services.AddControllers();
builder.Services.AddHttpClient(); // 👈 this is key
var app = builder.Build();
// Apply the CORS policy globally
app.UseCors("AllowAll");
app.MapControllers();
app.Run();
