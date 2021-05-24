using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeClient
{
    public class LocalAppSettings
    {
        public string Header { get; set; } = "";
        public double Height { get; set; } = 0;
        public double Width { get; set; } = 0;
        public int Top { get; set; } = 0;
        public int Left { get; set; } = 0;
        public int Right { get; set; } = 0;
    }
}
