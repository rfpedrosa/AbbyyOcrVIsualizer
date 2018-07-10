using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace AbbyyResultVisualizer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //var text = System.IO.File.ReadAllText(@"input.xml");
                //var ocrParser = new AbbyyOcrParser().Parse(text);

                //Console.WriteLine($"Nr of pages are {ocrParser.Pages.Count}");

                var imgOutput = DrawOcrResults("input.xml", "input.jpg");

                imgOutput.Save("output.jpg");
            }
            finally
            {
                Console.WriteLine("Press enter to close...");
                Console.ReadLine();
            }
        }

        private static Image DrawOcrResults(string inputXmlPath, string inputJpgPath)
        {
            XDocument xmlDoc = null;

            using (var oReader = new StreamReader(inputXmlPath, Encoding.GetEncoding("ISO-8859-1")))
            {
                xmlDoc = XDocument.Load(oReader);
            }

            var pages = xmlDoc.Root.Elements();

            var imgInput = Image.FromFile(inputJpgPath);

            // Create pen.
            var regionPen = new Pen(Color.Red, 2);

            using (Graphics g = Graphics.FromImage(imgInput))
            {
                foreach (var xPage in pages)
                {
                    IEnumerable<XElement> blocks = xPage.Elements();

                    foreach (var block in blocks)
                    {
                        g.DrawRectangle(
                            regionPen, 
                            (float)block.Attribute("l"),
                            (float)block.Attribute("t"),
                            (float)block.Attribute("r") - (float)block.Attribute("l"),
                            (float)block.Attribute("b") - (float)block.Attribute("t")
                            );
                    }
                }
                
            }

            return imgInput;
        }
    }
}
