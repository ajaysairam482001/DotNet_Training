using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryA6
{
    public class ConcessionCalc
    {
        //used by Assignment6.Question4
        public string calculateConcession(double cost,int age,string name)
        {
            if (age < 5)
            {
                return "Little Champs - Free Ticket";
            }
            else if(age > 60)
            {
                double calcfare = cost * 0.3;
                return "Senior citezen, Calculated fare: " + calcfare;
            }
            else
            {
                return "Tickets booked, Fare: " + cost;
            }
        }

    }

}
