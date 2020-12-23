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
    /// <summary>
    /// МОдель отображения группы
    /// </summary>
    public class ViewGroup : INotifyPropertyChanged
    {
        /// <summary>
        /// Таблица - родитель группы
        /// </summary>
        internal readonly ViewTable Parent;
        /// <summary>
        /// имя группы
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Список схем
        /// </summary>
        public ObservableCollection<ViewScheme> Schemes { get; set; }

        private bool? isChecked = true;
        /// <summary>
        /// Выбрана ли группа. Связь с CheckBox
        /// </summary>
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
        //проверить IsChecked (посмотрев на детей)
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
        /// <summary>
        /// Активирована ли группа (связана с CheckBox.Enabled)
        /// </summary>
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }
        /// <summary>
        /// Cписок изменений в группе
        /// </summary>
        private List<DifferenceType> differenceTypes = new List<DifferenceType>();
        /// <summary>
        /// Цвет строки группы в зависимости от списка изменений
        /// </summary>
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
        /// <summary>
        /// Скачать выбранные схемы с приказами
        /// </summary>
        /// <param name="folder"></param>
        internal void DownloadSelectedAll(string folder)
        {
            IsEnabled = false;
            if (this.IsChecked != false)
            {
                foreach (var item in Schemes)
                {
                    item.DownloadSelectedAll(folder);
                }
            }
            isChecked = false;
            OnPropertyChanged("IsChecked");
            IsEnabled = true;
        }
        /// <summary>
        /// Скачать выбранные схемы без приказов 
        /// </summary>
        /// <param name="folder"></param>
        internal void DownloadSelectedFile(string folder)
        {
            IsEnabled = false;
            if (this.IsChecked != false)
            {
                foreach (var item in Schemes)
                {
                    item.DownloadSelectedFile(folder);
                }
            }
            isChecked = false;
            OnPropertyChanged("IsChecked");
            IsEnabled = true;
        }
        /// <summary>
        /// Выделить отличия
        /// </summary>
        /// <param name="differences"></param>
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
