using CatBuddy.LibrariesSessao.Filtro;
using CatBuddy.Models;
using CatBuddy.Repository.Contract;
using CatBuddy.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CatBuddy.Controllers
{
    public class FornecedorController : Controller
    {
        private IFornecedorRepository _fornecedorRepository;
        private IUsuarioRepository _usuarioRepository;
        public FornecedorController(IFornecedorRepository fornecedorRepository, IUsuarioRepository usuarioRepository)
        {
            _fornecedorRepository = fornecedorRepository;
            _usuarioRepository = usuarioRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [ColaboradorAutorizacao]
        public IActionResult CadastrarForncedor()
        {
            return View(CarregaViewColaborador());
        }

        [HttpPost]
        [ColaboradorAutorizacao]
        public IActionResult CadastrarForncedor(Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                // Remove os caracteres extras 
                fornecedor.cnpj = Apoio.TransformaCNPJ(fornecedor.cnpj);
                fornecedor.Telefone = Apoio.TransformaTelefone(fornecedor.Telefone);
                fornecedor.cep = Apoio.TransformaCEP(fornecedor.cep);

                // Cadastra o novo fornecedor
                _fornecedorRepository.Cadastrar(fornecedor);

                return RedirectToAction(nameof(VisualizarFornecedor));
            }

            return View(CarregaViewColaborador(fornecedor));
        }

        [ColaboradorAutorizacao]
        public IActionResult DetalheFornecedor(int id)
        {
            return View(_fornecedorRepository.ObterFornecedor(id));
        }

        [ColaboradorAutorizacao]
        public IActionResult VisualizarFornecedor()
        {
            return View(_fornecedorRepository.ObterFornecedores());
        }

        [ColaboradorAutorizacao]
        public IActionResult EditarFornecedor(int id)
        {
            Fornecedor fornecedor = _fornecedorRepository.ObterFornecedor(id).Fornecedor;
            return View(CarregaViewColaborador(fornecedor));
        }

        [ColaboradorAutorizacao]
        [HttpPost]
        public IActionResult EditarFornecedor(Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                fornecedor.cnpj = Apoio.TransformaCNPJ(fornecedor.cnpj);
                fornecedor.Telefone = Apoio.TransformaTelefone(fornecedor.Telefone);
                fornecedor.cep = Apoio.TransformaCEP(fornecedor.cep);

                // Atualiza os dados 
                _fornecedorRepository.Atualizar(fornecedor);

                return RedirectToAction(nameof(VisualizarFornecedor));
            }
            return RedirectToAction(nameof(VisualizarFornecedor));
        }

        public ViewFornecedor CarregaViewColaborador(Fornecedor fornecedor = null)
        {
            // Recupera os logradouros do banco
            List<Logradouro> ListLogradouro = _usuarioRepository.RetornaLogradouro();

            // Carrega a view com um fornecedor nulo
            ViewFornecedor viewFornecedor = new ViewFornecedor()
            {
                ListLogradouro = ListLogradouro
            };

            // Se receber um fornecedor, carrega ele na model
            if (fornecedor != null)
            {
                viewFornecedor.Fornecedor = fornecedor;
            }

            return viewFornecedor;
        }
    }
}
