using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Data.SqlClient;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
   
        public Form1()
        {
            InitializeComponent();
        }
       

        private void btnsekilexport_Click(object sender, EventArgs e)
        {
            Class2 klas = new Class2();
            var exportPath = @"D:\namizedwekil";
            if (!Directory.Exists(exportPath))
            {
                Directory.CreateDirectory(exportPath);
            }
            DataTable imagesdatatable = klas.getdatatable(@"SELECT m.[dsk_kodu]
      ,[soyadi]
      ,[adi]
      ,[ata_adi]
      ,[FOTO] 
FROM [MM2020].[dbo].[Namized_MM] as m 
inner join MM2020.dbo.NamizedSenedMM as n on n.namID= m.Table_id 
where [FOTO] is not null 
and cix_tarix is null and qeyd_alinm is not null order by m.dsk_kodu,soyadi
      ,adi
      ,ata_adi");

            foreach (DataRow row in imagesdatatable.Rows)
            {
                if (row.ItemArray.Length > 0)
                {
                    if (row[0] != null)
                    {
                        byte[] image1 = (byte[])row["FOTO"];
                        Image newImage1 = convertbinarytoimage(image1);
                        var fileName1 = string.Format(row["dsk_kodu"].ToString() + "."
                                       + row["adi"].ToString() + row["soyadi"].ToString() +
                                       row["ata_adi"].ToString() + ".jpg");
                        using (var image = newImage1)
                        using (var newImage = ScaleImage(image, 180, 241))
                        {
                            newImage.Save(@"d:\namizedwekil\" + fileName1, ImageFormat.Jpeg);
                        }
                    }
                }
            }
            label1.Text = "hazirdi";
        }
        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var newImage = new Bitmap(maxWidth, maxHeight);
            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, maxWidth, maxHeight);
            return newImage;
        }
        Image convertbinarytoimage(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return Image.FromStream(ms);
            }
        }
    }
}
