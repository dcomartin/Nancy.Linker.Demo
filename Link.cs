using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nancy.Linker.Demo
{
    public class Link
    {
        public string Href { get; }

        public Link(string href)
        {
            Href = href;
        }
    }
}
