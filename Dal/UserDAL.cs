using form_authentication.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace form_authentication.Dal
{
    public class UserDAL
    {
        private string connectionString = @"Data Source=DESKTOP-GM8RGHQ;Initial Catalog=FormAuthentication;Integrated Security=True;TrustServerCertificate=True";

        public User GetUserByUsername(string username)
        {
            try
            {
                User user = null;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT UserID, Username, PasswordHash, Salt FROM Users WHERE Username = @Username";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", username);

                    conn.Open();
                    System.Diagnostics.Debug.WriteLine("Database connection opened successfully");

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        user = new User
                        {
                            UserID = (int)reader["UserID"],
                            Username = (string)reader["Username"],
                            PasswordHash = (string)reader["PasswordHash"],
                            Salt = (string)reader["Salt"]
                        };
                        System.Diagnostics.Debug.WriteLine($"Found user: {user.Username} with hash: {user.PasswordHash}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"No user found with username: {username}");
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database error: {ex.Message}");
                throw;
            }
        }
    }
}