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
        public ProdutoController(IProdutoRepository produtoRepository, CarrinhoDeCompraCookie carrinhoDeCompraCookie)
        {
            _produtoRepository = produtoRepository;
            _carrinhoDeCompraCookie = carrinhoDeCompraCookie;
        }
        public IActionResult InformacoesProduto(int id)
        {
            ViewProdutoeInformacoesNutricionais view;
            Produto produtoSelecionado;
            InfoNutricionais infoNutricionais;
            int categoriaAux;
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
                    else
                    {
                        // Variavel para facilitar a vizualização do código
                        categoriaAux = produtoSelecionado.CodCategoria;

                        // Se for um alimento
                        if (categoriaAux == Const.RacaoUmida || categoriaAux == Const.RacaoSeca || categoriaAux == Const.Petisco)
                        {
                            // Busca as informações nutricionais 
                            infoNutricionais = _produtoRepository.RetornaInformacacoesNutricionais(produtoSelecionado.CodIdProduto);

                            // Se não tiver cadastrado as informações nutricionais
                            if (infoNutricionais.cod_produto == 0)
                            {
                                view = new ViewProdutoeInformacoesNutricionais
                                {
                                    produto = produtoSelecionado,
                                    PossuiInformacoesNutricionais = false
                                };
                            }
                            else
                            {
                                view = new ViewProdutoeInformacoesNutricionais
                                {
                                    produto = produtoSelecionado,
                                    infoNutricionais = infoNutricionais,
                                    PossuiInformacoesNutricionais = true
                                };
                            }

                        }
                        // Se não for um alimento
                        else
                        {
                            // prepara a view com os dados 
                            view = new ViewProdutoeInformacoesNutricionais
                            {
                                produto = produtoSelecionado,
                                PossuiInformacoesNutricionais = false
                            };
                        }

                        return View(view);
                    }
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

    }
}
