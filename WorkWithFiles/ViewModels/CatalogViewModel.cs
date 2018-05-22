using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithFiles.Model;
using WorkWithFiles.Utility;

namespace WorkWithFiles.ViewModels
{
    public class CatalogViewModel : BindableBase
    {
        #region Data

        private Collection<CatalogViewModel> _subDirectories;
        private CatalogViewModel _previousDirectory;
        private FileModel _myDirectory;
        private bool _isExpanded;
        private bool _isSelected;

        #endregion //Data


        #region Constructors

        public CatalogViewModel(FileModel myDirectory)
            : this(myDirectory, null)
        {

        }

        private CatalogViewModel(FileModel myDirectory, CatalogViewModel previousDirectory)
        {
            _myDirectory = myDirectory;
            _previousDirectory = previousDirectory;

            _subDirectories = new Collection<CatalogViewModel>(
            (from dir in _myDirectory.Files
             select new CatalogViewModel(dir, this))
            .ToList());

        }
        #endregion //Constructors

        #region MyDirectory Properties

        public Collection<CatalogViewModel> SubDirectories
        {
            get => _subDirectories;

            set
            {
                _subDirectories = value;
                this.OnPropertyChanged();
            }

        }

        public CatalogViewModel PreviousDirectory
        {
            get => _previousDirectory;

            set
            {
                _previousDirectory = value;
                this.OnPropertyChanged();
            }

        }

        public string FullPath { get => _myDirectory.FullPath; set => _myDirectory.FullPath = value; }


        public string Name
        {
            get => _myDirectory.Name;
            set
            {
                _myDirectory.Name = value;
                this.OnPropertyChanged();
            }
        }

        #region IsExpanded

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (_isExpanded && _previousDirectory != null)
                    _previousDirectory.IsExpanded = true;
            }
        }

        #endregion // IsExpanded

        #region IsSelected

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        #endregion // IsSelected

        #endregion // MyDirectory Properties
    }
}
