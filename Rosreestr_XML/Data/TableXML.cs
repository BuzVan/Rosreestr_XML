using Rosreestr_XML.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosreestr_XML.Data
{

    /// <summary>
    /// Модель Таблицы XML-схем с Сайта Росреестра
    /// </summary>
    [Serializable]
    public class TableXML
    {

        /// <summary>
        /// Имя таблицы
        /// </summary>
        public string NameTable { get; set; }

        /// <summary>
        /// Список групп в таблице
        /// </summary>
        public List<GroupXML> Groups { get; set; }

        public GroupXML this[int i]
        {
            get { return Groups[i]; }
            set { Groups[i] = value; }
        }

        public TableXML()
        {
           
        }

        public TableXML(string nameTable): this()
        {
            Groups = new List<GroupXML>();
            NameTable = nameTable;
        }

        public TableXML(string nameTable, List<GroupXML> gropes) : this(nameTable)
        {
            this.Groups = gropes;
        }
        public override string ToString()
        {
            return string.Format("Table {0}: {1}", NameTable, Groups.Count); 
        }

    }
}
