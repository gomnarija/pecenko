using System.Collections;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using pečenko;
using UblSharp;
using UblSharp.CommonAggregateComponents;

class InvoiceFilesOrchestrator {

    public string? savePath;
    public InvoiceType loadInvoiceFromFile(String path){
        if(!File.Exists(path)){
            MessageBox.Show("pogrešna putanja\n" + path, "greška", MessageBoxButtons.OK);
            return new InvoiceType();
        }
        
        try{
            InvoiceType invoiceType = InvoiceXMLParser.parseInvoiceFromFile(path);
            return invoiceType;
        }catch(Exception ex){
            MessageBox.Show("neuspešno čitanje XML fakture:\n\n " + ex, "greška", MessageBoxButtons.OK);
            return new InvoiceType();
        }
    }

    public void loadInsertedFromFile(String path, ref List<InvoiceLineType> invoiceLines){
        if(!File.Exists(path)){
            MessageBox.Show("pogrešna putanja", "greška", MessageBoxButtons.OK);
            return;
        }
        
        try{
            InvoiceType invoiceType = InvoiceXMLParser.parseInvoiceFromFile(path);
            if((invoiceType?.InvoiceLine??new List<InvoiceLineType>()).Count > 0){
                invoiceLines = invoiceType?.InvoiceLine ?? new List<InvoiceLineType>();
            }else{
               MessageBox.Show("uneti artikli nisu pronadjeni.", "greška", MessageBoxButtons.OK);
            }
        }catch(Exception ex){
            MessageBox.Show("neuspešno čitanje XML fakture:\n\n " + ex, "greška", MessageBoxButtons.OK);
        }
    }

    public void saveInsertedToFile(String path, List<InvoiceLineType> insertedInvoiceLines){
        InvoiceType invoiceType = new InvoiceType();
        invoiceType.InvoiceLine = insertedInvoiceLines;
        XmlWriterSettings settings = new XmlWriterSettings
        {
            Indent = true,
            Encoding = Encoding.UTF8
        };

        String xml = InvoiceXMLParser.parseXmlFromInvoice(invoiceType);
        try{
            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write)){
                using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(InvoiceType));
                    serializer.Serialize(writer, invoiceType);
                }
            }
            savePath = path;
        }catch(Exception ex){
            MessageBox.Show("neuspešno čuvanje XML fakture:\n\n " + ex, "greška", MessageBoxButtons.OK);
        }
    }

}