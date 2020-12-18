using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rosreestr_XML.Parser.AngleSharp;

namespace UnitTest
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void ParseTitles()
        {
            Parser parser = new Parser();
            var res = parser.ParseAsync();
            Assert.ThrowsExceptionAsync<System.Exception>(parser.ParseAsync);
        }
    }
}
