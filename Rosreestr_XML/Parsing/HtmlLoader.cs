using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Rosreestr_XML.Parsing
{
    /// <summary>
    /// Класс скачивания HTML страницы
    /// </summary>
    class HtmlLoader
    {
        HttpClient client;
        string url;
        public HtmlLoader(string url)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Parser", "Hi");

            this.url = url;

        }
        /// <summary>
        /// Скачать страницу html
        /// </summary>
        /// <returns>содержание страницы</returns>
        public async Task<string> DownloadAsync()
        {
            HttpResponseMessage response = await client.GetAsync(url);
            string source = null;

            if (response != null &&
                    response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Помещаем код страницы в переменную.
                source = System.Text.Encoding.UTF8.GetString(await response.Content.ReadAsByteArrayAsync());
            }
            return source;
        } 
    }
}
