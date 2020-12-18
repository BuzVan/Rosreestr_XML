using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosreestr_XML.Data
{
    public class GroupXML: ICollection<XML_Scheme>
    {
        public string NameGroup;
        private List<XML_Scheme> shchemes;

        public int Count => shchemes.Count;

        public bool IsReadOnly => false;

        public XML_Scheme this[int i]
        {
            get { return shchemes[i]; }
            set { shchemes[i] = value; }
        }

        public GroupXML(string nameGroup)
        {
            NameGroup = nameGroup;
            shchemes = new List<XML_Scheme>();
        }

        public GroupXML(string nameGroup, List<XML_Scheme> xML_Schemes)
        {
            NameGroup = nameGroup;
            this.shchemes = xML_Schemes;
        }

        public void Add(XML_Scheme item)
        {
            shchemes.Add(item);
        }

        public void Clear()
        {
            shchemes.Clear();
        }

        public bool Contains(XML_Scheme item)
        {
            return shchemes.Contains(item);
        }

        public void CopyTo(XML_Scheme[] array, int arrayIndex)
        {
            shchemes.CopyTo(array, arrayIndex);
        }

        public bool Remove(XML_Scheme item)
        {
           return shchemes.Remove(item);
        }

        public IEnumerator<XML_Scheme> GetEnumerator()
        {
            return shchemes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return shchemes.GetEnumerator();
        }
        public override string ToString()
        {
            return string.Format("Group {0}: {1}", NameGroup, Count);
        }
    }
}
