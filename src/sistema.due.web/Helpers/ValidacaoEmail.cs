using System.Text.RegularExpressions;

namespace Sistema.DUE.Web.Helpers
{
    public class ValidacaoEmail
    {
        public static bool Validar(string email)
        {
            Regex regExpEmail = new Regex("^[A-Za-z0-9](([_.-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([.-]?[a-zA-Z0-9]+)*)([.][A-Za-z]{2,4})$");
            Match match = regExpEmail.Match(email);
            return match.Success;
        }
    }
}