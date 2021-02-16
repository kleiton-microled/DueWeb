using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Sistema.DUE.Web
{
    public partial class ListaCertificados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var certificados = ListarCertificadosInstalados();

            foreach (X509Certificate2 certificado in certificados)
            {
                    Response.Write(string.Format("{0} - {1} <br />", certificado.Subject, certificado.NotAfter));
            }           
        }

        public static IEnumerable<X509Certificate2> ListarCertificadosInstalados()
        {
            var stores = new X509Store(StoreName.My, StoreLocation.LocalMachine);

            stores.Open(OpenFlags.ReadOnly);

            var certificadosInstalados = stores.Certificates;

            certificadosInstalados.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
            certificadosInstalados.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false);

            stores.Close();

            var certificados = new List<X509Certificate2>();

            foreach (X509Certificate2 certificado in certificadosInstalados)
                yield return certificado;
        }
    }
}