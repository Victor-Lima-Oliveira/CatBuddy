using CatBuddy.Models;
using CatBuddy.Repository.Contract;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.Data;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace CatBuddy.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        // Conexao com o banco 
        private readonly string _conexao;

        // Variável de apoio para a sintaxe SQL
        private string _SintaxeSQl;
        public ProdutoRepository(IConfiguration configuration)
        {
            _conexao = configuration.GetConnectionString("ConexaoMySQL");
        }
        public void atualizaProduto(Produto produto)
        {
            throw new NotImplementedException();
        }

        public void deletaProduto(int id)
        {
            throw new NotImplementedException();
        }

        public int insereProduto(Produto produto)
        {
            StringBuilder sbAux = new StringBuilder();
            int idProduto;

            sbAux.Append(" INSERT INTO tbl_produto VALUE( ");
            sbAux.Append(" null, @codCategoria, @descricao, @qtdEstoque, @codFornecedor, ");
            sbAux.Append(" @idadeRecomendada, @sabor, @cor, @medidasAproximadas, @material, ");
            sbAux.Append(" @composicao, @preco, @imgPath, @imgPathInfoNutricionais, @ds_nome);  ");
            sbAux.Append(" SELECT LAST_INSERT_ID(); ");

            _SintaxeSQl = sbAux.ToString();

            using (var conexao = new MySqlConnection(_conexao))
            {

                MySqlCommand cmd = new MySqlCommand(_SintaxeSQl, conexao);

                // Passagem de parametros
                cmd.Parameters.Add("@codCategoria", MySqlDbType.Int32).Value = produto.CodCategoria;
                cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                cmd.Parameters.Add("@qtdEstoque", MySqlDbType.Int32).Value = produto.QtdEstoque;
                cmd.Parameters.Add("@codFornecedor", MySqlDbType.Int32).Value = produto.CodFornecedor;
                cmd.Parameters.Add("@idadeRecomendada", MySqlDbType.VarChar).Value = produto.Idade;
                cmd.Parameters.Add("@sabor", MySqlDbType.VarChar).Value = produto.Sabor;
                cmd.Parameters.Add("@cor", MySqlDbType.VarChar).Value = produto.Cor;
                cmd.Parameters.Add("@medidasAproximadas", MySqlDbType.VarChar).Value = produto.MedidasAproximadas;
                cmd.Parameters.Add("@material", MySqlDbType.VarChar).Value = produto.Material;
                cmd.Parameters.Add("@composicao", MySqlDbType.VarChar).Value = produto.Composicao;
                cmd.Parameters.Add("@preco", MySqlDbType.VarChar).Value = produto.Preco;
                cmd.Parameters.Add("@imgPath", MySqlDbType.VarChar).Value = produto.ImgPath;
                cmd.Parameters.Add("@imgPathInfoNutricionais", MySqlDbType.VarChar).Value = produto.imgPathinfoNutricionais;
                cmd.Parameters.Add("@ds_nome", MySqlDbType.VarChar).Value = produto.NomeProduto;

                // Abrir conexão com o banco
                conexao.Open();

                // Executar o insert e recuperar o ID
                idProduto = Convert.ToInt32(cmd.ExecuteScalar());

                // Fechar conexão com o banco 
                conexao.Close();
            }

            return idProduto;
        }

      
        public Produto retornaProduto(int idProduto)
        {
            Produto produto;
            MySqlDataAdapter mySqlDataAdapter;
            MySqlDataReader mySqlDataReader;

            // Montando a sintaxe do SQL
            _SintaxeSQl = "SELECT * FROM vwProduto WHERE cod_id_produto = @CodIdProduto";

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand(_SintaxeSQl, conexao);

                //Adicionar parametros
                cmd.Parameters.Add("@CodIdProduto", MySqlDbType.Int32).Value = idProduto;

                // Adapter para o comando
                mySqlDataAdapter = new MySqlDataAdapter(cmd);

                // Instancia do produto
                produto = new Produto();

                // leitor dos dados do livro
                mySqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                // Gravar os dados na model
                while (mySqlDataReader.Read())
                {
                    produto.CodIdProduto = (int)mySqlDataReader["cod_id_produto"];
                    produto.Descricao = mySqlDataReader["descricao"].ToString();
                    produto.QtdEstoque = (int)mySqlDataReader["qtdEstoque"];
                    produto.Idade = mySqlDataReader["idadeRecomendada"].ToString();
                    produto.Sabor = mySqlDataReader["sabor"].ToString();
                    produto.Cor = mySqlDataReader["cor"].ToString();
                    produto.MedidasAproximadas = mySqlDataReader["medidasAproximadas"].ToString();
                    produto.Composicao = mySqlDataReader["composicao"].ToString();
                    produto.Preco = (float)mySqlDataReader["preco"];
                    produto.ImgPath = mySqlDataReader["imgPath"].ToString();
                    produto.NomeProduto = mySqlDataReader["ds_nome"].ToString();
                    produto.CodCategoria = (int)mySqlDataReader["codCategoria"];
                    produto.NomeCategoria = mySqlDataReader["Categoria"].ToString();
                    produto.CodFornecedor = (int)mySqlDataReader["codFornecedor"];
                    produto.NomeFornecedor = mySqlDataReader["fornecedor"].ToString();
                    produto.imgPathinfoNutricionais = mySqlDataReader["imgPathinfoNutricionais"].ToString();
                }
            }
            return produto;
        }

        public IEnumerable<Produto> retornaProdutos()
        {
            List<Produto> listProduto = new List<Produto>();
            StringBuilder sbAux = new StringBuilder();
            DataTable dataTable;
            MySqlDataAdapter mySqlDataAdapter;

            // Montar a sintaxe SQL 
            // TODO: realizar as restrições por categoria
            sbAux.Append(" SELECT * FROM tbl_produto");

            // Juntar os dados SQL 
            _SintaxeSQl = sbAux.ToString();

            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // Instancia do Data Table
                dataTable = new DataTable();

                // comando My SQL
                MySqlCommand cmd = new MySqlCommand(_SintaxeSQl, conexao);

                // Recuperação dos dados em um adapter
                mySqlDataAdapter = new MySqlDataAdapter(cmd);

                //Passar dados para o Data Table
                mySqlDataAdapter.Fill(dataTable);

                // Inserir dados na lista que será retornada
                foreach (DataRow produtoItem in dataTable.Rows)
                {
                    listProduto.Add(
                        new Produto
                        {
                            CodIdProduto = (int)produtoItem["cod_id_produto"],
                            Descricao = (string)produtoItem["descricao"],
                            QtdEstoque = (int)produtoItem["qtdEstoque"],
                            Idade = (string)produtoItem["idade"],
                            Sabor = (string)produtoItem["sabor"],
                            Cor = (string)produtoItem["cor"],
                            MedidasAproximadas = (string)produtoItem["medidasAproximadas"],
                            Composicao = (string)produtoItem["composicao"],
                            Preco = (float)produtoItem["preco"],
                            ImgPath = (string)produtoItem["imgPath"],
                            NomeProduto = (string)produtoItem["ds_nome"],
                            CodCategoria = (int)produtoItem["codFornecedor"],
                            NomeCategoria = (string)produtoItem["Categoria"],
                            CodFornecedor = (int)produtoItem["codFornecedor"],
                            NomeFornecedor = (string)produtoItem["fornecedor"]
                        });
                }

                conexao.Close();
            }

            return listProduto;
        }
        public void VendeProduto(int codProduto, int qtdEstoque)
        {
            StringBuilder sbAux = new StringBuilder();

            // Sintaxe SQL
            sbAux.Append(" UPDATE tbl_produto SET qtdEstoque = @qtdEstoque ");
            sbAux.Append(" WHERE cod_id_produto = @codIdProduto ");

            // Monta o comando
            _SintaxeSQl = sbAux.ToString();

            using (var conexao = new MySqlConnection(_conexao))
            {
                // Comando MySQL
                MySqlCommand cmd = new MySqlCommand(_SintaxeSQl, conexao);

                // Passagem de parametros
                cmd.Parameters.Add("@codIdProduto", MySqlDbType.Int32).Value = codProduto;
                cmd.Parameters.Add("@qtdEstoque", MySqlDbType.Int32).Value = qtdEstoque;

                // Abre a conexao
                conexao.Open();

                // Insere o pedido e retorna o numero do pedido
                cmd.ExecuteNonQuery();

                conexao.Close();
            }
        }
        public List<Categoria> RetornaCategorias()
        {
            List<Categoria> listCategoria = new List<Categoria>();
            Categoria categoria;
            MySqlDataAdapter mySqlDataAdapter;
            MySqlDataReader mySqlDataReader;

            // Montando a sintaxe do SQL
            _SintaxeSQl = "SELECT * FROM tbl_categoria";

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
                    categoria = new Categoria
                    {
                        codCategoria = (int)mySqlDataReader["cod_id_categoria"],
                        nomeCategoria = mySqlDataReader["nomeCategoria"].ToString()
                    };

                    listCategoria.Add(categoria);
                }
            }
            return listCategoria;
        }
        public List<Fornecedor> RetornaFornecedores()
        {
            List<Fornecedor> listfornecedor = new List<Fornecedor>();
            Fornecedor fornecedor;
            MySqlDataAdapter mySqlDataAdapter;
            MySqlDataReader mySqlDataReader;

            // Montando a sintaxe do SQL
            _SintaxeSQl = "SELECT * FROM tbl_fornecedor";

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
                    fornecedor = new Fornecedor
                    {
                        codFornecedor = (int)mySqlDataReader["cod_id_fornecedor"],
                        nomeFornecedor = mySqlDataReader["nomeFantasia"].ToString(),
                        cnpj = mySqlDataReader["cnpj"].ToString()
                    };

                    listfornecedor.Add(fornecedor);
                }
            }

            return listfornecedor;
        }
    }
}
