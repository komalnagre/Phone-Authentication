using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using backendapploginapp.Models;
using System.Windows.Interop;

namespace backendapploginapp.Controllers
{
    [RoutePrefix("api/Test")]
    public class TestController : ApiController
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
        SqlCommand cmd = null;
        SqlDataAdapter da= null;

        [HttpPost]
        [Route ("Registeration")]
        public string Registeration(Login login)
        {
            string msg = string.Empty;
            try
            {
                string query = "INSERT INTO users (Otp, PhoneNo) VALUES (@Otp, @PhoneNo)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Otp", login.Otp);
                cmd.Parameters.AddWithValue("@PhoneNo", login.PhoneNo);
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                if (i > 0)
                {
                    msg = "Login Sucessfully";
                }
                else
                {
                    msg = "Error";
                }

            }
            catch (Exception ex) 
            {
                msg =ex.Message;

            }
           
            return msg;
      
        }

       
  
    }
}
