using OpenCodeChems.Service.Services;
using OpenCodeChems.BusinessLogic;
using OpenCodeChems.DataAccess;
using OpenCodeChems.BusinessLogic.Interface;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGrpcService<UsersService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
