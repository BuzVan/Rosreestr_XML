using Rosreestr_XML.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rosreestr_XML.Serialization
{
    /// <summary>
    /// Сериализация Схем
    /// </summary>
    public static class TableSerialization
    {
        /// <summary>
        /// Сериализовать массив таблиц XML
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        public static void Serialize(string filename, TableXML[] data)
        {
            File.Delete(filename);
            XmlSerializer formatter = new XmlSerializer(typeof(TableXML[]));
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, data);
            }
        }
        /// <summary>
        /// Десериализовать массив таблиц XML
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static TableXML[] Deserialize(string filename)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(TableXML[]));
            TableXML[] data;
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                data = (TableXML[])formatter.Deserialize(fs);
            }
            return data;
        }
        /// <summary>
        /// Попытаться десериализовать массив таблиц
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryDeserialize(string filename, out TableXML[] result)
        {
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(TableXML[]));
                using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    result = (TableXML[])formatter.Deserialize(fs);
                }
                return true;
            }
            catch (Exception)
            {
                result = null;
            }
            return false;

        }
    }
}
