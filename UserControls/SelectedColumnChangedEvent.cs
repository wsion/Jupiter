using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jupiter.UserControls
{
    public class SelectedColumnChangedEventArgs : EventArgs
    {
        public SelectedColumnChangedEventArgs(string columnName, bool isSelected)
        {
            this.ColumnName = columnName;
            this.IsSelected = isSelected;
        }

        public string ColumnName { get; internal set; }

        public bool IsSelected { get; internal set; }
    }

    public delegate void SelectedColumnChangedEventHandler(object sender, SelectedColumnChangedEventArgs args);
}
