using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

namespace Jupiter.Utility
{
    public static class XmlUtility
    {
        private static Dictionary<Type, XmlSerializer> serializers;
        private static Dictionary<string, XmlSchema> schemas;


        private static XmlSerializerNamespaces ns;
        private static XmlWriterSettings writerSettings;
        private static XmlReaderSettings readerSettings;

        static XmlUtility()
        {
            serializers = new Dictionary<Type, XmlSerializer>();
            schemas = new Dictionary<string, XmlSchema>();

            ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            writerSettings = new XmlWriterSettings(); ;
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.CloseOutput = true;
            writerSettings.Indent = true;

            readerSettings = new XmlReaderSettings();
            readerSettings.CloseInput = true;

            SerializerLock = new object();
            SchemaLock = new object();
        }

        private static object SerializerLock;
        private static object SchemaLock;
        public static XmlSerializer GetSerializerForType(Type T)
        {
            lock (SerializerLock)
            {
                if (serializers.ContainsKey(T))
                {
                    return serializers[T];
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(T);
                    serializers.Add(T, serializer);
                    return serializer;
                }
            }
        }
        public static XmlSchema GetSchemaFromFile(string SchemaPath)
        {
            lock (SchemaLock)
            {
                if (schemas.ContainsKey(SchemaPath))
                {
                    return schemas[SchemaPath];
                }
                else
                {


                    XmlSchema schema = new XmlSchema();
                    FileStream fs = new FileStream(SchemaPath, FileMode.Open);
                    schema = XmlSchema.Read(fs, null);
                    //schema.Compile(null);

                    XmlSchemaSet s = new XmlSchemaSet();
                    s.Add(schema);
                    s.Compile();

                    fs.Close();
                    schemas.Add(SchemaPath, schema);
                    return schema;
                }
            }
        }
        private static string RemoveInlineSchema(string xml)
        {
            int start = xml.IndexOf("<xs:schema");
            if (start == -1) return xml;
            int end = xml.IndexOf("</xs:schema>"); //12 char
            return xml.Remove(start, end + 12 - start);
        }
        public static T DeserializeObject<T>(string xml)
        {
            xml = RemoveInlineSchema(xml);
            StringReader sr = new StringReader(xml);
            T obj;

            using (XmlReader reader = XmlReader.Create(sr, readerSettings))
            {
                XmlSerializer serializer = GetSerializerForType(typeof(T));
                obj = (T)serializer.Deserialize(reader);
            }
            return obj;
        }

        public static T DeserializeFromFile<T>(string filePath)
        {
            T obj;
            using (StreamReader reader = new StreamReader(string.Format("{0}\\{1}", Application.StartupPath, filePath)))
            {
                var xml = reader.ReadToEnd();
                obj = XmlUtility.DeserializeObject<T>(xml);
            }
            return obj;
        }

        public static string SerializeObject<T>(T obj)
        {
            StringWriter sw = new StringWriter();
            string output;
            using (XmlWriter writer = XmlWriter.Create(sw, writerSettings))
            {
                XmlSerializer serializer = GetSerializerForType(typeof(T));
                serializer.Serialize(writer, obj, ns);
                output = sw.ToString();
                //output = CleanEmptyTags(output);
            }
            return output;
        }
        public static bool IsValidSchema(this XmlDocument XDoc, string SchemaPath)
        {
            int tryCount = 0;
            int errorCount = -1;
            XmlSchema schema = GetSchemaFromFile(SchemaPath);
            XDoc.Schemas.Add(schema);
            while (tryCount < 10 && errorCount != 0)
            {
                errorCount = 0;
                XDoc.Validate(delegate (object sender, ValidationEventArgs e)
                {
                    Console.WriteLine(e.Message);
                    errorCount++;
                });
                tryCount++;
            }
            return (errorCount == 0);
        }
        public static string GetNodeValue(this XmlDocument XDoc, string XPath)
        {
            XmlNode node;
            node = XDoc.SelectSingleNode(XPath);
            if (node != null)
                return node.InnerText.Trim();
            else
                return string.Empty;
        }
        /// <summary>
        /// //////////Deletes empty Xml tags from the passed xml
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string CleanEmptyTags(String xml)
        {
            Regex regex = new Regex(@"(\s)*<(\w)*(\s)*/>");
            return regex.Replace(xml, string.Empty);
        }

    }
}
