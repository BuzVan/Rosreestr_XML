using Rosreestr_XML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosreestr_XML.ModelView
{
    public class ViewScheme
    {
        public SchemeXML Scheme { get; }

        public ViewScheme(SchemeXML scheme)
        {
            Scheme = scheme;
        }
        public void DownloadScheme()
        {
             Scheme.DownloadScheme();
        }
        public void DownloadOrder()
        {
             Scheme.DownloadOrder();
        }

    }
}
