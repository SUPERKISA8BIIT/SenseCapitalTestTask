using MongoDB.Driver;
using SenseCapital.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
 builder.Services.AddCors(c =>
 {
     c.AddPolicy("AllowOrigin",
         options => options.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader()
         );
 });



builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.IgnoreNullValues = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient(c => new MongoClient(builder.Configuration.GetConnectionString("ConStr")));
builder.Services.AddTransient<GameService>();
var app = builder.Build();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
