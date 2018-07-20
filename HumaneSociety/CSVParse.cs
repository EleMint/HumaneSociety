using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace HumaneSociety
{
    public static class CSVParse
    {
        public static void ParseCSV(string filePath)
        {
            using (TextFieldParser parser = new TextFieldParser(@filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    int counter = 0;
                    for (int i = 0; i < fields.Length / 13; i++)
                    {
                        Animal animal = new Animal();
                        animal.Name = fields[1 + counter];
                        animal.Weight = int.Parse(fields[3 + counter]);
                        animal.Age = int.Parse(fields[4 + counter]);
                        animal.Demeanor = fields[7 + counter];
                        animal.KidFriendly = fields[8 + counter] == "1" ? true : false;
                        animal.PetFriendly = fields[9 + counter] == "1" ? true : false;
                        animal.AdoptionStatus = fields[11 + counter];
                        counter += 13;
                        Query.AddAnimal(animal);
                    }
                }
            }
        }
    }
}
