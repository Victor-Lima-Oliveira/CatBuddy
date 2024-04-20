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

            sbAux.Append(" SELECT * FROM tbl_produto ");
            sbAux.Append(" WHERE cod_id_produto = @CodIdProduto ");


            // Montando a sintaxe do SQL
            _SintaxeSQl = sbAux.ToString();

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
                    produto.CodIdProduto = (int) mySqlDataReader["cod_id_produto"];
                    produto.CodCategoria = (int)mySqlDataReader["cod_categoria"];
                    produto.Descricao = (string)mySqlDataReader["descricao"];
                    produto.QtdEstoque = (int)mySqlDataReader["qtdEstoque"];
                    produto.CodFornecedor = (int)mySqlDataReader["cod_fornecedor"];
                    produto.Idade = (string)mySqlDataReader["idade"];
                    produto.Sabor = (string)mySqlDataReader["sabor"];
                    produto.InformacoesNutricionais = (string)mySqlDataReader["informacoesNutricionais"];
                    produto.Cor = (string)mySqlDataReader["cor"];
                    produto.MedidasAproximadas = (string)mySqlDataReader["medidasAproximadas"];
                    produto.Composição = (string)mySqlDataReader["composicao"];
                    produto.Preco = (float)mySqlDataReader["preco"];
                    produto.ImgPath = (string)mySqlDataReader["imgPath"];
                    produto.DsNome = (string)mySqlDataReader["ds_nome"];
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
                            CodCategoria = (int)produtoItem["cod_categoria"],
                            Descricao = (string)produtoItem["descricao"],
                            QtdEstoque = (int)produtoItem["qtdEstoque"],
                            CodFornecedor = (int)produtoItem["cod_fornecedor"],
                            Idade = (string)produtoItem["idade"],
                            Sabor = (string)produtoItem["sabor"],
                            InformacoesNutricionais = (string)produtoItem["informacoesNutricionais"],
                            Cor = (string)produtoItem["cor"],
                            MedidasAproximadas = (string)produtoItem["medidasAproximadas"],
                            Composição = (string)produtoItem["composicao"],
                            Preco = (float)produtoItem["preco"],
                            ImgPath = (string)produtoItem["imgPath"],
                            DsNome = (string)produtoItem["ds_nome"]
                        });
                }

                conexao.Close();
            }

            return listProduto;
        }
    }
}
