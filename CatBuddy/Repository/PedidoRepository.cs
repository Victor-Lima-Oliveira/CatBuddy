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
        public void CadastrarItemPedido(int codPedido, int codProduto, int codCliente, int Qtd, float subtotal)
        {
            StringBuilder sbAux = new StringBuilder();

            // Monta a sintaxe SQL
            sbAux.Append(" INSERT INTO tbl_itempedido value( ");
            sbAux.Append(" @codProduto, @codPedido, @codCliente, @qtd, @subtotal); ");

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
                cmd.Parameters.Add("@codCliente", MySqlDbType.Int32).Value = codCliente;
                cmd.Parameters.Add("@qtd", MySqlDbType.Int32).Value = Qtd;
                cmd.Parameters.Add("@subtotal", MySqlDbType.Float).Value = subtotal;

                conexao.Open();

                // Executa o comando
                cmd.ExecuteNonQuery();

                conexao.Close();
            }
        }
        public List<ViewPedido> ObtemPedidos(int codCliente)
        {

            List<ViewPedido> listPedidos = new List<ViewPedido>();

            _SintaxeSQl = "SELECT * FROM vwpedido WHERE cod_cliente = @codCliente";

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta o comando
                MySqlCommand cmd = new MySqlCommand(_SintaxeSQl, conexao);

                // Passagem de parametro 
                cmd.Parameters.AddWithValue("@codCliente", codCliente);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                conexao.Close();

                // Carrega a lista 
                foreach (DataRow dr in dt.Rows)
                {
                    listPedidos.Add(
                        new ViewPedido
                        {
                            nomepagamento = Convert.ToString(dr["nomepagamento"]),
                            qtdProdutos = Convert.ToInt32(dr["qtdProdutos"]),
                            nomeUsuario = Convert.ToString(dr["nomeUsuario"]),
                            Pedido = new Pedido
                            {
                                cod_cliente = Convert.ToInt32(dr["cod_cliente"]),
                                cod_id_pedido = Convert.ToInt32(dr["cod_id_pedido"]),
                                cod_pagamento = Convert.ToInt32(dr["cod_pagamento"]),
                                dataPedido = Convert.ToDateTime(dr["datapedido"]),
                                valorTotal = Convert.ToDouble(dr["qtdProdutos"])
                            }
                        });
                };

            }
            return listPedidos;
        }

        public List<ViewItensPedido> ObtemItensPedido(int codPedido)
        {

            List<ViewItensPedido> listItensPedidos = new List<ViewItensPedido>();

            _SintaxeSQl = "SELECT * FROM vwitenspedido WHERE cod_pedido = @codPedido";

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta o comando
                MySqlCommand cmd = new MySqlCommand(_SintaxeSQl, conexao);

                // Passagem de parametro 
                cmd.Parameters.AddWithValue("@codPedido", codPedido);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                conexao.Close();

                // Carrega a lista 
                foreach (DataRow dr in dt.Rows)
                {
                    listItensPedidos.Add(
                        new ViewItensPedido
                        {
                            nomeProduto = Convert.ToString(dr["ds_nome"]),
                            isprodutoativo = Convert.ToBoolean(dr["isprodutoativo"]),
                            datapedido = Convert.ToDateTime(dr["datapedido"]),
                            valorTotal = Convert.ToDouble(dr["qtd"]),
                            imgPath = Convert.ToString(dr["imgPath"]),
                            ItemPedido = new ItemPedido
                            {
                                cod_pedido = Convert.ToInt32(dr["cod_pedido"]),
                                cod_produto = Convert.ToInt32(dr["cod_produto"]),
                                qtd = Convert.ToInt32(dr["qtd"]),
                                subtotal = Math.Round(Convert.ToDouble(dr["subtotal"]), 2)
                            }
                        });
                };

            }
            return listItensPedidos;
        }
    }
}
