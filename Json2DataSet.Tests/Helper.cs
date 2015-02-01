using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Json2DataSet.Tests
{
    static class Helper
    {
        public static string ReadJsonFromFile(string fileName)
        {
            return File.ReadAllText(Path.Combine("JsonFiles", fileName));
        }
    }
}
