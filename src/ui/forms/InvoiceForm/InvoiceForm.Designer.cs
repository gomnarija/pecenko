namespace pečenko;

partial class InvoiceForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private MenuStrip barStrip;
    private Button fakturaButton;
    private Button artikliButton;
    private Button prosloButton;
    private Button presekButton;

    private InvoicePanel invoicePanel;
    private InvoiceLinesPanel invoiceLinesPanel;



    private Color backColor = Color.FromArgb(108, 122, 137);
    private Color foreUIColor = Color.FromArgb(191, 191, 191);
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
        this.BackColor = ColorTranslator.FromHtml("#d2d7d3");;
        this.ForeColor = Color.Black;
        this.Width = 900;
        this.Height = 700;

        //bar strip
        barStrip = new MenuStrip();  
        barStrip.BackColor = ColorTranslator.FromHtml("#a0aebe");
        barStrip.MouseDown += MenuStrip_MouseDown;
        barStrip.MouseMove += MenuStrip_MouseMove;
        barStrip.Font = new Font(barStrip.Font.FontFamily, 12, FontStyle.Regular);

        barStrip.Renderer = new CustomToolStripProfessionalRenderer();
        //close
        ToolStripMenuItem closeItem = new ToolStripMenuItem("X");
        closeItem.Click += Close;
        //mini
        ToolStripMenuItem miniItem = new ToolStripMenuItem("-");
        miniItem.Click += Minimize;
        //help
        ToolStripMenuItem helpItem = new ToolStripMenuItem("?");
        barStrip.Items.Add(closeItem);
        barStrip.Items.Add(miniItem);
        barStrip.Items.Add(helpItem);
        //file
        if(Program.getProgramMode() == Program.ProgramMode.Server){
            ToolStripMenuItem fileItem = new ToolStripMenuItem("datoteka");
            ToolStripMenuItem insertedFileSubItem = new ToolStripMenuItem("učitaj unete artikle");
            insertedFileSubItem.Click += LoadInserted;
            ToolStripMenuItem saveInsertedSubItem = new ToolStripMenuItem("sačuvaj unete artikle");
            saveInsertedSubItem.Click += SaveInserted;
            ToolStripMenuItem newFileSubItem = new ToolStripMenuItem("učitaj novu fakturu");
            newFileSubItem.Click += LoadNewInvoice;
            fileItem.DropDownItems.Add(insertedFileSubItem);
            fileItem.DropDownItems.Add(saveInsertedSubItem);
            fileItem.DropDownItems.Add(newFileSubItem);

            
            barStrip.Items.Add(fileItem);
        }else if(Program.getProgramMode() == Program.ProgramMode.Client){
            ToolStripMenuItem syncItem = new ToolStripMenuItem("sinhronizuj");
            syncItem.Click += Sync;
            barStrip.Items.Add(syncItem);

        }
        ToolStripSeparator separatorItem = new ToolStripSeparator();
        separatorItem.Width = 100;
        barStrip.Items.Add(separatorItem);
        ToolStripLabel labelItem = new ToolStripLabel();
        labelItem.Text = Program.getUserName() + "@" + (Program.getProgramMode() == Program.ProgramMode.Server ?  NetworkUtils.getLocalIpAddress() : Program.getServerIp());
        barStrip.Items.Add(labelItem);

        this.Controls.Add(barStrip);



        //fakture button
        fakturaButton = new Button();
        fakturaButton.Text = "faktura";
        fakturaButton.Location =  new System.Drawing.Point(0, barStrip.Bottom);
        fakturaButton.FlatStyle = FlatStyle.Flat;
        fakturaButton.Click += FakturaButton_Click;
        this.Controls.Add(fakturaButton);

        //artikli button
        artikliButton = new Button();
        artikliButton.Text = "artikli";
        artikliButton.Location =  new System.Drawing.Point(fakturaButton.Right, barStrip.Bottom);
        artikliButton.FlatStyle = FlatStyle.Flat;
        artikliButton.Click += ArtikliButton_Click;
        this.Controls.Add(artikliButton);

        //proslo button
        prosloButton = new Button();
        prosloButton.Text = "prošlo";
        prosloButton.Location =  new System.Drawing.Point(artikliButton.Right, barStrip.Bottom);
        prosloButton.FlatStyle = FlatStyle.Flat;
        prosloButton.Click += ProsloButton_Click;
        this.Controls.Add(prosloButton);

        //presek button
        presekButton = new Button();
        presekButton.Text = "presek";
        presekButton.Location =  new System.Drawing.Point(prosloButton.Right, barStrip.Bottom);
        presekButton.FlatStyle = FlatStyle.Flat;
        presekButton.Click += PresekButton_Click;
        this.Controls.Add(presekButton);


        // //invoice panel
        invoicePanel = new InvoicePanel();
        invoicePanel.BackColor = backColor;
        invoicePanel.Top = fakturaButton.Bottom;
        invoicePanel.Width = this.Width;
        invoicePanel.Height = this.Height;
        this.Controls.Add(invoicePanel);

        //invoice lines panel
        invoiceLinesPanel = new InvoiceLinesPanel(this.Width, this.Height - fakturaButton.Bottom);
        invoiceLinesPanel.BackColor = backColor;
        invoiceLinesPanel.Top = fakturaButton.Bottom;
        invoiceLinesPanel.Visible = invoiceLinesPanel.Enabled = false;
        this.Controls.Add(invoiceLinesPanel);
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
