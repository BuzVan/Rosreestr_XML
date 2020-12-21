using Rosreestr_XML.Data;
using Rosreestr_XML.ModelView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Rosreestr_XML
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataXMLWorker dataWorker;
        readonly ObservableCollection<ViewTable> Tables;
        private void SetTables(List<ViewTable> data)
        {
            Tables.Clear();
            foreach (var item in data)
            {
                Tables.Add(item);
            }
        }
        public MainWindow()
        {
            dataWorker = new DataXMLWorker();
            Tables = new ObservableCollection<ViewTable>();

            InitializeComponent();
            treeView.ItemsSource = Tables;
            List<ViewTable> res;
            if (dataWorker.TryOpenTables(out res))
                SetTables(res);
            else
                UploadButton_Click(null, null);


        }

        private async void UploadButton_Click(object sender, RoutedEventArgs e)
        {
           
            UploadButton.IsEnabled = false;
            List<ViewTable> data = await dataWorker.ParseTables();
            SetTables(data);
            UploadButton.IsEnabled = true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            dataWorker.SaveTables();
            MessageBox.Show("Успешно сохранено");
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
           SetTables(dataWorker.OpenTables());
        }
    }
}
