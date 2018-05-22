using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.TeamFoundation.MVVM;
using WorkWithFiles.Model;
using WorkWithFiles.Utility;
using IDialogService = MvvmDialogs.IDialogService;

namespace WorkWithFiles.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Data

        private Collection<CatalogViewModel> _catalog; //full catalog

        private CatalogViewModel _rootDirectory;

        private readonly IDialogService dialogService;
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

        public MainWindowViewModel(FileModel rootMyDirectory , IDialogService dialogService)
        {
             this.dialogService = dialogService;

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

        #region SelectedItem Command

        private ICommand _selectItemCommand;

        public ICommand SelectItemCommand
        {
            get
            {
                if (_selectItemCommand == null)
                    _selectItemCommand = new RelayCommand(args => SelectedItemChanged(args));
                return _selectItemCommand;
            }
        }

        private void SelectedItemChanged(object args)
        {
            var a = args as CatalogViewModel;
            if (a != null)
            {
                try
                {
                    var f = new FileInfo(a.FullPath);
                    if (f.Exists)
                    {
                        _selectedFile = f;
                    }
                    var d = new DirectoryInfo(a.FullPath);
                }
                catch (Exception e)
                {
                    //invalid path
                }
            }
        }

        #endregion

        #region MouseDoubleClick Command

        private ICommand _mouseDoubleClickCommand;

        public ICommand MouseDoubleClickCommand
        {
            get
            {
                if (_mouseDoubleClickCommand == null)
                    _mouseDoubleClickCommand = new RelayCommand(MouseDoubleClick);
                return _mouseDoubleClickCommand;
            }
        }

        private void MouseDoubleClick()
        {

            try
            {
                Process.Start(_selectedFile.FullName);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Folder!");
            }

        }

        #endregion //MouseDoubleClick Command

        #region RightButtonClickCommand

        private ICommand _rightButtonClickCommand;

        public ICommand RightButtonClickCommand
        {
            get
            {
                if (_rightButtonClickCommand == null)
                    _rightButtonClickCommand = new RelayCommand((arg) => RightButtonClick(arg));
                return _rightButtonClickCommand;
            }
        }


        private void RightButtonClick(object arg)
        {
            var treeView = arg as TreeView;

            //MessageBox.Show(treeView.SelectedItem.ToString());

        }

        #endregion

        #region Copy  and Past Command

        private string fileName;

        private string sourcePath;
        private string targetPath;

        private string sourceFile;
        private string destFile;

        private ICommand _copyCommand;

        public ICommand CopyCommand
        {
            get
            {
                if (_copyCommand == null)
                    _copyCommand = new RelayCommand((arg) => CopyFileOrDirectory(arg));
                return _copyCommand;
            }
        }

        private void CopyFileOrDirectory(object arg)
        {
            var selectedFile = arg as CatalogViewModel;
            if (selectedFile != null)
            {
                selectedFile.IsSelected = true;
                fileName = selectedFile.Name;
                sourceFile = selectedFile.FullPath;
                sourcePath = sourceFile.Replace("\\" + selectedFile.Name, "");

            }
        }

        private ICommand _pasteCommand;

        public ICommand PasteCommand
        {
            get
            {
                if (_pasteCommand == null)
                    _pasteCommand = new RelayCommand((arg) => PasteFileOrDirectory(arg));
                return _pasteCommand;
            }
        }

        private async void PasteFileOrDirectory(object arg)
        {
            var selectedFile = arg as CatalogViewModel;
            selectedFile.IsSelected = true;
            destFile = selectedFile.FullPath + "\\" + fileName;
            targetPath = destFile.Replace("\\" + fileName, "");
            //если вставлять папку с именем совпадающим с именем папки в полном пути к месту назначения то они оба удаляются


            if (File.Exists(sourceFile))
            {
                System.IO.File.Copy(sourceFile, destFile, true);
                var list = new Collection<CatalogViewModel>(selectedFile.SubDirectories);
                list.Add(new CatalogViewModel(new FileModel() { Name = fileName, FullPath = targetPath }));

                selectedFile.SubDirectories = list;
            }
            if (System.IO.Directory.Exists(sourceFile))
            {
                Directory.CreateDirectory(destFile);
                CopyAllDirectories(sourceFile, destFile);
                var result = await HelpClass.GetCatalog(targetPath);
                selectedFile.SubDirectories = new Collection<CatalogViewModel>(
                    (from dir in result.Files
                     select new CatalogViewModel(dir))
                    .ToList());
            }

        }

        private void CopyAllDirectories(string dir, string targPath)
        {
            string[] files = System.IO.Directory.GetFiles(dir);
            var dirs = Directory.GetDirectories(dir);

            foreach (string s in files)
            {
                fileName = System.IO.Path.GetFileName(s);
                destFile = System.IO.Path.Combine(targPath, fileName);
                System.IO.File.Copy(s, destFile, true);
            }
            foreach (var d in dirs)
            {
                fileName = System.IO.Path.GetFileName(d);
                destFile = System.IO.Path.Combine(targPath, fileName);
                Directory.CreateDirectory(destFile);
                CopyAllDirectories(d, destFile);
            }
        }

        #endregion

        #region Delete Command

        private ICommand _deleteCommand;

        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand((arg) => DeleteAllFilesAndDirs(arg));
                return _deleteCommand;
            }
        }

        private void DeleteAllFilesAndDirs(object arg)
        {
            var selectedFile = arg as CatalogViewModel;

            if (File.Exists(selectedFile.FullPath))
            {
                try
                {
                    DeletDirFromCatalog(Catalog, selectedFile);
                    Catalog = new Collection<CatalogViewModel>(_catalog);
                    File.Delete(selectedFile.FullPath);

                }
                catch (IOException e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
            }

            try
            {
                DeletDirFromCatalog(Catalog, selectedFile);
                Catalog = new Collection<CatalogViewModel>(_catalog);
                System.IO.Directory.Delete(selectedFile.FullPath);
                //Catalog.Remove(selectedFile);
            }
            catch (System.IO.IOException e)
            {
                //MessageBox.Show(e.Message);
                // вывести сообщение о том что папка имеет содержимое, всё равно удалить?
            }
            // Delete a directory and all subdirectories with Directory static method...
            if (System.IO.Directory.Exists(selectedFile.FullPath))
            {
                try
                {
                    DeletDirFromCatalog(Catalog, selectedFile);
                    Catalog = new Collection<CatalogViewModel>(_catalog);
                    System.IO.Directory.Delete(selectedFile.FullPath, true);
                    //Catalog.Remove(selectedFile);

                }

                catch (System.IO.IOException e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
            }
        }

        private bool DeletDirFromCatalog(Collection<CatalogViewModel> catalog, CatalogViewModel item)
        {
            foreach (var element in catalog)
            {
                if (element.FullPath == item.FullPath)
                {
                    //сделать проверку на то что этот файл именно по тому пути что мне нужно
                    catalog.Remove(element);
                    return true;
                }
            }
            foreach (var catalogViewModel in catalog)
            {
                if (catalogViewModel.SubDirectories.Count != 0)
                {
                    if (DeletDirFromCatalog(catalogViewModel.SubDirectories, item))
                        return true;
                }
            }
            return false;
        }

        #endregion

        #region CustomDialogs

        #region CreateDialog

        private ICommand _createDialogCommand;

        
        private void CreateFile(object arg)
        {
            var ob = arg as CatalogViewModel;
            ob.IsSelected = true;
            ShowCreateDialog(viewModel => dialogService.ShowCustomDialog<CreateDialog>(this, viewModel), ob);
        }

        private void ShowCreateDialog(Func<CreateDialogViewModel, bool?> showDialog, CatalogViewModel currentItem)
        {
            var dialogViewModel = new CreateDialogViewModel();

            bool? success = showDialog(dialogViewModel);
            if (success == true)
            {

                var dir = new DirectoryInfo(currentItem.FullPath);
                if (dir.Exists)
                {
                    Directory.Move(dir.FullName, dir.FullName.Replace(dir.Name, dialogViewModel.Text));
                    currentItem.Name = dialogViewModel.Text;
                }
                var f = new FileInfo(currentItem.FullPath);
                if (f.Exists)
                {
                    File.Move(f.FullName, f.FullName.Replace(f.Name, dialogViewModel.Text));
                    currentItem.Name = dialogViewModel.Text;
                }
            }
        }

        #endregion

        #region RenameDialog

        private ICommand _explicitShowDialogCommand;

        public ICommand ExplicitShowDialogCommand
        {
            get
            {
                if (_explicitShowDialogCommand == null)
                    _explicitShowDialogCommand = new RelayCommand((arg) => ExplicitShowDialog(arg));
                return _explicitShowDialogCommand;
            }
        }

        private void ExplicitShowDialog(object arg)
        {
            var ob = arg as CatalogViewModel;
            ob.IsSelected = true;
            ShowDialog(viewModel => dialogService.ShowCustomDialog<RenameCustomDialog>(this, viewModel), ob);
        }

        private void ShowDialog(Func<RenameCustomDialogViewModel, bool?> showDialog, CatalogViewModel currentItem)
        {
            var dialogViewModel = new RenameCustomDialogViewModel();

            bool? success = showDialog(dialogViewModel);
            if (success == true)
            {

                var dir = new DirectoryInfo(currentItem.FullPath);
                if (dir.Exists)
                {
                    Directory.Move(dir.FullName, dir.FullName.Replace(dir.Name, dialogViewModel.Text));
                    currentItem.Name = dialogViewModel.Text;
                }
                var f = new FileInfo(currentItem.FullPath);
                if (f.Exists)
                {
                    File.Move(f.FullName, f.FullName.Replace(f.Name, dialogViewModel.Text));
                    currentItem.Name = dialogViewModel.Text;
                }
            }
        }

        #endregion //Rename Dialog


        #endregion //CustomDialogs

        #endregion //Commands
    }
}
