using Rosreestr_XML.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Rosreestr_XML.ModelView
{
    public class ViewScheme : INotifyPropertyChanged
    {

        internal readonly ViewGroup Parent;
        public SchemeXML Scheme { get; }

        private ChangedVisibility differences;
        public ChangedVisibility Differences
        {
            get => differences;
            set
            {
                differences = value;
                OnPropertyChanged("Differences");
            }
        }

        private bool isChecked = true;
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                this.isChecked = value;
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
                OnPropertyChanged("IsEnabled");
            }
        }
        public bool FileDownloadEnabled => this.Scheme.FileLink.Count > 0;
        public string FileDownloadText => FileDownloadEnabled ? "Скачать Схему" : "Недоступно";
        public bool OrderDownloadEnabled => this.Scheme.OrderLink.Count > 0;
        public string OrderDownloadText => OrderDownloadEnabled ? "Скачать Приказ" : this.Scheme.OrderInfo != null ? this.Scheme.OrderInfo : "Недоступно";
        public ViewScheme(ViewGroup parent, SchemeXML scheme)
        {
            this.Parent = parent;
            Scheme = scheme;
            this.Differences = new ChangedVisibility(differenceTypes);
            OnPropertyChanged("FileDownloadEnabled");
            OnPropertyChanged("OrderDownloadEnabled");
            OnPropertyChanged("FileDownloadText");
            OnPropertyChanged("OrderDownloadText");
        }
        public void DownloadFile(string folder)
        {
            Scheme.DownloadScheme(folder, Parent.Parent.Name);
        }
        public void DownloadOrder(string folder)
        {
            Scheme.DownloadOrder(folder, Parent.Parent.Name);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        internal void DownloadSelectedAll(string folder)
        {
            IsEnabled = false;
            if (IsChecked)
            {
                DownloadFile(folder);
                DownloadOrder(folder);

            }
            isChecked = false;
            OnPropertyChanged("IsChecked");
            IsEnabled = true;
            
        }
        internal void DownloadSelectedFile(string folder)
        {
            IsEnabled = false;
            if (IsChecked)
                DownloadFile(folder);
            isChecked = false;
            OnPropertyChanged("IsChecked");
            IsEnabled = true;
        }
        private DifferenceType[] differenceTypes = new DifferenceType[] { DifferenceType.Same };
        public Brush Background
        {
            get
            {
                if (differenceTypes[0] == DifferenceType.Same || differenceTypes[0] == DifferenceType.NotSame)
                    return Brushes.Transparent;
                if (differenceTypes.Contains(DifferenceType.DeleteScheme))
                    return Brushes.OrangeRed;
                if (differenceTypes.Contains(DifferenceType.NewScheme))
                    return Brushes.GreenYellow;
                return Brushes.Aquamarine;
            }
        }
        internal void SelectDifference(DifferenceType[] differences)
        {
            this.differenceTypes = differences;
            this.Differences = new ChangedVisibility(differences); 
            Parent.SelectDifference(differenceTypes);
            OnPropertyChanged("Background");
        }

    }
}
