namespace CatBuddy.LibrariesSessao
{
    public class Sessao
    {
        IHttpContextAccessor _httpContextAccessor;

        public Sessao(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Cadastra qual os dados do usuario na sessão
        /// </summary>
        public void Cadastar(string key, string value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, value);
        }

        /// <summary>
        /// Consulta os dados da sessão
        /// </summary>
        public string Consultar(string key)
        {
            return _httpContextAccessor.HttpContext.Session.GetString(key);
        }

        /// <summary>
        /// Retorna se existe dados salvos na sessão
        /// </summary>
        public bool Existe(string key)
        {
            // Se possuir a key na cadastrada na sessão
            if (_httpContextAccessor.HttpContext.Session.GetString(key) == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Limpa a sessão
        /// </summary>
        public void RemoverTodos()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }

        public void Atualizar(string key, string value)
        {
            // Se existe, remove a sessão antiga
            if (Existe(key))
            {
                _httpContextAccessor.HttpContext.Session.Remove(key);
            }

            // Salva uma nova sessão
            _httpContextAccessor.HttpContext.Session.SetString(key, value);
        }
    }
}
