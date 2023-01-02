using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadinProjectNotes
{
    public class IconCache
    {
        List<Icon> icons;

        public IconCache(List<Icon> icons)
        {
            this.icons = icons;
        }

        public void SaveIconCache(string filePath)
        {
            //create the memory stream that saves icons sequentially
            byte[] header = Encoding.ASCII.GetBytes(@"IC");
            byte[] iconCount = BitConverter.GetBytes(icons.Count);

            using (FileStream fs=new FileStream(filePath,FileMode.Create, FileAccess.Write))
            using(MemoryStream ms = new MemoryStream())
            {
                //write header
                fs.Write(header, 0, header.Length);
                //write icon count to be cached
                fs.Write(iconCount, 0, iconCount.Length);   //int, 4 bytes
                //loop icon contents
                WriteIcons(fs, ms);
            }
        }

        private void WriteIcons(FileStream fs, MemoryStream ms)
        {
            foreach (var icon in icons)
            {
                //save icon to memory stream to get length of bytes
                icon.Save(ms);
                //write int with number of bytes of next icon
                byte[] iconLength = BitConverter.GetBytes(ms.Length);   //long, 8 bytes
                fs.Write(iconLength, 0, iconLength.Length);
                //write icon data to file stream
                icon.Save(fs);

                //reset memory stream
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                ms.SetLength(0);
            }
        }

        public List<Icon> LoadIconCache()
        {
            List<Icon> icons = new List<Icon>();



            return icons;
        }
    }
}
