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
            StringBuilder sbAux = new StringBuilder();

            sbAux.Append(" UPDATE tbl_produto SET  ");
            sbAux.Append(" cod_categoria = @codCategoria , descricao = @descricao, qtdEstoque = @qtdEstoque, ");
            sbAux.Append(" cod_fornecedor = @codFornecedor, ");
            sbAux.Append(" preco = @preco, ds_nome = @ds_nome ");

            // Verifica se recebeu alguma coisa, para montar a sintaxe SQL
            if (produto.Idade != null) sbAux.Append(" ,idadeRecomendada = @idadeRecomendada ");
            if (produto.Sabor != null) sbAux.Append(" ,sabor = @sabor ");
            if (produto.Cor != null) sbAux.Append(" ,cor = @cor ");
            if (produto.MedidasAproximadas != null) sbAux.Append(" ,medidasAproximadas = @medidasAproximadas ");
            if (produto.Material != null) sbAux.Append(" ,material = @material ");
            if (produto.Composicao != null) sbAux.Append(" ,composicao = @composicao ");
            if (produto.ImgPath != null) sbAux.Append(" ,imgPath = @imgPath ");
            if (produto.imgPathinfoNutricionais != null) sbAux.Append(" ,imgPathinfoNutricionais = @imgPathInfoNutricionais ");

            sbAux.Append(" where cod_id_produto = @codProduto ");

            // Monta o comando
            _SintaxeSQl = sbAux.ToString();

            // Abre a conexão
            using (var conexao = new MySqlConnection(_conexao))
            {
                MySqlCommand cmd = new MySqlCommand(_SintaxeSQl, conexao);

                // Passagem de parametros
                cmd.Parameters.Add("@codProduto", MySqlDbType.Int32).Value = produto.CodIdProduto;
                cmd.Parameters.Add("@codCategoria", MySqlDbType.Int32).Value = produto.CodCategoria;
                cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                cmd.Parameters.Add("@qtdEstoque", MySqlDbType.Int32).Value = produto.QtdEstoque;
                cmd.Parameters.Add("@codFornecedor", MySqlDbType.Int32).Value = produto.CodFornecedor;
                cmd.Parameters.Add("@preco", MySqlDbType.Float).Value = produto.Preco;
                cmd.Parameters.Add("@ds_nome", MySqlDbType.VarChar).Value = produto.NomeProduto;
                cmd.Parameters.Add("@idadeRecomendada", MySqlDbType.VarChar).Value = produto.Idade;
                cmd.Parameters.Add("@sabor", MySqlDbType.VarChar).Value = produto.Sabor;
                cmd.Parameters.Add("@cor", MySqlDbType.VarChar).Value = produto.Cor;
                cmd.Parameters.Add("@medidasAproximadas", MySqlDbType.VarChar).Value = produto.MedidasAproximadas;
                cmd.Parameters.Add("@material", MySqlDbType.VarChar).Value = produto.Material;
                cmd.Parameters.Add("@composicao", MySqlDbType.VarChar).Value = produto.Composicao;
                cmd.Parameters.Add("@imgPath", MySqlDbType.VarChar).Value = produto.ImgPath;
                cmd.Parameters.Add("@imgPathInfoNutricionais", MySqlDbType.VarChar).Value = produto.imgPathinfoNutricionais;

                // Abrir conexão com o banco
                conexao.Open();

                // Executar o insert e recuperar o ID
                cmd.ExecuteNonQuery();

                // Fechar conexão com o banco 
                conexao.Close();
            }

        }

        public void deletaProduto(int codProduto)
        {
            StringBuilder sbAux = new StringBuilder();

            sbAux.Append(" DELETE FROM tbl_produto ");
            sbAux.Append(" WHERE cod_id_produto = @codProduto ");

            _SintaxeSQl = sbAux.ToString();

            using (var conexao = new MySqlConnection(_conexao))
            {
                MySqlCommand cmd = new MySqlCommand(_SintaxeSQl, conexao);

                cmd.Parameters.Add("@codProduto", MySqlDbType.Int32).Value = codProduto;

                conexao.Open();

                cmd.ExecuteNonQuery();

                conexao.Close();
            }
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

        public IEnumerable<Produto> retornaProdutos(Produto produto = null)
        {
            List<Produto> listProduto = new List<Produto>();
            StringBuilder sbAux = new StringBuilder();
            DataTable dataTable;
            MySqlDataAdapter mySqlDataAdapter;

            // Montar a sintaxe SQL 
            sbAux.Append(" SELECT * FROM vwproduto ");
            sbAux.Append(" WHERE IsProdutoAtivo = true");

            
            if (produto != null)
            {
                if (!String.IsNullOrEmpty(produto.NomeProduto))
                {
                    sbAux.Append(" AND ds_nome LIKE @nome ");
                }

                if(produto.CodCategoria != 0)
                {
                    sbAux.Append(" AND codCategoria = @codCategoria");
                }
            }

            // Ordena pela código do produto
            sbAux.Append(" ORDER BY cod_id_produto DESC ");

            // Juntar os dados SQL 
            _SintaxeSQl = sbAux.ToString();

            using (var conexao = new MySqlConnection(_conexao))
            {
                // Instancia do Data Table
                dataTable = new DataTable();

                // Abre conexão com o banco
                conexao.Open();

                // comando My SQL
                MySqlCommand cmd = new MySqlCommand(_SintaxeSQl, conexao);

                if(produto != null)
                {
                    cmd.Parameters.AddWithValue("@nome", "%" + produto.NomeProduto + "%");
                    cmd.Parameters.AddWithValue("@codCategoria", produto.CodCategoria);
                }

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
                            Descricao = produtoItem["descricao"].ToString(),
                            QtdEstoque = (int)produtoItem["qtdEstoque"],
                            Idade = produtoItem["idadeRecomendada"].ToString(),
                            Sabor = produtoItem["sabor"].ToString(),
                            Cor = produtoItem["cor"].ToString(),
                            MedidasAproximadas = produtoItem["medidasAproximadas"].ToString(),
                            Composicao = produtoItem["composicao"].ToString(),
                            Preco = (float)produtoItem["preco"],
                            ImgPath = produtoItem["imgPath"].ToString(),
                            NomeProduto = produtoItem["ds_nome"].ToString(),
                            CodCategoria = (int)produtoItem["codFornecedor"],
                            NomeCategoria = produtoItem["Categoria"].ToString(),
                            CodFornecedor = (int)produtoItem["codFornecedor"],
                            NomeFornecedor = produtoItem["fornecedor"].ToString()
                        });
                }

                conexao.Close();
            }

            if(listProduto == null)
            {
                return new List<Produto>();
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
