using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sistema.DUE.Web
{
    public partial class ConsultarDadosDUE : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["due"] != null)
            {
                using (var ws = new ServicoSiscomex.WsDUE())
                {
                    try
                    {
                        var dadosCompletos = new ServicoSiscomex.DueDadosCompletos();

                        dadosCompletos = ws.ObterDadosCompletos(Request.QueryString["due"].ToString(), ConfigurationManager.AppSettings["CpfCertificado"].ToString());
                        
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }                      
        }
    }
}