using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace WpfBasics
{
    /// <summary>
    /// A viewmodel for each DirectoryItem 
    /// </summary>
    public class DirectoryItemViewModel : BaseViewModel
    {
        public DirectoryItemType Type { get; set; }

        public string FullPath { get; set; }

        public string Name
        {
            get
            {
                return this.Type == DirectoryItemType.Drive ? this.FullPath : DirectoryStructure.GetFilesFoldersName(this.FullPath);
            }
        }


        /// <summary>
        /// This item's children in a list
        /// </summary>
        public ObservableCollection<DirectoryItemViewModel> Children { get; set; }

        public ICommand ExpandCommand { get; set; }

        public DirectoryItemViewModel(string fullPath, DirectoryItemType type)
        {
            this.ExpandCommand = new RelayCommand(Expand);

            this.FullPath = fullPath;
            this.Type = type;

            this.ClearChildren();
        }

        /// <summary>
        /// Set flag depending on wether or not the item is expandable   
        /// </summary>
        public bool CanExpand { get { return this.Type != DirectoryItemType.File; } }

        public bool IsExpanded
        {
            get
            {
                return this.Children?.Count(x => x != null) > 0;
            }

            set
            {
                //If the UI tells us to expand, then go and find all children
                if (value == true)
                {
                    Expand();
                }
                //If the UI tells us to close the folder or drive, get rid of the children for that item
                else
                {
                    ClearChildren();
                }
            }
        }

        private void Expand()
        {
            if (this.Type == DirectoryItemType.File)
            {
                return;
            }
            //Find all children for this item

            var children = DirectoryStructure.GetDirectoryContents(this.FullPath);
            this.Children = new ObservableCollection<DirectoryItemViewModel>(children.Select(x => new DirectoryItemViewModel(x.FullPath, x.Type)));
        }

        private void ClearChildren()
        {
            this.Children = new ObservableCollection<DirectoryItemViewModel>();

            if (this.Type != DirectoryItemType.File)
            {
                this.Children.Add(null);
            }
        }
    }
}
