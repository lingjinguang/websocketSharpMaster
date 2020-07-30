using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;


namespace PrintControl.Utils
{ 
    public class UnicodeFontFactory : FontFactoryImp
    {
        private static readonly string arialFontPath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "Fonts"),"arialuni.ttf");
        private static readonly string simkaiPath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "Fonts"), "simkai.ttf");//楷体


        public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color,
            bool cached)
        {
            //第一个参数导入字体路径
            BaseFont baseFont = BaseFont.CreateFont(simkaiPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            return new Font(baseFont, size, style, color);
        }
    }

}