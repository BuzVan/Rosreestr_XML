using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    [Serializable]
    public class SchemeXML
    {
        public static string MAIN_FOLDER = "Данные";
        public string Num { get; set; }
        public string Name { get; set; }
        public string NameInfo { get; set; }

        public List<string> FileLink { get; set; }
        
        public string FolderPath
        {
            get
            {
                if (Num == null) throw new ArgumentException("Num не установлен. Путь к файлу выделить невозможно");
                string[] str = Num.Trim(' ', '.').Split('.');
                if (str.Length ==0) throw new ArgumentException("Num не установлен. Путь к файлу выделить невозможно");
                string res = MAIN_FOLDER;
                foreach (var item in str)
                {
                    if (str.Length > 0)
                        res += "\\" + item;
                }
                return res;

            }
        }


        public String OrderInfo { get; set; }
        public List<string> OrderLink { get; set; }

        public SchemeXML()
        {
            FileLink = new List<string>();
            OrderLink = new List<string>();
        }

        public SchemeXML(string num, string name, string nameInfo, List<string> fileLink, string orderInfo, List<string> orderLink)
        {
            Num = num;
            Name = name;
            NameInfo = nameInfo;
            FileLink = fileLink;
            OrderInfo = orderInfo;
            OrderLink = orderLink;
        }

        internal void DownloadOrder()
        {
            if (FileLink.Count == 0) return;
            WebClient webClient = new WebClient();
            
            foreach (var link in FileLink)
            {
                Uri addr = new Uri(link);
                webClient.DownloadFile(addr, FolderPath + addr.Segments.Last() + "\\Order");
            }
           
        }
        internal void DownloadScheme()
        {
            if (OrderLink.Count == 0) return;
            WebClient webClient = new WebClient();

            foreach (var link in OrderLink)
            {
                Uri addr = new Uri(link);
                webClient.DownloadFile(addr, FolderPath + addr.Segments.Last() + "\\File");
            }
        }
        public override string ToString()
        {

            return
                $@"{Num}  {Name}:
    Info: {NameInfo}
    Link count: {FileLink.Count}
    OrderInfo: {OrderInfo}
    OrderLink count: {OrderLink.Count}";
        }
        
    }
}
