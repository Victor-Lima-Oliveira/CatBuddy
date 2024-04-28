using CatBuddy.Models;
using CatBuddy.Repository.Contract;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using MySql.Data.MySqlClient;
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

        public void insereProduto(Produto produto)
        {
            throw new NotImplementedException();
        }

        public Produto retornaProduto(int idProduto)
        {
            Produto produto;
            StringBuilder sbAux = new StringBuilder();
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
                    produto.Descricao = (string)mySqlDataReader["descricao"];
                    produto.QtdEstoque = (int)mySqlDataReader["qtdEstoque"];
                    produto.Idade = (string)mySqlDataReader["idade"];
                    produto.Sabor = (string)mySqlDataReader["sabor"];
                    produto.InformacoesNutricionais = (string)mySqlDataReader["informacoesNutricionais"];
                    produto.Cor = (string)mySqlDataReader["cor"];
                    produto.MedidasAproximadas = (string)mySqlDataReader["medidasAproximadas"];
                    produto.Composição = (string)mySqlDataReader["composicao"];
                    produto.Preco = (float)mySqlDataReader["preco"];
                    produto.ImgPath = (string)mySqlDataReader["imgPath"];
                    produto.NomeProduto = (string)mySqlDataReader["ds_nome"];
                    produto.CodCategoria = (int)mySqlDataReader["codFornecedor"];
                    produto.NomeCategoria = (string)mySqlDataReader["Categoria"];
                    produto.CodFornecedor = (int)mySqlDataReader["codFornecedor"];
                    produto.NomeFornecedor = (string)mySqlDataReader["fornecedor"];
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
                            InformacoesNutricionais = (string)produtoItem["informacoesNutricionais"],
                            Cor = (string)produtoItem["cor"],
                            MedidasAproximadas = (string)produtoItem["medidasAproximadas"],
                            Composição = (string)produtoItem["composicao"],
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
    }
}
