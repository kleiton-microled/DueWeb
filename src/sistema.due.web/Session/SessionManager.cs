using System;
using System.Web;

namespace LouisDreyfus.DUE.Web.Session
{
    public class SessionManager
    {
     
        public static int UsuarioId
        {
            get
            {
                if (HttpContext.Current.Session["UsuarioId"] != null)
                    return (int)HttpContext.Current.Session["UsuarioId"];

                throw new Exception("Sessão expirada");
            }
            set
            {
                HttpContext.Current.Session["UsuarioId"] = value;
            }
        }

        public static string NomeUsuario
        {
            get
            {
                if (HttpContext.Current.Session["NomeUsuario"] != null)
                    return HttpContext.Current.Session["NomeUsuario"].ToString();

                throw new Exception("Sessão expirada");
            }
            set
            {
                HttpContext.Current.Session["NomeUsuario"] = value;
            }
        }

        public static bool Logado
        {
            get
            {
                if (HttpContext.Current.Session["Logado"] != null)                
                    return (bool)HttpContext.Current.Session["Logado"];

                return false;
            }
            set
            {
                HttpContext.Current.Session["Logado"] = value;
            }
        }

        public static void Encerrar()
        {
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Clear();
        }
    }
}