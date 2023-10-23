using FastEndpoints.Swagger;
using V_Slice.Features.Todo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

builder.Services.SwaggerDocument();

builder.Services.AddEfCore();

// Features
builder.Services.AddTodoFeature();

var app = builder.Build();

app.UseAuthorization();
app.UseFastEndpoints();
app.UseSwaggerGen();

app.Run();