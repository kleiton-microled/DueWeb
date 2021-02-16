using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using System;
using System.Web;

namespace Sistema.DUE.Web
{
    public partial class VisualizarXML : System.Web.UI.Page
    {
        private readonly DocumentoUnicoExportacaoDAO documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null && Request.QueryString["dest"] != null && Request.QueryString["tipo"] != null)
            {
                var tipo = Request.QueryString["tipo"].ToUpper();
                var due = documentoUnicoExportacaoDAO.ObterDUEPorId(Request.QueryString["id"].ToInt());
                var xml = tipo.Equals("RETORNO") ? documentoUnicoExportacaoDAO.ObterUltimoXMLRetorno(due.Id) : documentoUnicoExportacaoDAO.ObterUltimoXMLGerado(due.Id);

                if (Request.QueryString["dest"].ToUpper().Equals("SALVAR") & !string.IsNullOrEmpty(xml))
                {
                    Salvar(xml, due.DUE, tipo);
                }
                else
                {
                    Visualizar(xml);
                }
            }
        }

        private void Salvar(string xml, string due, string tipo)
        {
            string fileName, contentType;
            fileName = $"{due}_XML_{tipo}.xml";
            contentType = "text/xml";
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            //Response.BinaryWrite(bytes);
            Response.Write(xml);
            Response.Flush();
            Response.End();
        }

        private void SalvarTexto(string xml, string due, string tipo)
        {
            string fileName, contentType;
            fileName = $"{due}_XML_{tipo}.xml";
            contentType = "text/xml";
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            //Response.BinaryWrite(bytes);

            Response.Write(xml);
            Response.Flush();
            Response.End();
        }

        private void Validar(string due)
        {
        }

        private void Visualizar(string xml)
        {
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/xml";
            Response.Write(xml);
            Response.Flush();
            Response.End();
        }
    }
}