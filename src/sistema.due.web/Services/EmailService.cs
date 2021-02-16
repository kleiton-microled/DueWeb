using Sistema.DUE.Web.Config;
using System.Data;
using System.Data.SqlClient;

namespace Sistema.DUE.Web.Services
{
    public class EmailService
    {
        public static void EnviarEmail(string to, string toName, string subject, string message)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                SqlCommand Command = new SqlCommand();

                Command.CommandText = "PROC_ENVIA_EMAIL";
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = con;

                Command.Parameters.AddWithValue("toAddress", to);
                Command.Parameters.AddWithValue("toName", toName);
                Command.Parameters.AddWithValue("ccName", string.Empty);
                Command.Parameters.AddWithValue("ccAddress", string.Empty); 
                Command.Parameters.AddWithValue("bccName", string.Empty);
                Command.Parameters.AddWithValue("bccAddress", string.Empty);
                Command.Parameters.AddWithValue("subject", subject);
                Command.Parameters.AddWithValue("message", message);                

                con.Open();
                Command.ExecuteNonQuery();
                con.Close();
            }
        }

    }
}