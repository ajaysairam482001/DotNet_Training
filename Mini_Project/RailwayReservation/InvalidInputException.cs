using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailwayReservation
{
    class InvalidInputException : ApplicationException
    {
        public InvalidInputException() : base("Its an Invaild Input.") { }

    }
}
