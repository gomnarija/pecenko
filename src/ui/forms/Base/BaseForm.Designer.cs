namespace pečenko;

partial class BaseForm : Form{
    
    private System.ComponentModel.IContainer components = new System.ComponentModel.Container();

    private BarStrip barStrip = new BarStrip();

    private void designInit(){
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = ProgramConstans.PROGRAM_NAME;
        this.MaximizeBox = false;
        this.FormBorderStyle = FormBorderStyle.None;
        this.Font = FontConstants.BASE_FORM_FONT;
        this.BackColor = ColorConstants.BASE_FORM_BACK;
        this.ForeColor = ColorConstants.BASE_FORM_FRONT;



        //bar strip
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