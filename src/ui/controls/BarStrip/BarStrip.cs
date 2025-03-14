namespace pečenko;

public partial class BarStrip : MenuStrip {
    public BarStrip(){
        this.Renderer = new CustomToolStripProfessionalRenderer();
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

