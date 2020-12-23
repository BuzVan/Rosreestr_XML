using Rosreestr_XML.Util;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// <summary>
    /// Возможные различия между новыми и старыми схемами
    /// </summary>
    public enum DifferenceType
    {
        /// <summary>
        /// Одинаковые
        /// </summary>
        Same,
        /// <summary>
        /// Совершенно разные
        /// </summary>
        NotSame,
        /// <summary>
        /// Разница в подписи
        /// </summary>
        DifTextProjectDoc,
        /// <summary>
        /// Разница в ссылках файла
        /// </summary>
        DifferentFileLink,
        /// <summary>
        /// Разница в ссылках приказов
        /// </summary>
        DifferentOrderLink,
        /// <summary>
        /// Новая схема
        /// </summary>
        NewScheme,
        /// <summary>
        /// Удалённая схема
        /// </summary>
        DeleteScheme
    }
    /// <summary>
    /// Модель представления XML-схемы сайта Росреестра
    /// </summary>
    [Serializable]
    
    public class SchemeXML
    {
        
        private string num;
        /// <summary>
        /// Номер схемы
        /// </summary>
        public string Num
        {
            get=> num;
            set
            {
                num = value.Trim(' ', '.');
            }
        }
        /// <summary>
        /// Имя схемы
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Информация о схеме (ПРОЕКТ ДОКУМЕНТА)
        /// </summary>
        public string NameInfo { get; set; }

        /// <summary>
        /// Все ссылки для скачивания схемы
        /// </summary>
        public List<string> FileLink { get; set; }
        
        /// <summary>
        /// Папка схемы. Путь состоит из номера группы и схемы
        /// </summary>
        private string SchemeFolderPath
        {
            get
            {
                if (Num == null) throw new ArgumentException("Num не установлен. Путь к файлу выделить невозможно");
                string[] str = Num.Trim(' ', '.').Split('.');
                if (str.Length ==0) throw new ArgumentException("Num не установлен. Путь к файлу выделить невозможно");
                string res = "";
                foreach (var item in str)
                {
                    if (str.Length > 0)
                        res += "\\" + item;
                }
                return res.Trim();

            }
        }
        /// <summary>
        /// Путь к папке файла начиная с имени таблицы
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetFolderPath(string tableName) => string.Format($"{tableName}\\{SchemeFolderPath}");

        /// <summary>
        /// Информация о приказе
        /// </summary>
        public String OrderInfo { get; set; }
        /// <summary>
        /// Ссылки приказов для скачивания
        /// </summary>
        public List<string> OrderLink { get; set; }

        public SchemeXML()
        {
            Num = "";
            NameInfo = "";
            Name = "";
            OrderInfo = "";
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

        /// <summary>
        /// Скачать приказ
        /// </summary>
        /// <param name="folder">путь к папке таблицы</param>
        /// <param name="tableName">имя таблицы, которой пренадлежит схема</param>
        internal void DownloadOrder(string folder, string tableName)
        {
            if (OrderLink.Count == 0) return;
            if (tableName.Length > 50) tableName = tableName.Remove(50);
            string folderPath = string.Format($"{folder}\\{tableName}\\{SchemeFolderPath}\\Приказ");
            foreach (var link in OrderLink)
            {
                Uri addr = new Uri(link);
                FileDownloader.Download(addr, folderPath);       
            }
           
        }
        /// <summary>
        /// Скачать схему
        /// </summary>
        /// <param name="folder">путь к папке таблицы</param>
        /// <param name="tableName">имя таблицы, которой пренадлежит схема</param>
        internal void DownloadScheme(string folder, string tableName)
        {
            if (FileLink.Count == 0) return;
            if (tableName.Length > 50) tableName = tableName.Remove(50);
            string folderPath = string.Format($"{folder}\\{tableName}\\{SchemeFolderPath}\\Схема");
            foreach (var link in FileLink)
            {
                Uri addr = new Uri(link);
                FileDownloader.Download(addr, folderPath);
            }
        }
        /// <summary>
        /// Поиск различий между схемами
        /// </summary>
        /// <param name="scheme">вторая схема</param>
        /// <returns>Массив различий схем</returns>
        internal DifferenceType[] FindDifferents(SchemeXML scheme)
        {
           
            //разные имена и номера - разные схемы
            if (scheme.Name != this.Name || scheme.Num != this.Num)
                return new DifferenceType[] { DifferenceType.NotSame };

            List<DifferenceType> res = new List<DifferenceType>();

            if (this.NameInfo != scheme.NameInfo)
                res.Add(DifferenceType.DifTextProjectDoc);
            
            if (this.FileLink.Count != scheme.FileLink.Count)
                res.Add(DifferenceType.DifferentFileLink);
            else
            {
                for (int i=0;i<FileLink.Count;i++)
                    if (this.FileLink[i] != scheme.FileLink[i])
                    {
                        res.Add(DifferenceType.DifferentFileLink);
                        break;
                    }

            }
            if (this.OrderLink.Count != scheme.OrderLink.Count )
                res.Add(DifferenceType.DifferentOrderLink);
            else
            {
                for (int i = 0; i < OrderLink.Count; i++)
                    if (this.OrderLink[i] != scheme.OrderLink[i])
                    {
                        res.Add(DifferenceType.DifferentOrderLink);
                        break;
                    }

            }

            if (res.Count == 0)
                res.Add(DifferenceType.Same);

            return res.ToArray();

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
