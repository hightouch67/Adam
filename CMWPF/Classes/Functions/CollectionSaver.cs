using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMWPF.Classes.Functions
{
    public class CollectionSaver
    {
        public void SaveCollectionToFile(object observablecollection, string path)
        {
            var jsonToOutput = JsonConvert.SerializeObject(observablecollection, Formatting.Indented);

            using (StreamWriter writetext = new StreamWriter(path))
            {
                writetext.WriteLine(jsonToOutput);
            }
            Debug.WriteLine(jsonToOutput);
        }
    }
}
