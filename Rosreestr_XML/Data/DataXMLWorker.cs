using Rosreestr_XML.ModelView;
using Rosreestr_XML.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rosreestr_XML.Data
{
    /// <summary>
    /// Помощник работы со схемами. Реализует основные функции работы с схемами-XML
    /// </summary>
    class DataXMLWorker
    {
        /// <summary>
        /// Имя файла, в котором сохраняются полученные xml-схемы
        /// </summary>
        private static string filename = "tables.xml";
        /// <summary>
        /// Массив полученных таблиц XML
        /// </summary>
        private TableXML[] data;
        public DataXMLWorker()
        {
            data = new TableXML[0];
        }
        /// <summary>
        /// Асинхронный парсинг таблиц с сайта росреестра
        /// </summary>
        /// <returns></returns>
        public async Task<List<ViewTable>> ParseTables()
        {
            Parsing.Parser taskParser = new Parsing.Parser();
            data = await taskParser.ParseAsync();
            return new List<ViewTable>(data.Select(x => new ViewTable(x)));
        }
        /// <summary>
        /// Сохранение таблиц в файл
        /// </summary>
        public void SaveTables()
        {
            TableSerialization.Serialize(filename, data);
        }
        /// <summary>
        /// Получитть таблицы из файла
        /// </summary>
        /// <returns></returns>
        public List<ViewTable> OpenTables()
        {
            data = TableSerialization.Deserialize(filename);
            return new List<ViewTable>(data.Select(x => new ViewTable(x)));
        }
        /// <summary>
        /// Попытаться получить таблицы из файла
        /// </summary>
        /// <param name="result"></param>
        /// <returns> true - если получилось получить таблицы</returns>
        public bool TryOpenTables(out List<ViewTable> result)
        {
            TableXML[] trydata = null;
            result = default;
            bool res = TableSerialization.TryDeserialize(filename, out trydata);
            if (res)
            {
                data = trydata;
                result = new List<ViewTable>(data.Select(x => new ViewTable(x)));
            }
            return res;
        }
        /// <summary>
        /// Получить схемы с сайта и выделить отличия с текущими схемами
        /// </summary>
        /// <returns>Таблицы с выделенем изменений</returns>
        public async Task<List<ViewTable>> DownloadAndSelectDifferentAsync()
        {
            Parsing.Parser taskParser = new Parsing.Parser();
            TableXML[] tempData = await taskParser.ParseAsync();
            List<ViewTable> new_data = new List<ViewTable>(tempData.Select(x => new ViewTable(x)));
            DifferenceType[] differences;

            //поиск новых и изменённых схем
            foreach (ViewTable new_t in new_data)
            {
                TableXML curTable = null;
                for (int i = 0; i < data.Length; i++)
                    if (data[i].NameTable == new_t.Name)
                    {
                        curTable = data[i];
                        break;
                    }
                        
                if (curTable == null)
                {
                    new_t.SelectDifference(new DifferenceType[] { DifferenceType.NewScheme });
                    continue;
                }

                foreach (ViewGroup new_gr in new_t.Groups)
                {
                    GroupXML curGroup = null;
                    for (int i = 0; i < curTable.Groups.Count; i++)
                        if (curTable.Groups[i].NameGroup == new_gr.Name)
                        {
                            curGroup = curTable.Groups[i];
                            break;
                        }
                           
                    if (curGroup == null)
                    {
                        new_gr.SelectDifference(new DifferenceType[] { DifferenceType.NewScheme });
                        continue;
                    }

                    foreach (ViewScheme new_sch in new_gr.Schemes)
                    {
                        bool IsNew = true;
                        for (int i = 0; i < curGroup.Schemes.Count; i++)
                        {
                            differences = curGroup.Schemes[i].FindDifferents(new_sch.Scheme);
                            //Новый, когда точно нет таких же
                            if (differences[0] != DifferenceType.NotSame)
                            {
                                IsNew = false;
                                //есть такой же, возможно изменён:
                                if (differences[0] != DifferenceType.Same)
                                    new_sch.SelectDifference(differences);
                                break;
                            }
                             
                        }
                        if (IsNew)
                        {
                            new_sch.SelectDifference(new DifferenceType[] { DifferenceType.NewScheme });

                        }

                    }
                }
            }
            //поиск старых удалённых схем
            foreach (TableXML old_t in data)
            {
                ViewTable curTable = null;
                for (int i = 0; i < new_data.Count; i++)
                    if (new_data[i].Name == old_t.NameTable)
                    {
                        curTable = new_data[i];
                        break;
                    }
                       
                if (curTable == null)
                {
                    curTable = new ViewTable(old_t);
                    curTable.SelectDifference(new DifferenceType[] { DifferenceType.DeleteScheme });
                    new_data.Add(curTable);
                    continue;
                }

                foreach (GroupXML old_gr in old_t.Groups)
                {
                    ViewGroup curGroup = null;
                    for (int i = 0; i < curTable.Groups.Count; i++)
                        if (curTable.Groups[i].Name == old_gr.NameGroup)
                        {
                            curGroup = curTable.Groups[i];
                            break;
                        }
                            
                    if (curGroup == null)
                    {
                        curGroup = new ViewGroup(curTable, old_gr);
                        curGroup.SelectDifference(new DifferenceType[] { DifferenceType.DeleteScheme });
                        curTable.Groups.Add(curGroup);
                        continue;
                    }
                    foreach (SchemeXML old_sch in old_gr.Schemes)
                    {
                        bool IsDelete = true;
                        for (int i = 0; i < curGroup.Schemes.Count; i++)
                        {
                            differences = curGroup.Schemes[i].Scheme.FindDifferents(old_sch);
                            if (differences[0] != DifferenceType.NotSame)
                            {
                                IsDelete = false;
                                break;
                            }
                        }
                        if (IsDelete)
                        {
                            ViewScheme curScheme = new ViewScheme(curGroup, old_sch);
                            curGroup.Schemes.Add(curScheme);
                            curScheme.SelectDifference(new DifferenceType[] { DifferenceType.DeleteScheme });
                            
                        }

                    }
                }
            }
            data = tempData;
            return new_data;
        }


    }
}
