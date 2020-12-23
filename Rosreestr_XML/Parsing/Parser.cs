using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Rosreestr_XML.Data;
using Rosreestr_XML.Parsing.AngleSharp;
using System.Threading.Tasks;
using System.Windows;

namespace Rosreestr_XML.Parsing
{
    /// <summary>
    /// Скачивание и разбор страницы Росреестра
    /// </summary>
    public class Parser
    {
        const string Address = "https://rosreestr.gov.ru/site/fiz/zaregistrirovat-nedvizhimoe-imushchestvo-/xml-skhemy/";
        private HtmlLoader htmlLoader;
        private AngleSharpParser angleSharpParser;
        public Parser()
        {
             htmlLoader = new HtmlLoader(Address);
             angleSharpParser = new AngleSharpParser();
        }
        public TableXML[] Parse()
        {
            try
            {
                string data = htmlLoader.DownloadAsync().Result;
                HtmlParser htmlParser = new HtmlParser();
                IHtmlDocument document = htmlParser.ParseDocument(data);
                return angleSharpParser.Parse(document);
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message +" \n"+e.StackTrace, "Произошла ошибка при скачивании информации с сайта", MessageBoxButton.OK, MessageBoxImage.Error);
                return new TableXML[0];
            }
           

            
        }

        public async Task<TableXML[]> ParseAsync()
        {
            return await Task.Run(() => Parse());
        }
    }
}
