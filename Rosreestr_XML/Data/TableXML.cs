using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosreestr_XML.Data
{
    public class TableXML : ICollection<GroupXML>
    {
        public string NameTable { get; set; }

        public int Count => gropes.Count;

        public bool IsReadOnly => false;

        private List<GroupXML> gropes;

        public GroupXML this[int i]
        {
            get { return gropes[i]; }
            set { gropes[i] = value; }
        }

        public TableXML(string nameTable)
        {
            NameTable = nameTable;
            gropes = new List<GroupXML>();
        }

        public TableXML(string nameTable, List<GroupXML> gropes) : this(nameTable)
        {
            this.gropes = gropes;
        }

        public void Add(GroupXML item)
        {
            gropes.Add(item);
        }

        public void Clear()
        {
            gropes.Clear();
        }

        public bool Contains(GroupXML item)
        {
            return gropes.Contains(item);
        }

        public void CopyTo(GroupXML[] array, int arrayIndex)
        {
            gropes.CopyTo(array, arrayIndex);
        }

        public bool Remove(GroupXML item)
        {
           return gropes.Remove(item);
        }

        public IEnumerator<GroupXML> GetEnumerator()
        {
            return gropes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return gropes.GetEnumerator();
        }
        public override string ToString()
        {
            return string.Format("Table {0}: {1}", NameTable, Count); 
        }
    }
}
