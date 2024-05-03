using CatBuddy.Models;
using CatBuddy.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace CatBuddy.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        // Conexao com o banco 
        private readonly string _conexao;

        // Variável de apoio para a sintaxe SQL
        private string _SintaxeSQl;
        public PedidoRepository(IConfiguration configuration)
        {
            _conexao = configuration.GetConnectionString("ConexaoMySQL");
        }

        /// <summary>
        /// Cadastra o pedido e retorna qual o id do pedido
        /// </summary>
        public int CadastrarPedido(int codUsuario, float valorTotal, int codPagamento)
        {
            StringBuilder sbAux = new StringBuilder();
            int codPedido;

            // Sintaxe SQL
            sbAux.Append(" INSERT INTO tbl_pedido VALUE( null, ");
            sbAux.Append(" @codUsuario, @codPagamento, @valorTotal, @dtAtual); ");
            sbAux.Append(" SELECT LAST_INSERT_ID(); ");

            // Monta o comando
            _SintaxeSQl = sbAux.ToString();

            using (var conexao = new MySqlConnection(_conexao))
            {
                // Comando MySQL
                MySqlCommand cmd = new MySqlCommand(_SintaxeSQl, conexao);

                // Passagem de parametros
                cmd.Parameters.Add("@codUsuario", MySqlDbType.Int32).Value = codUsuario;
                cmd.Parameters.Add("@codPagamento", MySqlDbType.Int32).Value = codPagamento;
                cmd.Parameters.Add("@valorTotal", MySqlDbType.Float).Value = valorTotal;
                cmd.Parameters.Add("@dtAtual", MySqlDbType.DateTime).Value = DateTime.Now;

                // Abre a conexao
                conexao.Open();

                // Insere o pedido e retorna o numero do pedido
                codPedido = Convert.ToInt32(cmd.ExecuteScalar());

                conexao.Close();
            }
            return codPedido;
        }

        /// <summary>
        /// Cadastra os itens do pedido
        /// </summary>
        public void CadastrarItemPedido(int codPedido, int codProduto, int Qtd, float subtotal)
        {
            StringBuilder sbAux = new StringBuilder();

            // Monta a sintaxe SQL
            sbAux.Append(" INSERT INTO tbl_itempedido value( ");
            sbAux.Append(" @codProduto, @codPedido, @qtd, @subtotal); ");

            // Monta o comando 
            _SintaxeSQl = sbAux.ToString();

            using (var conexao = new MySqlConnection(_conexao))
            {
                MySqlCommand cmd = new MySqlCommand(_SintaxeSQl, conexao);

                // Limpeza dos parametros
                cmd.Parameters.Clear();

                // Passagem de parametros
                cmd.Parameters.Add("@codProduto", MySqlDbType.Int32).Value = codProduto;
                cmd.Parameters.Add("@codPedido", MySqlDbType.Int32).Value = codPedido;
                cmd.Parameters.Add("@qtd", MySqlDbType.Int32).Value = Qtd;
                cmd.Parameters.Add("@subtotal", MySqlDbType.Float).Value = subtotal;

                conexao.Open();

                // Executa o comando
                cmd.ExecuteNonQuery();

                conexao.Close();
            }
        }

        public void FinalizarPedido(int codPedido, int codPagamento)
        {
            // TODO: Atualizar o estoque da loja 
        }





    }
}
