﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ItechoPdf;

namespace testconsole
{
    class Program
    {
        static byte[] Create(int i)
        {
            var renderer = new PdfRenderer(settings => {
                // Set global settings for all documents rendered through this service
                settings.DPI = 300;
                settings.Margins.Set(0, 0, 0, 0,Unit.Millimeters);
            });
            
            Console.WriteLine("WkHTML version:" + renderer.GetVersion());

            // var cover = renderer.AddDocument(PdfSource.FromFile("res/cover.html"));
            var content = renderer.AddDocument(PdfSource.FromFile("res/content.html"));
            // (@"PDF. <a href=\"http://www.google.com\">Go to google</a>"));
            
            content.SetHeader(PdfSource.FromFile("res/header.html"), 25, 5);
            content.SetFooter(PdfSource.FromFile("res/footer.html"), 50, 5);
            
            return renderer.RenderToBytes();
        }

        static void Main(string[] args)
        {
            // Console.WindowWidth;
            // No data is available for encoding 1252
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var bytes = Create(0);

            var replace = new List<VariableReplace> 
            { 
                new VariableReplace("page", "1"), 
                new VariableReplace("total", "2")
            };
            
            var editor = new PdfEditor();
            using (var ms = new MemoryStream(bytes))
            {
                var output = editor.ReplaceAnnotations(ms, replace, 25 + 5);
                File.WriteAllBytes("output.pdf", output.ToArray());
            }
      
            // Parallel.For(0, 2, i => {
            // });
        }
    }
}

