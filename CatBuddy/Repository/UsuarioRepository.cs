using CatBuddy.Models;
using CatBuddy.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace CatBuddy.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _conexao;
        private string _SintaxeSQl;

        public UsuarioRepository(IConfiguration configuration)
        {
            _conexao = configuration.GetConnectionString("ConexaoMySQL");
        }

        public List<Genero> RetornaGenero()
        {
            List<Genero> listGenero = new List<Genero>();
            Genero genero;
            MySqlDataAdapter mySqlDataAdapter;
            MySqlDataReader mySqlDataReader;

            // Montando a sintaxe do SQL
            _SintaxeSQl = "SELECT * FROM tbl_genero order by cod_id_genero";

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
                    genero = new Genero()
                    {
                        cod_id_genero = (int)mySqlDataReader["cod_id_genero"],
                        ds_genero = mySqlDataReader["ds_genero"].ToString()
                    };

                    listGenero.Add(genero);
                }
            }
            return listGenero;
        }

        public List<Logradouro> RetornaLogradouro()
        {
            List<Logradouro> listGenero = new List<Logradouro>();
            Logradouro logradouro;
            MySqlDataAdapter mySqlDataAdapter;
            MySqlDataReader mySqlDataReader;

            // Montando a sintaxe do SQL
            _SintaxeSQl = "SELECT * FROM tbl_logradouro order by cod_id_logradouro";

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
                    logradouro = new Logradouro()
                    {
                        codLogradouro = (int)mySqlDataReader["cod_id_logradouro"],
                        nomeLogradouro = mySqlDataReader["nomeLogradouro"].ToString()
                    };

                    listGenero.Add(logradouro);
                }
            }
            return listGenero;
        }
    }
}
