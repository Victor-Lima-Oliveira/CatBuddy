using CatBuddy.httpContext;
using CatBuddy.LibrariesSessao.Login;
using CatBuddy.LibrariesSessao;
using CatBuddy.Repository;
using CatBuddy.Repository.Contract;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using CatBuddy.LibrariesSessao.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();

// Adiciona o serviço de contexto para uso de cookies e sessão
builder.Services.AddHttpContextAccessor();

// Adiciona as interfaces como serviços do projeto
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IColaboradorRepository, ColaboradorRepository>();


// Configuração da utiização de cookies 
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

// Corrigir problema com TEMPDATA para aumentar o tempo de duracao
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    // Set a short timeout for easy testing. 
    options.IdleTimeout = TimeSpan.FromSeconds(900);
    options.Cookie.HttpOnly = true;
    // Deixar informado para o navegador que a sess�o � essencial
    options.Cookie.IsEssential = true;
});

builder.Services.AddMvc().AddSessionStateTempDataProvider();

// Guardar os dados na memoria
builder.Services.AddMemoryCache();

// Adiciona a sessão como serviço 
builder.Services.AddScoped<Sessao>();
builder.Services.AddScoped<LoginCliente>();
builder.Services.AddScoped<LoginColaborador>();

// Adiciona os cookies como serviço 
builder.Services.AddScoped<CarrinhoDeCompraCookie>();
builder.Services.AddScoped<Cookie>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

// Utilizacao de cookies e sessao
app.UseCookiePolicy();
app.UseSession();

// Protege todas as actions com http post
app.UseMiddleware<ValidateAntiForgeryTokenMiddleware>();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
