﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Rosreestr_XML.Parser
{
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