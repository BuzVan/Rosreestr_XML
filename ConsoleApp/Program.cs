using Rosreestr_XML.Data;
using Rosreestr_XML.Parsing;
using System;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser taskParser = new Parser();
            TableXML[] res = taskParser.Parse();
            Display(res);
            Console.ReadKey();
        }

        private static void Display(TableXML[] res)
        {
            foreach (var tab in res)
            {
                Console.WriteLine(tab);
                Console.ReadKey();
                foreach (var gr in tab.Groups)
                {
                    Console.WriteLine(gr);
                    Console.ReadKey();
                    foreach (var item in gr.Schemes)
                    {
                        Console.WriteLine(item);
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
