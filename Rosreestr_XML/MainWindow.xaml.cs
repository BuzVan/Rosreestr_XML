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
            await viewModel.SelectDifferentAsync();
            UploadButton.IsEnabled = true;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            viewModel.SelectScheme(e.NewValue);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            viewModel.Save();
        }

        private async void DownloadSelectedAllButton_Click(object sender, RoutedEventArgs e)
        {
            DownloadSelectedAllButton.IsEnabled = false;
            DownloadSelectedFileButton.IsEnabled = false;
            await viewModel.DownloadSelectedAllAsync();
            DownloadSelectedAllButton.IsEnabled = true;
            DownloadSelectedFileButton.IsEnabled = true;
        }

        private async void DownloadSelectedFileButton_Click(object sender, RoutedEventArgs e)
        {
            DownloadSelectedAllButton.IsEnabled = false;
            DownloadSelectedFileButton.IsEnabled = false;
            await viewModel.DownloadSelectedFileAsync();
            DownloadSelectedFileButton.IsEnabled = true;
            DownloadSelectedAllButton.IsEnabled = true;
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
