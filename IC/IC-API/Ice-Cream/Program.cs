using Ice_Cream.DB;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod())
);

builder.Services.AddControllers(); // Add controllers

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework with MySQL
var connectionString = builder.Configuration.GetConnectionString("MySqlConn");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Configure HttpClient for PayPal
builder.Services.AddHttpClient("PayPal", client =>
{
    var config = builder.Configuration.GetSection("PayPal");
    client.BaseAddress = new Uri(config["BaseUrl"] ?? throw new ArgumentNullException("PayPal:BaseUrl"));

    var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{config["ClientId"]}:{config["Secret"]}"));
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    // Remove "Content-Type" here; it will be set in the request content where necessary
});

// Configure SMTP
builder.Services.AddTransient<SmtpClient>();

// Configure session handling
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1); // Session timeout
    options.Cookie.HttpOnly = true; // Make cookies accessible only via HTTP
    options.Cookie.IsEssential = true; // Ensure session cookies are essential
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Enable CORS
app.UseCors();

// Enable session middleware
app.UseSession();

// Enable authorization middleware
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Run the application
app.Run();
