using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class Validate<T>
    {
        public static string BOARD_NULL = "Board can not be null!";
        public static string COMMAND_NULL = "Command can not be null!";

        public static T IsNotNull(string errorMsg, T obj)
        {
            if (obj == null)
                throw new NullReferenceException(errorMsg);
            else
                return obj;
        }
    }
}
