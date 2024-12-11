using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Assesments.Assesment6
{
    class ADO
    {
        static void Main(string[] args)
        { 
            using (SqlConnection conn = new SqlConnection("Data source = DESKTOP-9ROIE47\\SQLEXPRESS;Database=Infinite_db;Trusted_Connection=True"))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("Insert_ProdDetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductName", "Stanley Bottle");
                        cmd.Parameters.AddWithValue("@Price", 1000);
                        SqlParameter productIdParam = new SqlParameter("@GeneratedProductId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(productIdParam);
                        SqlParameter discountedPriceParam = new SqlParameter("@DiscountedPrice", SqlDbType.Decimal)
                        {
                            Precision = 10,
                            Scale = 2,
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(discountedPriceParam);
                        cmd.ExecuteNonQuery();
                        int generatedProductId = (int)productIdParam.Value;
                        decimal discountedPrice = (decimal)discountedPriceParam.Value;
                        Console.WriteLine($"ProductId: {generatedProductId}");
                        Console.WriteLine($"Discounted Price: {discountedPrice}");
                        Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

        }
    }
}
