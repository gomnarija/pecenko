public class AutoCompleteTextBox : TextBox
    {
        private ListBox _listBox;
        private bool _isAdded;
        private String[] _values;
        private String _formerValue = String.Empty;
        private int _maxNumberOfItems = 7;

        public AutoCompleteTextBox()
        {
            _values = [];
            _listBox = new ListBox();
            KeyDown += _KeyDown;
            KeyUp += _KeyUp;
            ResetListBox();
        }


        private void ShowListBox()
        {
            if (!_isAdded)
            {
                _listBox.ItemHeight = Height;
                _listBox.Height = _maxNumberOfItems * Height;
                _listBox.Width = Width;
                _listBox.Top = Top + Height;
                _listBox.Left = Left;
                _listBox.Click += _listBox_Click;
                Parent?.Controls.Add(_listBox);
                _isAdded = true;
            }
            _listBox.Visible = true;
            _listBox.BringToFront();
        }

        private void ResetListBox()
        {
            _listBox.Visible = false;
        }

        private void _KeyUp(object? sender, KeyEventArgs e)
        {
            UpdateListBox();
        }

        private void _KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Return: // Select
                    {
                        if (_listBox.Visible)
                        {
                            Focus();
                            InsertWord((String?)_listBox.SelectedItem??"");
                            ResetListBox();
                        }
                        break;
                    }
                case Keys.Down: //scroll down
                    {
                        if ((_listBox.Visible) ){
                            e.Handled = true;
                            if(_listBox.SelectedIndex < _listBox.Items.Count - 1)
                                _listBox.SelectedIndex++;
                        }

                        break;
                    }
                case Keys.Up: // scroll up
                    {
                        if ((_listBox.Visible)){
                            e.Handled = true;
                            if(_listBox.SelectedIndex > 0)
                                _listBox.SelectedIndex--;
                        }
                        break;
                    }
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Tab:
                    return true;
                default:
                    return base.IsInputKey(keyData);
            }
        }

        private void UpdateListBox()
        {
            if (Text == _formerValue) return;
            _formerValue = Text;
            String word = Text;
            if (_values != null && word.Length > 0)
            {
                String[] matches = Array.FindAll(_values, // first check for the ones that start with
                                                 x => (x.StartsWith(word, StringComparison.OrdinalIgnoreCase) && !x.Equals(word)))
                            .Concat(Array.FindAll(_values,// and then for the ones that contain and doesn't start with ( to avoid duplicates )
                                                x => (x.Contains(word, StringComparison.OrdinalIgnoreCase) && !x.StartsWith(word, StringComparison.OrdinalIgnoreCase) && !x.Equals(word)))).ToArray();
                if (matches.Length > 0)
                {
                    ShowListBox();
                    _listBox.Items.Clear();
                    Array.ForEach(matches, x => _listBox.Items.Add(x));
                    _listBox.SelectedIndex = 0;
                    _listBox.Width = 0;
                    _listBox.Height = Math.Min(_listBox.Items.Count, _maxNumberOfItems) * Height;
                    Focus();
                    using (Graphics graphics = _listBox.CreateGraphics())
                    {
                        for (int i = 0; i < _listBox.Items.Count; i++)
                        {
                            int itemWidth = (int)graphics.MeasureString(((String)_listBox.Items[i]) + "_", _listBox.Font).Width;
                            _listBox.Width = (_listBox.Width < itemWidth) ? itemWidth : _listBox.Width;
                        }
                    }
                }
                else
                {
                    ResetListBox();
                }
            }
            else
            {
                ResetListBox();
            }
        }

        private void _listBox_Click(object? sender, EventArgs e){
            Focus();
            InsertWord(_listBox.GetItemText(_listBox.SelectedItem)??"");
            ResetListBox();
        }

        private void InsertWord(String updatedText)
        {
            Text = updatedText;
            SelectionStart = updatedText.Length;
            _formerValue = Text;
            OnTextChanged(EventArgs.Empty);
        }

        public String[] Values
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
            }
        }

        public int MaxNumberOfItems
        {
            get
            {
                return _maxNumberOfItems;
            }
            set
            {
                _maxNumberOfItems = value;
            }
        }

    }


