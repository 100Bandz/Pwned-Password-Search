using System;
using System.Security.Cryptography;
using System.Text;

namespace Pwned_Password_Search
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter your password: ");

            String input = Console.ReadLine();

            if (input == null)
            {
                return;
            }

            //Convert the String into a byte array
            Byte[] DataArray;

            DataArray = Encoding.ASCII.GetBytes(input);

            //Compute the hash of DataArray
            HashAlgorithm SHA = SHA1.Create();

            byte[] result = SHA.ComputeHash(DataArray);

            //Loop through each byte and convert it into a hexadecimal String
            StringBuilder StrBuild = new StringBuilder(result.Length * 2);

            foreach (byte ele in result)
            {
                StrBuild.AppendFormat("{0:x2}", ele);
            }

            String HexString = StrBuild.ToString();

            Console.Write(HexString);

        }
    }
}
