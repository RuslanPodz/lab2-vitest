using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MobileYogaApp.Extensions;
using NpgsqlTypes;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using YogaApp.Application.Helpers;
using YogaApp.Application.Options;
using YogaApp.Application.ServiceContact.Program;
using YogaApp.Application.ServiceContact.Progress;
using YogaApp.Application.ServiceContact.User;
using YogaApp.Application.ServiceContact.Workout;
using YogaApp.Application.Services.Program;
using YogaApp.Application.Services.Progress;
using YogaApp.Application.Services.User;
using YogaApp.Application.Services.Workout;
using YogaApp.Core.RepositoryContracts;
using YogaApp.Infrastructure.Data;
using YogaApp.Infrastructure.Repository.Program;
using YogaApp.Infrastructure.Repository.Progress;
using YogaApp.Infrastructure.Repository.User;
using YogaApp.Infrastructure.Repository.Workout;

var builder = WebApplication.CreateBuilder(args);
//HomePage
builder.Services.AddScoped<IWorkoutRepository, WorkoutRepository>(); // Репозиторій
builder.Services.AddScoped<IWorkoutService, WorkoutService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserWorkoutRepository, UserWorkoutRepository>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();

//ProgramPage
builder.Services.AddScoped<IProgramService, ProgramService>();
builder.Services.AddScoped<IProgramRepository, ProgramRepository>();
builder.Services.AddScoped<IUserProgramService, UserProgramService>();
builder.Services.AddScoped<IUserProgramRepository, UserProgramRepository>();

//CustomWorkoutPage
builder.Services.AddScoped<ICustomWorkoutRepository, CustomWorkoutRepository>();
builder.Services.AddScoped<ICustomWorkoutService, CustomWorkoutService>();

//ProgressPage
builder.Services.AddScoped<IUserGeneralStatsService, UserGeneralStatsService>(); 
builder.Services.AddScoped<IUserWeightTrackingService, UserWeightTrackingService>();
builder.Services.AddScoped<IUserWeightTrackingRepository, UserWeightTrackingRepository>();
builder.Services.AddScoped<IUserWorkoutService, UserWorkoutService>();
builder.Services.AddScoped<IUserGeneralStatsRepository, UserGeneralStatsRepository>();

// Connection to database
// builder.Services.AddDbContext<AppDbContext>(option => 
//     option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Logging
var columnWriters = new Dictionary<string, ColumnWriterBase>
{
    {"message", new RenderedMessageColumnWriter(NpgsqlDbType.Text)},
    {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text)},
    {"level", new LevelColumnWriter(true, NpgsqlDbType.Varchar)},
    {"raise_date", new TimestampColumnWriter(NpgsqlDbType.Timestamp)},
    {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text)},
    {"properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb)},
};

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.PostgreSQL(
        connectionString: "Host=localhost;Port=5432;Database=logs;Username=postgres;Password=password",
        tableName: "logs",
        columnWriters,
        needAutoCreateTable: true
    )
    .CreateLogger();


builder.Host.UseSerilog(); 

builder.Logging.ClearProviders().AddConsole();

builder.Services.AddApiAuthenticationExtension(builder.Configuration);

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddSingleton<JwtTokenHelper>();

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("JwtOptions"));

builder.Services.AddHttpLogging(logg =>
{
    logg.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties
        | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
});



builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseHttpLogging();
    app.UseSwaggerUI(); 
}

app.MapControllers();

app.Run();