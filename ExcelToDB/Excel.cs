using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace ExcelToDB
{
    internal class Excel
    {
        Workbook workbook;
        Worksheet sheet;
        _Application application = new Application();
        public Excel(string path)
        {
            workbook = application.Workbooks.Open(path);
            sheet = workbook.Worksheets[1];
        }

        public List<GameItemDto> ReadgameItems()
        {
            int row = 2;

            List<GameItemDto> items = new List<GameItemDto>();

            while (sheet.Cells[1, row].Value != null)
            {
                row++;
            }

            for (int i = 0; i < row - 2; i++)
            {
                items.Add(new GameItemDto());
            }

            int j = 0;
            foreach (string name in ReadColumns(1, row - 2))
            {
                items[j].Name = name;
                j++;
            }


            j = 0;
            foreach (string genre in ReadColumns(2, row - 2))
            {
                items[j].Genre = genre;
                j++;
            }


            j = 0;
            foreach (double releaseYear in ReadColumns(3, row - 2))
            {
                items[j].ReleaseYear = Convert.ToInt32(releaseYear);
                j++;
            }



            j = 0;
            foreach (string publisher in ReadColumns(4, row - 2))
            {
                List<string> publishers = new List<string>(); 
                publishers.AddRange(publisher.Split(','));
                items[j].Publishers = new List<string>();
                items[j].Publishers.AddRange(publishers);
                j++;

            }




            return items;
        }

        public ArrayList ReadColumns(int column, int rowAmount)
        {
            ArrayList columns = new ArrayList();

            for (int i = 0; i < rowAmount; i++)
            {
                columns.Add(sheet.Cells[i + 2, column].Value2);
            }

            return columns;

        }

        public void Quit()
        {
            workbook.Close();
            application.Quit();
        }
    }
}
