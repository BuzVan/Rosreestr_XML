using System;
using System.Collections.Generic;

namespace Rosreestr_XML.Data
{
    /// <summary>
    /// Модель группы таблицы XML-схем с сайта Росрееста
    /// </summary>
    [Serializable]
    public class GroupXML
    {
        /// <summary>
        /// Имя группы
        /// </summary>
        public string NameGroup { get; set; }
        /// <summary>
        /// Список схем группы
        /// </summary>
        public List<SchemeXML> Schemes { get; set; }

        public SchemeXML this[int i]
        {
            get { return Schemes[i]; }
            set { Schemes[i] = value; }
        }
        public GroupXML()
        {
            Schemes = new List<SchemeXML>();
        }
        public GroupXML(string nameGroup) : this()
        {
            NameGroup = nameGroup;
        }

        public GroupXML(string nameGroup, List<SchemeXML> xML_Schemes)
        {
            NameGroup = nameGroup;
            this.Schemes = xML_Schemes;
        }


        public override string ToString()
        {
            return string.Format("Group {0}: {1}", NameGroup, Schemes.Count);
        }
    }
}
