namespace pečenko;

using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using UblSharp;

class Fantom{
    private TcpClient tcpClient;
    private Int32 port = ProgramConstans.PORT;
    IPAddress ipAddress;
    public readonly bool isConnected = false;
    public invoiceOrchestratorClient invoiceOrchestratorClient;
    //default constructor
    public Fantom() : this(ProgramConstans.IP){}
    public Fantom(string serverIp){
        invoiceOrchestratorClient = new invoiceOrchestratorClient(this);
        ipAddress = IPAddress.Parse(serverIp);
        try{
            tcpClient = new TcpClient(ipAddress.ToString(), port);
            tcpClient.SendTimeout = NetworkingConstants.TIMEOUT;
            tcpClient.ReceiveTimeout = NetworkingConstants.TIMEOUT;
            tcpClient.SendBufferSize = NetworkingConstants.MAX_MESSAGE_SIZE;
            tcpClient.ReceiveBufferSize = NetworkingConstants.MAX_MESSAGE_SIZE;
            isConnected = true;
        }catch(Exception e){
            MessageBox.Show(
                "povezivanje na server neuspešno :(\n\n\n"+
                e.ToString());
            tcpClient = new TcpClient();
        }
    }


    public async Task<ResponseEnvelope> sendRequestAsync(RequestEnvelope request){
        if(!isConnected)
            return ResponseEnvelope.Fail();

        NetworkStream stream = tcpClient.GetStream();
        stream.ReadTimeout = NetworkingConstants.TIMEOUT;
        var messageBytes = NetworkUtils.serializeRequestToByteArray(request);
        try{
            await stream.WriteAsync(messageBytes, 0, messageBytes.Length);

            using (MemoryStream responseStream = new MemoryStream()){
                byte[] buffer = new byte[1024*1024];
                int bytesRead=1;
            
                do{
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    await responseStream.WriteAsync(buffer, 0, bytesRead);
                } while (bytesRead > 0 && buffer[bytesRead - 1] != NetworkingConstants.MESSAGE_END);
                return NetworkUtils.deserializeResponse(responseStream.ToArray()[0..((int)responseStream.Length-1)], (int)responseStream.Length-1);
            }
        }catch(Exception e){
            MessageBox.Show(e.ToString());
            return ResponseEnvelope.Fail();
        }
    }

    public void disconnect(){
        if(isConnected){
            tcpClient.Client?.Close();
            tcpClient.Close();
        }
    }
}