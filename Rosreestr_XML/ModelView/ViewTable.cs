using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Rosreestr_XML.Data;
namespace Rosreestr_XML.ModelView
{
    public class ViewTable : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public ObservableCollection<ViewGroup> Groups { get; set; }

        private bool? isChecked = true;
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
        private bool isEnabled = true;
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                foreach (var item in Groups)
                {
                    item.IsEnabled = value;
                }
                OnPropertyChanged("IsEnabled");
            }
        }
        public object DownloadSelectredFiles { get; internal set; }

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
        public bool IsChanged => differenceTypes.Count != 0;

        private List<DifferenceType> differenceTypes = new List<DifferenceType>();
        public Brush Background
        {
            get
            {
                if (differenceTypes.Count == 0)
                    return Brushes.Transparent;
                if (differenceTypes.Count > 1)
                    return Brushes.Yellow;
                if (differenceTypes.Contains(DifferenceType.DeleteScheme))
                    return Brushes.OrangeRed;
                if (differenceTypes.Contains(DifferenceType.NewScheme))
                    return Brushes.GreenYellow;
                return Brushes.Aquamarine;
            }
        }
        public ViewTable(TableXML table)
        {  
            Name = table.NameTable;
            Groups = new ObservableCollection<ViewGroup>
                (table.Groups.Select(x => new ViewGroup(this,x)));
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public async Task DownloadSelectedAllAsync(string folder)
        {
            await Task.Run(() => DownloadSelectedAll(folder));
        }

        private void DownloadSelectedAll(string folder)
        { 
            if (this.IsChecked != false)
            {
                foreach (var item in Groups)
                {
                    item.DownloadSelectedAll(folder);
                }
            }
            isChecked = false;
            OnPropertyChanged("IsChecked");
            isEnabled = true;
            OnPropertyChanged("IsEnabled");
        }
        internal async Task DownloadSelectedFileAsync(string downloadPath)
        {
            await Task.Run(() => DownloadSelectedFile(downloadPath));
        }

        private void DownloadSelectedFile(string downloadPath)
        {
            IsEnabled = false;
            if (this.IsChecked != false)
            {
                foreach (var item in Groups)
                {
                    item.DownloadSelectedFile(downloadPath);
                }
            }
            isChecked = false;
            OnPropertyChanged("IsChecked");
            IsEnabled = true;
        }

        internal void SelectDifference(DifferenceType[] differences)
        {
            if (differences[0] == DifferenceType.Same || differences[0] == DifferenceType.NotSame)
                return;
            differenceTypes.AddRange(differences);
            differenceTypes = differenceTypes.Distinct().ToList();
            OnPropertyChanged("Background");
        }


    }
}
