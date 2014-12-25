using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Strat!");
            Dictionary<string,string> dict= new Dictionary<string,string>();
            dict.Add("AK", "0");
            checked
            {
                int NUM = 1;
                string txt = "AK";
                while (dict.ContainsKey(txt))
                {
                    txt = "AK" + (NUM.ToString());
                    NUM++;
                }
            }
            Console.ReadKey();
        }
    }
}
