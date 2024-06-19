using CatBuddy.Models;
using CatBuddy.Repository.Contract;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Data;
using System.Text;

namespace CatBuddy.Repository
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly string _conexao;

        public EnderecoRepository(IConfiguration configuration)
        {
            _conexao = configuration.GetConnectionString("ConexaoMySQL");
        }

        public void Atualizar(Endereco endereco)
        {
            StringBuilder sbAux = new StringBuilder();

            // Sintaxe SQL
            sbAux.Append(" update tbl_enderecocliente set enderecoUsuario = @endereco, ");
            sbAux.Append(" bairroUsuario = @bairro, ");
            sbAux.Append(" cepUsuario = @cep, cod_logradouro = @codLogradouro, ");
            sbAux.Append(" nomeEnderecoUsuario = @apelido ");
            sbAux.Append(" where cod_id_endereco = @codEndereco AND cod_cliente = @codCliente ");

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta o comando
                MySqlCommand cmd = new MySqlCommand(sbAux.ToString(), conexao);

                // Passagem de parametros 
                cmd.Parameters.Add("@endereco", MySqlDbType.VarChar).Value = endereco.enderecoUsuario;
                cmd.Parameters.Add("@bairro", MySqlDbType.VarChar).Value = endereco.bairroUsuario;
                cmd.Parameters.Add("@cep", MySqlDbType.VarChar).Value = endereco.cepUsuario;
                cmd.Parameters.Add("@codLogradouro", MySqlDbType.VarChar).Value = endereco.cod_logradouro;
                cmd.Parameters.Add("@apelido", MySqlDbType.VarChar).Value = endereco.nomeEnderecoUsuario;
                cmd.Parameters.Add("@codCliente", MySqlDbType.VarChar).Value = endereco.cod_cliente;
                cmd.Parameters.Add("@codEndereco", MySqlDbType.VarChar).Value = endereco.cod_id_endereco;

                // Executa o comando
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Cadastrar(Endereco endereco)
        {
            StringBuilder sbAux = new StringBuilder();

            // Sintaxe SQL
            sbAux.Append(" insert into tbl_enderecocliente values ");
            sbAux.Append(" (default, @endereco, @bairro, @cep, ");
            sbAux.Append("  @codLogradouro, @apelido, @codCliente ) ");


            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta comando SQL
                MySqlCommand cmd = new MySqlCommand(sbAux.ToString(), conexao);

                // Passagem de parametros
                cmd.Parameters.Add("@endereco", MySqlDbType.VarChar).Value = endereco.enderecoUsuario;
                cmd.Parameters.Add("@bairro", MySqlDbType.VarChar).Value = endereco.bairroUsuario;
                cmd.Parameters.Add("@cep", MySqlDbType.VarChar).Value = endereco.cepUsuario;
                cmd.Parameters.Add("@codLogradouro", MySqlDbType.VarChar).Value = endereco.cod_logradouro;
                cmd.Parameters.Add("@apelido", MySqlDbType.VarChar).Value = endereco.nomeEnderecoUsuario;
                cmd.Parameters.Add("@codCliente", MySqlDbType.VarChar).Value = endereco.cod_cliente;

                // Executa o comando
                cmd.ExecuteNonQuery();

                // Fecha a conexão com o banco
                conexao.Close();
            }
        }

        public void Excluir(int id)
        {
            StringBuilder sbAux = new StringBuilder();

            // Sintaxe SQL
            sbAux.Append(" DELETE FROM tbl_enderecocliente  ");
            sbAux.Append(" WHERE cod_id_endereco = @codEndereco ");

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta comando SQL
                MySqlCommand cmd = new MySqlCommand(sbAux.ToString(), conexao);

                // Passagem de parametros
                cmd.Parameters.AddWithValue("@codEndereco", id);

                // Executa o comando
                cmd.ExecuteNonQuery();

                // Fecha a conexão com o banco
                conexao.Close();
            }
        }

        public Endereco ObtemEndereco(int id)
        {
            StringBuilder sbAux = new StringBuilder();
            Endereco endereco = new Endereco();

            // Sintaxe SQL
            sbAux.Append(" SELECT * FROM tbl_enderecocliente  ");
            sbAux.Append(" WHERE cod_id_endereco = @codEndereco ");

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta comando SQL
                MySqlCommand cmd = new MySqlCommand(sbAux.ToString(), conexao);

                // Passagem de parametros
                cmd.Parameters.AddWithValue("@codEndereco", id);
                // Cria um adapter
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                // Recupera os dados
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                // Salva na Model
                while (dr.Read())
                {
                    endereco = new Endereco()
                    {
                        cod_id_endereco = Convert.ToInt32(dr["cod_id_endereco"]),
                        cod_cliente = Convert.ToInt32(dr["cod_cliente"]),
                        cod_logradouro = Convert.ToInt32(dr["cod_logradouro"]),
                        enderecoUsuario = Convert.ToString(dr["enderecoUsuario"]),
                        nomeEnderecoUsuario = Convert.ToString(dr["nomeEnderecoUsuario"]),
                        bairroUsuario = Convert.ToString(dr["bairroUsuario"]),
                        cepUsuario = Convert.ToString(dr["cepUsuario"]),
                    };
                }

                return endereco;
            }
        }

        public List<ViewEndereco> ObtemEnderecos(int id)
        {
            List<ViewEndereco> listEndereco = new List<ViewEndereco>();

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta comando SQL
                MySqlCommand cmd = new MySqlCommand("select * from vwendereco where cod_cliente = @codCliente", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                cmd.Parameters.AddWithValue("@codCliente", id);

                // Recebe os dados
                da.Fill(dt);

                conexao.Close();

                // Realiza a leitura dos dados 
                foreach (DataRow dr in dt.Rows)
                {
                    listEndereco.Add(
                        new ViewEndereco()
                        {
                            nomeLogradouro = Convert.ToString(dr["nomeLogradouro"]),
                            Endereco = new Endereco
                            {
                                cod_id_endereco = Convert.ToInt32(dr["cod_id_endereco"]),
                                cod_cliente = Convert.ToInt32(dr["cod_cliente"]),
                                cod_logradouro = Convert.ToInt32(dr["cod_logradouro"]),
                                enderecoUsuario = Convert.ToString(dr["enderecoUsuario"]),
                                nomeEnderecoUsuario = Convert.ToString(dr["nomeEnderecoUsuario"]),
                                bairroUsuario = Convert.ToString(dr["bairroUsuario"]),
                                cepUsuario = Convert.ToString(dr["cepUsuario"]),
                            }
                        }) ;
                }
            }

            return listEndereco;
        }
    }
}
