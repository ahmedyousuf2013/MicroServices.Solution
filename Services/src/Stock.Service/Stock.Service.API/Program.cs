using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RabbitMQ.Kernel.Base.Contstants;
using RabbitMQ.Kernel.QueueingStartupInjection;
using Stock.Service.API;
using Stock.Service.Application.DependencyInjection;
using Stock.Service.Persistence.Context;

{
   
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    //builder.Services.AddOpenApi();

    builder.Services.AddSwaggerGen();

    builder.Services.Configure<QueueSettings>(builder.Configuration);

    builder.Services.AddDbContext<StockServiceContext>(options =>
    {
        options.ConfigureWarnings(w =>
        w.Ignore(RelationalEventId.PendingModelChangesWarning));
        options.UseNpgsql(builder.Configuration["ConnectionStrings:Default"]);

    });
   
    builder.Services.AddQueueing(builder.Configuration);

    builder.Services.AddAddQueueMessageConsumer();


    var app = builder.Build();


    app.ApplyMigrations();


    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
