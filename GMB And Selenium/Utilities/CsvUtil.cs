using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMB_And_Selenium.Models;

namespace GMB_And_Selenium.Utilities
{
    class CsvUtil
    {
        public async Task<IEnumerable<ProjectData>> ReadAllAsync(string filename)
        {
            return await Task.Run(() =>
            {
               var trueStart = 0;
               using (var text = File.OpenText(filename))
               {

                   while (!text.ReadLine().StartsWith("Country"))
                   {
                       trueStart++;
                   }
               }

               using (var text = File.OpenText(filename))
               {
                   while (trueStart-- > 0)
                   {
                       text.ReadLine();
                   }

                   using (var csvHelper = new CsvHelper.CsvReader(text))
                   {
                       csvHelper.Configuration.HeaderValidated = null;
                       csvHelper.Configuration.MissingFieldFound = null;

                       return csvHelper.GetRecords<ProjectData>().ToArray();
                   }
               }
           });
        }
    }
}
