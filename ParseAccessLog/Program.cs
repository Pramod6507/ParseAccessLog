using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace ParseAccessLog
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {


                Console.WriteLine("Hello World!");
                string[] text = AccessParseHelper.ReadAccessLog(@"E:\Work\Valtech\access.log");

                List<string> clientIPs = AccessParseHelper.ExtractClientIPs(text);
                IOrderedEnumerable<string> sortedClientIPs = AccessParseHelper.SortIPsByAddress(clientIPs);
                List<string> uniqueIPs = AccessParseHelper.ExtractDistinctIPs(sortedClientIPs);

                List<KeyValuePair<int, string>> accessCount = AccessParseHelper.CountClientAccess(sortedClientIPs, uniqueIPs);
                List<KeyValuePair<int, string>> sortedAccessCount = AccessParseHelper.SortIPsByNoOfAccess(accessCount);

                AccessParseHelper.WriteReportToFile(sortedAccessCount, @"E:\Work\Valtech\report.txt");

                Console.WriteLine("New file created: {0}", DateTime.Now.ToString());
                Console.WriteLine("=======================================================================");
                Console.WriteLine("Final List");
                Console.WriteLine("=======================================================================");

                foreach (var countPair in sortedAccessCount)
                {
                    Console.WriteLine(string.Join(", ", countPair.Key, countPair.Value));
                }


                Console.WriteLine("End");

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());                
            }
        }


    }
}
