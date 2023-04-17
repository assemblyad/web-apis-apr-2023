using HrApi.Domain;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var hrConnectionString = builder.Configuration.GetConnectionString("hr-data");

if (hrConnectionString is null)
{
    throw new Exception("No Connection String for HR Database");
}


builder.Services.AddDbContext<HrDataContext>(options =>
{
    options.UseSqlServer(hrConnectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
