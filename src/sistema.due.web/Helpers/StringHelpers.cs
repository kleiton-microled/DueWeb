using System;

namespace Sistema.DUE.Web.Helpers
{
    public class StringHelpers
    {
        public static bool IsDate(string valor)
        {
            DateTime data;

            if (DateTime.TryParse(valor, out data))
                return true;

            return false;
        }

        public static bool IsNumero(string texto)
        {
            decimal valor;

            if (Decimal.TryParse(texto, out valor))
                return true;

            return false;
        }
    }
}