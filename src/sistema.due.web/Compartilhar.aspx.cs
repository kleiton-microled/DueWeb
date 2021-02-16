using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sistema.DUE.Web
{
    public partial class Compartilhar : System.Web.UI.Page
    {
        private readonly UsuarioDAO _usuarioDAO = new UsuarioDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ObterUsuarios();

                if (this.gvUsuarios.Rows.Count > 0)
                {
                    var usuariosVinculados = _usuarioDAO.ObterUsuariosVinculados(Convert.ToInt32(Session["UsuarioId"].ToString()));

                    if (usuariosVinculados.Count() > 0)
                    {
                        foreach (GridViewRow linha in this.gvUsuarios.Rows)
                        {
                            var id = this.gvUsuarios.DataKeys[linha.RowIndex]["Id"].ToString();

                            if (usuariosVinculados.Contains(id.ToInt()))
                            {
                                linha.BackColor = System.Drawing.Color.LightGreen;
                                var chk = (CheckBox)this.gvUsuarios.Rows[linha.RowIndex].FindControl("UsuarioCheck");

                                if (chk != null)
                                {
                                    chk.Checked = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void ObterUsuarios()
        {
            this.gvUsuarios.DataSource = _usuarioDAO.ObterUsuariosAtivos();
            this.gvUsuarios.DataBind();
        }

        protected void btnCompartilhar_Click(object sender, EventArgs e)
        {
            try
            {
                var usuariosVinculados = new List<int>();

                for (int i = 0; i < this.gvUsuarios.Rows.Count; i++)
                {
                    var chk = (CheckBox)this.gvUsuarios.Rows[i].FindControl("UsuarioCheck");

                    if (chk.Checked)
                    {
                        usuariosVinculados.Add(this.gvUsuarios.DataKeys[i]["Id"].ToString().ToInt());
                    }                     
                }

                _usuarioDAO.CompartilharUsuario(Convert.ToInt32(Session["UsuarioId"].ToString()), usuariosVinculados.ToArray());

                ViewState["Sucesso"] = true;

                Response.Redirect("Compartilhar.aspx");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}