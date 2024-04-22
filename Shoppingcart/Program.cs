using Microsoft.EntityFrameworkCore;
using Shoppingcart.Data;
using Shoppingcart.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<UserDbContext>(x =>
{
    x.UseSqlServer("Server=ELW5234;Database=Shoppingcart; Trusted_Connection=True;Encrypt=False");
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IproductRepository, productRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});



app.Run();
