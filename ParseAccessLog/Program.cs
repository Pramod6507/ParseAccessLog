using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ParseAccessLog
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string[] text = File.ReadAllLines(@"E:\Work\Valtech\access.log", Encoding.UTF8);

            List<string> clientIPs = new List<string>();
            var accessCount = new List<KeyValuePair<int, string>>();

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
            
            var sortedClientIPs = clientIPs.OrderByDescending(ip => AccessParseHelper.IpAddressLabel(ip));
            List<string> uniqueIPs = sortedClientIPs.Distinct().ToList();
            foreach (var IP in uniqueIPs)
            {
                int count = sortedClientIPs.Where(x => x.Equals(IP)).Count();
                //Console.WriteLine(string.Join(", ",count, IP ));
                accessCount.Add(new KeyValuePair<int, string>(count, IP));

            }
            
            var sortedAccessCount = accessCount.OrderByDescending(c => c.Key).ToList();
            var arrayList = new ArrayList();

            foreach (var countPair in sortedAccessCount)
            {
                List<string> ipOctets = countPair.Value.Split('.').ToList();
                int[] arr = { countPair.Key, int.Parse(ipOctets[0]), int.Parse(ipOctets[1]), int.Parse(ipOctets[2]), int.Parse(ipOctets[3]) };
                arrayList.Add(arr);
                Console.WriteLine(string.Join(", ", countPair.Key, countPair.Value));
            }

            
            Console.WriteLine("End");
        }


    }
}
