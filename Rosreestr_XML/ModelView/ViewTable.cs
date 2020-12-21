using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosreestr_XML.Data;
namespace Rosreestr_XML.ModelView
{
    public class ViewTable
    {
        public string Name { get; set; }
        public ObservableCollection<ViewGroup> Groups { get; set; }

        public ViewTable(TableXML table)
        {
            Name = table.NameTable;
            Groups = new ObservableCollection<ViewGroup>
                (table.Groups.Select(x => new ViewGroup(x)));
        }
        /*
        public void DownloadSelectedSchemes()
        {
            foreach (var item in Groups)
            {
                item.DownloadSelectedSchemes();
            }
        }
        */
    }
}
