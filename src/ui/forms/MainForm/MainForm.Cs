namespace pečenko;

public partial class MainForm : BaseForm{


    public MainForm(){
        initDesign();
    }



    public void ServerSelect(object sender, EventArgs e){
        if(imeTextBox.Text.Length == 0){
            MessageBox.Show("unesi korisničko ime");
            return;
        }
        String userName = imeTextBox.Text.Trim();
        Program.setUserName(userName);
        Program.serverModeStart();
        this.Close();
    }


    public void ClientSelect(object sender, EventArgs e){
        if(imeTextBox.Text.Length == 0){
            MessageBox.Show("unesi korisničko ime");
            return;
        }
        String userName = imeTextBox.Text.Trim();
        Program.setUserName(userName);
        Program.clientModeStart();
        this.Close();
    }
}