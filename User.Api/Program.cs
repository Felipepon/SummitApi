using User.Api.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


var app = builder.Build();

Database.Instance = new MemoryDatabase();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
