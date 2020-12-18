using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosreestr_XML.Data
{
    public class XML_Scheme
    {
        public string Num { get; set; }
        public string Name { get; set; }
        public string NameInfo { get; set; }
        public string FileLink { get; set; }

        public String OrderInfo { get; set; }
        public string[] AllOrderLink = new string[0];

        public XML_Scheme()
        {
        }

        public XML_Scheme(string name, string nameInfo, 
            string fileLink, string orderInfo, string[] allOrderLink)
        {
            Name = name;
            NameInfo = nameInfo;
            FileLink = fileLink;
            OrderInfo = orderInfo;
            AllOrderLink = allOrderLink;
        }



        public override string ToString()
        {

            string links = "";
            
            foreach (var item in AllOrderLink)
                links += "\n\t\t" + item;
            
            return
                $@"{Num}  {Name}:
    Info: {NameInfo}
    Link: {FileLink}
    OrderInfo: {OrderInfo}
    OrderLink: {links}";
        }
    }
}
