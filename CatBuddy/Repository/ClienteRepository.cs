using CatBuddy.Models;
using CatBuddy.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace CatBuddy.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly string _conexao;

        public ClienteRepository(IConfiguration configuration)
        {
            _conexao = configuration.GetConnectionString("ConexaoMySQL");
        }

        public void Atualizar(Cliente cliente)
        {
            StringBuilder sbAux = new StringBuilder();

            // Sintaxe SQL
            sbAux.Append(" update tbl_cliente set nomeUsuario = @Nome, ");
            sbAux.Append(" dtNascimento = @Nascimento, ");
            sbAux.Append(" CPF = @CPF, telefone = @Telefone, ");
            sbAux.Append(" Email = @Email, Senha = @Senha, ");
            sbAux.Append(" cod_genero = @codGenero ");
            sbAux.Append(" IsClienteAtivo = @Situacao ");
            sbAux.Append(" where cod_id_cliente = @Id ");

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta o comando
                MySqlCommand cmd = new MySqlCommand(sbAux.ToString(), conexao);

                // Passagem de parametros 
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = cliente.nomeUsuario;
                cmd.Parameters.Add("@Nascimento", MySqlDbType.Datetime).Value = cliente.DtNascimento;
                cmd.Parameters.Add("@CPF", MySqlDbType.VarChar).Value = cliente.CPF;
                cmd.Parameters.Add("@Telefone", MySqlDbType.VarChar).Value = cliente.Telefone;
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = cliente.Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = cliente.Senha;
                cmd.Parameters.Add("@codGenero", MySqlDbType.VarChar).Value = cliente.codGenero;
                cmd.Parameters.Add("@Situacao", MySqlDbType.Binary).Value = cliente.IsClienteAtivo;

                // Executa o comando
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Cadastrar(Cliente cliente)
        {
            StringBuilder sbAux = new StringBuilder();

            // Sintaxe SQL
            sbAux.Append(" insert into tbl_cliente values ");
            sbAux.Append(" (default, @Nome, @Nascimento, @CPF, @Email, ");
            sbAux.Append("  @Telefone, @Senha, @Genero, true ) ");


            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta comando SQL
                MySqlCommand cmd = new MySqlCommand(sbAux.ToString(), conexao);

                // Passagem de parametros
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = cliente.nomeUsuario;
                cmd.Parameters.Add("@Nascimento", MySqlDbType.Datetime).Value = cliente.DtNascimento;
                cmd.Parameters.Add("@Genero", MySqlDbType.VarChar).Value = cliente.codGenero;
                cmd.Parameters.Add("@CPF", MySqlDbType.VarChar).Value = cliente.CPF;
                cmd.Parameters.Add("@Telefone", MySqlDbType.VarChar).Value = cliente.Telefone;
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = cliente.Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = cliente.Senha;

                // Executa o comando
                cmd.ExecuteNonQuery();

                // Fecha a conexão com o banco
                conexao.Close();
            }
        }

        public void Excluir(int id)
        {
            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta um comando
                MySqlCommand cmd = new MySqlCommand("delete from tbl_cliente where cod_id_cliente = @Id", conexao);

                // Passagem de parametros
                cmd.Parameters.AddWithValue("@Id", id);

                // Executa o comando
                cmd.ExecuteNonQuery();

                // Fecha a conexão com o banco
                conexao.Close();
            }
        }

        public Cliente Login(string Email, string Senha)
        {
            Cliente cliente = new Cliente();

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Comando SQL
                MySqlCommand cmd = new MySqlCommand("select * from tbl_cliente where Email = @Email and Senha = @Senha", conexao);

                // Passagem de parametros
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = Senha;

                // Adaptador para receber os dados
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                // Executa o comando
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                // Reaiza a leitura dos dados no banco 
                while (dr.Read())
                {
                    cliente.cod_id_cliente = Convert.ToInt32(dr["cod_id_cliente"]);
                    cliente.nomeUsuario = Convert.ToString(dr["nomeUsuario"]);
                    cliente.DtNascimento = Convert.ToDateTime(dr["dtNascimento"]);

                    cliente.codGenero = Convert.ToInt32(dr["cod_genero"]);
                    cliente.CPF = Convert.ToString(dr["CPF"]);
                    cliente.Telefone = Convert.ToString(dr["Telefone"]);
                    cliente.IsClienteAtivo = Convert.ToBoolean(dr["IsClienteAtivo"]);

                    cliente.Email = Convert.ToString(dr["email"]);
                    cliente.Senha = Convert.ToString(dr["Senha"]);
                }

                return cliente;
            }
        }

        public Cliente ObterCliente(int id)
        {
            Cliente cliente = new Cliente();

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta o comando 
                MySqlCommand cmd = new MySqlCommand("select * from tbl_cliente where cod_id_cliente = @Id", conexao);

                // Passagem de parametros
                cmd.Parameters.AddWithValue("@Id", id);

                // Cria um adapter
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                // Recupera os dados
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                // Salva na Model
                while (dr.Read())
                {
                    cliente.cod_id_cliente = (Int32)(dr["Id"]);
                    cliente.nomeUsuario = Convert.ToString(dr["Nome"]);
                    cliente.DtNascimento = Convert.ToDateTime(dr["Nascimento"]);
                    cliente.codGenero = Convert.ToInt32(dr["cod_genero"]);
                    cliente.CPF = Convert.ToString(dr["CPF"]);
                    cliente.Telefone = Convert.ToString(dr["Telefone"]);
                    cliente.Email = Convert.ToString(dr["Email"]);
                    cliente.Senha = Convert.ToString(dr["Senha"]);
                    cliente.IsClienteAtivo = Convert.ToBoolean(dr["IsClienteAtivo"]);
                }

                return cliente;
            }
        }

        public IEnumerable<Cliente> ObterTodosClientes()
        {
            List<Cliente> listCliente = new List<Cliente>();

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta comando SQL
                MySqlCommand cmd = new MySqlCommand("select * from tbl_cliente", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                // Recebe os dados
                da.Fill(dt);

                conexao.Close();

                // Realiza a leitura dos dados 
                foreach (DataRow dr in dt.Rows)
                {
                    listCliente.Add(
                        new Cliente
                        {
                            cod_id_cliente = Convert.ToInt32(dr["cod_id_cliente"]),
                            nomeUsuario = (string)(dr["Nome"]),
                            DtNascimento = Convert.ToDateTime(dr["dtNascimento"]),
                            codGenero = Convert.ToInt32(dr["cod_genero"]),
                            CPF = Convert.ToString(dr["CPF"]),
                            Telefone = Convert.ToString(dr["Telefone"]),
                            Email = Convert.ToString(dr["Email"]),
                            Senha = Convert.ToString(dr["Senha"]),
                            IsClienteAtivo = Convert.ToBoolean(dr["IsClienteAtivo"])
                        });
                }

                return listCliente;
            }
        }
    }
}
