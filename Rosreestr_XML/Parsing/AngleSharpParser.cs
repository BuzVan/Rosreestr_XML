using AngleSharp;
using Rosreestr_XML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace Rosreestr_XML.Parsing.AngleSharp
{
    class AngleSharpParser
    {
        private const string BaseAddr = "https://rosreestr.gov.ru";

        public TableXML[] Parse(IHtmlDocument document)
        {
            //таблицы
            var docTables = document.QuerySelectorAll("tbody").Where(x => x.ChildElementCount > 2);
            //заголовки таблиц
            string[] titles = document.
                QuerySelectorAll("[color='#0072bc']").
                Select(x => x.TextContent).
                Where(x => x.Trim().Length > 0).ToArray();

            TableXML[] result = new TableXML[titles.Length];
            for (int i = 0; i < titles.Length; i++)
            {
                TableXML table = new TableXML(titles[i]);
                var lines = docTables.ElementAt(i).QuerySelectorAll("tr").Skip(1);
                //пропуск названий столбцов

                int nextGroup = 1;

                foreach (var item in lines)
                {

                    if (IsGroup(item, nextGroup))
                    {
                        nextGroup++;
                        if (nextGroup == table.Groups.Count + 2)
                            table.Groups.Add(GetGroup(item));
                    }
                    else
                    {
                        // Ошибочная строка - новая группа 9, (но после неё описаны 8.3 и 8.3 в  неактуальных)
                        if (item.ChildElementCount != 4)
                        {
                            var docGr = item.QuerySelector("b");
                            if (docGr != null)
                            {
                                GroupXML group = new GroupXML();
                                group.NameGroup = docGr.TextContent.Trim();
                                table.Groups.Add(group);
                            }
                        }
                        else
                            table.Groups[nextGroup - 2].Schemes.Add(GetScheme(item));
                    }
                }
                result[i] = table;
            }

            return result;
        }

        private GroupXML GetGroup(IElement item)
        {
            GroupXML group;
            // группа  - только имя
            if (item.ChildElementCount == 1)
            {
                group = new GroupXML(item.TextContent.Trim());
            }
            // группа - отдельная xml схема
            else
            {
                group = new GroupXML();
                group.Schemes.Add(GetScheme(item));
                group.NameGroup = group.Schemes.First().Num;

            }
            return group;
        }

        //строка - группа, котрая имеет только имя
        private bool IsGroup(IElement item, int num)
        {

            return item.Children.First().TextContent.Trim().StartsWith(num + "");
        }
        // разбор схемы
        private SchemeXML GetScheme(IElement item)
        {
            SchemeXML scheme = new SchemeXML();
            SetNum(ref scheme, item.Children[0]);
            SetName(ref scheme, item.Children[1]);
            SetFile(ref scheme, item.Children[2]);
            SetOrder(ref scheme, item.Children[3]);
            return scheme;
        }

        private void SetOrder(ref SchemeXML scheme, IElement docOrder)
        {
            var docLinks = docOrder.QuerySelectorAll("a");
            if (docLinks.Length > 0)
            {
                for (int i = 0; i < docLinks.Length; i++)
                    scheme.OrderLink.Add_NotEq(BaseAddr + docLinks[i].GetAttribute("href").Trim());

            }
            else
            {
                scheme.OrderInfo = RemoveAllTrim(docOrder.TextContent);
            }
        }

        private void SetFile(ref SchemeXML scheme, IElement docFile)
        {
            var docLink = docFile.QuerySelector("a");
            if (docLink != null)
                scheme.FileLink.Add_NotEq(BaseAddr + docLink.GetAttribute("href").Trim());
        }

        //выделение наименования
        /*
         * [NAME_INFO]
         * 
         * [ссылка на файл с текстом NAME]
         * [остаток текста NAME]
         */
        private void SetName(ref SchemeXML scheme, IElement docName)
        {
            //первый не пустой ребёнок
            IElement child = docName.Children.FirstOrDefault(x => x.TextContent.Length > 1);
            //ссылка
            var docLink = docName.QuerySelector("a");
            //нет ссылки => есть только имя
            if (docLink == null)
            {
                scheme.Name = docName.TextContent.Trim();
            }

            //нет ссылки в первом ребёнке => это Name_Info
            else if (child != null && child.QuerySelector("a") == null)
            {
                scheme.NameInfo = RemoveAllTrim(child.TextContent);
                //ссылка это имя

                scheme.Name = RemoveAllTrim(docLink.TextContent);
                scheme.FileLink.Add_NotEq(BaseAddr + docLink.GetAttribute("href").Trim());


            }
            //имя находится в сслыке и в тексте после ней
            else
            {
                scheme.Name = RemoveAllTrim(docName.TextContent);
                scheme.FileLink.Add_NotEq(BaseAddr + docLink.GetAttribute("href").Trim());
            }
        }

        private string RemoveAllTrim(string textContent)
        {
            string[] parts = textContent.Trim().Split('\n');
            StringBuilder res = new StringBuilder();
            foreach (var item in parts)
            {
                if (item.Trim().Length > 0)
                    res.Append(item.Trim());
            }
            return res.ToString();
        }

        private void SetNum(ref SchemeXML scheme, IElement docNum)
        {
            //выделение номера
            if (docNum != null)
                scheme.Num = docNum.TextContent.Trim();
        }


    }
}
