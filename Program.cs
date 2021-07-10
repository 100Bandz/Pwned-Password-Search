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
            Console.WriteLine();

            String first5 = HexString.Substring(0, 5);

            String url = "https://api.pwnedpasswords.com/range/" + first5;

            Console.Write(first5);
            Console.WriteLine();

            //Create request for the URL
            WebRequest request = WebRequest.Create(url);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            /*string responseFromServer = reader.ReadToEnd();

            Console.WriteLine(responseFromServer);*/

            String lineToCheck = reader.ReadLine();

            //Need a loop 


            while (true)
            {
                bool equals = false;


                if (lineToCheck == null)
                {
                    Console.WriteLine("Password was not found");
                    reader.Close();
                    dataStream.Close();
                    response.Close();
                    return;
                }

                if (lineToCheck.Length == HexString.Length)
                {
                    int i = 0;

                    while ((i < lineToCheck.Length) && (lineToCheck[i] == HexString[i]))
                    {
                        i += 1;
                    }

                    if (i == lineToCheck.Length)
                    {
                        equals = true;
                    }
                }

                if (equals)
                {
                    Console.WriteLine("Password has been compromised");
                    reader.Close();
                    dataStream.Close();
                    response.Close();
                }
                else
                {
                    Console.WriteLine("hashes are different");
                    reader.Close();
                    dataStream.Close();
                    response.Close();
                }
            }
            







        }
    }
}
