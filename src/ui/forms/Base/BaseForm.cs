namespace pečenko;

public partial class BaseForm : Form {



    public BaseForm(){
       designInit();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void Close(object sender, EventArgs e){
        this.Close();
    }

    private void Minimize(object sender, EventArgs e){
        this.WindowState = FormWindowState.Minimized;
    }

    private Point startDragPoint;
    private void MenuStrip_MouseDown(object sender, MouseEventArgs e){
        if (e.Button == MouseButtons.Left)
        {
            startDragPoint = new Point(e.X, e.Y);
        }
    }
    private void MenuStrip_MouseMove(object sender, MouseEventArgs e){
        if (e.Button == MouseButtons.Left)
        {
            int deltaX = e.X - startDragPoint.X;
            int deltaY = e.Y - startDragPoint.Y;
            this.Location = new Point(this.Left + deltaX, this.Top + deltaY);
        }
    }
}