using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Rosreestr_XML.Data;
using Rosreestr_XML.Parsing.AngleSharp;
using System.Threading.Tasks;

namespace Rosreestr_XML.Parsing
{
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
            string data = htmlLoader.DownloadAsync().Result;
            HtmlParser htmlParser = new HtmlParser();
            IHtmlDocument document = htmlParser.ParseDocument(data);

            return angleSharpParser.Parse(document);
        }

        public async Task<TableXML[]> ParseAsync()
        {
            return await Task.Run(() => Parse());
        }
    }
}
