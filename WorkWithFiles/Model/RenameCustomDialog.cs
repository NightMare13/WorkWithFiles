using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MvvmDialogs;

namespace WorkWithFiles.Model
{
    public class RenameCustomDialog : IWindow
    {
        private readonly RenameDialog dialog;

        public RenameCustomDialog()
        {
            dialog = new RenameDialog();
        }

        object IWindow.DataContext
        {
            get => dialog.DataContext;
            set => dialog.DataContext = value;
        }
        bool? IWindow.DialogResult
        {
            get => dialog.DialogResult;
            set => dialog.DialogResult = value;
        }

        ContentControl IWindow.Owner
        {
            get => dialog.Owner;
            set => dialog.Owner = (Window)value;
        }
        public bool? ShowDialog()
        {
            return dialog.ShowDialog();
        }

        public void Show()
        {
            dialog.Show();
        }
    }
}
