using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Amazoom
{
    public class WarehouseItem
    {
        private int ID = 0;
        private string name = "";
        private double weight = 0.0;
        private double Volume = 0.0;
        private int count = 0;

        public WarehouseItem(int iD, string name, double weight, double volume, int count)
        {
            ID = iD;
            this.name = name;
            this.weight = weight;
            Volume = volume;
            this.count = count;
        }

        public int ID1 { get => ID; set => ID = value; }
        public string Name { get => name; set => name = value; }
        public double Weight { get => weight; set => weight = value; }
        public double Volume1 { get => Volume; set => Volume = value; }
        public int Count { get => count; set => count = value; }
    }
    public partial class Inventory : Page
    {
        List<WarehouseItem> itemsList = new List<WarehouseItem>();
        protected void Page_Load(object sender, EventArgs e)
        {
            WarehouseItem item1 = new WarehouseItem(01,"Cheese",0.003,0.005,5);
            itemsList.Add(item1);
            item1 = new WarehouseItem(02, "Paper", 3.4, 0.67, 56);
            itemsList.Add(item1);
            item1 = new WarehouseItem(03, "Beans", 0.5, 0.4, 23);
            itemsList.Add(item1);
            item1 = new WarehouseItem(04, "Chicken Breast", 3.4, 3.2, 3);
            itemsList.Add(item1);
            item1 = new WarehouseItem(05, "Meat", 0.5, 0.4, 4);
            itemsList.Add(item1);

            FillInventoryTable();

        }

        protected void BtnAddInvFromUserInput(object sender, EventArgs e)
        {
            int t = 10;
            StringBuilder sb = new StringBuilder();
            for (int i = 0;i<t;i++)
            {
                sb.AppendFormat("Loop {0}: Hello <br />", i);
            }

            Page.Controls.Add(new LiteralControl(sb.ToString()));
        }

        protected void FillInventoryTable()
        {
            //int numberOfCells = 5; //ID, Name, Volume, Weight, Count
            //Page.Controls.Add(new LiteralControl("Printing table " + "Items list count is : " + itemsList.Count));
            for (int i = 0; i < itemsList.Count; i++)
            {
                WarehouseItem currentItem = itemsList[i];
                TableRow row = new TableRow();

                TableCell cell1 = new TableCell();
                cell1.Controls.Add(new LiteralControl(currentItem.ID1.ToString()));
                row.Cells.Add(cell1);

                TableCell cell2 = new TableCell();
                cell2.Controls.Add(new LiteralControl(currentItem.Name.ToString()));
                row.Cells.Add(cell2);
                TableCell cell3 = new TableCell();
                cell3.Controls.Add(new LiteralControl(currentItem.Volume1.ToString()));
                row.Cells.Add(cell3);
                TableCell cell4 = new TableCell();
                cell4.Controls.Add(new LiteralControl(currentItem.Weight.ToString()));
                row.Cells.Add(cell4);
                TableCell cell5 = new TableCell();
                cell5.Controls.Add(new LiteralControl(currentItem.Count.ToString()));
                row.Cells.Add(cell5);

                InventoryTable.Rows.Add(row);
            }
            
        }

        protected void BtnSaveCSVFile(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            FileUploadMessage.Text = "";
            if (FileUploadcsvinventory.HasFile)
            {
                try
                {
                    Page.Controls.Add(new LiteralControl("Saving File"));

                    sb.AppendFormat(" Uploading file: {0}", FileUploadcsvinventory.FileName);

                    //saving the file
                    FileUploadcsvinventory.SaveAs(".\\App_Data" + FileUploadcsvinventory.FileName);

                    //Showing the file information
                    sb.AppendFormat("<br/> Save As: {0}", FileUploadcsvinventory.PostedFile.FileName);
                    sb.AppendFormat("<br/> File type: {0}", FileUploadcsvinventory.PostedFile.ContentType);
                    sb.AppendFormat("<br/> File length: {0}", FileUploadcsvinventory.PostedFile.ContentLength);
                    sb.AppendFormat("<br/> File name: {0}", FileUploadcsvinventory.PostedFile.FileName);
                    sb.AppendFormat("Successfully Uploaded File!");
                    Page.Controls.Add(new LiteralControl(sb.ToString()));
                    FileUploadMessage.Text = "Successfully Uploaded File!";

                }
                catch (Exception ex)
                {
                    sb.Append("<br/> Error <br/>");
                    sb.AppendFormat("Unable to save file <br/> {0}", ex.Message);
                    
                }
            }
            else
            {
                sb.AppendFormat("Please input upload file!");
                FileUploadMessage.Text = sb.ToString();
            }
        }


    }
}