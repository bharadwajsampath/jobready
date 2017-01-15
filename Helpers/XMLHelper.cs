using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Worker.Helpers
{
    public static class XMLHelper
    {
        public static string ConvertToXML(List<ExportStudent> studentList)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(List<ExportStudent>));
            StringWriter sww = new StringWriter();
            var settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            using (XmlWriter writer = XmlWriter.Create(sww, settings))
            {
                xsSubmit.Serialize(writer, studentList);
                var xml = sww.ToString(); // Your XML
                return xml;
            }

        }
    }
}
