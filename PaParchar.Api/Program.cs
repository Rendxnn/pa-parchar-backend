
using Microsoft.EntityFrameworkCore;
using PaParchar.Application.Interfaces.Repositories;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Infrastructure.Configuration.Contexts;
using PaParchar.Infrastructure.Mapping;
using PaParchar.Infrastructure.Repositories;
using PaParchar.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddTransient(typeof(_IBaseRepository<,>), typeof(_BaseRepository<,>));
builder.Services.AddTransient(typeof(_IBaseService<,>), typeof(_BaseService<,>));

builder.Services
    .AddTransient<IParcheService, ParcheService>()
    .AddTransient<IParcheRepository, ParcheRepository>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
