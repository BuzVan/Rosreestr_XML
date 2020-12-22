using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Rosreestr_XML.Data;
namespace Rosreestr_XML.ModelView
{
    public class ViewTable : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public ObservableCollection<ViewGroup> Groups { get; set; }

        private bool? isChecked;
        public bool? IsChecked
        {
            get => isChecked;
            set
            {
                if (value != (bool?)null)
                    foreach (var item in Groups)
                    {
                        item.IsChecked = value;
                    }
                this.isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }
        public void IsCheckedCheck()
        {
            int countTrue = Groups.Count(x => x.IsChecked == true);
            int countFalse = Groups.Count(x => x.IsChecked == false);
            bool? new_val = countTrue == Groups.Count ? true : countFalse == Groups.Count ? false : (bool?)null;
            if (isChecked != new_val)
            {
                isChecked = new_val;
                OnPropertyChanged("IsChecked");
            }

        }
        public ViewTable(TableXML table)
        {
           
            Name = table.NameTable;
            Groups = new ObservableCollection<ViewGroup>
                (table.Groups.Select(x => new ViewGroup(this,x)));
            isChecked = true;
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
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
