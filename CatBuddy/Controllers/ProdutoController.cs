using CatBuddy.httpContext;
using CatBuddy.LibrariesSessao.Filtro;
using CatBuddy.LibrariesSessao.Login;
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
        private LoginCliente _loginCliente;

        protected List<Categoria> listCategoria = new List<Categoria>();
        protected List<Fornecedor> listFornecedor = new List<Fornecedor>();
        public ProdutoController(IProdutoRepository produtoRepository, CarrinhoDeCompraCookie carrinhoDeCompraCookie, LoginCliente loginCliente)
        {
            _produtoRepository = produtoRepository;
            _carrinhoDeCompraCookie = carrinhoDeCompraCookie;
            _loginCliente = loginCliente;
        }

        public IActionResult filtrarProduto(string nome = "", int codCategoria = 0, int faixapreco = 0, int codFornecedor = 0)
        {
            Produto produto = new Produto();
            if(faixapreco != 0 || codFornecedor != 0)
            {
                produto.CodFornecedor = codFornecedor;
                produto.Query = faixapreco;
            }
            else
            {
                produto.NomeProduto = nome;
                produto.CodCategoria = codCategoria;
            }

            return View(_produtoRepository.retornaProdutos(produto));
        }

        [HttpPost]
        public IActionResult filtrarProduto(string nome)
        {
            Produto produto = new Produto();
            produto.NomeProduto = nome;

            return View(_produtoRepository.retornaProdutos(produto));
        }

        public IActionResult filtrarfixo(int faixapreco, int codFornecedor)
        {
            return RedirectToAction("filtrarProduto", new {nome = "", codCategoria = 0, faixapreco = faixapreco, codFornecedor = codFornecedor });
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

        public IActionResult AdicionarItemAoCarrinho(ViewProdutoeInformacoesNutricionais produtoSelecionado, string action)
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
                        Subtotal = (float) Math.Round(produtoSelecionado.produto.QtdDeProduto * Convert.ToDouble(produto.Preco), 2),
                        NomeFornecedor = produto.NomeFornecedor
                    };

                    // Salva os dados nos cookies
                    _carrinhoDeCompraCookie.AdicionarAoCarrinho(produtoNoCarrinho);

                    // verifica se possui login na sessão
                    if (_loginCliente.ObterCliente() != null)
                    {
                        if (action == "Comprar agora")
                        {
                            return RedirectToAction("Pagamento", "Carrinho");
                        }
                        else if (action == "Adicionar ao Carrinho")
                        {
                            return RedirectToAction("InformacoesProduto", new { id = produtoSelecionado.produto.CodIdProduto });
                        }
                        else
                        {
                            return BadRequest(); // Ação desconhecida
                        }
                    }
                    else
                    {
                        return RedirectToAction("Login", "Cliente");
                    }
                }
            }
            catch (Exception err)
            {
                TempData[Const.ErroTempData] = err.Message;
                return RedirectToAction(Const.ErroAction, Const.ErroController);
            }
        }

        [HttpGet]
        [ColaboradorAutorizacao]
        public IActionResult CadastrarProduto()
        {
            try
            {
                return View(CarregaOsSelectsDosProdutos());
            }
            catch (Exception err)
            {
                TempData[Const.ErroTempData] = err.Message;
                return RedirectToAction(Const.ErroAction, Const.ErroController);
            }
        }

      

        [HttpPost]
        [ColaboradorAutorizacao]
        public IActionResult CadastrarProduto(Produto produto)
        {
            try
            {
                bool bValido = ValidaCampos(produto);
                
                if (ModelState.IsValid && bValido)
                {
                    produto.ImgPath = GerenciadorArquivos.CadastrarImagemProduto(produto.ImgProduto);

                    produto.Preco = Apoio.TransformaPreco(produto.Preco.Value);

                    _produtoRepository.insereProduto(produto);

                    MainLayout.OpenSnackbar("Produto cadastrado com sucesso!");
                    return RedirectToAction("VizualizarProdutos", "Produto");
                }
                else
                {
                    listCategoria = _produtoRepository.RetornaCategorias();
                    listFornecedor = _produtoRepository.RetornaFornecedores();
                    return View(new ViewModelProduto { listCategoria = listCategoria, listFornecedor = listFornecedor, produto = produto }) ;
                }
            }
            catch (Exception err)
            {
                TempData[Const.ErroTempData] = err.Message;
                return RedirectToAction(Const.ErroAction, Const.ErroController);
            }
        }

        private bool ValidaCampos(Produto produto)
        {
            bool bvalido = true;

            if(produto.CodCategoria == 0)
            {
                ModelState.AddModelError("produto.CodCategoria", "Escolha uma categoria para o produto");
                bvalido = false;
            }
            if(produto.CodFornecedor == 0)
            {
                ModelState.AddModelError("produto.CodFornecedor", "Escolha um fornecedor para o produto");
                bvalido = false;
            }
            if(produto.ImgProduto == null)
            {
                ModelState.AddModelError("produto.ImgProduto", "Escolha uma foto para o produto");
                bvalido = false;
            }

            return bvalido;
        }

        public IActionResult VizualizarProdutos(int pag = 1)
        {
            if (TempData[Const.AvisoPaginaVizualizarProdutos] != null)
            {
                ViewBag.AvisoPaginaVizualizarProdutos = TempData[Const.AvisoPaginaVizualizarProdutos];
            }

            if (TempData["AvisoDeletadoComSucesso"] != null)
            {
                ViewBag.AvisoPaginaVizualizarProdutos = TempData["AvisoDeletadoComSucesso"];
            }

            if (TempData["OpenDialog"] != null)
            {
                ViewBag.nomeProduto = TempData["nomeProduto"];
                ViewBag.OpenDialog = "true";
            }

            // Recebe em qual pagina está o usuario
            ViewBag.pag = pag;

            return View(_produtoRepository.retornaProdutos());
        }

        [ColaboradorAutorizacao]
        public IActionResult EditarProduto(int codProduto)
        {
            try
            {
                return View(CarregaOsSelectsDosProdutos(codProduto));
            }
            catch (Exception err)
            {
                TempData[Const.ErroTempData] = err.Message;
                return RedirectToAction(Const.ErroAction, Const.ErroController);
            }
        }

        [HttpPost]
        [ColaboradorAutorizacao]
        public IActionResult EditarProduto(ViewModelProduto view)
        {
            Produto produtoAux;
            try
            {
                if (ModelState.IsValid)
                {
                    // Se receber uma nova imagem
                    if(view.produto.ImgProduto != null && view.produto.ImgProduto.Length > 0)
                    {
                        // Recupera as informações antigas
                        produtoAux = _produtoRepository.retornaProduto(view.produto.CodIdProduto);

                        // Exclui a imagem antiga
                        GerenciadorArquivos.DeletarImagemProduto(produtoAux.ImgPath);

                        // Salva a nova foto no servidor
                        view.produto.ImgPath = GerenciadorArquivos.CadastrarImagemProduto(view.produto.ImgProduto);
                    }
     
                    // Aualiza a foto
                    _produtoRepository.atualizaProduto(view.produto);

                    TempData[Const.AvisoPaginaVizualizarProdutos] = "Produto Editado com sucesso!";
                    return RedirectToAction("VizualizarProdutos", "Produto");
                }
                else
                {
                    view.listCategoria = _produtoRepository.RetornaCategorias();
                    view.listFornecedor = _produtoRepository.RetornaFornecedores();
                    return View(view);
                }
            }
            catch (Exception err)
            {
                TempData[Const.ErroTempData] = err.Message;
                return RedirectToAction(Const.ErroAction, Const.ErroController);
            }
        }

        public IActionResult DetalhamentoProduto(int codProduto)
        {
            return View(_produtoRepository.retornaProduto(codProduto));
        }

        public IActionResult AbrirDialogDeletar(string nomeProduto, int CodProduto)
        {
            MainLayout.OpenDialog(nomeProduto, "Tem certeza que deseja DELETAR esse produto?", CodProduto);
            return RedirectToAction(nameof(VizualizarProdutos));
        }

        [ColaboradorAutorizacao]
        public IActionResult ExecutarDialog()
        {
            try
            {
                _produtoRepository.deletaProduto(Convert.ToInt32(MainLayout.ObterParametro()));

                MainLayout.CloseDialog();
            }
            catch (Exception err)
            {
                MainLayout.OpenDialog("ERRO", err.Message);
                return RedirectToAction(nameof(VizualizarProdutos));
            }

            return RedirectToAction(nameof(VizualizarProdutos));
        }

        public IActionResult CancelarDeletarProduto()
        {
            TempData["ExcluirCodProduto"] = null;
            TempData["OpenDialog"] = null;
            TempData["nomeProduto"] = null;

            return RedirectToAction(nameof(VizualizarProdutos));
        }

        private ViewModelProduto CarregaOsSelectsDosProdutos(int codIdProduto = 0)
        {
            List<Categoria> listCategoriaAux;
            List<Fornecedor> listFornecedorAux;
            ViewModelProduto view;
            Fornecedor fornecedor;
            Categoria categoria;

            // Retorna as listas de categoria e fornecedor
            listFornecedorAux = _produtoRepository.RetornaFornecedores();
            listCategoriaAux = _produtoRepository.RetornaCategorias();

            // Coloca os outros valores do banco para os fornecedores
            foreach (Fornecedor fornecedorItem in listFornecedorAux)
            {
                listFornecedor.Add(fornecedorItem);
            }

            // Insere as categorias do banco na lista
            foreach (Categoria categoriaItem in listCategoriaAux)
            {
                listCategoria.Add(categoriaItem);
            }

            if(codIdProduto != 0)
            {
                Produto produtoAux = _produtoRepository.retornaProduto(codIdProduto);
                view = new ViewModelProduto
                {
                    listCategoria = listCategoria,
                    listFornecedor = listFornecedor,
                    produto = produtoAux
                };

                return view;
            }

            // Carrega a view para apresenta na tela 
            view = new ViewModelProduto
            {
                listCategoria = listCategoria,
                listFornecedor = listFornecedor
            };

            return view;
        }
    }
}
