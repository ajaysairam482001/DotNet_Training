using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace RailwayReservation
{
    class AdminFunc
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

        public static void AdminMenu()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("\nWelcome to Admin Menu.\nPlease Choose an Option to proceed.");
                    Console.Write("\n1.Add a new Train\n2.Modify an Existing Train\n3.Delete/Retire a Train\n4.Add a Train Schedule" +
                        "\n5.Cancel a Train Schedule\n6.Show All Train Schedules\n7.Show Train Status\n8.Exit\nOption: ");
                    int option = int.Parse(Console.ReadLine());
                    switch (option)
                    {
                        case 1:
                            AddTrain();
                            break;
                        case 2:
                            ModifyTrain();
                            break;
                        case 3:
                            DeleteTrain();
                            break;
                        case 4:
                            AddTrainSchedule();
                            break;
                        case 5:
                            CancelTrainSchedule();
                            break;
                        case 6:
                            DisplayTrainSchedules();
                            break;
                        case 7:
                            DisplayTrainDetails();
                            break;
                        case 8:
                            Console.WriteLine("Logged out.");
                            break;
                    }
                    if (option == 8 || option == 0)
                    {
                        break;
                    }
                    else if(option > 8)
                    {
                        throw new InvalidInputException();
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Enter a Valid Option (Number).");
                }
                catch(InvalidInputException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
        private static void AddTrain()
        {
            try
            {
                Console.Write("Enter the train number (5 Digits Recommended): ");
                int trainNum = int.Parse(Console.ReadLine());
                Console.Write("Enter the train name: ");
                string trainName = Console.ReadLine();
                Console.Write("Enter the train Type (eg.Express / SuperFast / Normal): ");
                string traintype = Console.ReadLine();
                //just a normal add operation

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();
                    try
                    {
                        int Lines;
                        using (SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["AddTrain"], conn,transaction))
                        {
                            //@TrainNum, @TrainName, @TrainType
                            cmd.Transaction = transaction;
                            cmd.Parameters.AddWithValue("@TrainNum", trainNum);
                            cmd.Parameters.AddWithValue("@TrainName", trainName);
                            cmd.Parameters.AddWithValue("@Traintype", traintype);
                            Lines = cmd.ExecuteNonQuery();
                        }
                        if(Lines > 0 )
                        {
                            transaction.Commit();
                            conn.Close();
                            Console.WriteLine("Train Successfully Added");
                        }
                        else
                        {
                            Console.WriteLine("Failed to Create Train.");
                            transaction.Rollback();
                        }
                    }catch(SqlException ex)
                    {
                        if(ex.Number == 2627)
                        {
                            Console.WriteLine("Train Already Exists");
                        }
                        else
                        {
                            Console.WriteLine("Error: "+ ex.Message);
                        }
                        transaction.Rollback();
                    }
                }
            }
            catch(FormatException)
            {
                Console.WriteLine("Enter Valid Input");
            }
        }
        private static void ModifyTrainorg()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("\nWhat to modify\nEnter the options(number): ");
                    Console.WriteLine("1.Train Number.\n2.Train Name.\n3.Train Type.\n4.Back");
                    Console.Write("Option: ");
                    int option = int.Parse(Console.ReadLine());
                    if (option == 4)
                    {
                        break;
                    }
                    Console.Write("Enter Old Train Number: ");
                    int number = int.Parse(Console.ReadLine());

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        try
                        {
                            int Lines;
                            int exists;
                            //check if train exists
                            using (SqlCommand cmd = new SqlCommand("sp_checkIfTrainExists", conn))
                            {
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@TrainNum", number);

                                SqlParameter existsParam = new SqlParameter("@Exists", System.Data.SqlDbType.Bit)
                                {
                                    Direction = ParameterDirection.Output
                                };
                                cmd.Parameters.Add(existsParam);
                                cmd.ExecuteNonQuery();
                                exists = (bool)existsParam.Value ? 1 : 0;
                                conn.Close();
                            }
                            if (exists == 0)
                            {
                                throw new Exception("Train Does not exists");
                            }
                            else if (exists == 1)
                            {
                                conn.Open();

                                using (SqlTransaction trans = conn.BeginTransaction())
                                {
                                    switch (option)
                                    {
                                        case 1:
                                            //{write another cmd to change all the trainNum in other instances }
                                            Console.Write("Enter New Train number: ");
                                            int tn = int.Parse(Console.ReadLine());
                                            int exists1;
                                            using (SqlCommand cmd = new SqlCommand("sp_checkIfTrainExistInTrainScheduleAndChangeIt", conn, trans))
                                            {
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.AddWithValue("@OldTrainNum", number);
                                                cmd.Parameters.AddWithValue("@NewTrainNum", tn);
                                                SqlParameter existsParam = new SqlParameter("@Exists", SqlDbType.Bit)
                                                {
                                                    Direction = ParameterDirection.Output
                                                };
                                                cmd.Parameters.Add(existsParam);
                                                cmd.ExecuteNonQuery();
                                                exists1 = (bool)existsParam.Value ? 1 : 0;
                                                conn.Close();
                                            }
                                            if (exists1 == 1)
                                            {
                                                Console.WriteLine("InstanceTable Affected");
                                            }
                                            else
                                            {
                                                Console.WriteLine("No Train in instance Table");
                                            }
                                            using (SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["ModifyTrainNumber"], conn, trans))
                                            {
                                                Lines = 0;
                                                cmd.Parameters.AddWithValue("@NewTrainNumber", tn);
                                                cmd.Parameters.AddWithValue("@OldTrainNumber", number);
                                                Lines = cmd.ExecuteNonQuery();
                                            }
                                            if (Lines > 0)
                                            {
                                                trans.Commit();
                                                conn.Close();
                                                Console.WriteLine("Train Number Changed ");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Failed to Change TrainNumber.");
                                                trans.Rollback();
                                            }
                                            break;
                                        case 2:
                                            Console.Write("Enter New Train Name: ");
                                            string tname = Console.ReadLine();
                                            using (SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["ModifyTrainName"], conn, trans))
                                            {
                                                Lines = 0;
                                                cmd.Parameters.AddWithValue("@OldTrainNumber", number);
                                                cmd.Parameters.AddWithValue("@NewTrainName", tname);
                                                Lines = cmd.ExecuteNonQuery();
                                            }
                                            if (Lines > 0)
                                            {
                                                trans.Commit();
                                                conn.Close();
                                                Console.WriteLine("Train Name Changed ");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Failed to Change Train Name.");
                                            }
                                            break;
                                        case 3:
                                            Console.Write("Enter New Train Type: ");
                                            string ttype = Console.ReadLine();
                                            using (SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["ModifyTrainType"], conn, trans))
                                            {
                                                Lines = 0;
                                                cmd.Parameters.AddWithValue("@OldTrainNumber", number);
                                                cmd.Parameters.AddWithValue("@NewTrainType", ttype);
                                                Lines = cmd.ExecuteNonQuery();
                                            }
                                            if (Lines > 0)
                                            {
                                                trans.Commit();
                                                conn.Close();
                                                Console.WriteLine("Train Type Changed ");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Failed to Change Train Type.");
                                                trans.Rollback();
                                            }
                                            break;
                                        default:
                                            throw new InvalidInputException();
                                    }
                                }
                            }
                        }
                        catch (InvalidInputException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Enter Valid Input");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                        conn.Close();
                    }

                }
                catch (FormatException)
                {
                    Console.WriteLine("Enter Valid Input");
                }
                
            }
        }
        private static void ModifyTrain()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("\nWhat to modify\nEnter the options(number): ");
                    Console.WriteLine("1.Train Number.\n2.Train Name.\n3.Train Type.\n4.Back");
                    Console.Write("Option: ");
                    int option = int.Parse(Console.ReadLine());
                    if (option == 4)
                    {
                        break;
                    }
                    Console.Write("Enter Old Train Number: ");
                    int number = int.Parse(Console.ReadLine());

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        try
                        {
                            int Lines;
                            int exists;

                            // Check if train exists
                            using (SqlCommand cmd = new SqlCommand("sp_checkIfTrainExists", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@TrainNum", number);

                                SqlParameter existsParam = new SqlParameter("@Exists", SqlDbType.Bit)
                                {
                                    Direction = ParameterDirection.Output
                                };
                                cmd.Parameters.Add(existsParam);
                                cmd.ExecuteNonQuery();
                                exists = (bool)existsParam.Value ? 1 : 0;
                            }

                            if (exists == 0)
                            {
                                throw new Exception("Train does not exist.");
                            }
                                try
                                {
                                    switch (option)
                                    {
                                    case 1: // Modify Train Number
                                        Console.Write("Enter New Train number: ");
                                        int newTrainNum = int.Parse(Console.ReadLine());

                                        using (SqlTransaction trans = conn.BeginTransaction())
                                        {
                                            try
                                            {
                                                int result;

                                                // Call the stored procedure to update train number
                                                using (SqlCommand cmd = new SqlCommand("sp_UpdateTrainNumber", conn, trans))
                                                {
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.AddWithValue("@OldTrainNum", number);
                                                    cmd.Parameters.AddWithValue("@NewTrainNum", newTrainNum);

                                                    SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.Bit)
                                                    {
                                                        Direction = ParameterDirection.Output
                                                    };
                                                    cmd.Parameters.Add(resultParam);

                                                    cmd.ExecuteNonQuery();
                                                    result = (bool)resultParam.Value ? 1 : 0;
                                                }

                                                if (result == 1)
                                                {
                                                    trans.Commit();
                                                    Console.WriteLine("Train Number updated successfully.");
                                                }
                                                else
                                                {
                                                    trans.Rollback();
                                                    Console.WriteLine("Failed to update Train Number.");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                trans.Rollback();
                                                Console.WriteLine("Error: " + ex.Message);
                                            }
                                        }
                                        break;

                                    case 2:
                                            Console.Write("Enter New Train Name: ");
                                            string tname = Console.ReadLine();
                                            using (SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["ModifyTrainName"], conn))
                                            {
                                                Lines = 0;
                                                cmd.Parameters.AddWithValue("@OldTrainNumber", number);
                                                cmd.Parameters.AddWithValue("@NewTrainName", tname);
                                                Lines = cmd.ExecuteNonQuery();
                                            }

                                            if (Lines > 0)
                                            {
                                                ;
                                                Console.WriteLine("Train Name Changed Successfully.");
                                            }
                                            else
                                            {
                                                
                                                Console.WriteLine("Failed to Change Train Name.");
                                            }
                                            break;

                                        case 3:
                                            Console.Write("Enter New Train Type: ");
                                            string ttype = Console.ReadLine();
                                            using (SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["ModifyTrainType"], conn))
                                            {
                                                Lines = 0;
                                                cmd.Parameters.AddWithValue("@OldTrainNumber", number);
                                                cmd.Parameters.AddWithValue("@NewTrainType", ttype);
                                                Lines = cmd.ExecuteNonQuery();
                                            }

                                            if (Lines > 0)
                                            {
                                               
                                                Console.WriteLine("Train Type Changed Successfully.");
                                            }
                                            else
                                            {
                                                
                                                Console.WriteLine("Failed to Change Train Type.");
                                            }
                                            break;

                                        default:
                                            throw new InvalidInputException();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    
                                    throw ex;
                                }
                            
                        }
                        catch (InvalidInputException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Enter Valid Input");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Enter Valid Input");
                }
            }
        }
        private static void DeleteTrain()
        {
            while (true)
            {
                int loop = 1;
                try
                {
                    Console.Write("\nEnter Existing Train Number: ");
                    int number = int.Parse(Console.ReadLine());
                    if (number == 0)
                    {
                        loop = 0;
                        break;
                    }
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        try
                        {
                            int Lines;
                            int exists;
                            //check if train exists
                            using (SqlCommand cmd = new SqlCommand("sp_checkIfTrainExists", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@TrainNum", number);

                                SqlParameter existsParam = new SqlParameter("@Exists", SqlDbType.Bit)
                                {
                                    Direction = ParameterDirection.Output
                                };
                                cmd.Parameters.Add(existsParam);
                                cmd.ExecuteNonQuery();
                                exists = (bool)existsParam.Value ? 1 : 0;
                            }
                            if (exists == 0)
                            {
                                throw new ApplicationException("Train Does not exists");
                            }
                            else if (exists == 1)
                            {
                                loop = 0;
                                int status;
                                //check if train is Active and doesnt have any Schedules
                                using (SqlCommand cmd = new SqlCommand("sp_checkTrainStatus", conn))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@TrainNum", number);
                                    SqlParameter statusParam = new SqlParameter("@StatusNum", SqlDbType.Int)
                                    {
                                        Direction = ParameterDirection.Output
                                    };
                                    cmd.Parameters.Add(statusParam);
                                    cmd.ExecuteNonQuery();
                                    status = (int)statusParam.Value;
                                }
                                switch (status)
                                {
                                    case 1:
                                        
                                        throw new ApplicationException("Train is Active but In Schedule,\nCancelled the respective Schedule to retire the Train.");
                                    case 2:
                                        //delete the train
                                        Lines = 0;
                                        
                                        using (SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["DeleteTrain"], conn))
                                        {
                                            cmd.Parameters.AddWithValue("@TrainNumber", number);
                                            Lines = cmd.ExecuteNonQuery();
                                        }
                                        if (Lines > 0)
                                        {
                                            Console.WriteLine("Train Number " + number + " Successfully Retired");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failed to retire train");
                                        }
                                        break;
                                    case 3:
                                        Console.WriteLine("Retiring a new Train it seems!.");

                                        Lines = 0;
                                        using (SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["DeleteTrain"], conn))
                                        {
                                            cmd.Parameters.AddWithValue("@TrainNumber", number);
                                            Lines = cmd.ExecuteNonQuery();
                                        }
                                        if (Lines > 0)
                                        {
                                            Console.WriteLine("Train Number " + number + " Successfully Retired");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failed to retire train");
                                        }
                                        break;
                                    case 4:
                                        Console.WriteLine("Train is Already Retired.");
                                        break;
                                    default:
                                        throw new ApplicationException("Status is active but SP didnt work it seems.");
                                }
                            }

                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                        catch (ApplicationException ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                        conn.Close();
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Enter Valid Train Number.");
                }
                if(loop == 0)
                {
                    break;
                }
                Console.WriteLine("Enter 0 to go back........");
            }
        }  //check the option after adding schedule
        private static void AddTrainSchedule()
        {
            int loop = 1;
            while (true)
            {
                try
                {
                    if (loop == 0)
                    {
                        break;
                    }
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        int TSexists;
                        Console.Write("\nEnter a Schedule ID (3 digits Recommended) : ");
                        int ID = int.Parse(Console.ReadLine());
                        if (ID == 0)
                            break;
                        //check if Schedule Exists even if cancelled cant use it, create a new one
                        using (SqlCommand cmd = new SqlCommand("sp_checkifTrainInstanceExistsBoth", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@TrainInstance", ID);

                            SqlParameter existsParam = new SqlParameter("@Exists", SqlDbType.Bit)
                            {
                                Direction = ParameterDirection.Output
                            };
                            cmd.Parameters.Add(existsParam);
                            cmd.ExecuteNonQuery();
                            TSexists = (bool)existsParam.Value ? 1 : 0;
                        }
                        if (TSexists == 1)
                        {
                            throw new ApplicationException("An Train Schedule with that ID Exists.");
                        }
                        else if (TSexists == 0)
                        {
                            Console.Write("Enter Train Number: ");
                            int trainNum = int.Parse(Console.ReadLine());
                            try
                            {
                                int Texists;
                                int Lines;
                                //check if train exists
                                using (SqlCommand cmd = new SqlCommand("sp_checkIfTrainExists", conn))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.AddWithValue("@TrainNum", trainNum);

                                    SqlParameter existsParam = new SqlParameter("@Exists", SqlDbType.Bit)
                                    {
                                        Direction = ParameterDirection.Output
                                    };
                                    cmd.Parameters.Add(existsParam);
                                    cmd.ExecuteNonQuery();
                                    Texists = (bool)existsParam.Value ? 1 : 0;
                                }

                                if (Texists == 0)
                                {
                                    throw new ApplicationException("Train does not exist.");
                                }
                                else if (Texists == 1)
                                {
                                    Console.Write("Enter Train Starting Location: ");
                                    string from = Console.ReadLine();
                                    Console.Write("Enter Train Destination Location: ");
                                    string to = Console.ReadLine();
                                    Console.Write("Enter the Timings(Eg.,'07:45 pm'): ");
                                    DateTime timings = DateTime.ParseExact(Console.ReadLine(), "hh:mm tt", CultureInfo.InvariantCulture);
                                    Console.Write("Enter the Hours Of Journey: ");
                                    int hoj = int.Parse(Console.ReadLine());
                                    Console.Write("Enter the Date of Departure(Eg.,dd/MM/yyyy): ");
                                    DateTime dod = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    int cct;
                                    //one train can have one shift per day
                                    using (SqlCommand cmd = new SqlCommand("sp_checkTrainScheduleCollision", conn))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.AddWithValue("@TrainNumber", trainNum);
                                        cmd.Parameters.AddWithValue("@DateOfDeparture", dod);
                                        SqlParameter existsParam = new SqlParameter("@Exists", SqlDbType.Bit)
                                        {
                                            Direction = ParameterDirection.Output
                                        };
                                        cmd.Parameters.Add(existsParam);
                                        cmd.ExecuteNonQuery();
                                        cct = (bool)existsParam.Value ? 1 : 0;
                                    }
                                    if (cct == 1)
                                    {
                                        string formattedDate = dod.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        throw new ApplicationException("\nThere is already a Schedule for the train " + trainNum + " on " + formattedDate);
                                    }
                                    else if (cct == 0)
                                    {
                                        //add the schedule finally
                                        using (SqlCommand cmd = new SqlCommand("sp_addTrainSchedule", conn))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.AddWithValue("@TrainNumber", trainNum);
                                            cmd.Parameters.AddWithValue("@DOD", dod);
                                            cmd.Parameters.AddWithValue("@TrainInstanceId", ID);
                                            cmd.Parameters.AddWithValue("@from", from);
                                            cmd.Parameters.AddWithValue("@to", to);
                                            cmd.Parameters.AddWithValue("@timings", timings);
                                            cmd.Parameters.AddWithValue("@HOJ", hoj);
                                            Lines = cmd.ExecuteNonQuery();
                                            loop = 0;
                                        }
                                        if (Lines > 0)
                                        {
                                            conn.Close();
                                            Console.WriteLine("Schedule Added.");
                                            AddScheduleDetails(ID, conn);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Unreachable code");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Unreachable code");
                                }
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine("Enter Input in Valid Format." + ex.Message);
                            }
                            catch (SqlException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            catch (ApplicationException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        
                    }

                }
                catch (ApplicationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Enter Valid Input.");
                }
                Console.WriteLine("\nNote: Enter TrainInstanceId as 0 to go back......");
            }
        }
        private static void AddScheduleDetails(int InstanceId,SqlConnection conn)
        {
            Console.WriteLine("\nNow lets add Berth & Class Details");
            try
            {
                    conn.Open();
                    for (int i = 1; i <= 4; i++)
                    {
                        int lines = 0;
                        if (i == 4)
                        {
                            int loop = 1;
                            while (true)
                            {
                                if (loop == 0)
                                    break;
                                try
                                {
                                    Console.WriteLine("\nEntering details for Instance " + InstanceId + ",Class Sleeper");
                                    string _class = "Sleeper";
                                    Console.Write("Enter the number of berth Available(Eg.,1->(L),3->(U,M,L)): ");
                                    int ba = int.Parse(Console.ReadLine());
                                    if (ba > 3 || ba == 2 || ba < 1)
                                        throw new InvalidInputException();
                                    Console.Write("Enter Seat Limit for this Class Sleeper: ");
                                    int seatLimit = int.Parse(Console.ReadLine());
                                    int seatsAvailable = seatLimit;
                                    using (SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["AddBerthDetails"], conn))
                                    {
                                        cmd.Parameters.AddWithValue("@InstanceId", InstanceId);
                                        cmd.Parameters.AddWithValue("@_class", _class);
                                        cmd.Parameters.AddWithValue("@NaB", ba);
                                        cmd.Parameters.AddWithValue("@SL", seatLimit);
                                        cmd.Parameters.AddWithValue("@As", seatsAvailable);
                                        lines = cmd.ExecuteNonQuery();
                                    }
                                    if (lines > 0)
                                    {
                                        Console.WriteLine("\nClass Sleeper Added.");
                                        loop = 0;
                                        
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failed to add class");                                        
                                        loop = 0;
                                    }
                                }
                                catch (InvalidInputException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Enter Valid Input.");
                                }
                            }
                        }
                        else
                        {
                            int loop = 1;
                            while (true)
                            {
                                if (loop == 0)
                                    break;
                                try
                                {
                                    Console.WriteLine("\nEntering details for Instance " + InstanceId + ",Class " + i + "A.");
                                    string _class = i + "A";
                                    Console.Write("Enter the number of berth Available(Eg.,1->(L),3->(U,M,L)): ");
                                    int ba = int.Parse(Console.ReadLine());
                                    if (ba > 3 || ba == 2 || ba < 1)
                                        throw new InvalidInputException();
                                    Console.Write("Enter Seat Limit for this Class(" + i + "A): ");
                                    int seatLimit = int.Parse(Console.ReadLine());
                                    int seatsAvailable = seatLimit;
                                    using (SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["AddBerthDetails"], conn))
                                    {
                                        cmd.Parameters.AddWithValue("@InstanceId",InstanceId);
                                        cmd.Parameters.AddWithValue("@_class",_class);
                                        cmd.Parameters.AddWithValue("@NaB",ba);
                                        cmd.Parameters.AddWithValue("@SL",seatLimit);
                                        cmd.Parameters.AddWithValue("@As",seatsAvailable);
                                        lines = cmd.ExecuteNonQuery();
                                    }
                                    if(lines > 0)
                                    {
                                        Console.WriteLine("\nClass "+i+"A Added.");
                                        loop = 0;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failed to add class");
                                    }
                                }
                                catch (InvalidInputException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Error: Enter Valid Input.");
                                }
                            }
                        }
                    }
                    conn.Close();
                
                AddPriceDetails(InstanceId,conn);
            }
            catch (FormatException)
            {
                Console.WriteLine("Enter Valid Input");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: "+ex.Message);
            }
            
        }
        private static void AddPriceDetails(int InstanceId,SqlConnection conn)
        {
            Console.WriteLine("Now lets Enter the Compartment Price Details");
            try
            {
                
                    conn.Open();
                    for (int i = 1; i <= 4; i++)
                    {
                        int lines = 0;
                        if (i == 4)
                        {
                            int loop = 1;
                            while (true)
                            {
                                if (loop == 0)
                                    break;
                                try
                                {
                                    Console.WriteLine("\nEnter the Price for Instance " + InstanceId + ",Class Sleeper.");
                                    string _class = "Sleeper";
                                    Console.Write("Enter the Price: ");
                                    double price = double.Parse(Console.ReadLine());
                                    using (SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["AddPriceDetails"], conn))
                                    {
                                        cmd.Parameters.AddWithValue("@Instance", InstanceId);
                                        cmd.Parameters.AddWithValue("@_class", _class);
                                        cmd.Parameters.AddWithValue("@price", price);
                                        lines = cmd.ExecuteNonQuery();
                                    }
                                    if (lines > 0)
                                    {
                                        Console.WriteLine("\nClass Sleeper Price Added.");
                                        loop = 0;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failed to add class");                                    }
                                }
                                catch (InvalidInputException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Error: Enter Valid Input.");
                                }
                            }
                        }
                        else
                        {
                            int loop = 1;
                            while (true)
                            {
                                if (loop == 0)
                                    break;
                                try
                                {
                                    Console.WriteLine("\nEnter the Price for Instance " + InstanceId + ",Class " + i + "A.");
                                    string _class = i + "A";
                                    Console.Write("Enter the Price: ");
                                    double price = double.Parse(Console.ReadLine());
                                    using (SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["AddPriceDetails"], conn))
                                    {
                                        cmd.Parameters.AddWithValue("@Instance", InstanceId);
                                        cmd.Parameters.AddWithValue("@_class", _class);
                                        cmd.Parameters.AddWithValue("@price", price);
                                        lines = cmd.ExecuteNonQuery();
                                    }
                                    if (lines > 0)
                                    {
                                        Console.WriteLine("\nClass " + i + "A Price Added.");
                                        loop = 0; 
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failed to add class");
                                    }
                                }
                                catch (InvalidInputException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                catch (SqlException ex)
                                {
                                    Console.WriteLine("Error: "+ex.Message);
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Error: Enter Valid Input.");
                                }

                            }
                        }
                    }
                conn.Close();
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Enter Valid Input.");
            }
        }
        private static void CancelTrainSchedule()
        {
            while (true)
            {
                int loop = 1;
                if (loop == 0)
                    break;
                try
                {
                    Console.Write("\nEnter the Train Schedule ID: ");
                    int ID = int.Parse(Console.ReadLine());
                    if (ID == 0)
                        break;
                    int OverallExists, ActiveOnlyExists, Lines;
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("sp_checkifTrainInstanceExistsBoth", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@TrainInstance", ID);
                            SqlParameter existsParam = new SqlParameter("@Exists", SqlDbType.Bit)
                            {
                                Direction = ParameterDirection.Output
                            };
                            cmd.Parameters.Add(existsParam);
                            cmd.ExecuteNonQuery();
                            OverallExists = (bool)existsParam.Value ? 1 : 0;
                        }
                        if(OverallExists == 0)
                        {
                            Console.WriteLine("Train Schedule does not exist.");
                        }
                        else if(OverallExists == 1)
                        {
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
                                ActiveOnlyExists = (bool)existsParam.Value ? 1 : 0;
                            }
                            //not active
                            if(ActiveOnlyExists == 0)
                            {
                                Console.WriteLine("Train Schedule is Already Cancelled.");
                            }
                            //active
                            else if(ActiveOnlyExists == 1) 
                            {
                                //cancelles schedule and all related tickets
                                using (SqlCommand cmd = new SqlCommand("sp_CancelTrainScheduleAndRelevantTickets", conn))
                                {
                                    cmd.CommandType= CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@TrainInstanceId", ID);
                                    SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.Bit)
                                    {
                                        Direction = ParameterDirection.Output
                                    };
                                    cmd.Parameters.Add(resultParam);
                                    Lines = cmd.ExecuteNonQuery();
                                }
                                if (Lines > 0)
                                {
                                    Console.WriteLine("Train Schedule Cancelled Successfully.");
                                    loop = 0;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Unable to Cancel the Schedule.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Unreachable Code");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Unreachable Code");
                        }
                        
                    }
                    if(loop == 1)
                        Console.WriteLine("\nPress 0 as Schedule ID to go back.....");
                }
                catch(SqlException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                catch (ApplicationException ex)
                {
                    Console.WriteLine("Error: "+ex.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Enter Valid Input.");
                }
            }
        }
        private static void DisplayTrainSchedules()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["ShowAllTrainSchedules"], conn))
                    {
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("\nTrain Schedules:");
                            Console.WriteLine("------------------------------------------------------------------------------------------");
                            Console.WriteLine("Instance ID | Train Number | From      | To        | Time     | Departure Date | Status");
                            Console.WriteLine("-------------------------------------------------------------------------------------------");
                            while (reader.Read())
                            {
                                int instanceId = reader.GetInt32(0);
                                int trainNumber = reader.GetInt32(1);
                                string fromStation = reader.GetString(2);
                                string toStation = reader.GetString(3);
                                TimeSpan timings = reader.GetTimeSpan(4);
                                DateTime departureDate = reader.GetDateTime(5);
                                string status = reader.GetString(6);
                                Console.WriteLine($"{instanceId,11} | {trainNumber,12} | {fromStation,-9} | {toStation,-9} | {timings:hh\\:mm}    | {departureDate:dd/MM/yyyy}     | {status}");
                            }
                        }
                    }
                }
                catch(SqlException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                catch(Exception ex)
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
        private static void DisplayTrainDetails()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["ShowAllTrains"], conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("\nTrain Details:");
                            Console.WriteLine("------------------------------------------------------------");
                            Console.WriteLine("Train Number | Train Name           | Train Type       | Status");
                            Console.WriteLine("------------------------------------------------------------");

                            while (reader.Read())
                            {
                                int trainNumber = reader.GetInt32(0);
                                string trainName = reader.GetString(1);
                                string trainType = reader.GetString(2);
                                if (string.IsNullOrEmpty(trainType))
                                {
                                    trainType = "Not Mentioned";
                                }
                                string status = reader.GetString(3);

                                Console.WriteLine($"{trainNumber,12} | {trainName,-20} | {trainType,-16} | {status}");
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
    }


}
