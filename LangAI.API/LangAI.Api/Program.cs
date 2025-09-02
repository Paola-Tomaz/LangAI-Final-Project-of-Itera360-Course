using LangAI.Aplicacao.Interfaces;
using LangAI.Aplicacao.Services;
using LangAI.Repositorio.Contextos;
using LangAI.Repositorio.Interfaces;
using LangAI.Aplicacao.Configuracoes;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = null,
    ContentRootPath = Directory.GetCurrentDirectory() // Adicione esta linha
});

builder.WebHost.UseStaticWebAssets(); // Isso resolve o problema do wwwroot

var configuration = builder.Configuration;

builder.Services.AddControllers();

// 🔐 Configurações do JWT
var jwtSection = configuration.GetSection("Jwt");
builder.Services.Configure<JwtConfiguracoes>(jwtSection);

var jwtConfig = jwtSection.Get<JwtConfiguracoes>();
var chave = Encoding.ASCII.GetBytes(jwtConfig.SecretKey);

// 📦 Entity Framework com SQL Server
builder.Services.AddDbContext<LangAIContexto>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// 💉 Injeção de dependência: Repositórios
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

// 💉 Injeção de dependência: Serviços
builder.Services.AddScoped<IIdiomaServico, IdiomaServico>();
builder.Services.AddScoped<IPersonagemServico, PersonagemServico>();
builder.Services.AddScoped<IAiServico, AiServico>();

// ✅ Registro correto do UsuarioServico com IConfiguration
builder.Services.AddScoped<IUsuarioServico>(provider =>
{
    var usuarioRepositorio = provider.GetRequiredService<IUsuarioRepositorio>();
    var config = provider.GetRequiredService<IConfiguration>();
    return new UsuarioServico(usuarioRepositorio, config);
});

// S3 (ACESSO AWS S3)
builder.Services.AddSingleton<S3Servico>();

// Senha
builder.Services.AddScoped<IPasswordHasher<string>, PasswordHasher<string>>();

// HTTP Client
builder.Services.AddHttpClient();

// 🔐 Autenticação JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // true em produção
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig.Emissor,
        ValidAudience = jwtConfig.Publico,
        IssuerSigningKey = new SymmetricSecurityKey(chave)
    };
});

builder.Services.AddAuthorization();

// 🔁 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 📘 Swagger (documentação da API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "LangAI.Api", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token: Bearer {seu_token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Authorization",
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }
    });
});

// 🚀 Build do app
var app = builder.Build();

// 🔧 Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
