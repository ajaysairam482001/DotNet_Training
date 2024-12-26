using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace RailwayReservation
{
    class Login
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
        public static void LoginMethod()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nWelcome to Infinite Railway Reservation System!");
                Console.Write("1.Login\n2.SignUp\n3.Exit\nEnter Option (1/2/3): ");
                int opt;
                try
                {
                    opt = int.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Enter a Valid Number.");
                    Console.ReadLine();
                    continue;
                }
                try
                {
                    if (opt == 1)
                    {
                        userLogin();
                        continue;
                    }
                    else if (opt == 2)
                    {
                        signup();
                        continue;
                    }
                    else if (opt == 3)
                    {
                        Console.Clear();
                        Console.WriteLine("Thank You!\nVisit Again.\n");
                        return;
                    }
                    else
                    {
                        throw new InvalidInputException();
                    }
                }
                catch (InvalidInputException e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();

                }
            }
        }

        public static void userLogin() 
        {
            Console.Write("\nEnter User_Name: ");
            string UserName = Console.ReadLine();
            Console.Write("Enter Password: ");
            string Pass = Console.ReadLine();
            

            string role = string.Empty;
            bool isSuccess = false;
            int UserId = 0;
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
                        SqlParameter roleParam = new SqlParameter("@Role", SqlDbType.VarChar, 5)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(roleParam);
                        SqlParameter isSuccessParam = new SqlParameter("@isSuccess", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(isSuccessParam);
                        SqlParameter UserParam = new SqlParameter("@UserId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(UserParam);

                        cmd.ExecuteNonQuery();
                        role = roleParam.Value != DBNull.Value ? roleParam.Value.ToString() : null;
                        isSuccess = (bool)isSuccessParam.Value;
                        UserId = (int)UserParam.Value;
                        conn.Close();
                    }
                    if (isSuccess)
                    {
                        Console.WriteLine("Welcome "+role+"\n");
                        if (role.Equals("Admin"))
                        {
                            AdminFunc.AdminMenu();
                            conn.Close();
                            return;
                        }
                        else if (role.Equals("User"))
                        {
                            UserFunc.UserMenu(UserId);
                            conn.Close(); 
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Login Failed");
                            Console.ReadLine();
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine("Incorrect UserName and Password.");
                        Console.ReadLine();
                        //return "Incorrect UserName and Password.";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Occured: "+ ex.Message);
                    Console.ReadLine();
                    //return "Error Occured: " + ex.Message;
                }
                finally
                {
                    conn.Close();
                }

            }
        }

        public static void signup()
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
            string insertQuery = ConfigurationManager.AppSettings["InsertQuery"];
            string insertQueryUser = ConfigurationManager.AppSettings["InsertInUser"];


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
                            Console.WriteLine("Admin Already exists.");
                            Console.ReadLine();
                            return;
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
                        //fetching userId from Login Table
                        using (SqlCommand cmd = new SqlCommand("sp_fetchUserId", conn, transc))
                        {
                            cmd.Transaction = transc;
                            cmd.CommandType = CommandType.StoredProcedure;
                            //Inputs
                            cmd.Parameters.AddWithValue("@email", Email);
                            //output
                            SqlParameter IdParam = new SqlParameter("@Id", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
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
                        Console.WriteLine("Successfully registered " + category);
                        Console.ReadLine();
                    }
                    else
                    {
                        //Console.WriteLine("Failed to register");
                        Console.WriteLine("Failed to register");
                        Console.ReadLine();
                    }

                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627) //unique constraint violation
                    {
                        Console.WriteLine("Error: Username or Email already exists.");
                        Console.ReadLine();
                    }
                    else
                    {
                        transc.Rollback();
                        conn.Close();
                        Console.WriteLine("Error occured: " + ex.Message);
                        Console.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                    transc.Rollback();
                    conn.Close();
                    Console.WriteLine("Error occured: " + ex.Message);
                    Console.ReadLine();
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
                        Direction = ParameterDirection.Output
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
