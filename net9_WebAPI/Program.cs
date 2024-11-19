var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();

    #region SwaggerUI
    // �ѩ�.net9 �w�]���A�]�tSwaggerUI
    // 1. �ݭn�bnuget�M��޲z���ۦ�w�� Swachbuckle.AspNetCore
    // 2. �s�W���U��M������
    app.UseSwagger();
    app.UseSwaggerUI();
    // 3. �p�G�ݭn�P����OpenApi�@�s, �}�� 7,24-28��, ����9, 23��
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