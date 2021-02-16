using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using Sistema.DUE.Web.Helpers;
using Sistema.DUE.Web.Models;
using Sistema.DUE.Web.Services;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;
using NLog;
using System.Diagnostics;

namespace Sistema.DUE.Web
{
    public partial class EnviarNfeCCT : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Session["PROGRESSO"] = "0";
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("Default.aspx"));
        }

        protected void btnImportar_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if ()
            //    {

            //    }
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public static string ObterProgresso()
        {
            if (HttpContext.Current.Session["PROGRESSO"] != null)
            {
                return HttpContext.Current.Session["PROGRESSO"].ToString();
            }

            return "N/A";
        }
        //private bool UploadArquivo(FileUpload arquivo)
        //{
        //    //string nomeArquivo = Path.Combine(Server.MapPath("Uploads"), this.txtUpload.PostedFile.FileName);

        //    //try
        //    //{
        //    //    arquivo.SaveAs(nomeArquivo);
        //    //    return true;
        //    //}
        //    //catch
        //    //{
        //    //    return false;
        //    //}
        //}

        protected void btnGerarExcelParcial_Click(object sender, EventArgs e)
        {
            
        }

        
    }
}