using System;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;

namespace Sistema.DUE.Web.Extensions
{
    public static class StringExtensions
    {
        public static int ToInt(this string valor)
        {
            int resultado = 0;

            if (Int32.TryParse(valor, out resultado))
                return resultado;

            return 0;
        }

        public static string RemoverCaracteresEspeciaisDUE(this string valor)
        {
            return Regex.Replace(valor, @"\-", ""); ;
        }

        public static decimal ToDecimal(this string valor)
        {
            decimal resultado = 0;

            if (Decimal.TryParse(valor, out resultado))
                return resultado;

            return 0;
        }

        public static double ToDouble(this string valor)
        {
            double resultado = 0;

            if (Double.TryParse(valor, out resultado))
                return resultado;

            return 0;
        }

        public static string PPonto(this string valor)
        {
            var valorFormatado = string.Format("{0:N4}", Convert.ToDecimal(valor));

            return valorFormatado
                .Replace(".", "")
                .Replace(",", ".");
        }

        public static string RemoverCaracteresEspeciais(this string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return   string.Empty;

            valor = valor.Replace("&", "&amp;");
            valor = valor.Replace("<", "&lt;");
            valor = valor.Replace(">", "&gt;");

            return valor;
        }
		
		public static string RemoverCaracteresEspeciaisCNPJ(this string valor)
        {
            return Regex.Replace(valor, @"\/|\-|\.", ""); ;
        }

        public static string QuebraDeLinhaXML(this string valor)
        {
            return Regex.Replace(valor, @"\r\n?|\n", " ");
        }

        public static DateTime ToDateTime(this string valor)
        {
            DateTime resultado;

            if (DateTime.TryParse(valor, out resultado))
                return resultado;
            return SqlDateTime.MinValue.Value;
        }
    }
}