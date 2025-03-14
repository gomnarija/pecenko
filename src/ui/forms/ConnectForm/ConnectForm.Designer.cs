namespace pečenko;

partial class ConnectForm
{
    private Label ipLabel;
    private TextBox ipTextBox;
    private Button connectButton;



    private void initDesign()
    {
        this.Width = 400;
        this.Height = 150;


        //ime
        ipLabel = new Label();
        ipLabel.Text = "adresa servera";
        ipLabel.ForeColor = ColorConstants.UI_FORE;
        ipLabel.AutoSize = true;
        ipLabel.Font = new Font(this.Font.FontFamily, 11, FontStyle.Regular);
        ipLabel.Location = new System.Drawing.Point(80, 40);
        this.Controls.Add(ipLabel);

        ipTextBox = new TextBox();
        ipTextBox.BackColor = ColorConstants.UI_BACK;
        ipTextBox.BorderStyle = BorderStyle.None;
        ipTextBox.Font = new Font(this.Font.FontFamily, 13, FontStyle.Regular);
        ipTextBox.Width = 200;
        ipTextBox.MaxLength = 16;
        ipTextBox.Location = new System.Drawing.Point((this.Width/2) - (ipTextBox.Width / 2), ipLabel.Bottom + 5);
        this.Controls.Add(ipTextBox);

        //connect
        connectButton = new FlatButton();
        connectButton.Width = 110;
        connectButton.Height = 30;
        connectButton.Text = "nakači se";
        connectButton.Location = new System.Drawing.Point((this.Width/2)-(connectButton.Width/2), this.Bottom - connectButton.Height - 15);
        connectButton.Click += TryConnect;
        Controls.Add(connectButton);
    }
    
}
