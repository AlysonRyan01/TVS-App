using QuestPDF.Infrastructure;
using TVS_App.Api.Common;
using TVS_App.Api.SignalR;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;
builder.AddSqlServer();
builder.AddIdentity();
builder.AddAuthentication();
builder.AddJwtService();
builder.ConfigureJsonSerializer();
builder.AddDependencies();
builder.AddSwagger();
builder.Services.AddSignalR();

var app = builder.Build();

app.MapHub<ServiceOrderHub>("/osHub");

app.UseAuthentication();
app.UseAuthorization();

app.AddEndpoints();
app.AddSwagger();

app.MapGet("/", () => "API RODANDO");

app.Run();
