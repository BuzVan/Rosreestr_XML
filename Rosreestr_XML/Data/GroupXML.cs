using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosreestr_XML.Data
{
    [Serializable]
    public class GroupXML
    {
        public string NameGroup { get; set; }
        public List<SchemeXML> Schemes { get; set; }

        public SchemeXML this[int i]
        {
            get { return Schemes[i]; }
            set { Schemes[i] = value; }
        }
        public GroupXML()
        {

        }
        public GroupXML(string nameGroup) : this()
        {
            Schemes = new List<SchemeXML>();
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
