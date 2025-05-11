using QuestPDF.Infrastructure;
using TVS_App.Api.Common;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;
builder.AddAuthentication();
builder.AddSqlServer();
builder.AddDependencies();
builder.AddIdentity();
builder.AddJwtService();
builder.ConfigureJsonSerializer();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer(); 

var app = builder.Build();

app.AddAuthorization();
app.AddEndpoints();
app.AddSwagger();

app.MapGet("/", () => "API RODANDO");

app.Run();
