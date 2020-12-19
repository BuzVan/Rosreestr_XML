using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosreestr_XML.Data
{
    // расшириение списка. 
    public static class ListExtensions
    {
        // добавление нового элемента, если его нет в списке.
        public static void Add_NotEq(this List<string> arr, string str)
        {
            if (!arr.Contains(str)) arr.Add(str);
        }
    }
    public class XML_Scheme
    {
        public string Num { get; set; }
        public string Name { get; set; }
        public string NameInfo { get; set; }
        public List<string> FileLink { get; set; }

        public String OrderInfo { get; set; }
        public List<string> OrderLink { get; set; }

        public XML_Scheme()
        {
            FileLink = new List<string>();
            OrderLink = new List<string>();
        }

        public XML_Scheme(string num, string name, string nameInfo, List<string> fileLink, string orderInfo, List<string> orderLink)
        {
            Num = num;
            Name = name;
            NameInfo = nameInfo;
            FileLink = fileLink;
            OrderInfo = orderInfo;
            OrderLink = orderLink;
        }

        public override string ToString()
        {

            //string orderLinks = "";
            
            //foreach (var item in OrderLink)
            //    orderLinks += "\n\t\t" + item;

            //string fileLinks = "";
            //foreach (var item in FileLink)
            //{
            //    fileLinks+= "\n\t\t" + item;
            //}
            return
                $@"{Num}  {Name}:
    Info: {NameInfo}
    Link count: {FileLink.Count}
    OrderInfo: {OrderInfo}
    OrderLink count: {OrderLink.Count}";
        }

    }
}
