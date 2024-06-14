using System.Text;

namespace CatBuddy.Utils
{
    public static class Apoio
    {
        /// <summary>
        /// Remove os caracteres extras do CPF
        /// </summary>
        public static string TransformaCPF (string CPF)
        {
            string CPFtransformado ;

            CPFtransformado = CPF.Replace(".", "");
            CPFtransformado = CPFtransformado.Replace("-", "");

            return CPFtransformado.Trim();
        }

        /// <summary>
        /// Remove os caracteres extras do telefone
        /// </summary>
        /// <param name="Telefone"></param>
        /// <returns></returns>
        public static string TransformaTelefone (string Telefone)
        {
            string TelefoneTransformado;

            TelefoneTransformado = Telefone.Replace("(", "");
            TelefoneTransformado = TelefoneTransformado.Replace(")", "");
            TelefoneTransformado = TelefoneTransformado.Replace("-", "");
            TelefoneTransformado = TelefoneTransformado.Replace(" ", "");

            return TelefoneTransformado.Trim();
        }
    }
}
