using Microsoft.EntityFrameworkCore;
using PaParchar.Application.Interfaces.Repositories;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Infrastructure.Configuration.Contexts;
using PaParchar.Infrastructure.Mapping;
using PaParchar.Infrastructure.Repositories;
using PaParchar.Infrastructure.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddTransient(typeof(_IBaseRepository<,>), typeof(_BaseRepository<,>));
builder.Services.AddTransient(typeof(_IBaseService<,>), typeof(_BaseService<,>));

builder.Services
    .AddTransient<IParcheService, ParcheService>()
    .AddTransient<IParcheRepository, ParcheRepository>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

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

// Conversor personalizado para TimeOnly
public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private readonly string _format = "HH:mm";

    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return TimeOnly.Parse(reader.GetString()!);
        }
        else if (reader.TokenType == JsonTokenType.StartObject)
        {
            reader.Read();
            var propertyName = reader.GetString();
            reader.Read();
            var hour = reader.TokenType == JsonTokenType.Number ? reader.GetInt32() : 0;
            
            reader.Read();
            propertyName = reader.GetString();
            reader.Read();
            var minute = reader.TokenType == JsonTokenType.Number ? reader.GetInt32() : 0;
            
            reader.Read(); // Leer el EndObject
            
            return new TimeOnly(hour, minute);
        }
        
        throw new JsonException("El formato TimeOnly no es válido.");
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_format));
    }
}
