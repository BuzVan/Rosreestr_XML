using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rosreestr_XML.Data;
namespace Rosreestr_XML.ModelView
{
    public class ViewGroup
    {
        public string Name { get; set; }
        public ObservableCollection<ViewScheme> Schemes { get; set; }

        public ViewGroup()
        {
        }
        public ViewGroup(GroupXML group)
        {
            Name = group.NameGroup;
            Schemes = new ObservableCollection<ViewScheme>
                (group.Schemes.Select(x => new ViewScheme(x)));
        }
        /*
        public void DownloadSelectedSchemes()
        {
            foreach (var item in Schemes)
            {
                if (item.IsChecked) 
                    item.DownloadScheme();
            }
        }
        */
    }
}
