using CatBuddy.Models;
using CatBuddy.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace CatBuddy.Repository
{
    public class ColaboradorRepository : IColaboradorRepository
    {
        // Propriedade privada para injetar Banco de Dados
        private readonly string _conexao;

        // Variável auxiliar para facilitar visualização do código 
        private string _SintaxeSQl;

        // Método Construtor da classe ClienteRepository
        public ColaboradorRepository(IConfiguration configuration)
        {
            // Injeção da dependência do Banco de Dados
            _conexao = configuration.GetConnectionString("ConexaoMySQL");
        }

        public Colaborador Login(string Email, string Senha)
        {
            Colaborador colaborador;

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta o comando 
                MySqlCommand cmd = new MySqlCommand("select * from tbl_colaborador where Email = @Email and Senha = @Senha", conexao);

                // Passagem de parametros
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = Senha;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                // Recebe os dados
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                // Carrega os dados e carrega na Model
                while (dr.Read())
                {
                    colaborador = new Colaborador()
                    {
                        cod_id_colaborador = Convert.ToInt32(dr["cod_id_colaborador"]),
                        nomeColaborador = Convert.ToString(dr["nomeColaborador"].ToString()),
                        Email = Convert.ToString(dr["email"]),
                        Senha = Convert.ToString(dr["senha"]),
                        NivelDeAcesso = Convert.ToInt32(dr["cod_nivelDeAcesso"]),
                        CPF = Convert.ToString(dr["CPF"]),
                        codGenero = Convert.ToInt32(dr["cod_genero"]),
                        Telefone = Convert.ToString(dr["telefone"])
                    };

                    // Retorna o colaborador do banco
                    return colaborador;
                }

                // Retorna um colaborador Nulo
                return colaborador = new Colaborador();
            }
        }

        public void Cadastrar(Colaborador colaborador)
        {
            StringBuilder sbAux = new StringBuilder();

            // Sintaxe SQL 
            sbAux.Append(" insert into tbl_colaborador values ");
            sbAux.Append(" (default, @Nome, @Email, @CPF, @telefone, ");
            sbAux.Append(" @Senha, @NivelDeAcesso, @codGenero,  1 ) ");

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta o comando
                MySqlCommand cmd = new MySqlCommand(sbAux.ToString(), conexao);

                // Passagem de parametros
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = colaborador.nomeColaborador;
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = colaborador.Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = colaborador.Senha;
                cmd.Parameters.Add("@CPF", MySqlDbType.VarChar).Value = colaborador.CPF;
                cmd.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = colaborador.Telefone;
                cmd.Parameters.Add("@NivelDeAcesso", MySqlDbType.Int32).Value = colaborador.NivelDeAcesso;
                cmd.Parameters.Add("@codGenero", MySqlDbType.Int32).Value = colaborador.codGenero;

                // Executa o comando
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public IEnumerable<Colaborador> ObterTodosColaboradores()
        {
            List<Colaborador> listColaborador = new List<Colaborador>();

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta o comando
                MySqlCommand cmd = new MySqlCommand("select * from tbl_colaborador where IsColaboradorAtivo = true ", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                conexao.Close();

                // Carrega a lista 
                foreach (DataRow dr in dt.Rows)
                {
                    listColaborador.Add(
                        new Colaborador
                        {
                            cod_id_colaborador = Convert.ToInt32(dr["cod_id_colaborador"]),
                            nomeColaborador = (string)(dr["nomeColaborador"]),
                            Senha = (string)(dr["Senha"]),
                            Email = (string)(dr["Email"]),
                            NivelDeAcesso = Convert.ToInt32(dr["cod_nivelDeAcesso"]),
                            Telefone = Convert.ToString(dr["telefone"]),
                            CPF = Convert.ToString(dr["CPF"]),
                            codGenero = Convert.ToInt32(dr["cod_genero"]),
                            IsColaboradorAtivo = Convert.ToBoolean(dr["IsColaboradorAtivo"])
                        });
                }
                return listColaborador;
            }
        }

        public Colaborador ObterColaborador(int Id)
        {
            Colaborador colaborador;

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta o comando
                MySqlCommand cmd = new MySqlCommand("select * from tbl_colaborador where cod_id_colaborador = @Id", conexao);

                // Passagem de parametros
                cmd.Parameters.AddWithValue("@Id", Id);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                // Carrega a model
                while (dr.Read())
                {
                    colaborador = new Colaborador()
                    {
                        cod_id_colaborador = (Int32)(dr["cod_id_colaborador"]),
                        nomeColaborador = (string)(dr["nomeColaborador"]),
                        Email = (string)(dr["Email"]),
                        Senha = (string)(dr["Senha"]),
                        NivelDeAcesso = Convert.ToInt32(dr["cod_nivelDeAcesso"]),
                        CPF = dr["CPF"].ToString(),
                        Telefone = Convert.ToString(dr["telefone"]),
                        codGenero = Convert.ToInt32(dr["cod_genero"]),
                        IsColaboradorAtivo = Convert.ToBoolean(dr["IsColaboradorAtivo"])
                    };

                    return colaborador;
                }

                // se não recuperar, tras o colaborador vazio
                return colaborador = new Colaborador();
            }
        }

        public void Atualizar(Colaborador colaborador)
        {
            StringBuilder sbAux = new StringBuilder();

            // Sintaxe SQL 
            sbAux.Append(" update tbl_colaborador set nomeColaborador = @Nome, ");
            sbAux.Append(" Email = @Email, ");
            sbAux.Append(" cod_nivelDeAcesso = @NivelDeAcesso, ");
            sbAux.Append(" CPF = @CPF, telefone = @telefone, ");
            sbAux.Append(" cod_genero = @codGenero ");
            sbAux.Append(" where cod_id_colaborador = @Id ");

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta o comando
                MySqlCommand cmd = new MySqlCommand(sbAux.ToString(), conexao);

                // Passagem de parametros
                cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = colaborador.cod_id_colaborador;
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = colaborador.nomeColaborador;
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = colaborador.Email;
                cmd.Parameters.Add("@NivelDeAcesso", MySqlDbType.Int32).Value = colaborador.NivelDeAcesso;
                cmd.Parameters.Add("@CPF", MySqlDbType.VarChar).Value = colaborador.CPF;
                cmd.Parameters.Add("@telefone", MySqlDbType.String).Value = colaborador.Telefone;
                cmd.Parameters.Add("@codGenero", MySqlDbType.Int32).Value = colaborador.codGenero;

                // Executa o comando
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void AtualizarSenha(Colaborador colaborador)
        {
            throw new NotImplementedException();
        }

        public void Excluir(int Id)
        {
            StringBuilder sbAux = new StringBuilder();

            // Sintaxe SQL 
            sbAux.Append(" update tbl_colaborador set IsColaboradorAtivo = false ");
            sbAux.Append(" where cod_id_colaborador = @Id ");

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta o comando
                MySqlCommand cmd = new MySqlCommand(sbAux.ToString(), conexao);

                // Passagem de parametros
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = Id;

                // Executa o comando
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }


        public List<Colaborador> ObterColaboradorPorEmail(string Email)
        {
            throw new NotImplementedException();
        }

        public List<NivelAcesso> ObtemNivelDeAcesso()
        {
            List<NivelAcesso> listGenero = new List<NivelAcesso>();
            NivelAcesso nivelAcesso;
            MySqlDataAdapter mySqlDataAdapter;
            MySqlDataReader mySqlDataReader;

            // Montando a sintaxe do SQL
            _SintaxeSQl = "SELECT * FROM tbl_niveldeacesso";

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand(_SintaxeSQl, conexao);

                // Adapter para o comando
                mySqlDataAdapter = new MySqlDataAdapter(cmd);

                // leitor dos dados 
                mySqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                // Gravar os dados na model
                while (mySqlDataReader.Read())
                {
                    nivelAcesso = new NivelAcesso()
                    {
                        cod_id_NivelAcesso = (int)mySqlDataReader["cod_id_nivelAcesso"],
                        ds_nivelAcesso = mySqlDataReader["ds_nivelAcesso"].ToString()
                    };

                    listGenero.Add(nivelAcesso);
                }
            }
            return listGenero;
        }

    }
}
