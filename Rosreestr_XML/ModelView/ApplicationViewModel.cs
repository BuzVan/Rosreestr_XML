using Rosreestr_XML.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Rosreestr_XML.ModelView
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        private ViewScheme selectedScheme;
        public ObservableCollection<ViewTable> Tables { get; set; }

        DataXMLWorker dataWorker;

        public ViewScheme SelectedScheme
        {
            get { return selectedScheme; }
            private set
            {
                selectedScheme = value;
                OnPropertyChanged("SelectedScheme");
            }
            
        }
        public void SelectScheme(object scheme)
        {
            if (scheme is ViewScheme)
                SelectedScheme = (ViewScheme)scheme;
        }
        public ApplicationViewModel()
        {
            dataWorker = new DataXMLWorker();
            Tables = new ObservableCollection<ViewTable>();
            List<ViewTable> res;
            if (dataWorker.TryOpenTables(out res))
                SetTables(res);
            selectedScheme = Tables.First().Groups.First().Schemes.First();
        }

        public async Task Download()
        {
            List<ViewTable> data = await dataWorker.ParseTables();
            SetTables(data);
        }

        public void Save()
        {
            dataWorker.SaveTables();
        }

        public void Open()
        {
            SetTables(dataWorker.OpenTables());
        }
        private void SetTables(List<ViewTable> data)
        {
            Tables.Clear();
            foreach (var item in data)
            {
                Tables.Add(item);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
