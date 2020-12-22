using Rosreestr_XML.Data;
using Rosreestr_XML.ModelView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Rosreestr_XML
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();

            viewModel = new ApplicationViewModel();
            DataContext = viewModel;

        }

        private async void UploadButton_Click(object sender, RoutedEventArgs e)
        {
           
            UploadButton.IsEnabled = false;
            await viewModel.Download();
            UploadButton.IsEnabled = true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Save();
            MessageBox.Show("Успешно сохранено");
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Open();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            viewModel.SelectScheme(e.NewValue);
        }
    }
}
