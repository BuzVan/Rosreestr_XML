using Rosreestr_XML.Data;
using Rosreestr_XML.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Rosreestr_XML.ModelView
{
    /// <summary>
    /// Представление Приложения
    /// </summary>
    class ApplicationViewModel : INotifyPropertyChanged
    {

        DataXMLWorker dataWorker;
        /// <summary>
        /// Все схемы
        /// </summary>
        public ObservableCollection<ViewTable> Tables { get; set; }
        /// <summary>
        /// Обновить схемы по данным
        /// </summary>
        /// <param name="data">Новые данные</param>
        private void SetTables(List<ViewTable> data)
        {
            Tables.Clear();
            foreach (var item in data)
            {
                Tables.Add(item);
            }
        }
        private ViewScheme selectedScheme;
        /// <summary>
        /// Выбранная схема
        /// </summary>
        public ViewScheme SelectedScheme
        {
            get { return selectedScheme; }
            private set
            {
                selectedScheme = value;
                OnPropertyChanged("SelectedScheme");
                OnPropertyChanged("SchemeSelectVis");
            }

        }



        /// <summary>
        /// Отображение информации о выбранной схеме
        /// Отображет, если выбрана какая-либо схема
        /// </summary>
        public Visibility SchemeSelectVis
        {
            get
            {
                return (SelectedScheme == null)? Visibility.Collapsed: Visibility.Visible;
            }
        }

        private string infoPanel;
        /// <summary>
        /// Текст информационной панели
        /// </summary>
        public string InfoPanel
        {
            get => infoPanel;
            set
            {
                infoPanel = value;
                OnPropertyChanged("InfoPanel");
            }
        }


        /// <summary>
        /// Выбрать схему
        /// </summary>
        /// <param name="scheme">Схема</param>
        public void SelectScheme(object scheme)
        {
            if (scheme is ViewScheme)
            {
                SelectedScheme = (ViewScheme)scheme;
            }
                
            else
                SelectedScheme = null;
        }
        public ApplicationViewModel()
        {
            dataWorker = new DataXMLWorker();
            Tables = new ObservableCollection<ViewTable>();
            List<ViewTable> res;
            if (dataWorker.TryOpenTables(out res))
            {
                SetTables(res);
                SelectDifferentAsync();
                InfoPanel = "Таблицы скачены с локального файла";
            }
                
            else
            {
                System.Windows.MessageBox.Show("Схемы не найдены. Будет произведено формирование схем по сайту ростеестра. Это займёт несколько секунд. Нажмите ОК чтобы продолжить");
                Download();
            }
           
        }

        /// <summary>
        /// Скачать таблицы с сайта
        /// </summary>
        /// <returns></returns>
        public async Task Download()
        {
            InfoPanel = "Скачивание таблиц с сайта...";
            List<ViewTable> data = await dataWorker.ParseTables();
            SetTables(data);
            selectedScheme = Tables.First().Groups.First().Schemes.First();
            Save();
            InfoPanel = "Скачивание таблиц завершено";
        }


        /// <summary>
        /// Поиск обновлений на сайте
        /// </summary>
        /// <returns></returns>
        public async Task SelectDifferentAsync()
        {
            InfoPanel = "Поиск обновлений на сайте...";
            var data = await dataWorker.DownloadAndSelectDifferentAsync();
            if (data.Any(x => x.IsChanged))
            {
                System.Windows.MessageBox.Show("На сайте найдены изменения схем. Таблица обновится до новой версии. Изменённые схемы будут выделены цветом в списке до перезапуска программы");
                SetTables(data);
            }
            else
            {
                InfoPanel = "На сайте изменения не обнаружены";
            }
            
        }
        /// <summary>
        /// Сохранение схем в файле
        /// </summary>
        public void Save()
        {
            dataWorker.SaveTables();
        }


        /// <summary>
        /// Открыть схемы из файла
        /// </summary>
        public void Open()
        {

            List<ViewTable> res;
            if (dataWorker.TryOpenTables(out res))
                SetTables(res);
            else
                System.Windows.MessageBox.Show("Ошибка открытия файла. Рекомендуется обновить с сайта. При повторении ошибки удалите файл tables.xml");
            
        }
        //Связь изменений свойств с отображением их
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        // команда открытия главной папки
        private RelayCommand openMainFolderCommand;
        public RelayCommand OpenMainFolderCommand => openMainFolderCommand ??
                  (openMainFolderCommand = new RelayCommand(obj =>
                  {
                      if (Directory.Exists(downloadPath))
                          Process.Start(downloadPath);
                      else
                          System.Windows.MessageBox.Show("Папки с этим файлом не существует. Скачайте выбранную схему",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                  }));

        // команда открытия папки файлов
        private RelayCommand openFolderCommand;
        public RelayCommand OpenFolderCommand => openFolderCommand ??
                  (openFolderCommand = new RelayCommand(obj =>
                  {
                      //действие
                      string path = selectedScheme.Scheme.GetFolderPath(Path.Combine(downloadPath,selectedScheme.Parent.Parent.Name));
                      if (Directory.Exists(path))
                          Process.Start(path);
                      else
                          System.Windows.MessageBox.Show("Папки с этим файлом не существует. Скачайте выбранную схему",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                  }));

        // команда скачивания схемы
        private RelayCommand downloadFileCommand;
        public RelayCommand DownloadFileCommand => downloadFileCommand ??
                  (downloadFileCommand = new RelayCommand(obj =>
                  {
                      selectedScheme.DownloadFile(downloadPath);
                      OpenFolderCommand.Execute(null);
                  }));

        // команда скачивания приказа
        private RelayCommand downloadOrderCommand;
        public RelayCommand DownloadOrderCommand => downloadOrderCommand ??
                  (downloadOrderCommand = new RelayCommand(obj =>
                  {
                      selectedScheme.DownloadOrder(downloadPath);
                      OpenFolderCommand.Execute(null);
                  }));

        /// <summary>
        /// Главная папка проекта
        /// </summary>
        private static string downloadPath = FileDownloader.MAIN_FOLDER;
        internal async Task DownloadSelectedAllAsync()
        {
            /*
            if (downloadPath == null || !Directory.Exists(downloadPath))
                using (var dialog = new FolderBrowserDialog())
                {
                    
                    dialog.ShowNewFolderButton = true;
                    dialog.Description = "Выберите папку, в которую необходимо скачать схемы";
                    if (dialog.ShowDialog() == DialogResult.OK)
                        downloadPath = dialog.SelectedPath;
                }
            */
            InfoPanel = "Скачивание выделенных схем и приказов...";
            foreach (var item in Tables)
            {
                await item.DownloadSelectedAllAsync(downloadPath);
            }
            InfoPanel = "Скачивание схем и приказов завершено";
            System.Windows.MessageBox.Show("Скачивание схем и приказов завершено");
        }
        internal async Task DownloadSelectedFileAsync()
        {
            foreach (var item in Tables)
            {
                await item.DownloadSelectedFileAsync(downloadPath);
            }
            InfoPanel = "Скачивание схем завершено";
            System.Windows.MessageBox.Show("Скачивание схем завершено");
        }
    }
}
