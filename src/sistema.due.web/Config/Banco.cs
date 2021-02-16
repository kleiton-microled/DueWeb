using System.Configuration;

namespace Sistema.DUE.Web.Config
{
    public class Banco
    {
        public static string StringConexao()
        {
            return ConfigurationManager.ConnectionStrings["Banco"].ConnectionString;
        }
    }
}