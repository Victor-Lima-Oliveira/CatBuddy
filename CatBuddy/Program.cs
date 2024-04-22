using CatBuddy.httpContext;
using CatBuddy.Repository;
using CatBuddy.Repository.Contract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Adiciona o serviço de contexto para uso de cookies e sessão
builder.Services.AddHttpContextAccessor();

// Adiciona as interfaces como serviços do projeto
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
