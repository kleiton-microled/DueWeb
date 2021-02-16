namespace Sistema.DUE.Web.Extensions
{
    public static class DoubleExtensions
    {
        public static string ToBD(this double valor)
        {
            return valor.ToString().Replace(".", "").Replace(",", ".");
        }
    }
}