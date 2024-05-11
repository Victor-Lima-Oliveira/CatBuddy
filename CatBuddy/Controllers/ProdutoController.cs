using CatBuddy.httpContext;
using CatBuddy.Models;
using CatBuddy.Repository.Contract;
using CatBuddy.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CatBuddy.Controllers
{
    public class ProdutoController : Controller
    {
        private IProdutoRepository _produtoRepository;
        private CarrinhoDeCompraCookie _carrinhoDeCompraCookie;

        protected List<Categoria> listCategoria = new List<Categoria>();
        protected List<Fornecedor> listFornecedor = new List<Fornecedor>();
        public ProdutoController(IProdutoRepository produtoRepository, CarrinhoDeCompraCookie carrinhoDeCompraCookie)
        {
            _produtoRepository = produtoRepository;
            _carrinhoDeCompraCookie = carrinhoDeCompraCookie;
        }
        public IActionResult InformacoesProduto(int id)
        {
            ViewProdutoeInformacoesNutricionais view;
            Produto produtoSelecionado;

            try
            {
                if (id != 0)
                {
                    produtoSelecionado = _produtoRepository.retornaProduto(id);

                    // Produto não encontrado
                    if (produtoSelecionado.CodIdProduto == 0)
                    {
                        return RedirectToAction("MostrarErro", "Erro", Const.ErroProdutoNaoEncontrado);
                    }

                    // prepara a view com os dados 
                    view = new ViewProdutoeInformacoesNutricionais
                    {
                        produto = produtoSelecionado,
                        PossuiInformacoesNutricionais = false
                    };

                    return View(view);
                }
                else
                {
                    return RedirectToAction("MostrarErro", "Erro", Const.ErroProdutoNaoEncontrado);
                }
            }
            catch (Exception err)
            {
                TempData[Const.ErroTempData] = err.Message;
                return RedirectToAction(Const.ErroAction, Const.ErroController);
            }

        }

        public IActionResult AdicionarItemAoCarrinho(ViewProdutoeInformacoesNutricionais produtoSelecionado)
        {
            try
            {
                // Busca os dados do produto
                Produto produto = _produtoRepository.retornaProduto(produtoSelecionado.produto.CodIdProduto);

                // Verifica se o produto existe
                if (produto == null)
                {
                    return View("Index");
                }
                else
                {
                    // Salva nos cookies apenas os dados que serão apresentados no carrinho
                    Produto produtoNoCarrinho = new Produto()
                    {
                        CodIdProduto = produto.CodIdProduto,
                        ImgPath = produto.ImgPath,
                        QtdDeProduto = produtoSelecionado.produto.QtdDeProduto,
                        NomeProduto = produto.NomeProduto,
                        Preco = produto.Preco,
                        Subtotal = (float)Math.Round(produtoSelecionado.produto.QtdDeProduto * produto.Preco, 2),
                        NomeFornecedor = produto.NomeFornecedor
                    };

                    // Salva os dados nos cookies
                    _carrinhoDeCompraCookie.AdicionarAoCarrinho(produtoNoCarrinho);

                    // Depois redireciona para o carrinho 
                    return RedirectToAction("Carrinho", "Carrinho");
                }
            }
            catch (Exception err)
            {
                TempData[Const.ErroTempData] = err.Message;
                return RedirectToAction(Const.ErroAction, Const.ErroController);
            }
        }

        [HttpGet]
        public IActionResult CadastrarProduto()
        {
            List<Categoria> listCategoriaAux;
            List<Fornecedor> listFornecedorAux;
            ViewModelCadastrarProduto view;
            Fornecedor fornecedor;
            Categoria categoria;

            try
            {
                // Retorna as listas de categoria e fornecedor
                listFornecedorAux = _produtoRepository.RetornaFornecedores();
                listCategoriaAux = _produtoRepository.RetornaCategorias();

                // Declara o primeiro valor como selecione
                fornecedor = new Fornecedor
                {
                    codFornecedor = Const.SEM_FORNECEDOR_SELECIONADO,
                    nomeFornecedor = "Selecione um fornecedor"
                };
                listFornecedor.Add(fornecedor);

                // Coloca os outros valores do banco para os fornecedores
                foreach (Fornecedor fornecedorItem in listFornecedorAux)
                {
                    listFornecedor.Add(fornecedorItem);
                }

                // Declara a primeira categoria como nula
                categoria = new Categoria
                {
                    codCategoria = Const.SEM_CATEGORIA_SELECIONADA,
                    nomeCategoria = "Selecione uma categoria"
                };
                listCategoria.Add(categoria);

                // Insere as categorias do banco na lista
                foreach (Categoria categoriaItem in listCategoriaAux)
                {
                    listCategoria.Add(categoriaItem);
                }


                // Carrega a view para apresenta na tela 
                view = new ViewModelCadastrarProduto
                {
                    listCategoria = listCategoria,
                    listFornecedor = listFornecedor
                };

                return View(view);
            }
            catch (Exception err)
            {
                TempData[Const.ErroTempData] = err.Message;
                return RedirectToAction(Const.ErroAction, Const.ErroController);
            }
        }

        [HttpPost]
        public IActionResult CadastrarProduto(ViewModelCadastrarProduto view, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    view.produto.ImgPath = GerenciadorArquivos.CadastrarImagemProduto(file);

                    _produtoRepository.insereProduto(view.produto);

                    ViewBag.AvisoPaginaPrincipal = "Produto cadastrado com sucesso!";
                    return RedirectToAction(Actions.PaginaPrincipal, Controladores.Colaborador);
                }
                else
                {
                    view.listCategoria = listCategoria;
                    view.listFornecedor = listFornecedor;
                    return View(view);
                }
            }
            catch (Exception err)
            {
                TempData[Const.ErroTempData] = err.Message;
                return RedirectToAction(Const.ErroAction, Const.ErroController);
            }
        }
    }
}
