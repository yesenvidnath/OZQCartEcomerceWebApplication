using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace ozq
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string email = this.email.Text.Trim();
            string password = this.password.Text.Trim();

            // Validate the email and password (you might want to add more robust validation)
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                // Show error alert for invalid input
                ClientScript.RegisterStartupScript(this.GetType(), "InvalidInputAlert", "alert('Please enter both email and password.');", true);
                return;
            }

            // Check the database for a matching admin
            string connectionString = ConfigurationManager.ConnectionStrings["ozqDB"].ConnectionString;
            string query = "SELECT AdminID FROM Admins WHERE Email = @Email AND Password = @Password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null) // Login successful
                    {
                        // Set a session indicating a successful login
                        Session["AdminLoggedIn"] = true;

                        ClientScript.RegisterStartupScript(this.GetType(), "Login Successful", "alert('Hey Admin.');", true);
                        // Redirect to the admin dashboard
                        Response.Redirect("ozq-admin/index.aspx");
                    }
                    else // Invalid login
                    {

                        // Show error alert for invalid login
                        ClientScript.RegisterStartupScript(this.GetType(), "InvalidLoginAlert", "alert('Invalid login credentials.');", true);
                    }
                }
            }
        }



    }
}