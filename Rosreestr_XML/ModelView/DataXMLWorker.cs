using Rosreestr_XML.ModelView;
using Rosreestr_XML.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rosreestr_XML.Data
{
    class DataXMLWorker
    {
        private static string filename = "tables.xml";
        private TableXML[] data;
        public DataXMLWorker()
        {
            data = new TableXML[0];
        }

        public async Task<List<ViewTable>> ParseTables()
        {
            Parsing.Parser taskParser = new Parsing.Parser();
            data = await taskParser.ParseAsync();
            return new List<ViewTable>(data.Select(x=> new ViewTable(x)));
        }
        public void SaveTables()
        {
            TableSerialization.Serialize(filename, data);
        }

        public List<ViewTable> OpenTables()
        {
            data = TableSerialization.Deserialize(filename);
            return new List<ViewTable>(data.Select(x => new ViewTable(x)));
        }
        public bool TryOpenTables(out List<ViewTable> result)
        {
            TableXML[] trydata = null;
            result = default;
            bool res = TableSerialization.TryDeserialize(filename, out trydata);
            if (res)
            {
                data = trydata;
                result = new List<ViewTable>(data.Select(x => new ViewTable(x)));
            }
            return res;
        }
    }
}
