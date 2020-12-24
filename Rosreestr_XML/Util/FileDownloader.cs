using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Security.Cryptography;

namespace Rosreestr_XML.Util
{
    /// <summary>
    /// Скачивание файлов
    /// </summary>
    class FileDownloader
    {
        /// <summary>
        /// Главная папка схем
        /// </summary>
        public static string MAIN_FOLDER = "Данные";
        /// <summary>
        /// Скачать файл
        /// </summary>
        /// <param name="addr">ссылка на скачивание</param>
        /// <param name="folderPath">путь к файлу</param>
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
                string fileDownloadPath = null;
                if (File.Exists(folderPath + fileName))
                {
                    fileDownloadPath = Path.GetTempFileName();
                    webClient.DownloadFile(addr, fileDownloadPath);
                    FindHashDifference(folderPath + fileName, fileDownloadPath);
                   
                }
                else
                    webClient.DownloadFile(addr, folderPath + fileName);


            }
            //всякое бывает
            catch (Exception e)
            {
                MessageBox.Show(e.Message +"\n" + e.StackTrace, "Произошла ошибка при скачивании файла", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void FindHashDifference(string fileOld, string fileNew)
        {
            string hashOld = ComputeMD5Checksum(fileOld);
            string hashNew = ComputeMD5Checksum(fileNew);
            if (hashOld != hashNew)
                SaveFiles(fileNew, fileOld);
            else
                File.Delete(fileNew);
            
        }

        private static void SaveFiles(string fileNew, string fileOld)
        {
            // Файлы могли быть:
            // *_old.* -> fileDelete
            // *.* -> fileOld

            //самый старый файл
            string fileDelete = AddSuff(fileOld, "_old");
            // замена его на старый
            File.Copy(fileOld, fileDelete, true);
            // замена старого на новый
            File.Copy(fileNew, fileOld, true);
            // удаление нового файла, чтобы не бесил
            File.Delete(fileNew);
        }

        private static string AddSuff(string file, string suf)
        {
            int pointInd = file.LastIndexOf('.');
            if (pointInd == -1) pointInd = file.Length;
            return file.Insert(pointInd, suf);
        }

        private static string ComputeMD5Checksum(string path)
        {
            using (FileStream fs = System.IO.File.OpenRead(path))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int)fs.Length);
                byte[] checkSum = md5.ComputeHash(fileData);
                string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
                return result;
            }
        }
    }
}
