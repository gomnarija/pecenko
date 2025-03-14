namespace pečenko;

partial class MainForm
{
    private Label pecenkoLabel;
    private Label imeLabel;
    private TextBox imeTextBox;
    private Button serverButton;
    private Button klijentButton;



    private void initDesign()
    {
        this.Width = 500;
        this.Height = 250;

        //pecenko
        pecenkoLabel = new Label();
        pecenkoLabel.Text = ProgramConstans.PROGRAM_NAME + " v" + ProgramConstans.PROGRAM_VERSION;
        pecenkoLabel.ForeColor = ColorConstants.UI_FORE;
        pecenkoLabel.AutoSize = true;
        pecenkoLabel.Anchor = AnchorStyles.None;
        pecenkoLabel.Font = new Font(this.Font.FontFamily, 13, FontStyle.Regular);
        pecenkoLabel.Location = new System.Drawing.Point(5,50);
        this.Controls.Add(pecenkoLabel);


        //ime
        imeLabel = new Label();
        imeLabel.Text = "korisničko ime";
        imeLabel.ForeColor = ColorConstants.UI_FORE;
        imeLabel.AutoSize = true;
        imeLabel.Font = new Font(this.Font.FontFamily, 11, FontStyle.Regular);
        imeLabel.Location = new System.Drawing.Point(185, 90);
        this.Controls.Add(imeLabel);

        imeTextBox = new TextBox();
        imeTextBox.BackColor = ColorConstants.UI_BACK;
        imeTextBox.BorderStyle = BorderStyle.None;
        imeTextBox.Font = new Font(this.Font.FontFamily, 13, FontStyle.Regular);
        imeTextBox.Width = 200;
        imeTextBox.MaxLength = 10;
        imeTextBox.Location = new System.Drawing.Point((this.Width/2) - (imeTextBox.Width / 2), imeLabel.Bottom + 5);
        this.Controls.Add(imeTextBox);

        //server
        serverButton = new FlatButton();
        serverButton.Width = 125;
        serverButton.Height = 40;
        serverButton.Text = "pokreni";
        serverButton.Location = new System.Drawing.Point((this.Width/2)-(serverButton.Width/2), this.Bottom - serverButton.Height - 15);
        serverButton.Click += ServerSelect;
        Controls.Add(serverButton);

        //klijent
        klijentButton = new FlatButton();
        klijentButton.Width = 110;
        klijentButton.Height = 36;
        klijentButton.Text = "nakači se";
        klijentButton.Location = new System.Drawing.Point(serverButton.Right + 50, serverButton.Top + 3);
        klijentButton.Click += ClientSelect;
        Controls.Add(klijentButton);
    }
    
}
