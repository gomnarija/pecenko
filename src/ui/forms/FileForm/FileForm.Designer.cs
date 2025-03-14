namespace pečenko;

partial class FileForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private BarStrip barStrip;

    private Button pathButton;
    private TextBox pathTextBox;
    private Label pathLabel;
    private Button loadButton;
    private Color backColor = ColorTranslator.FromHtml("#283133");
    private Color foreColor = ColorTranslator.FromHtml("#effbff");
    private Color foreUIColor = Color.Black;
    private Color backUIColor = Color.FromArgb(183, 183, 183);

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "pečenko";
        this.MaximizeBox = false;
        this.FormBorderStyle = FormBorderStyle.None;
        this.Font = new Font("Courier New", 12, FontStyle.Regular);
        this.BackColor = backColor;
        this.ForeColor = foreColor;
        this.Width = 500;
        this.Height = 165;

        //path button
        pathButton = new Button();
        pathButton.Text = "izaberi";
        pathButton.Font = new Font(pathButton.Font.FontFamily, 9, FontStyle.Regular);
        pathButton.BackColor = backUIColor;
        pathButton.ForeColor = foreUIColor;
        pathButton.FlatStyle = FlatStyle.Flat;
        pathButton.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#eaf2d7");
        pathButton.FlatAppearance.BorderSize = 1;        
        pathButton.Click += PathButton_Click;
        this.Controls.Add(pathButton);
        
        //textBox
        pathTextBox = new TextBox();
        pathTextBox.Text = AppDomain.CurrentDomain.BaseDirectory;
        pathTextBox.Font = new Font(pathTextBox.Font.FontFamily, 9, FontStyle.Regular);
        pathTextBox.BackColor = backUIColor;
        pathTextBox.ForeColor = foreUIColor;
        pathTextBox.BorderStyle = BorderStyle.None;
        pathTextBox.Width = 330;
        pathTextBox.HideSelection = true;
        pathTextBox.Location = new System.Drawing.Point((this.Width/2)-(pathTextBox.Width/2)-(pathButton.Width/2), 70);
        this.Controls.Add(pathTextBox);
        
        pathButton.Location = new System.Drawing.Point(pathTextBox.Location.X + pathTextBox.Width + 5, 67);

        //label
        pathLabel = new Label();
        pathLabel.Text = formText;
        pathLabel.Font = new Font(pathLabel.Font.FontFamily, 11, FontStyle.Regular);
        pathLabel.Width = 150;
        pathLabel.Location = new System.Drawing.Point(pathTextBox.Location.X - 15, 45);
        this.Controls.Add(pathLabel);


        //load button
        loadButton = new Button();
        loadButton.Text = buttonText;
        loadButton.Width = 170;
        loadButton.Height = 36;
        loadButton.Font = new Font(loadButton.Font.FontFamily, 11, FontStyle.Regular);
        loadButton.BackColor = backUIColor;
        loadButton.ForeColor = foreUIColor;
        loadButton.FlatStyle = FlatStyle.Flat;
        loadButton.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#eaf2d7");
        loadButton.FlatAppearance.BorderSize = 2;   
        loadButton.Click += Button_Click;
        loadButton.Location = new System.Drawing.Point((this.Width/2)-(loadButton.Width/2), 110);
        this.Controls.Add(loadButton);


        //menu strip
        barStrip = new BarStrip();  
        barStrip.BackColor = ColorTranslator.FromHtml("#a0aebe");
        barStrip.MouseDown += MenuStrip_MouseDown;
        barStrip.MouseMove += MenuStrip_MouseMove;

        //close
        ToolStripMenuItem closeItem = new ToolStripMenuItem("X");
        closeItem.Click += Close;
        closeItem.Font = new Font(closeItem.Font.FontFamily, 12, FontStyle.Regular);
        //mini
        ToolStripMenuItem miniItem = new ToolStripMenuItem("-");
        miniItem.Click += Minimize;
        miniItem.Font = new Font(miniItem.Font.FontFamily, 12, FontStyle.Regular);
        //help
        ToolStripMenuItem helpItem = new ToolStripMenuItem("?");
        helpItem.Font = new Font(helpItem.Font.FontFamily, 12, FontStyle.Regular);

        barStrip.Items.Add(closeItem);
        barStrip.Items.Add(miniItem);
        barStrip.Items.Add(helpItem);

        this.Controls.Add(barStrip);

    }






    
    
}
