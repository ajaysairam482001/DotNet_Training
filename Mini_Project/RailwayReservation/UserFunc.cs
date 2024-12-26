using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml;

namespace RailwayReservation
{
    class UserFunc
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
        public static void UserMenu(int userId)
        {
            while (true)
            {
                try
                {
                    using(SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        int exists;
                        using(SqlCommand cmd = new SqlCommand("sp_checkifUserExists",conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@userId", userId);
                            SqlParameter existsParam = new SqlParameter("@Exists", SqlDbType.Bit)
                            {
                                Direction = ParameterDirection.Output
                            };
                            cmd.Parameters.Add(existsParam);
                            cmd.ExecuteNonQuery();
                            exists = (bool)existsParam.Value ? 1 : 0;
                        }
                        if(exists == 1)
                        {
                            Console.Clear();
                            Console.WriteLine("\nWelcome To User Menu.\nPlease Choose an Option to proceed.");
                            Console.Write("\n1.Show All Train Schedules.\n2.Book a Ticket.\n3.Cancel a Ticket.\n4.Show My tickets.\n5.Exit.\nOption: ");
                            int option = int.Parse(Console.ReadLine());
                            switch (option)
                            {
                                case 1:
                                    ShowAllActiveTrainSchedules();
                                    break;
                                case 2:
                                    BookTicket(userId); 
                                    break;
                                case 3:
                                    CancelTicket(userId);
                                    break;
                                case 4:
                                    ShowMyTickets(userId);
                                    break;
                                case 5:
                                    Console.WriteLine("Logged out");
                                    Console.ReadLine();
                                    break;
                            }
                            if (option == 5 || option == 0)
                            {
                                break;
                            }
                            else if (option > 5)
                            {
                                throw new InvalidInputException();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid user.");
                            Console.ReadLine();
                        }
                    }
                    
                }
                catch (FormatException)
                {
                    Console.WriteLine("Enter a Valid Option (Number).");
                    Console.ReadLine();
                }
                catch (InvalidInputException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            }
        }
        private static void ShowAllActiveTrainSchedules()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["ShowAllTrainsActiveOnly"], conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        { 
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("No Trains Schedules Yet.\nContact Administrator.\n");
                                conn.Close();
                                return;
                            }
                        
                            Console.WriteLine("\nTrain Schedules:");
                            Console.WriteLine("------------------------------------------------------------------------------------------");
                            Console.WriteLine("Instance ID | Train Number | From           | To              | Time     | Departure Date |");
                            Console.WriteLine("-------------------------------------------------------------------------------------------");
                            while (reader.Read())
                            {
                                int instanceId = reader.GetInt32(0);
                                int trainNumber = reader.GetInt32(1);
                                string fromStation = reader.GetString(2);
                                string toStation = reader.GetString(3);
                                TimeSpan timings = reader.GetTimeSpan(4);
                                DateTime departureDate = reader.GetDateTime(5);
                                
                                Console.WriteLine($"{instanceId,11} | {trainNumber,12} | {fromStation,-14} | {toStation,-15} | {timings:hh\\:mm}    | {departureDate:dd/MM/yyyy}    ");
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                    Console.ReadLine();
                }
            }
        }
        private static void BookTicket(int userId)
        {
            int loop = 1;
            while (true)
            {
                if(loop == 0)
                {
                    break;
                }
                try
                {
                    Console.Write("\nEnter the ID of Train Schedule in which you want to book: ");
                    int ID = int.Parse(Console.ReadLine());
                    if(ID == 0)
                    {
                        loop = 0;
                        break;
                    }
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        int exists;
                        using(SqlCommand cmd = new SqlCommand("sp_checkifTrainInstanceExistsOnlyActive",conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@TrainInstance", ID);
                            SqlParameter existsParam = new SqlParameter("@Exists", SqlDbType.Bit)
                            {
                                Direction = ParameterDirection.Output
                            };
                            cmd.Parameters.Add(existsParam);
                            cmd.ExecuteNonQuery();
                            exists = (bool)existsParam.Value ? 1 : 0;
                        }
                        if (exists == 1)
                        {
                            int loop1 = 1;
                            while (true)
                            {
                                if(loop == 0)
                                {
                                    break;
                                }
                                try
                                {
                                    Console.Write("\nEnter the class(Eg.,(Sleeper/3A/2A/1A): ");
                                    string _class = Console.ReadLine();
                                    if (_class.Equals("0"))
                                    {
                                        loop1 = 0;
                                        continue;
                                    }
                                    if (_class.Equals("Sleeper") || _class.Equals("3A") || _class.Equals("2A") || _class.Equals("1A"))
                                    {
                                        Console.Write("Enter the number of tickets to book in class " + _class + ": ");
                                        int numTic = int.Parse(Console.ReadLine());
                                        if (numTic < 1)
                                        {
                                            throw new InvalidInputException();
                                        }
                                        DateTime DateOFDeparture;
                                        double price;
                                        using (SqlCommand cmd = new SqlCommand("sp_GetBookingDetails", conn))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@TrainInstanceId", ID);
                                            cmd.Parameters.AddWithValue("@Class", _class);
                                            SqlParameter departureDateparam = new SqlParameter("@DepartureDate", SqlDbType.Date)
                                            {
                                                Direction = ParameterDirection.Output
                                            };
                                            SqlParameter priceParam = new SqlParameter("@Price", SqlDbType.Decimal)
                                            {
                                                Direction = ParameterDirection.Output
                                            };
                                            cmd.Parameters.Add(priceParam);
                                            cmd.Parameters.Add(departureDateparam);
                                            cmd.ExecuteNonQuery();
                                            DateOFDeparture = (DateTime)departureDateparam.Value;
                                            price = Convert.ToDouble(priceParam.Value);
                                        }
                                        Console.WriteLine("Single Ticket Price: " + price);
                                        Console.Write("Estimated total cost for " + numTic + " tickets: " + numTic * price + "\nDo you want to continue?(Y/N):");
                                        char ch = char.Parse(Console.ReadLine());
                                        if (ch == 'y' || ch == 'Y')
                                        {
                                            //main line booking tickets
                                            int lines = 0;
                                            string message;
                                            using (SqlCommand cmd = new SqlCommand("sp_BookTicketsWithCancelledCheck", conn))
                                            {
                                                cmd.CommandType= CommandType.StoredProcedure;
                                                cmd.Parameters.AddWithValue("@TrainInstanceId",ID);
                                                cmd.Parameters.AddWithValue("@UserId",userId);
                                                cmd.Parameters.AddWithValue("@Class",_class);
                                                cmd.Parameters.AddWithValue("@RequestedTickets",numTic);
                                                cmd.Parameters.AddWithValue("@BookingDate",DateOFDeparture);
                                                cmd.Parameters.AddWithValue("@Price",price);
                                                SqlParameter remarksParam = new SqlParameter("@Remarks", SqlDbType.VarChar)
                                                {
                                                    Size = 225,
                                                    Direction = ParameterDirection.Output
                                                };
                                                cmd.Parameters.Add(remarksParam);
                                                lines = cmd.ExecuteNonQuery();
                                                Console.WriteLine((string)remarksParam.Value);
                                                message = (string)remarksParam.Value;
                                            }
                                            if(message.Equals("Insufficient tickets available"))
                                            {
                                                continue;
                                            }
                                            else if(lines > 0)
                                            {
                                                loop = 0;
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Unable to book Tickets.");
                                                break;
                                            }
                                        }
                                        else if (ch == 'n' || ch == 'N')
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            throw new InvalidInputException();
                                        }
                                    }
                                    else
                                    {
                                        throw new InvalidInputException();
                                    }
                                }
                                catch (InvalidInputException ex)
                                {
                                    Console.WriteLine("Error: " + ex.Message);
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Enter Valid Input.");
                                }
                                catch (SqlException ex)
                                {
                                    Console.WriteLine("Error: " + ex.Message);
                                }
                            }
                            Console.ReadLine();
                            if(loop1 == 1)
                            {
                                Console.WriteLine("\nNote: Enter Class as 0 to go back...... ");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No such trains Scheduled yet.");
                        }
                        conn.Close();
                    }
                }
                catch(SqlException ex)
                {
                    Console.WriteLine("Error: "+ex.Message);
                }
                catch(InvalidInputException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Enter Valid Input.");
                }
                if(loop == 1)
                {
                    Console.WriteLine("\nEnter Train Schedule as 0 to go back.......");
                }
            }

        }
        private static void CancelTicket(int userId)
        {
            int loop = 1;
            while(true)
            {
                if(loop == 0)
                {
                    break;
                }
                try
                {
                    Console.Write("\nEnter Train Schedule ID: ");
                    int ID = int.Parse(Console.ReadLine());
                    if (ID == 0)
                    {
                        loop = 0;
                        break;
                    }
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        int exists;
                        using (SqlCommand cmd = new SqlCommand("sp_checkifTrainInstanceExistsOnlyActive", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@TrainInstance", ID);
                            SqlParameter existsParam = new SqlParameter("@Exists", SqlDbType.Bit)
                            {
                                Direction = ParameterDirection.Output
                            };
                            cmd.Parameters.Add(existsParam);
                            cmd.ExecuteNonQuery();
                            exists = (bool)existsParam.Value ? 1 : 0;
                        }
                        if(exists == 1)
                        {
                            //display the tickets in that schedule before the user cancels them
                            bool tic = DisplayTicketsByUserAndInstance(userId,ID);

                            if (tic)
                            {
                                while (true)
                                {
                                    try
                                    {
                                        Console.WriteLine("\nNote: Enter 0 as ticket number to go back........\n");
                                        Console.Write("Enter the ticket number you want to cancel: ");
                                        int cancel = int.Parse(Console.ReadLine());
                                        if (cancel == 0)
                                        {
                                            break;
                                        }
                                        using (SqlCommand cmd = new SqlCommand("sp_CancelTicketsByUserAndInstance", conn))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@UserId", userId);
                                            cmd.Parameters.AddWithValue("@TrainInstanceId", ID);
                                            cmd.Parameters.AddWithValue("@TicketNumber", cancel);
                                            SqlParameter remarksParam = new SqlParameter("@Remarks", SqlDbType.VarChar, 255)
                                            {
                                                Direction = ParameterDirection.Output
                                            };
                                            cmd.Parameters.Add(remarksParam);
                                            cmd.ExecuteNonQuery();
                                            string remarks = (string)remarksParam.Value;
                                            Console.WriteLine(remarks);
                                        }
                                    }
                                    catch (FormatException)
                                    {
                                        Console.WriteLine("Enter Valid Input.");
                                    }
                                    catch (SqlException ex)
                                    {
                                        Console.WriteLine("Error: " + ex.Message);
                                    }
                                }
                            }
                            else
                            {
                                conn.Close();
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Train Schedule is either cancelled or does not exists.");
                            conn.Close();
                            continue;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                catch (InvalidInputException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Enter Valid Input.");
                }
                if(loop == 1)
                {
                    Console.WriteLine("Note: Enter Train Schedule Id as 0 to go back......");
                }
            }
        }
        private static bool DisplayTicketsByUserAndInstance(int userId, int trainInstanceId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_GetTicketsByUserAndInstance", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@TrainInstanceId", trainInstanceId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("No Tickets Booked by you in this Schedule.");
                                conn.Close();
                                return false;
                            }
                            else
                            {
                                Console.WriteLine("\nTickets Booked by UserId "+userId+" on TrainSchedule Instance "+trainInstanceId+": ");
                                Console.WriteLine("------------------------------------------------------------------------------------------");
                                Console.WriteLine("Ticket No | Schedule ID | Class    | Berth | Seat No | Booking Date | Price ");
                                Console.WriteLine("------------------------------------------------------------------------------------------");

                                while (reader.Read())
                                {
                                    int ticketNumber = reader.GetInt32(0);
                                    int instanceId = reader.GetInt32(1);
                                    string ticketClass = reader.GetString(3);
                                    string berth = reader.GetString(4);
                                    int seatNumber = reader.GetInt32(5);
                                    DateTime bookingDate = reader.GetDateTime(6);
                                    decimal price = reader.GetDecimal(7);

                                    Console.WriteLine($"{ticketNumber,10} | {instanceId,11} | {ticketClass,-8} | {berth,-5} | {seatNumber,8} | {bookingDate:dd/MM/yyyy} | {price,6} ");
                                }
                                conn.Close();
                                return true;
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return false;
        }
        private static void ShowMyTickets(int userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_GetActiveTicketsByUserId", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("No active tickets found for this user.");
                                conn.Close();
                                return;
                                
                            }

                            Console.WriteLine("\nActive Tickets:");
                            Console.WriteLine("--------------------------------------------------------------------------------");
                            Console.WriteLine("Ticket No | Instance ID | Class   | Seat No | Berth | Date of Departure  | Price (Rs.)  ");
                            Console.WriteLine("-----------------------------------------------------------------------------------");

                            while (reader.Read())
                            {
                                int ticketNumber = reader.GetInt32(0);
                                int trainInstanceId = reader.GetInt32(1);
                                string classType = reader.GetString(2);
                                int seatNumber = reader.GetInt32(3);
                                string berth = reader.GetString(4);
                                DateTime bookingDate = reader.GetDateTime(5);
                                decimal price = reader.GetDecimal(6);
                                Console.WriteLine($"{ticketNumber,9} | {trainInstanceId,11} | {classType,7} | {seatNumber,7} | {berth,-5} | {bookingDate:dd/MM/yyyy}    | {price,7} ");
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL Error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                    Console.ReadLine();
                }
            }
        }
    }
}
