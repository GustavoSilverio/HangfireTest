using Hangfire;
using HangfireTest.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddHangfire(config => config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddHangfireServer()
    .InstantiateJobs();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.ScheduleAllJobs();
app.UseHangfireDashboard("/jobs"); // add basic auth later

app.MapControllers();
app.Run();