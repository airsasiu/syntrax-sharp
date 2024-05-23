using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SyntraxCore.Util
{
    public class SVGTranscoder
    {
        private static int _initStreamSize = 1024;

        public static Stream SvgToPng(string svgString)
        {
            using (Stream stream = new MemoryStream())
            {
                SvgDocument svgDocument = SvgDocument.FromSvg<SvgDocument>(svgString);
                var bitmap = svgDocument.Draw();
                bitmap.Save(stream, ImageFormat.Png);
                return stream;
            }
        }
    }
}
