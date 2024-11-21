using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Serilog取代內建Log功能
//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("log/villaLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();
//builder.Host.UseSerilog();
#endregion

builder.Services.AddControllers(option => { option.ReturnHttpNotAcceptable = true; }).AddNewtonsoftJson();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();

    #region SwaggerUI
    // 由於.net9 預設不再包含SwaggerUI
    // 1. 需要在nuget套件管理員自行安裝 Swachbuckle.AspNetCore
    // 2. 新增註冊到專案環境
    app.UseSwagger();
    app.UseSwaggerUI();
    // 3. 如果需要與內建OpenApi共存, 開啟 7,24-28行, 關閉9, 23行
    //app.UseSwaggerUI(options =>
    //{
    //    options.SwaggerEndpoint("/openapi/v1.json", "WebAPI");
    //});
    #endregion
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
