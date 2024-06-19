namespace CatBuddy.Models
{
    public class ViewEndereco
    {
        public Endereco Endereco {  get; set; }

        public List<Logradouro> ListLogradouro {  get; set; }
        public string? nomeLogradouro { get; set; }
        public string getEnderecoCompleto()
        {
            return $"{nomeLogradouro} {Endereco.enderecoUsuario}";
        }
    }
}
