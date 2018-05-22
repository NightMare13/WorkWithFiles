using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using WorkWithFiles.Model;
using WorkWithFiles.Utility;

namespace WorkWithFiles.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Data

        private Collection<CatalogViewModel> _catalog; //full catalog

        private CatalogViewModel _rootDirectory;

        // private readonly IDialogService dialogService;
        private FileInfo _selectedFile;

        #endregion //Data

        #region Properties

        #region Catalog

        public Collection<CatalogViewModel> Catalog
        {
            get => _catalog;
            set => UpdateValue(value, ref _catalog);
        }

        #endregion // Catalog

        #endregion


        #region Constructors

        public MainWindowViewModel(FileModel rootMyDirectory /*, IDialogService dialogService*/)
        {
            // this.dialogService = dialogService;

            _rootDirectory = new CatalogViewModel(rootMyDirectory);

            _catalog = new ObservableCollection<CatalogViewModel>(
                new CatalogViewModel[]
                {
                    _rootDirectory
                });
        }

        #endregion //Constructors

        #region Commands

        #region OpenDialogCommand

        private ICommand _openDialogCommand;

        public ICommand OpenDialogCommand
        {
            get
            {
                if (_openDialogCommand == null)
                    _openDialogCommand = new RelayCommand(OpenDialog);
                return _openDialogCommand;
            }
        }

        private async void OpenDialog()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            _rootDirectory = new CatalogViewModel(new FileModel() {Name = "Loading..."});

            Catalog = new ObservableCollection<CatalogViewModel>(
                new CatalogViewModel[]
                {
                    _rootDirectory
                });


            if (result == System.Windows.Forms.DialogResult.OK)
            {
                var rootMyDirectory = await HelpClass.GetCatalog(dialog.SelectedPath);

                _rootDirectory = new CatalogViewModel(rootMyDirectory);

                Catalog = new ObservableCollection<CatalogViewModel>(
                    new CatalogViewModel[]
                    {
                        _rootDirectory
                    });
            }
        }

        #endregion //Open Dialog Command


        #endregion //Commands
    }
}
