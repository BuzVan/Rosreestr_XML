using Rosreestr_XML.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Rosreestr_XML.ModelView
{
    /// <summary>
    /// Класс отображений всех изменений схемы
    /// </summary>
    public class ChangedVisibility : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private Visibility differentOrderLink;
        private Visibility changedVis;
        private Visibility newScheme;
        private Visibility deleteScheme;
        private Visibility differentNameInfo;
        private Visibility differentFileLink;

        public ChangedVisibility(DifferenceType[] differences)
        {
            ChangedVis = NewScheme = DeleteScheme = DifferentFileLink = DifferentNameInfo = DifferentOrderLink = Visibility.Collapsed;
            
            foreach (var dif in differences)
                switch (dif)
                {
                    case DifferenceType.DifTextProjectDoc:
                        DifferentNameInfo = Visibility.Visible;
                        ChangedVis = Visibility.Visible;
                        break;
                    case DifferenceType.DifferentFileLink:
                        DifferentFileLink = Visibility.Visible;
                        ChangedVis = Visibility.Visible;
                        break;
                    case DifferenceType.DifferentOrderLink:
                        DifferentOrderLink = Visibility.Visible;
                        ChangedVis = Visibility.Visible;
                        break;
                    case DifferenceType.NewScheme:
                        NewScheme = Visibility.Visible;
                        ChangedVis = Visibility.Visible;
                        break;
                    case DifferenceType.DeleteScheme:
                        DeleteScheme = Visibility.Visible;
                        ChangedVis = Visibility.Visible;
                        break;
                    default:
                        break;
                }
        }

        public Visibility ChangedVis
        {
            get => changedVis;
            set
            {
                changedVis = value;
                OnPropertyChanged("ChangedVis");
            }
        }

        public Visibility NewScheme
        {
            get => newScheme;
            set
            {
                newScheme = value;
                OnPropertyChanged("NewScheme");
            }
        }


        public Visibility DeleteScheme
        {
            get => deleteScheme;
            set
            {
                deleteScheme = value;
                OnPropertyChanged("DeleteScheme");
            }
        }

        public Visibility DifferentNameInfo
        {
            get => differentNameInfo;
            set
            {
                differentNameInfo = value;
                OnPropertyChanged("DifferentNameInfo");
            }
        }

        public Visibility DifferentFileLink
        {
            get => differentFileLink;
            set
            {
                differentFileLink = value;
                OnPropertyChanged("DifferentFileLink");
            }
        }

        public Visibility DifferentOrderLink
        {
            get => differentOrderLink;
            set
            {
                differentOrderLink = value;
                OnPropertyChanged("DifferentOrderLink");
            }
        }
    }
}
