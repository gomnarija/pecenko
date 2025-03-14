using System.Net;

namespace pečenko;

public partial class ConnectForm : BaseForm{
    
    public ConnectForm(){
        initDesign();
    }

    public void TryConnect(object sender, EventArgs e){
        if(ipTextBox.Text.Length == 0){
            MessageBox.Show("unesi ip adresu");
            return;
        }
        String serverIp = ipTextBox.Text.Trim();
        if(!IPAddress.TryParse(serverIp, out _)){
            MessageBox.Show("nevažeća ip adresa");
            return;
        }
        Program.setServerIp(serverIp);
        this.Close();
    }

}