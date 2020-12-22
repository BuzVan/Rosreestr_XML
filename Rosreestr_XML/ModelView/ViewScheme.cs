using Rosreestr_XML.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rosreestr_XML.ModelView
{
    public class ViewScheme : INotifyPropertyChanged
    {
        private ViewGroup parent;
        public SchemeXML Scheme { get; }
        internal bool isChecked;
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                this.isChecked = value;
                parent.IsCheckedCheck();
                OnPropertyChanged("IsChecked");
            }
        }
        
        public ViewScheme(ViewGroup parent, SchemeXML scheme)
        {
            this.parent = parent;
            Scheme = scheme;
            isChecked = true;
        }
        public void DownloadScheme()
        {
             Scheme.DownloadScheme();
        }
        public void DownloadOrder()
        {
             Scheme.DownloadOrder();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
