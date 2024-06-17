using CatBuddy.Models;
using CatBuddy.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace CatBuddy.Repository
{
    public class FornecedorRepository : IFornecedorRepository
    {
        // Propriedade privada para injetar Banco de Dados
        private readonly string _conexao;

        // Variável auxiliar para facilitar visualização do código 
        private string _SintaxeSQl;

        // Método Construtor da classe ClienteRepository
        public FornecedorRepository(IConfiguration configuration)
        {
            // Injeção da dependência do Banco de Dados
            _conexao = configuration.GetConnectionString("ConexaoMySQL");
        }

        public void Atualizar(Fornecedor fornecedor)
        {
            StringBuilder sbAux = new StringBuilder();

            // Sintaxe SQL 
            sbAux.Append(" update tbl_fornecedor set cnpj = @cnpj, ");
            sbAux.Append(" nomeFantasia = @Nome,  ");
            sbAux.Append(" telefone = @telefone, ");
            sbAux.Append(" endereco = @endereco, bairro = @bairro,  ");
            sbAux.Append(" cep = @cep, municipio = @municipio,  ");
            sbAux.Append(" cod_logradouro = @codLogradouro  ");
            sbAux.Append(" where cod_id_fornecedor = @Id ");

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta o comando
                MySqlCommand cmd = new MySqlCommand(sbAux.ToString(), conexao);

                // Passagem de parametros
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = fornecedor.codFornecedor;
                cmd.Parameters.Add("@cnpj", MySqlDbType.VarChar).Value = fornecedor.cnpj;
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = fornecedor.nomeFornecedor;
                cmd.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = fornecedor.Telefone;
                cmd.Parameters.Add("@endereco", MySqlDbType.VarChar).Value = fornecedor.endereco;
                cmd.Parameters.Add("@bairro", MySqlDbType.VarChar).Value = fornecedor.bairro;
                cmd.Parameters.Add("@cep", MySqlDbType.VarChar).Value = fornecedor.cep;
                cmd.Parameters.Add("@municipio", MySqlDbType.VarChar).Value = fornecedor.municipio;
                cmd.Parameters.Add("@codLogradouro", MySqlDbType.Int32).Value = fornecedor.CodLogradouro;

                // Executa o comando
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Cadastrar(Fornecedor fornecedor)
        {
            StringBuilder sbAux = new StringBuilder();

            // Sintaxe SQL 
            sbAux.Append(" insert into tbl_fornecedor values ");
            sbAux.Append(" (default, @Cnpj, @Nome, @telefone, ");
            sbAux.Append(" @endereco, @bairro, @cep, @codLogradouro, ");
            sbAux.Append(" @municipio, true) ");

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta o comando
                MySqlCommand cmd = new MySqlCommand(sbAux.ToString(), conexao);

                // Passagem de parametros
                cmd.Parameters.Add("@Cnpj", MySqlDbType.VarChar).Value = fornecedor.cnpj;
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = fornecedor.nomeFornecedor;
                cmd.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = fornecedor.Telefone;
                cmd.Parameters.Add("@endereco", MySqlDbType.VarChar).Value = fornecedor.endereco;
                cmd.Parameters.Add("@bairro", MySqlDbType.VarChar).Value = fornecedor.bairro;
                cmd.Parameters.Add("@cep", MySqlDbType.VarChar).Value = fornecedor.cep;
                cmd.Parameters.Add("@codLogradouro", MySqlDbType.Int32).Value = fornecedor.CodLogradouro;
                cmd.Parameters.Add("@municipio", MySqlDbType.VarChar).Value = fornecedor.municipio;

                // Executa o comando
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Excluir(int Id)
        {
            throw new NotImplementedException();
        }

        public ViewFornecedor ObterFornecedor(int Id)
        {
            ViewFornecedor viewFornecedor = null;

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta o comando
                MySqlCommand cmd = new MySqlCommand("select * from vwfornecedor where cod_id_fornecedor = @Id", conexao);

                // Passagem de parametros
                cmd.Parameters.AddWithValue("@Id", Id);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                // Carrega a model
                while (dr.Read())
                {
                    viewFornecedor = new ViewFornecedor()
                    {
                        Fornecedor = new Fornecedor()
                        {
                            codFornecedor = Convert.ToInt32(dr["cod_id_fornecedor"]),
                            nomeFornecedor = (string)(dr["nomeFantasia"]),
                            cnpj = (string)(dr["cnpj"]),
                            endereco = (string)(dr["endereco"]),
                            bairro = Convert.ToString(dr["bairro"]),
                            Telefone = Convert.ToString(dr["telefone"]),
                            cep = Convert.ToString(dr["cep"]),
                            CodLogradouro = Convert.ToInt32(dr["cod_logradouro"]),
                            municipio = Convert.ToString(dr["municipio"]),
                            IsFornecedorAtivo = Convert.ToBoolean(dr["IsFornecedorAtivo"])
                        },
                        nomeLogradouro = Convert.ToString(dr["nomeLogradouro"])
                    };
                };

                return viewFornecedor;
            }
        }

        public List<ViewFornecedor> ObterFornecedores()
        {
            List<ViewFornecedor> listForncedor = new List<ViewFornecedor>();

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Monta o comando
                MySqlCommand cmd = new MySqlCommand("select * from vwFornecedor where IsFornecedorAtivo = true ", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                conexao.Close();

                // Carrega a lista 
                foreach (DataRow dr in dt.Rows)
                {
                    listForncedor.Add(
                        new ViewFornecedor
                        {
                            Fornecedor = new Fornecedor()
                            {
                                codFornecedor = Convert.ToInt32(dr["cod_id_fornecedor"]),
                                nomeFornecedor = (string)(dr["nomeFantasia"]),
                                cnpj = (string)(dr["cnpj"]),
                                endereco = (string)(dr["endereco"]),
                                bairro = Convert.ToString(dr["bairro"]),
                                Telefone = Convert.ToString(dr["telefone"]),
                                cep = Convert.ToString(dr["cep"]),
                                CodLogradouro = Convert.ToInt32(dr["cod_logradouro"]),
                                municipio = Convert.ToString(dr["municipio"]),
                                IsFornecedorAtivo = Convert.ToBoolean(dr["IsFornecedorAtivo"])
                            },
                            nomeLogradouro = Convert.ToString(dr["nomeLogradouro"])
                        });
                        
                }
                return listForncedor;
            }
        }
    }
}
