using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MvvmDialogs;
using System.Windows.Controls;


namespace WorkWithFiles.Model
{
    public class CreateCustomDialog : IWindow
    {
        private readonly CreateCustomDialog dialog;

        public CreateCustomDialog()
        {
            dialog = new CreateCustomDialog();
        }

        public object DataContext
        {
            get => dialog.DataContext;
            set => dialog.DataContext = value;
        }

        public bool? DialogResult
        {
            get => dialog.DialogResult;
            set => dialog.DialogResult = value;
        }

        public ContentControl Owner
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
