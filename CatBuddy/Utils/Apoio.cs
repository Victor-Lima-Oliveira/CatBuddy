using System.Text;

namespace CatBuddy.Utils
{
    public static class Apoio
    {

        /// <summary>
        /// Remove os caracteres extras do CPF
        /// </summary>
        public static string TransformaCPF(string CPF)
        {
            string CPFtransformado;

            CPFtransformado = CPF.Replace(".", "");
            CPFtransformado = CPFtransformado.Replace("-", "");

            return CPFtransformado.Trim();
        }

        /// <summary>
        /// Remove os caracteres extras do telefone
        /// </summary>
        public static string TransformaTelefone(string Telefone)
        {
            //Se receber um telefone
            if(Telefone != null && Telefone.Length != 0)
            {
                string TelefoneTransformado;

                TelefoneTransformado = Telefone.Replace("(", "");
                TelefoneTransformado = TelefoneTransformado.Replace(")", "");
                TelefoneTransformado = TelefoneTransformado.Replace("-", "");
                TelefoneTransformado = TelefoneTransformado.Replace(" ", "");
                return TelefoneTransformado.Trim();
            }

            return String.Empty;
        }

        /// <summary>
        ///  Remove os caracteres extras do CNPJ
        /// </summary>
        public static string TransformaCNPJ(string CNPJ)
        {
            string CNPJtransformado;

            CNPJtransformado = CNPJ.Replace(".", "");
            CNPJtransformado = CNPJtransformado.Replace("-", "");
            CNPJtransformado = CNPJtransformado.Replace("/", "");

            return CNPJtransformado.Trim();
        }

        public static string TransformaCEP(string CEP)
        {
            string CEPtransformado;

            CEPtransformado = CEP.Replace("-", "");

            return CEPtransformado.Trim();
        }

        public static float TransformaPreco(float preco)
        {
            string precoAux = preco.ToString();

            precoAux = precoAux.Replace(".", "");
            precoAux = precoAux.Replace(",", ".");

            return (float)Convert.ToDouble(precoAux);
        }
    }
}
