using CatBuddy.Models;
using CatBuddy.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace CatBuddy.Repository
{
    public class CartaoRepository : ICartaoRepository
    {
        private readonly string _conexao;

        public CartaoRepository(IConfiguration configuration)
        {
            _conexao = configuration.GetConnectionString("ConexaoMySQL");
        }

        public void Atualizar(Cartao cartao)
        {
            StringBuilder sbAux = new StringBuilder();

            // Sintaxe SQL
            sbAux.Append(" update tbl_cartaocliente set nomeTitular = @nometitular, ");
            sbAux.Append(" numeroCartaoCred = @numeroCartaoCred, ");
            sbAux.Append(" dataDeValidade = @dataValidade, codSeguranca = @codigoSeguranca ");
            sbAux.Append(" where cod_id_pagamento = @codPagamento AND cod_cliente = @codCliente ");

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta o comando
                MySqlCommand cmd = new MySqlCommand(sbAux.ToString(), conexao);

                // Passagem de parametros 
                cmd.Parameters.Add("@codPagamento", MySqlDbType.VarChar).Value = cartao.cod_id_pagamento;
                cmd.Parameters.Add("@codCliente", MySqlDbType.VarChar).Value = cartao.cod_cliente;
                cmd.Parameters.Add("@nometitular", MySqlDbType.VarChar).Value = cartao.nomeTitular;
                cmd.Parameters.Add("@numeroCartaoCred", MySqlDbType.VarChar).Value = cartao.numeroCartaoCred;
                cmd.Parameters.Add("@dataValidade", MySqlDbType.VarChar).Value = cartao.dataDeValidade;
                cmd.Parameters.Add("@codigoSeguranca", MySqlDbType.VarChar).Value = cartao.codSeguranca;

                // Executa o comando
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Cadastrar(Cartao cartao)
        {
            StringBuilder sbAux = new StringBuilder();

            // Sintaxe SQL
            sbAux.Append(" insert into tbl_cartaocliente values ");
            sbAux.Append(" (default, @codCliente, @nometitular, @numeroCartaoCred, ");
            sbAux.Append("  @dataValidade, @codigoSeguranca ) ");


            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta comando SQL
                MySqlCommand cmd = new MySqlCommand(sbAux.ToString(), conexao);

                // Passagem de parametros
                cmd.Parameters.Add("@codCliente", MySqlDbType.VarChar).Value = cartao.cod_cliente;
                cmd.Parameters.Add("@nometitular", MySqlDbType.VarChar).Value = cartao.nomeTitular;
                cmd.Parameters.Add("@numeroCartaoCred", MySqlDbType.VarChar).Value = cartao.numeroCartaoCred;
                cmd.Parameters.Add("@dataValidade", MySqlDbType.VarChar).Value = cartao.dataDeValidade;
                cmd.Parameters.Add("@codigoSeguranca", MySqlDbType.VarChar).Value = cartao.codSeguranca;

                // Executa o comando
                cmd.ExecuteNonQuery();

                // Fecha a conexão com o banco
                conexao.Close();
            }
        }

        public void Excluir(int idPagamento)
        {
            StringBuilder sbAux = new StringBuilder();

            // Sintaxe SQL
            sbAux.Append(" DELETE FROM tbl_cartaocliente  ");
            sbAux.Append(" WHERE cod_id_pagamento = @codPagamento ");

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta comando SQL
                MySqlCommand cmd = new MySqlCommand(sbAux.ToString(), conexao);

                // Passagem de parametros
                cmd.Parameters.AddWithValue("@codPagamento", idPagamento);

                // Executa o comando
                cmd.ExecuteNonQuery();

                // Fecha a conexão com o banco
                conexao.Close();
            }
        }

        public Cartao ObtemCartao(int id)
        {
            StringBuilder sbAux = new StringBuilder();
            Cartao cartao = new Cartao();

            // Sintaxe SQL
            sbAux.Append(" SELECT * FROM tbl_cartaocliente  ");
            sbAux.Append(" WHERE cod_id_pagamento = @codPagamento ");

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta comando SQL
                MySqlCommand cmd = new MySqlCommand(sbAux.ToString(), conexao);

                // Passagem de parametros
                cmd.Parameters.AddWithValue("@codPagamento", id);
                // Cria um adapter
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                // Recupera os dados
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                // Salva na Model
                while (dr.Read())
                {
                    cartao = new Cartao()
                    {
                        cod_cliente = Convert.ToInt32(dr["cod_cliente"]),
                        cod_id_pagamento = Convert.ToInt32(dr["cod_id_pagamento"]),
                        nomeTitular = Convert.ToString(dr["nomeTitular"]),
                        numeroCartaoCred = Convert.ToString(dr["numeroCartaoCred"]),
                        dataDeValidade = Convert.ToString(dr["dataDeValidade"]),
                        codSeguranca = Convert.ToString(dr["codSeguranca"])
                    };
                }

                return cartao;
            }
        }

        public List<Cartao> ObtemCartoes(int idCliente)
        {
            List<Cartao> listEndereco = new List<Cartao>();

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta comando SQL
                MySqlCommand cmd = new MySqlCommand("select * from tbl_cartaocliente where cod_cliente = @codCliente", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                cmd.Parameters.AddWithValue("@codCliente", idCliente);

                // Recebe os dados
                da.Fill(dt);

                conexao.Close();

                // Realiza a leitura dos dados 
                foreach (DataRow dr in dt.Rows)
                {
                    listEndereco.Add(
                        new Cartao()
                        {
                            cod_cliente = Convert.ToInt32(dr["cod_cliente"]),
                            cod_id_pagamento = Convert.ToInt32(dr["cod_id_pagamento"]),
                            nomeTitular = Convert.ToString(dr["nomeTitular"]),
                            numeroCartaoCred = Convert.ToString(dr["numeroCartaoCred"]),
                            dataDeValidade = Convert.ToString(dr["dataDeValidade"]),
                            codSeguranca = Convert.ToString(dr["codSeguranca"])
                        });
                }
            }

            return listEndereco;
        }
    }
}

