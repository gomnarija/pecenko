namespace pečenko;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Serialization;
using UblSharp.CommonAggregateComponents;

class Mandrak{
    private TcpListener tcpListener;
    private Int32 port;
    private IPAddress ipAddress;
    private bool isRunning;
    public InvoiceOrchestratorImpl invoiceOrchestrator = new InvoiceOrchestratorImpl();


    //default constructor
    public Mandrak(){
        port = 2626;
        ipAddress = IPAddress.Parse(NetworkUtils.getLocalIpAddress());
        tcpListener = new TcpListener(ipAddress, port);
    }


    public void Start(){
        tcpListener.Start();
        isRunning = true;
        _ = AcceptClientAsync();
    }

    private async Task AcceptClientAsync(){
        while(isRunning){
            TcpClient client = await tcpListener.AcceptTcpClientAsync();
            _ = Task.Run(() => HandleClientAsync(client));
        }
    }

    private async Task HandleClientAsync(TcpClient client){
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[NetworkingConstants.MAX_MESSAGE_SIZE];
        try
        {
            int bytesRead = 0 ;
            while((bytesRead += await stream.ReadAsync(buffer, bytesRead, buffer.Length - bytesRead)) > 0){
                if(buffer[bytesRead-1] == NetworkingConstants.MESSAGE_END){
                    RequestEnvelope request = NetworkUtils.deserializeRequest(buffer[0..(bytesRead-1)], bytesRead-1);
                    ResponseEnvelope response = parseRequest(request);
                    await NetworkUtils.serializeResponseToStreamAsync(response, stream);
                    buffer[bytesRead-1] = (byte)'\0';
                    bytesRead = 0;
                }
            }
        }
        catch (Exception ex){
           MessageBox.Show($"Client connection error: {ex.Message}");
        }
        finally{
            MessageBox.Show("Client disconnected.");
        }
    }


    private ResponseEnvelope parseRequest(RequestEnvelope request){
        if(request?.action == null){
            return ResponseEnvelope.Fail();
        }
        switch (request.action){
            case NetworkingConstants.ACTION_GET_INVOICE:
                return getInvoice();
            case NetworkingConstants.ACTION_GET_INSERTED:
                return getInserted();
            case NetworkingConstants.ACTION_INSERT:
                return insert(request);
            case NetworkingConstants.ACTION_EDIT:
                return edit(request);
            case NetworkingConstants.ACTION_REMOVE:
                return remove(request);
            case NetworkingConstants.ACTION_GET_QUANTITIES:
                return getQuantities(request);

            default:
                return new ResponseEnvelope();
        }
    }

    private ResponseEnvelope getInvoice(){
        ResponseEnvelope responseEnvelope = new ResponseEnvelope();
        responseEnvelope.invoice =  invoiceOrchestrator.getInvoice();
        return responseEnvelope;
    }


    private ResponseEnvelope getInserted(){
        ResponseEnvelope responseEnvelope = new ResponseEnvelope();
        responseEnvelope.inserted =  invoiceOrchestrator.getInserted();
        return responseEnvelope;
    }

    private ResponseEnvelope insert(RequestEnvelope request){
        List<InvoiceLineType> inserted = request?.inserted??new List<InvoiceLineType>();
        if(inserted.Count == 0){
            return ResponseEnvelope.Fail();
        }
        this.invoiceOrchestrator.insertInvoiceLine(inserted);
        return ResponseEnvelope.OK();
    }

    private ResponseEnvelope edit(RequestEnvelope request){
        List<InvoiceLineType> inserted = request?.inserted??new List<InvoiceLineType>();
        if(inserted.Count == 0){
            return ResponseEnvelope.Fail();
        }
        this.invoiceOrchestrator.editInsertedInvoiceLine(inserted);
        return ResponseEnvelope.OK();
    }

    private ResponseEnvelope remove(RequestEnvelope request){
        List<string> ids = request?.ids??new List<string>();
        if(ids.Count == 0){
            return ResponseEnvelope.Fail();
        }
        this.invoiceOrchestrator.removeInsertedInvoiceLine(ids);
        return ResponseEnvelope.OK();
    }

        private ResponseEnvelope getQuantities(RequestEnvelope request){
        List<string> ids = request?.ids??new List<string>();
        if(ids.Count == 0){
            return ResponseEnvelope.Fail();
        }
        ResponseEnvelope response = ResponseEnvelope.OK();
        foreach(string id in request?.ids??new List<string>()){
            QuantitiesTypeSerializable quantitiesType = new QuantitiesTypeSerializable(
                id,
                invoiceOrchestrator.getOriginalQuantityForCode(id),
                invoiceOrchestrator.getInsertedQuantityForCode(id)
            );
            if(response.quantities == null){
                response.quantities = new List<QuantitiesTypeSerializable>(){quantitiesType};
            }else{
                response.quantities.Add(quantitiesType);
            }
        }
        return response;
    }
}