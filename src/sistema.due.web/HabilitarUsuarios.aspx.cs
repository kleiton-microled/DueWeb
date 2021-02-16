using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using Sistema.DUE.Web.Models;
using System;
using System.Web.UI.WebControls;

namespace Sistema.DUE.Web
{
    public partial class HabilitarUsuarios : System.Web.UI.Page
    {
        private readonly UsuarioDAO _usuarioDAO = new UsuarioDAO();        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ObterUsuarios();
            }
        }
        
        protected void ObterUsuarios()
        {
            this.gvUsuarios.DataSource = _usuarioDAO.ObterUsuarios();
            this.gvUsuarios.DataBind();
        }        

        protected void btnHabilitarUsuarios_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.gvUsuarios.Rows.Count; i++)
                {
                    var id = this.gvUsuarios.DataKeys[i]["Id"].ToString();
                    var checkAdmin = (CheckBox)this.gvUsuarios.Rows[i].FindControl("CheckAdmin");
                    var checkAtivo = (CheckBox)this.gvUsuarios.Rows[i].FindControl("CheckAtivo");

                    _usuarioDAO.AtualizarUsuario(new Usuario
                    {
                        Id = id.ToInt(),
                        FlagAdmin = checkAdmin.Checked,
                        FlagAtivo = checkAtivo.Checked
                    });
                }

                ViewState["Sucesso"] = true;
            }
            catch (Exception)
            {
            }            
        }
    }
}