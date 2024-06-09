namespace CatBuddy.httpContext
{
    public class Cookie
    {
        // Interface de acesso ao contexto http
        private IHttpContextAccessor _httpContextAccessor;

        // Interface de acesso as configurações do sistema
        private IConfiguration _configuration;

        public Cookie(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        /// <summary>
        /// Cadastra um novo cookie 
        /// </summary>
        private void Cadastrar(string key, string value)
        {
            // Instância utilizada para criar um cookie 
            CookieOptions options = new CookieOptions();

            // Define o tempo que o cookie irá durar
            options.Expires = DateTime.Now.AddDays(7);

            // Define a chave, valor para o cookie, options é utilizada para a criação de um novo cookie
            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, options);
        }

        /// <summary>
        /// Persiste um cookie
        /// </summary>
        public void Atualizar(string key, string value)
        {
            // Se ja existe determinada chave (Se o usuário já possui aquele cookie)
            // A chave será removida para criar outra 
            if (Existe(key))
            {
                Remover(key);
            }

            // Cadastra o novo cookie
            Cadastrar(key, value);
        }

        /// <summary>
        /// Remove um cookie
        /// </summary>
        public void Remover(string key)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }

        /// <summary>
        /// Consulta se um cookie os valores armazendos de um cookies
        /// </summary>
        public string Consultar (string key)
        {
            // Realiza a consulta de um cookie em determinado caso 
            string sValorArmazenadoNoCookie = _httpContextAccessor.HttpContext.Request.Cookies[key];
            return sValorArmazenadoNoCookie;
        }

        /// <summary>
        /// Indica se um cookie existe ou não
        /// </summary>
        public bool Existe(string key)
        {
            // Pesquisa se um cookie existe ou não
            if(Consultar(key) == null)
            {
                return false;
            }
            return true;
        }
    }
}
