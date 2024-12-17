using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RailwayReservation
{
    class Login
    {
        private static readonly string connectionString = "Data Source=DESKTOP-9ROIE47\\SQLEXPRESS;Initial Catalog=Railway_db;Trusted_Connection=True;Integrated Security=True";
        public static string LoginMethod()
        {
            Console.WriteLine("1.Login/ 2.SignUp");
            int opt = int.Parse(Console.ReadLine());
            try
            {
                if(opt == 1)
                {
                    return userLogin();
                }
                else if(opt == 2)
                {
                    return signup();
                }
                else
                {
                    throw new InvalidInputException();
                }
            }
            catch (InvalidInputException e)
            {
                //Console.WriteLine(e.Message);
                return e.Message;
            }
        }

        public static string userLogin() {
            Console.Write("\nEnter User_Name: ");
            string UserName = Console.ReadLine();
            Console.Write("Enter Password: ");
            string Pass = Console.ReadLine();
            

            string role = string.Empty;
            bool isSuccess = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_checkLogin", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //Inputs
                        cmd.Parameters.AddWithValue("@UserName",UserName);
                        cmd.Parameters.AddWithValue("@Password", Pass);

                        //Outputs
                        SqlParameter roleParam = new SqlParameter("@Role", System.Data.SqlDbType.VarChar, 5)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        };
                        cmd.Parameters.Add(roleParam);
                        SqlParameter isSuccessParam = new SqlParameter("@isSuccess", System.Data.SqlDbType.Bit)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        };
                        cmd.Parameters.Add(isSuccessParam);

                        cmd.ExecuteNonQuery();
                        role = roleParam.Value != DBNull.Value ? roleParam.Value.ToString() : null;
                        isSuccess = (bool)isSuccessParam.Value;
                        conn.Close();
                    }
                    if (isSuccess)
                    {
                        //Console.WriteLine("Welcome "+role);
                        return "Welcome " + role;
                    }
                    else
                    {
                        //Console.WriteLine("Incorrect UserName and Password.");
                        return "Incorrect UserName and Password.";
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error Occured: "+ ex.Message);
                    return "Error Occured: " + ex.Message;
                }

            }
        }

        public static string signup()
        {
            Console.Write("\nEnter UserName: ");
            string un = Console.ReadLine();
            Console.Write("Enter Password: ");
            string pass = Console.ReadLine();
            Console.Write("Enter Email: ");
            string Email = Console.ReadLine();
            Console.Write("Are you signing Up as (User/Admin)?: ");
            string category = Console.ReadLine();

            if(category != "User" && category != "Admin")
            {
                throw new InvalidInputException();
            }
            string insertQuery = "insert into login_table (UserName,Password,Category,Email) values (@username,@password,@category,@email);";
            string insertQueryUser = "insert into user_table (Id,Name,Email,Contact_number) values (@Id,@username,@email,@Contact_number);";


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transc = conn.BeginTransaction();
                try
                { 
                    //checkif Admin exists
                    if (category.Equals("Admin"))
                    {
                        bool temp = checkAdminExists(conn,transc);
                        if (temp)
                        {
                            transc.Rollback();
                            conn.Close();
                            return temp ? "Admin Already exists." : "";
                        }
                        
                    }

                    int loginId = 0;
                    int Lines;
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn, transc))
                    {
                        cmd.Transaction = transc;
                        cmd.Parameters.AddWithValue("@username", un);
                        cmd.Parameters.AddWithValue("@password", pass);
                        cmd.Parameters.AddWithValue("@category", category);
                        cmd.Parameters.AddWithValue("@email", Email);

                        Lines = cmd.ExecuteNonQuery();
                        
                    }
                    if (category.Equals("User"))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_fetchUserId", conn, transc))
                        {
                            cmd.Transaction = transc;
                            cmd.CommandType = CommandType.StoredProcedure;
                            //Inputs
                            cmd.Parameters.AddWithValue("@email", Email);
                            //output
                            SqlParameter IdParam = new SqlParameter("@Id", System.Data.SqlDbType.Int)
                            {
                                Direction = System.Data.ParameterDirection.Output
                            };
                            cmd.Parameters.Add(IdParam);

                            cmd.ExecuteNonQuery();
                            loginId = (int)IdParam.Value;
                        }
                        using (SqlCommand cmd = new SqlCommand(insertQueryUser, conn, transc))
                        {
                            cmd.Transaction = transc;
                            Console.Write("Enter Contact Number: ");
                            string cn = Console.ReadLine();
                            cmd.Parameters.AddWithValue("@Id", loginId);
                            cmd.Parameters.AddWithValue("@username", un);
                            cmd.Parameters.AddWithValue("@email", Email);
                            cmd.Parameters.AddWithValue("@Contact_number", long.Parse(cn));

                            cmd.ExecuteNonQuery();
                        }
                    }
                    if (Lines > 0)
                    {
                        //Console.WriteLine("Successfully registered " + category);
                        transc.Commit();
                        conn.Close();
                        return "Successfully registered " + category;
                    }
                    else
                    {
                        //Console.WriteLine("Failed to register");
                        return "Failed to register";
                    }

                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627) //unique constraint violation
                    {
                        return "Error: Username or Email already exists.";
                    }
                    else
                    {
                        transc.Rollback();
                        conn.Close();
                        return "Error occured: " + ex.Message;
                    }
                }
                catch (Exception ex)
                {
                    transc.Rollback();
                    conn.Close();
                    return "Error occured: " + ex.Message;
                }
            }
        }

        private static bool checkAdminExists(SqlConnection conn, SqlTransaction transc)
        {
            bool adminExists = false;

            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_checkAdminExists", conn, transc))
                {
                    cmd.Transaction = transc;
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    SqlParameter existsParam = new SqlParameter("@Exists", SqlDbType.Bit)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(existsParam);
                    cmd.ExecuteNonQuery();
                    adminExists = (bool)existsParam.Value;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return adminExists;
        }
    }
}
