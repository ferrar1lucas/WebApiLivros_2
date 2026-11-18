using Microsoft.EntityFrameworkCore;
using WebApiLivros_2.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionDefault")));

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); ;
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
