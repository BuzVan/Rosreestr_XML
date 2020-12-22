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
    public class ViewGroup : INotifyPropertyChanged
    {
        private ViewTable parent;
        public string Name { get; set; }
        public ObservableCollection<ViewScheme> Schemes { get; set; }

        private bool? isChecked;
        public bool? IsChecked
        {
            get => isChecked;
            set
            {
                if (value != (bool?)null)
                    foreach (var item in Schemes)
                    {
                        item.IsChecked = (bool)value;
                    }
                this.isChecked = value;
                parent.IsCheckedCheck();
                OnPropertyChanged("IsChecked");
            }
        }
        public void IsCheckedCheck()
        { 
            int count = Schemes.Count(x => x.IsChecked);
            bool? new_val = count == Schemes.Count ? true : count == 0 ? false : (bool?)null;
            if (isChecked != new_val)
            {
                isChecked = new_val;
                parent.IsCheckedCheck();
                OnPropertyChanged("IsChecked");
            }
               
        }
        public ViewGroup(ViewTable parent, GroupXML group)
        {
            this.parent = parent;
            Name = group.NameGroup;
            Schemes = new ObservableCollection<ViewScheme>
                (group.Schemes.Select(x => new ViewScheme(this,x)));
            isChecked = true;

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
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
