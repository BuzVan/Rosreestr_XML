using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Rosreestr_XML.Util
{
    class FileDownloader
    {
        public static string MAIN_FOLDER = "Данные";
        internal static void Download(Uri addr, string folderPath)
        {
            WebClient webClient = new WebClient();
            folderPath = folderPath.Trim('\\') + "\\";

            string fileName;

            if (addr.Query == "")
                fileName = Path.GetFileName(addr.LocalPath);
            else
            {
                fileName = addr.Query.Split('&').
                    Where(x => x.Contains("file")).
                    Select(x => x.Split('=')).
                    First(x => x.Length == 2 && x[1].Contains('.')).Last();
            }
            folderPath = new string(folderPath.Select(x => x == '\\' || !Path.GetInvalidPathChars().Contains(x) ? x : '_').ToArray());
            fileName = new string(fileName.Select(x => Path.GetInvalidFileNameChars().Contains(x) ? '_' : x).ToArray());
            try
            {
                System.IO.Directory.CreateDirectory(folderPath);
                webClient.DownloadFile(addr, folderPath + fileName);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message +"\n" + e.StackTrace, "Произошла ошибка при скачивании файла", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
