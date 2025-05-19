using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Sysachad.Models;
using Sysachad.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<EnviromentVariables>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var rsa = RSAKeyProvider.GetPublicKey();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new RsaSecurityKey(rsa)
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("IsAdmin", "True"));
});

builder.Services.AddDbContext<UniversidadContext>();
builder.Services.AddScoped<StudentsService>();
builder.Services.AddScoped<SubjectsService>();
builder.Services.AddScoped<StudentSubjectsService>();
builder.Services.AddScoped<ClassesService>();
builder.Services.AddScoped<CorrelativesService>();
builder.Services.AddScoped<StudentExamService>();
builder.Services.AddScoped<ExamsService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UniversidadContext>();
    var sImporter = new CsvSubjectsImporter(context);
    await sImporter.ImportFromCsv("Data/subjects.csv");
    var dImporter = new CsvClassesImporter(context);
    await dImporter.ImportFromCsv("Data/classes.csv");
    var ssImporter = new CsvStudentsSubjectsImporter(context);
    await ssImporter.ImportFromCsv("Data/studentsSubjects.csv");
    var cImporter = new CsvCorrelativesImporter(context);
    await cImporter.ImportFromCsv("Data/correlatives.csv");
    var eImporter = new CsvExamsImporter(context);
    await eImporter.ImportFromCsv("Data/exams.csv");
}
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();