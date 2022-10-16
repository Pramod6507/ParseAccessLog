using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ParseAccessLog
{
    public static class AccessParseHelper
    {
        public static string IpAddressLabel(string ipAddress)
                => string.Join(".", ipAddress.Split('.').Select(part => part.PadLeft(3, '0')));

        public static string[] ReadAccessLog(string location)
        {
            return File.ReadAllLines(location, Encoding.UTF8);
        }

        public static List<string> ExtractClientIPs(string[] text)
        {
            List<string> clientIPs = new List<string>();
            foreach (var line in text)
            {
                if (line[0] != '#')
                {
                    string[] words = line.Split(' ');
                    if (words[7] == "80" && words[8] == "GET" && !words[2].StartsWith("207.114"))
                    {
                        clientIPs.Add(words[2]);
                    }

                }

            }

            return clientIPs;
        }



        public static List<KeyValuePair<int, string>> CountClientAccess(IOrderedEnumerable<string> sortedClientIPs, List<string> uniqueIPs)
        {
            var accessCount = new List<KeyValuePair<int, string>>();
            foreach (var IP in uniqueIPs)
            {
                int count = sortedClientIPs.Where(x => x.Equals(IP)).Count();
                Console.WriteLine(string.Join(", ", count, IP));
                accessCount.Add(new KeyValuePair<int, string>(count, IP));

            }

            return accessCount;
        }

        public static IOrderedEnumerable<string> SortIPsByAddress(List<string> clientIPs)
        {
            return clientIPs.OrderByDescending(ip => AccessParseHelper.IpAddressLabel(ip));
        }

        public static List<string> ExtractDistinctIPs(IOrderedEnumerable<string> sortedClientIPs)
        {
            return sortedClientIPs.Distinct().ToList();
        }

        public static List<KeyValuePair<int, string>> SortIPsByNoOfAccess(List<KeyValuePair<int, string>> accessCount)
        {
            return accessCount.OrderByDescending(c => c.Key).ToList();
        }


        public static void WriteReportToFile(List<KeyValuePair<int, string>> sortedAccessCount, string location)
        {
            FileInfo fileInfo = new FileInfo(location);

            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }

            using (StreamWriter sw = fileInfo.CreateText())
            {
                sw.WriteLine("New file created: {0}", DateTime.Now.ToString());
                sw.WriteLine("===============================================================================================================");
                sw.WriteLine("Final IP List");
                sw.WriteLine("===============================================================================================================");
                foreach (var countPair in sortedAccessCount)
                {
                    sw.WriteLine(string.Join(", ", countPair.Key, countPair.Value));
                }
                sw.WriteLine("End of file");
            }
        }

    }
}
