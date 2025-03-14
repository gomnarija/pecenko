namespace pečenko;

public partial class FileForm : Form{
    private String          formText, buttonText;
    bool                    isSave;

    public delegate void OnButtonClick(String x);
    private OnButtonClick onButtonClick;

    public FileForm(String formText, String buttonText, OnButtonClick onButtonClick, bool isSave=false){
        this.formText = formText;
        this.buttonText = buttonText;
        this.onButtonClick = onButtonClick;
        this.isSave = isSave;
        InitializeComponent();
    }


    private void PathButton_Click(object sender, EventArgs e){
        if(!isSave){
            using (OpenFileDialog  openFileDialog = new OpenFileDialog ()){
                openFileDialog.Title = "izaberi fakturu";
                openFileDialog.Filter = "XML fakture|*.xml|sve datoteke|*.*";

                DialogResult result = openFileDialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(openFileDialog.FileName)){
                    this.pathTextBox.Text = openFileDialog.FileName;
                }
            }
        }else{
            using (SaveFileDialog  saveFileDialog = new SaveFileDialog ()){
                saveFileDialog.Title = "izaberi putanju";
                saveFileDialog.Filter = "XML fakture|*.xml|sve datoteke|*.*";

                DialogResult result = saveFileDialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(saveFileDialog.FileName)){
                    this.pathTextBox.Text = saveFileDialog.FileName;
                }
            }
        }
    }

    private void Button_Click(object sender, EventArgs e){
        String path = this.pathTextBox.Text;
        if(path == null || path.Length == 0){
            MessageBox.Show("putanja prazna", "greška", MessageBoxButtons.OK);
            return;
        }
        this.onButtonClick(path);
        this.Close();
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