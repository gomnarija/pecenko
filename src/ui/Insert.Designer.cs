using System.Globalization;

namespace pečenko;

partial class Insert
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private bool oneTime;

    private MenuStrip barStrip;

    private Label insertLabel;
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
    private TextBox kolicinaTextBox;
    private Label   kolicinaLabel;
    Font font = new Font("Courier New", 12, FontStyle.Regular);



    private Color backColor = ColorTranslator.FromHtml("#283133");
    private Color foreUIColor = ColorTranslator.FromHtml("#effbff");

    private Color backUIColor = Color.FromArgb(183, 183, 183);

    private Color ROTextBoxBackColor = ColorTranslator.FromHtml("#555f61");

    private Color ROTextBoxForeColor = ColorTranslator.FromHtml("#effbff");
    private Color TextBoxSelectedBackColor = ColorTranslator.FromHtml("#82708f");
    private Color TextBoxSelectedForeColor = ColorTranslator.FromHtml("#82708f");


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
        this.BackColor = backColor;
        this.ForeColor = Color.Black;
        this.Width = 600;
        this.Height = 350;
        this.KeyPreview = true;
        this.KeyDown += Insert_KeyDown;

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
        kodTextBox.Width = 500;
        kodTextBox.Location = new System.Drawing.Point(50, kodLabel.Bottom + 5);
        this.Controls.Add(kodTextBox);
        kodTextBox.KeyPress += Enter_NextControl;
        kodTextBox.TextChanged += fillInFromCode;
        kodTextBox.Enter += TextBox_Enter;
        kodTextBox.Leave += TextBox_Leave;
        kodTextBox.AutoCompleteMode = AutoCompleteMode.Suggest;
        kodTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
        AutoCompleteStringCollection kodAutoCompleteSource = new AutoCompleteStringCollection();
        kodAutoCompleteSource.AddRange(collectInvoiceCodes().ToArray());
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
        valutaTextBox.HideSelection = true;
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
        cenaTextBox.HideSelection = true;
        cenaTextBox.TabStop = false;

        //mera label
        meraLabel = new Label();
        meraLabel.Text = "mera";
        meraLabel.ForeColor = foreUIColor;
        meraLabel.AutoSize = true;
        meraLabel.Font = new Font(font.FontFamily, 11, FontStyle.Regular);
        meraLabel.Location = new System.Drawing.Point(375, kodTextBox.Bottom + 10);
        this.Controls.Add(meraLabel);
      
        //mera textBox
        meraTextBox = new TextBox();
        meraTextBox.Text = "kom";
        meraTextBox.BackColor = ROTextBoxBackColor;
        meraTextBox.ForeColor = ROTextBoxForeColor;
        meraTextBox.BorderStyle = BorderStyle.None;
        meraTextBox.Font = new Font(font.FontFamily, 13, FontStyle.Regular);
        meraTextBox.Width = 150;
        meraTextBox.Location = new System.Drawing.Point(400, meraLabel.Bottom + 5);
        this.Controls.Add(meraTextBox);
        meraTextBox.ReadOnly = true;
        meraTextBox.HideSelection = true;
        meraTextBox.TabStop = false;

        //kolicina label
        kolicinaLabel = new Label();
        kolicinaLabel.Text = "količina";
        kolicinaLabel.ForeColor = foreUIColor;
        kolicinaLabel.AutoSize = true;
        kolicinaLabel.Font = new Font(font.FontFamily, 11, FontStyle.Regular);
        kolicinaLabel.Location = new System.Drawing.Point(375, meraTextBox.Bottom + 10);
        this.Controls.Add(kolicinaLabel);


        //kolicina textBox
        kolicinaTextBox = new TextBox();
        kolicinaTextBox.BackColor = backUIColor;
        kolicinaTextBox.BorderStyle = BorderStyle.None;
        kolicinaTextBox.Font = new Font(font.FontFamily, 13, FontStyle.Regular);
        kolicinaTextBox.Width = 150;
        kolicinaTextBox.Location = new System.Drawing.Point(400, kolicinaLabel.Bottom + 5);
        this.Controls.Add(kolicinaTextBox);
        kolicinaTextBox.KeyPress += Enter_NextControl;
        kolicinaTextBox.Enter += TextBox_Enter;
        kolicinaTextBox.Leave += TextBox_Leave;

        //label
        insertLabel = new Label();
        insertLabel.Text = "unos artikla";
        insertLabel.ForeColor = foreUIColor;
        insertLabel.AutoSize = true;
        insertLabel.Font = new Font(font.FontFamily, 14, FontStyle.Regular);
        insertLabel.Location = new System.Drawing.Point(this.Width/2 - (insertLabel.Width/2), barStrip.Bottom + 5);
        this.Controls.Add(insertLabel);


        //insert button
        insertButton = new Button();
        insertButton.Text = "unesi";
        insertButton.Width = 170;
        insertButton.Height = 36;
        insertButton.Font = new Font(font.FontFamily, 11, FontStyle.Regular);
        insertButton.BackColor = backUIColor;
        insertButton.FlatStyle = FlatStyle.Flat;
        insertButton.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#eaf2d7");
        insertButton.FlatAppearance.BorderSize = 2;   
        insertButton.Location = new System.Drawing.Point((this.Width/2)-(insertButton.Width/2), this.Bottom - 15 - insertButton.Height);
        this.Controls.Add(insertButton);
        insertButton.Click += insertInvoiceLine;
        insertButton.Enter += Button_Enter;
        insertButton.Leave += Button_Leave;
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
