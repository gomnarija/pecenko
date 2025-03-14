namespace pečenko;

partial class Result
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private MenuStrip barStrip;

    private Label insertLabel;
    private Button okButton;
    private Button editButton;
    private Button insertButton;

    private AutoCompleteTextBox nazivTextBox;
    private Label   nazivLabel;
    private TextBox kodTextBox;
    private Label   kodLabel;
    private TextBox valutaTextBox;
    private Label   valutaLabel;
    private TextBox cenaTextBox;
    private Label   cenaLabel;
    private TextBox meraTextBox;
    private Label   meraLabel;
    private TextBox presekTextBox;
    private Label   presekLabel;
    private TextBox originalnaKolicinaTextBox;
    private Label   originalnaKolicinaLabel;
    private TextBox unetoTextBox;
    private Label   unetoLabel;
    Font font = new Font("Courier New", 12, FontStyle.Regular);



    private Color backColor = ColorTranslator.FromHtml("#283133");
    private Color foreUIColor = ColorTranslator.FromHtml("#effbff");

    private Color backUIColor = Color.FromArgb(183, 183, 183);

    private Color ROTextBoxBackColor = ColorTranslator.FromHtml("#555f61");

    private Color ROTextBoxForeColor = ColorTranslator.FromHtml("#effbff");

    private Color TextBoxSelectedBackColor = ColorTranslator.FromHtml("#82708f");
    private Color TextBoxSelectedForeColor = ColorTranslator.FromHtml("#82708f");

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "pečenko";
        this.MaximizeBox = false;
        this.FormBorderStyle = FormBorderStyle.None;
        this.BackColor = backColor;
        this.ForeColor = Color.Black;
        this.Width = 600;
        this.Height = 350;
        this.KeyPreview = true;
        this.KeyDown += Result_KeyDown;

        //menu strip
        barStrip = new MenuStrip();  
        barStrip.BackColor = ColorTranslator.FromHtml("#a0aebe");
        barStrip.MouseDown += MenuStrip_MouseDown;
        barStrip.MouseMove += MenuStrip_MouseMove;

        barStrip.Renderer = new CustomToolStripProfessionalRenderer();
        //close
        ToolStripMenuItem closeItem = new ToolStripMenuItem("X");
        closeItem.Click += Close;
        closeItem.Font = new Font(closeItem.Font.FontFamily, 12, FontStyle.Regular);
        //help
        ToolStripMenuItem helpItem = new ToolStripMenuItem("?");
        helpItem.Font = new Font(helpItem.Font.FontFamily, 12, FontStyle.Regular);

        barStrip.Items.Add(closeItem);
        barStrip.Items.Add(helpItem);
        this.Controls.Add(barStrip);


        //naziv label
        nazivLabel = new Label();
        nazivLabel.Text = "naziv";
        nazivLabel.ForeColor = foreUIColor;
        nazivLabel.AutoSize = true;
        nazivLabel.Font = new Font(font.FontFamily, 11, FontStyle.Regular);
        nazivLabel.Location = new System.Drawing.Point(25, 70);
        this.Controls.Add(nazivLabel);
      
        //naziv textBox
        nazivTextBox = new AutoCompleteTextBox();
        nazivTextBox.BackColor = backUIColor;
        nazivTextBox.BorderStyle = BorderStyle.None;
        nazivTextBox.Font = new Font(font.FontFamily, 13, FontStyle.Regular);
        nazivTextBox.Width = 500;
        nazivTextBox.Location = new System.Drawing.Point(50, nazivLabel.Bottom + 5);
        this.Controls.Add(nazivTextBox);
        nazivTextBox.AutoCompleteMode = AutoCompleteMode.Suggest;
        nazivTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
        nazivTextBox.TextChanged += fillInFromName;
        nazivTextBox.KeyPress += Enter_NextControl;
        nazivTextBox.Enter += TextBox_Enter;
        nazivTextBox.Leave += TextBox_Leave;
        nazivTextBox.TabStop = false;

        nazivTextBox.Values = collectInvoiceLineNames().ToArray();


        //kod label
        kodLabel = new Label();
        kodLabel.Text = "kod";
        kodLabel.ForeColor = foreUIColor;
        kodLabel.AutoSize = true;
        kodLabel.Font = new Font(font.FontFamily, 11, FontStyle.Regular);
        kodLabel.Location = new System.Drawing.Point(25, nazivTextBox.Bottom + 10);
        this.Controls.Add(kodLabel);
      
        //kod textBox
        kodTextBox = new TextBox();
        kodTextBox.BackColor = backUIColor;
        kodTextBox.BorderStyle = BorderStyle.None;
        kodTextBox.Font = new Font(font.FontFamily, 13, FontStyle.Regular);
        kodTextBox.Width = 300;
        kodTextBox.Location = new System.Drawing.Point(50, kodLabel.Bottom + 5);
        this.Controls.Add(kodTextBox);
        kodTextBox.AutoCompleteMode = AutoCompleteMode.Suggest;
        kodTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
        AutoCompleteStringCollection kodAutoCompleteSource = new AutoCompleteStringCollection();
        kodAutoCompleteSource.AddRange(collectInvoiceCodes().ToArray());
        kodTextBox.KeyPress += Enter_NextControl;
        kodTextBox.TextChanged += fillInFromCode;
        kodTextBox.Enter += TextBox_Enter;
        kodTextBox.Leave += TextBox_Leave;
        kodTextBox.AutoCompleteCustomSource = kodAutoCompleteSource;
        kodTextBox.TabStop = false;

        
        //valuta label
        valutaLabel = new Label();
        valutaLabel.Text = "valuta";
        valutaLabel.ForeColor = foreUIColor;
        valutaLabel.AutoSize = true;
        valutaLabel.Font = new Font(font.FontFamily, 11, FontStyle.Regular);
        valutaLabel.Location = new System.Drawing.Point(25, kodTextBox.Bottom + 10);
        this.Controls.Add(valutaLabel);
      
        //valuta textBox
        valutaTextBox = new TextBox();
        valutaTextBox.Text = "RSD";
        valutaTextBox.BackColor = ROTextBoxBackColor;
        valutaTextBox.ForeColor = ROTextBoxForeColor;
        valutaTextBox.BorderStyle = BorderStyle.None;
        valutaTextBox.Font = new Font(font.FontFamily, 13, FontStyle.Regular);
        valutaTextBox.Width = 150;
        valutaTextBox.Location = new System.Drawing.Point(50, valutaLabel.Bottom + 5);
        this.Controls.Add(valutaTextBox);
        valutaTextBox.ReadOnly = true;
        valutaTextBox.TabStop = false;

        //cena label
        cenaLabel = new Label();
        cenaLabel.Text = "cena";
        cenaLabel.ForeColor = foreUIColor;
        cenaLabel.AutoSize = true;
        cenaLabel.Font = new Font(font.FontFamily, 11, FontStyle.Regular);
        cenaLabel.Location = new System.Drawing.Point(25, valutaTextBox.Bottom + 10);
        this.Controls.Add(cenaLabel);
      
        //cena textBox
        cenaTextBox = new TextBox();
        cenaTextBox.BackColor = ROTextBoxBackColor;
        cenaTextBox.ForeColor = ROTextBoxForeColor;
        cenaTextBox.BorderStyle = BorderStyle.None;
        cenaTextBox.Font = new Font(font.FontFamily, 13, FontStyle.Regular);
        cenaTextBox.Width = 150;
        cenaTextBox.Location = new System.Drawing.Point(50, cenaLabel.Bottom + 5);
        this.Controls.Add(cenaTextBox);
        cenaTextBox.ReadOnly = true;
        cenaTextBox.TabStop = false;

        //mera label
        meraLabel = new Label();
        meraLabel.Text = "mera";
        meraLabel.ForeColor = foreUIColor;
        meraLabel.AutoSize = true;
        meraLabel.Font = new Font(font.FontFamily, 11, FontStyle.Regular);
        meraLabel.Location = new System.Drawing.Point(375, nazivTextBox.Bottom + 10);
        this.Controls.Add(meraLabel);
      
        //mera textBox
        meraTextBox = new TextBox();
        meraTextBox.Text = "kom";
        meraTextBox.BackColor = ROTextBoxBackColor;
        meraTextBox.ForeColor =ROTextBoxForeColor;
        meraTextBox.BorderStyle = BorderStyle.None;
        meraTextBox.Font = new Font(font.FontFamily, 13, FontStyle.Regular);
        meraTextBox.Width = 150;
        meraTextBox.Location = new System.Drawing.Point(400, meraLabel.Bottom + 5);
        this.Controls.Add(meraTextBox);
        meraTextBox.ReadOnly = true;
        meraTextBox.TabStop = false;

        //presek label
        presekLabel = new Label();
        presekLabel.Text = "presek";
        presekLabel.ForeColor = foreUIColor;
        presekLabel.AutoSize = true;
        presekLabel.Font = new Font(font.FontFamily, 11, FontStyle.Regular);
        presekLabel.Location = new System.Drawing.Point(375, meraTextBox.Bottom + 10);
        this.Controls.Add(presekLabel);
      
        //presek textBox
        presekTextBox = new TextBox();
        presekTextBox.BackColor = ROTextBoxBackColor;
        presekTextBox.ForeColor = ROTextBoxForeColor;
        presekTextBox.BorderStyle = BorderStyle.None;
        presekTextBox.Font = new Font(font.FontFamily, 13, FontStyle.Regular);
        presekTextBox.Width = 150;
        presekTextBox.Location = new System.Drawing.Point(400, presekLabel.Bottom + 5);
        this.Controls.Add(presekTextBox);
        presekTextBox.ReadOnly = true;
        presekTextBox.TabStop = false;

        //kolicina label
        originalnaKolicinaLabel = new Label();
        originalnaKolicinaLabel.Text = "kol. faktura";
        originalnaKolicinaLabel.ForeColor = foreUIColor;
        originalnaKolicinaLabel.AutoSize = true;
        originalnaKolicinaLabel.Font = new Font(font.FontFamily, 11, FontStyle.Regular);
        originalnaKolicinaLabel.Location = new System.Drawing.Point(310, presekTextBox.Bottom + 10);
        this.Controls.Add(originalnaKolicinaLabel);
      
        //kolicina textBox
        originalnaKolicinaTextBox = new TextBox();
        originalnaKolicinaTextBox.BackColor = ROTextBoxBackColor;
        originalnaKolicinaTextBox.ForeColor = ROTextBoxForeColor;
        originalnaKolicinaTextBox.BorderStyle = BorderStyle.None;
        originalnaKolicinaTextBox.Font = new Font(font.FontFamily, 13, FontStyle.Regular);
        originalnaKolicinaTextBox.Width = 100;
        originalnaKolicinaTextBox.Location = new System.Drawing.Point(350, originalnaKolicinaLabel.Bottom + 5);
        this.Controls.Add(originalnaKolicinaTextBox);
        originalnaKolicinaTextBox.ReadOnly = true;
        originalnaKolicinaTextBox.TabStop = false;

        //uneto label
        unetoLabel = new Label();
        unetoLabel.Text = "uneto";
        unetoLabel.ForeColor = foreUIColor;
        unetoLabel.AutoSize = true;
        unetoLabel.Font = new Font(font.FontFamily, 11, FontStyle.Regular);
        unetoLabel.Location = new System.Drawing.Point(450, presekTextBox.Bottom + 10);
        this.Controls.Add(unetoLabel);
      
        //uneto textBox
        unetoTextBox = new TextBox();
        unetoTextBox.BackColor = ROTextBoxBackColor;
        unetoTextBox.ForeColor = ROTextBoxForeColor;
        unetoTextBox.BorderStyle = BorderStyle.None;
        unetoTextBox.Font = new Font(font.FontFamily, 13, FontStyle.Regular);
        unetoTextBox.Width = 100;
        unetoTextBox.Location = new System.Drawing.Point(475, originalnaKolicinaLabel.Bottom + 5);
        this.Controls.Add(unetoTextBox);
        unetoTextBox.ReadOnly = true;
        unetoTextBox.TabStop = false;

        //label
        insertLabel = new Label();
        insertLabel.Text = "presek artikla";
        insertLabel.ForeColor = foreUIColor;
        insertLabel.AutoSize = true;
        insertLabel.Font = new Font(font.FontFamily, 14, FontStyle.Regular);
        insertLabel.Location = new System.Drawing.Point(this.Width/2 - (insertLabel.Width), barStrip.Bottom + 5);
        this.Controls.Add(insertLabel);


        //ok button
        okButton = new Button();
        okButton.Text = "dobro";
        okButton.Width = 170;
        okButton.Height = 36;
        okButton.Font = new Font(font.FontFamily, 11, FontStyle.Regular);
        okButton.BackColor = backUIColor;
        okButton.FlatStyle = FlatStyle.Flat;
        okButton.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#eaf2d7");
        okButton.FlatAppearance.BorderSize = 2;   
        okButton.Location = new System.Drawing.Point((this.Width/2)-(okButton.Width/2), this.Bottom - 15 - okButton.Height);
        this.Controls.Add(okButton);
        okButton.Click += fine;
        okButton.Enter += Button_Enter;
        okButton.Leave += Button_Leave;

        //edit button
        editButton = new Button();
        editButton.Text = "[i]zmeni";
        editButton.Width = 125;
        editButton.Height = 28;
        editButton.Font = new Font(font.FontFamily, 11, FontStyle.Regular);
        editButton.BackColor = backUIColor;
        editButton.FlatStyle = FlatStyle.Flat;
        editButton.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#eaf2d7");
        editButton.FlatAppearance.BorderSize = 2;   
        editButton.Location = new System.Drawing.Point(15, this.Bottom - 15 - editButton.Height);
        this.Controls.Add(editButton);
        editButton.Click += edit;
        editButton.TabStop = false;

        //insert button
        insertButton = new Button();
        insertButton.Text = "[u]nesi";
        insertButton.Width = 125;
        insertButton.Height = 28;
        insertButton.Font = new Font(font.FontFamily, 11, FontStyle.Regular);
        insertButton.BackColor = backUIColor;
        insertButton.FlatStyle = FlatStyle.Flat;
        insertButton.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#eaf2d7");
        insertButton.FlatAppearance.BorderSize = 2;   
        insertButton.Location = new System.Drawing.Point(this.Right - 15 - insertButton.Width, this.Bottom - 15 - insertButton.Height);
        this.Controls.Add(insertButton);
        insertButton.Click += insert;
        insertButton.TabStop = false;
    }



     // changes highlight color
    private class CustomToolStripProfessionalRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs myMenu)
        {
            if (!myMenu.Item.Selected)
            base.OnRenderMenuItemBackground(myMenu);
            else
            {
                Rectangle menuRectangle = new Rectangle(Point.Empty, myMenu.Item.Size);
                //Fill Color
                myMenu.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(108, 122, 137)), menuRectangle);
                // Border Color
                myMenu.Graphics.DrawRectangle(Pens.WhiteSmoke, 1, 0, menuRectangle.Width - 2, menuRectangle.Height - 1);
            }
        }
    }


    
    
}
