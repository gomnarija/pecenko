namespace pečenko;

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Xml.Serialization;

class NetworkUtils{
    private static XmlSerializer requestSerializer = new XmlSerializer(typeof(RequestEnvelope));
    private static XmlSerializer responseSerializer = new XmlSerializer(typeof(ResponseEnvelope));


    public static RequestEnvelope deserializeRequest(byte[] buffer, int length){
        using (MemoryStream memoryStream = new MemoryStream(buffer, 0, length)){
            try{
                return ((RequestEnvelope?)requestSerializer.Deserialize(memoryStream))??RequestEnvelope.Fail();
            }catch{
                return RequestEnvelope.Fail();
            }
        }
    }

    public static byte[] serializeResponseToByteArray(ResponseEnvelope responseEnvelope){
        if (responseEnvelope == null){
            return [];
        }

        using (MemoryStream memoryStream = new MemoryStream()){
            responseSerializer.Serialize(memoryStream, responseEnvelope);
            return memoryStream.ToArray().Append((byte)NetworkingConstants.MESSAGE_END).ToArray();
        }
    }

    public static async Task serializeResponseToStreamAsync(ResponseEnvelope responseEnvelope, NetworkStream stream){
        if (responseEnvelope == null){
            return;
        }

        using (MemoryStream memoryStream = new MemoryStream()){
            responseSerializer.Serialize(memoryStream, responseEnvelope);
            byte[] responseBytes = memoryStream.ToArray().Append((byte)NetworkingConstants.MESSAGE_END).ToArray();
            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);   
        }
    }

    public static ResponseEnvelope deserializeResponse(byte[] buffer, int length){
        using (MemoryStream memoryStream = new MemoryStream(buffer, 0, length)){
            try{
                ResponseEnvelope? envelope =  ((ResponseEnvelope?)responseSerializer.Deserialize(memoryStream));

                return envelope??ResponseEnvelope.Fail();
            }catch{
                return ResponseEnvelope.Fail();
            }
        }
    }

    public static byte[] serializeRequestToByteArray(RequestEnvelope requestEnvelope){
        if (requestEnvelope == null){
            return [];
        }

        using (MemoryStream memoryStream = new MemoryStream())
        {
            try{
                requestSerializer.Serialize(memoryStream, requestEnvelope);
            }catch(Exception e){
                MessageBox.Show(e.ToString());
            }
            return memoryStream.ToArray().Append((byte)NetworkingConstants.MESSAGE_END).ToArray();
        }
    }


    public static string getLocalIpAddress(){
        string localIP;
        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0)){
            socket.Connect(getGatewayIp(), 65530);
            IPEndPoint? endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint?.Address?.ToString()??"127.0.0.1";
        }
        return localIP;
    }


    private static string getGatewayIp(){
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces()){
            if (ni.OperationalStatus == OperationalStatus.Up && 
                (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                 ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)){
                foreach (GatewayIPAddressInformation gateway in ni.GetIPProperties().GatewayAddresses){
                    if (gateway.Address.AddressFamily == AddressFamily.InterNetwork){
                        return gateway.Address.ToString();
                    }
                }
            }
        }
        return "127.0.0.1";
    }

}