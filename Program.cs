using System;
using System.Net;
using System.Net.Security;
using static System.Console;
using System.Security.Cryptography.X509Certificates;


namespace CValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            String defaultUrl = "https://www.google.com";
            WriteLine("\n Insert website url or press enter key for default url: https://www.google.com \n");
            try
            {
                var userUrl = ReadLine();
                if(userUrl != "")
                {
                    defaultUrl = userUrl;
                }
                
            }
            catch(Exception ex)
            {
                WriteLine($"\n {ex}");
            }
            try
            {
                WriteLine($"Checking certificate chain for {defaultUrl} \n");
                HttpWebRequest request = WebRequest.CreateHttp(defaultUrl);
                request.ServerCertificateValidationCallback += ServerCertificateValidationCallback;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) { }
                WriteLine("\n End");
            }
            catch(Exception ex)
            {
                WriteLine($"\n {ex}");
                ReadLine();
            }
            ReadLine();
        }
        private static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            bool isValid = false;
            switch(sslPolicyErrors)
            {
                case SslPolicyErrors.None: WriteLine("Certificate is OK \n");
                    WriteLine("The chain is:");
                    for (int cert = 0; cert < chain.ChainElements.Count; cert++)
                    {
                        WriteLine($" cert {chain.ChainElements[cert].Certificate.Subject.ToString()}, expiration date  {chain.ChainElements[cert].Certificate.GetExpirationDateString()}");
                    }
                     isValid = true;  break;
                case SslPolicyErrors.RemoteCertificateChainErrors: WriteLine("Error(s) in certificate Chain:");
                    for (int err = 0; err < chain.ChainStatus.Length; err++)
                    {
                        WriteLine(chain.ChainElements[err].Certificate.RawData.ToString());
                        WriteLine($"status: { chain.ChainStatus[err].Status}" );
                        WriteLine($"status: { chain.ChainStatus[err].StatusInformation}");
                    }
                    break;
            }
            return isValid;
            
        }
    }


}
