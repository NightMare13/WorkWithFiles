using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.TeamFoundation.MVVM;
using MvvmDialogs;
using WorkWithFiles.Utility;

namespace WorkWithFiles.ViewModels
{
    class CreateDialogViewModel : BindableBase, IModalDialogViewModel
    {
        private string text;
        private bool? dialogResult;

        public CreateDialogViewModel()
        {
            OkCommand = new RelayCommand(Ok);
        }

        public string Text
        {
            get => text;
            set => UpdateValue(value, ref text);
        }

        public ICommand OkCommand { get; }

        public bool? DialogResult
        {
            get => dialogResult;
            private set => UpdateValue(value, ref dialogResult);
        }

        private void Ok()
        {
            if (!string.IsNullOrEmpty(Text))
            {
                DialogResult = true;
            }
        }
    }
}
