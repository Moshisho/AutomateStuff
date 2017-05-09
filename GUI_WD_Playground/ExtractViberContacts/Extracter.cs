using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ExtractViberContacts
{
    class Extracter
    {
        static void Main(string[] args)
        {
            string contactsFilePath = @"C:\Users\moshea\Downloads\imaContacts.json";

            string fileAsString = File.ReadAllText(contactsFilePath);



            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //settings.CheckAdditionalContent = true;


            //DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(fileAsString);
            //DataTable dataTable = dataSet.Tables["AddressBook"];

            //foreach (DataRow dr in dataTable.Rows) 
            //{
 
            //}

            //ViberContact vc = JsonConvert.DeserializeObject<ViberContact>(fileAsString, settings);

            
        }
    }
}
