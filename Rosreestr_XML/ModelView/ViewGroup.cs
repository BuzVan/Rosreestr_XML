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
    public class ViewGroup : INotifyPropertyChanged
    {
        internal readonly ViewTable Parent;
        public string Name { get; set; }
        public ObservableCollection<ViewScheme> Schemes { get; set; }

        private bool? isChecked = true;
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
                Parent.IsCheckedCheck();
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
                Parent.IsCheckedCheck();
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
                foreach (var item in Schemes)
                {
                    item.IsEnabled = value;
                }
                OnPropertyChanged("IsEnabled");
            }
        }
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

        public ViewGroup(ViewTable parent, GroupXML group)
        {
            this.Parent = parent;
            Name = group.NameGroup;
            Schemes = new ObservableCollection<ViewScheme>
                (group.Schemes.Select(x => new ViewScheme(this,x)));
            
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        internal void DownloadSelectedAll(string folder)
        {
            if (this.IsChecked != false)
            {
                foreach (var item in Schemes)
                {
                    item.DownloadSelectedAll(folder);
                }
            }
            isChecked = false;
            OnPropertyChanged("IsChecked");
            isEnabled = true;
            OnPropertyChanged("IsEnabled");
        }
        internal void DownloadSelectedFile(string folder)
        {
            if (this.IsChecked != false)
            {
                foreach (var item in Schemes)
                {
                    item.DownloadSelectedFile(folder);
                }
            }
            isChecked = false;
            OnPropertyChanged("IsChecked");
            isEnabled = true;
            OnPropertyChanged("IsEnabled");
        }

        internal void SelectDifference(DifferenceType[] differences)
        {
            if (differences[0] == DifferenceType.Same || differences[0] == DifferenceType.NotSame)
                return;
            differenceTypes.AddRange(differences);
            differenceTypes = differenceTypes.Distinct().ToList();
            Parent.SelectDifference(differenceTypes.ToArray());
            OnPropertyChanged("Background");
        }


    }
}
