using pečenko;
using System.Collections;
using System.Data;
using System.Drawing;
using UblSharp;
using UblSharp.CommonAggregateComponents;


public class InvoiceLinesPanel : Panel{
    public InvoiceLinesPanel(int width, int height){
        this.Width = width;
        this.Height = height;
        if(Program.invoiceOrchestrator != null)
            Program.invoiceOrchestrator.InsertedUpdated += InsertedUpdated;
        InitializeComponent();
    }

    Font font = new Font("Courier New", 12, FontStyle.Regular);
    DataGridView dataGridView = new DataGridView();

    Color rowBackColor1 = ColorTranslator.FromHtml("#3b4856");
    Color rowBackColor2 = ColorTranslator.FromHtml("#b8c3c6");
    
    Color rowForeColor1 = ColorTranslator.FromHtml("#effbff");
    Color rowForeColor2 = ColorTranslator.FromHtml("#3b4856");


    public enum CurrentView{
        Artikli,
        Proslo,
        Presek
     };
     public CurrentView currentView = CurrentView.Artikli;

    private void InitializeComponent(){
        this.ForeColor = Color.LightGray;
        dataGridView.Width = this.Width;
        dataGridView.Height = this.Height;
        dataGridView.ReadOnly = true;
        dataGridView.Font = font;
        dataGridView.BackgroundColor = Color.FromArgb(108, 122, 137);
        dataGridView.ForeColor = Color.LightGray;
        dataGridView.DefaultCellStyle.SelectionBackColor =  ColorTranslator.FromHtml("#82708f");;
        dataGridView.ScrollBars = ScrollBars.Vertical;
        dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridView.AllowUserToAddRows = false;
        dataGridView.MultiSelect = false;

        dataGridView.Columns.Add("id", "id");
        dataGridView.Columns["id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        dataGridView.Columns["id"].ValueType = typeof(int);
        dataGridView.Columns.Add("naziv", "naziv");
        dataGridView.Columns["naziv"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        dataGridView.Columns.Add("kod", "kod      ");
        dataGridView.Columns["kod"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

        dataGridView.Columns.Add("mera", "mera");
        dataGridView.Columns["mera"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        dataGridView.Columns.Add("količina", "količina");
        dataGridView.Columns["količina"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        dataGridView.Columns["količina"].ValueType = typeof(double);

        dataGridView.Columns.Add("cena", "cena");
        dataGridView.Columns["cena"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        dataGridView.Columns["cena"].ValueType = typeof(double);

        dataGridView.Columns.Add("valuta", "valuta");
        dataGridView.Columns["valuta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

        dataGridView.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#cddae5");
        dataGridView.RowHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#cddae5");
        dataGridView.EnableHeadersVisualStyles = false;
        dataGridView.RowHeadersDefaultCellStyle.SelectionBackColor =  ColorTranslator.FromHtml("#82708f");
        dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =  ColorTranslator.FromHtml("#82708f");

        


        dataGridView.Sorted += UpdateRowColors;
        dataGridView.RowsAdded += UpdateRowColors;
        dataGridView.RowsRemoved += UpdateRowColors;
        dataGridView.KeyDown += DataGridKeyDown;
        dataGridView.CellDoubleClick += DataGridDoubleClick;
        dataGridView.KeyPress += DataGridKeyPress;


        dataGridView.AllowUserToDeleteRows = false;
        dataGridView.UserDeletingRow += DataGridDeletingInsertedRow;


        this.Controls.Add(dataGridView);
    }





    private void ShowInsertForm(InvoiceLineType invoiceLine){
        Insert insert = new Insert(invoiceLine);
        insert.StartPosition = FormStartPosition.CenterScreen;
        insert.ShowDialog();
    }

    private void ShowInsertForm(String text){
        Insert insert = new Insert(text);
        insert.StartPosition = FormStartPosition.CenterScreen;
        insert.ShowDialog();
    }

    private void ShowEditInsertedForm(InvoiceLineType invoiceLine){
        EditInserted editInserted = new EditInserted(invoiceLine);
        editInserted.StartPosition = FormStartPosition.CenterScreen;
        editInserted.ShowDialog();
    }

    private void ShowEditInsertedForm(String text){
        EditInserted editInserted = new EditInserted(text);
        editInserted.StartPosition = FormStartPosition.CenterScreen;
        editInserted.ShowDialog();
    }

    private void ShowResultForm(InvoiceLineType invoiceLine){
        Result result = new Result(invoiceLine);
        result.StartPosition = FormStartPosition.CenterScreen;
        result.ShowDialog();
    }

    private void ShowResultForm(String text){
        Result result = new Result(text);
        result.StartPosition = FormStartPosition.CenterScreen;
        result.ShowDialog();
    }

    private void ShowEditInsertedForm(){
        EditInserted editInserted = new EditInserted();
        editInserted.StartPosition = FormStartPosition.CenterScreen;
        editInserted.ShowDialog();
    }

    private void ShowResultForm(){
        Result result = new Result();
        result.StartPosition = FormStartPosition.CenterScreen;
        result.ShowDialog();
    }

    private void ShowInsertForm(){
        Insert insert = new Insert();
        insert.StartPosition = FormStartPosition.CenterScreen;
        insert.ShowDialog();             
    }

    public void InsertedUpdated(object? sender, EventArgs e){
        if(Program.getProgramMode()==Program.ProgramMode.Client){
            Program.clientSync();
        }
        if(this.currentView == CurrentView.Proslo){
            _ = FillGridInsertedInvoiceLinesAsync();
        }else if(this.currentView == CurrentView.Presek){
            _ = FillGridInvoiceRemainingAsync();
        }
    }

    private void DataGridKeyDown(object? sender, KeyEventArgs  e){
        if(e.Control && e.KeyCode == Keys.S){
            Program.saveInserted();
        }else if(e.KeyCode == Keys.Enter && dataGridView.SelectedRows.Count > 0){
            DataGridViewRow  selectedRow = dataGridView.SelectedRows[0];
            String id = selectedRow.Cells["id"].Value.ToString()??"";
            e.Handled = true;
            if(currentView == CurrentView.Artikli){
                InvoiceLineType invoiceLine = Program.invoiceOrchestrator?.getInvoice()?.InvoiceLine?.Find(il => il.ID.Value.Equals(id))??new InvoiceLineType();
                ShowInsertForm(invoiceLine);
            }else if(currentView == CurrentView.Proslo){
                InvoiceLineType invoiceLine = Program.invoiceOrchestrator?.getInserted()?.Find(il => il.ID.Value.Equals(id))??new InvoiceLineType();
                ShowEditInsertedForm(invoiceLine);
            }else if(currentView == CurrentView.Presek){
                InvoiceLineType invoiceLine =Program.invoiceOrchestrator?.getInvoice()?.InvoiceLine?.Find(il => il.ID.Value.Equals(id))??new InvoiceLineType();
                ShowResultForm(invoiceLine);
            }
        }else if(e.KeyCode == Keys.Space){
            e.Handled = true;
            if(currentView == CurrentView.Artikli){
                ShowInsertForm();
            }else if(currentView == CurrentView.Proslo){
                ShowEditInsertedForm();
            }else if(currentView == CurrentView.Presek){
                ShowResultForm();
            }
        }
    }

    private void DataGridKeyPress(object? sender, KeyPressEventArgs  e){
        if(char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar)){
            String text = e.KeyChar.ToString();
            e.Handled = true;
            if(currentView == CurrentView.Artikli){
                ShowInsertForm(text);
            }else if(currentView == CurrentView.Proslo){
                ShowEditInsertedForm(text);
            }else if(currentView == CurrentView.Presek){
                ShowResultForm(text);
            }
        }
    }

    private void DataGridDoubleClick(object? sender, DataGridViewCellEventArgs   e){
        if(dataGridView.SelectedRows.Count > 0 && e.RowIndex >= 0){
            DataGridViewRow  selectedRow = dataGridView.SelectedRows[0];
            String id = selectedRow.Cells["id"].Value.ToString()??"";
            if(currentView == CurrentView.Artikli){
                InvoiceLineType invoiceLine = Program.invoiceOrchestrator?.getInvoice()?.InvoiceLine?.Find(il => il.ID.Value.Equals(id))??new InvoiceLineType();
                ShowInsertForm(invoiceLine);
            }else if(currentView == CurrentView.Proslo){
                InvoiceLineType invoiceLine = Program.invoiceOrchestrator?.getInserted()?.Find(il => il.ID.Value.Equals(id))??new InvoiceLineType();
                ShowEditInsertedForm(invoiceLine);
            }else if(currentView == CurrentView.Presek){
                InvoiceLineType invoiceLine = Program.invoiceOrchestrator?.getInvoice()?.InvoiceLine?.Find(il => il.ID.Value.Equals(id))??new InvoiceLineType();
                ShowResultForm(invoiceLine);
            }
        }
    }

    private void UpdateRowColors(object? sender, EventArgs e){
        foreach (DataGridViewRow row in dataGridView.Rows){
            if(row.Index % 2 == 0){
                row.DefaultCellStyle.BackColor = rowBackColor1;
                row.DefaultCellStyle.ForeColor = rowForeColor1;
            }else{
                row.DefaultCellStyle.BackColor = rowBackColor2;
                row.DefaultCellStyle.ForeColor = rowForeColor2;
            }
        }
    }

    private void DataGridDeletingInsertedRow(object? sender, EventArgs e){
        if(dataGridView.SelectedRows.Count > 0){
            DataGridViewRow  selectedRow = dataGridView.SelectedRows[0];
            String id = selectedRow.Cells["id"].Value.ToString()??"";
            if(currentView == CurrentView.Proslo){
                Program.invoiceOrchestrator?.removeInsertedInvoiceLine(id);
            }
        }
    }

    private List<object> GetInvoiceLineData(InvoiceLineType invoiceLine){
        List<object> data = new List<object>();
        //id
        data.Add(int.Parse(invoiceLine.ID?.Value??"0"));
        //name
        data.Add(invoiceLine.Item?.Name?.Value??"N/A");
        //kod
        data.Add(invoiceLine.Item?.SellersItemIdentification?.ID?.Value??"N/A");
        //mera
        data.Add(Program.invoiceOrchestrator?.getUnitOfMeasurementFromCode(invoiceLine.InvoicedQuantity?.unitCode??"N/a")??"N/a");
        //količina
        data.Add(double.Parse(invoiceLine.InvoicedQuantity?.Value.ToString()??"0"));
        //cena
        data.Add(double.Parse(invoiceLine.Price?.PriceAmount?.Value.ToString()??"0"));
         //valuta
        data.Add(invoiceLine.Price?.PriceAmount?.currencyID??"N/A");
        return data;
    }

    public async Task FillGridInvoiceAsync(){
        dataGridView.Rows.Clear();
        InvoiceType invoice = await Task.Run(()=> Program.invoiceOrchestrator?.getInvoice()??new InvoiceType());
        foreach(InvoiceLineType invoiceLine in invoice.InvoiceLine??new List<InvoiceLineType>()){
            dataGridView.Rows.Add(GetInvoiceLineData(invoiceLine).ToArray());
        }
        dataGridView.Focus();
        dataGridView.AllowUserToDeleteRows = false;
    }

    public async Task FillGridInsertedInvoiceLinesAsync(){
        List<InvoiceLineType> inserted = await Task.Run(()=> Program.invoiceOrchestrator?.getInserted())??new List<InvoiceLineType>();
        List<InvoiceLineType> data = new List<InvoiceLineType>();
        for(int i=0;i<inserted.Count;i++){
            data.Add(inserted[i]);
        }

        if (dataGridView.InvokeRequired){
            dataGridView.Invoke(new Action(() => UpdateDataGridViewInserted(data)));
        }
        else{
            UpdateDataGridViewInserted(data);
        }
    }

    public async Task FillGridInvoiceRemainingAsync(){
        if(dataGridView.InvokeRequired){
            dataGridView.Invoke(() => dataGridView.Rows.Clear);
        }else{
            dataGridView.Rows.Clear();
        }
        dataGridView.Rows.Clear();
        InvoiceType invoice = await Task.Run(()=> Program.invoiceOrchestrator?.getInvoice()??new InvoiceType());
        List<InvoiceLineType> invoiceLines = invoice?.InvoiceLine??new List<InvoiceLineType>();
        int loadCount = 100;
        int currentIndex;
        for(currentIndex=0;currentIndex<invoiceLines.Count;currentIndex+=loadCount){
            List<InvoiceLineType> load = invoiceLines.GetRange(currentIndex, Math.Min(currentIndex+loadCount, invoiceLines.Count) - currentIndex);
            List<String> codes = new List<String>();
            foreach(InvoiceLineType invoiceLine in load){
                codes.Add(invoiceLine.Item?.SellersItemIdentification?.ID?.Value??"");
            }
            List<decimal> remainingQuantities = await Task.Run(()=>Program.invoiceOrchestrator?.getRemainingQuantityForCodes(codes)??new List<decimal>());
            if (dataGridView.InvokeRequired){
                dataGridView.Invoke(new Action(() => UpdateDataGridViewRemaining(load, remainingQuantities)));
            }
            else{
                UpdateDataGridViewRemaining(load, remainingQuantities);
            }   
        }
        dataGridView.Focus();
        dataGridView.AllowUserToDeleteRows = false;
    }


    private void UpdateDataGridViewInserted(object data){
        dataGridView.Rows.Clear();
        foreach (var inserted in (List<InvoiceLineType>)data)
        {
            dataGridView.Rows.Add(GetInvoiceLineData(inserted).ToArray());
        }
        dataGridView.Focus();
        dataGridView.AllowUserToDeleteRows = true;
    }

    private void UpdateDataGridViewRemaining(List<InvoiceLineType> load, List<decimal> remainingQuantities){
        for(int i=0;i<remainingQuantities.Count;i++){
                decimal remainingQuantity = remainingQuantities[i];
                if(remainingQuantity == 0){
                    continue;
                }else{
                    List<object> data = GetInvoiceLineData(load[i]);
                    data[4] = remainingQuantity > 0 ? "+" + remainingQuantity.ToString() : remainingQuantity.ToString();//nije idealno
                    dataGridView.Rows.Add(data.ToArray());
                }
            }
    }
}