using System;
using System.IO;
using System.Xml.Serialization;

namespace Service.Api.Common
{
    public static class XmlExtensions
    {
        public static T Deserialize<T>(string strXML)
            where T : class
        {
            try
            {
                using (StringReader sr = new StringReader(strXML))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(sr) as T;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
