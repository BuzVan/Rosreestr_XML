using Rosreestr_XML.Data;
using Rosreestr_XML.Parser;
using Rosreestr_XML.Parser.AngleSharp;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser taskParser = new Parser();
            Task<TableXML[]> task = taskParser.ParseAsync();
            TableXML[] res = task.Result;
            Display(res);
            Console.ReadKey();
        }

        private static void Display(TableXML[] res)
        {
            foreach (var tab in res)
            {
                Console.WriteLine(tab);
                Console.ReadKey();
                foreach (var gr in tab)
                {
                    Console.WriteLine(gr);
                    Console.ReadKey();
                    foreach (var item in gr)
                    {
                        Console.WriteLine(item);
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
