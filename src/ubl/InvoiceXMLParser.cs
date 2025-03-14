using System.Xml;
using System.Xml.Serialization;
using System.Text;
using UblSharp;

namespace pečenko;



static class InvoiceXMLParser {    
    public static InvoiceType parseInvoiceFromFile(String xmlFilePath) {
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.Async = true;
        //open file
        FileStream xmlFileStream = new FileStream(xmlFilePath, FileMode.Open, FileAccess.Read);
        // parse xml
        using (var streamReader = new StreamReader(xmlFileStream, Encoding.UTF8)){
            using (XmlReader reader = XmlReader.Create(streamReader, settings)){
                while (reader.Read()){
                    if(reader.NodeType == XmlNodeType.Element && reader.Name == "Invoice"){
                        try{
                            XmlSerializer serializer = new XmlSerializer(typeof(InvoiceType));
                            InvoiceType? invoice = (InvoiceType?)serializer.Deserialize(reader);
                            invoice?.AdditionalDocumentReference?.Clear();
                            return invoice ?? new InvoiceType();
                        }catch(Exception e){
                                throw new Exception("failed", e);
                        }
                    }
                }
            }    
        }

        return new InvoiceType();
    }

    public static String parseXmlFromInvoice(InvoiceType invoice) {
        XmlWriterSettings settings = new XmlWriterSettings
        {
            Indent = true,
            Encoding = Encoding.UTF8
        };

        using (StringWriter stringWriter = new StringWriter())
        {
           using (XmlWriter writer = XmlWriter.Create(stringWriter, settings))
          {
                XmlSerializer serializer = new XmlSerializer(typeof(InvoiceType));
                serializer.Serialize(writer, invoice);
            }

            return stringWriter.ToString();
        }
    }


}