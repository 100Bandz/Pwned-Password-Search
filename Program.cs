using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Net;

namespace Pwned_Password_Search
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Please enter your password: ");
            Console.WriteLine();

            String input = Console.ReadLine();

            //Convert the String into a byte array
            Byte[] DataArray;

            DataArray = Encoding.UTF8.GetBytes(input);

            //Compute the hash of DataArray
            HashAlgorithm SHA = SHA1.Create();

            byte[] result = SHA.ComputeHash(DataArray);

            //Loop through each byte and convert it into a hexadecimal String
            StringBuilder StrBuild = new StringBuilder(result.Length * 2);

            foreach (byte ele in result)
            {
                StrBuild.AppendFormat("{0:x2}", ele);
            }

            String HexString = StrBuild.ToString().ToUpper();
            Console.WriteLine($"The SHA1 hash of {input} is: {HexString}");
            Console.WriteLine();


            String first5 = HexString.Substring(0, 5);

            String url = "https://api.pwnedpasswords.com/range/" + first5;


            //Create request for the URL
            WebRequest request = WebRequest.Create(url);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            //Split the hashes and compare them to see if they match
            string hashToCheck = HexString.Substring(5);
            while (true)
            {
                String lineToCheck = reader.ReadLine();

                if (lineToCheck == null)
                {
                    Console.WriteLine("Password was not found");
                    reader.Close();
                    dataStream.Close();
                    response.Close();
                    break;
                }

                String[] split = lineToCheck.Split(":");

                if (split[0] == hashToCheck)
                {
                    Console.WriteLine("Password has been compromised");
                    reader.Close();
                    dataStream.Close();
                    response.Close();
                    break;
                }
            }

            Console.ReadKey();

        }
    }
}
