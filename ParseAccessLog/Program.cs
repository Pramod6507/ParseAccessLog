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

                Console.WriteLine("End");

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());                
            }
        }


    }
}
