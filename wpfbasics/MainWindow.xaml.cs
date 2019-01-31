using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;


namespace WpfBasics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region On loaded

        /// <summary>
        /// When the application first opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var drive in Directory.GetLogicalDrives())
            {
                var item = new TreeViewItem();
                item.Header = drive;
                item.Tag = drive;

                item.Items.Add(null);

                item.Expanded += Folder_Expanded;

                Folderview.Items.Add(item);
            }
        }

        #region Folder Expansion

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;

            if (item.Items.Count != 1 || item.Items[0] != null)
            {
                return;
            }

            //clear dummy data
            item.Items.Clear();

            // get full path 
            var fullPath = (string)item.Tag;

            #region Get Folders
            var directories = new List<string>();

            try
            {
                var dirs = Directory.GetDirectories(fullPath);

                if (dirs.Length > 0)
                {
                    directories.AddRange(dirs);
                }
            }
            catch (Exception)
            {
                throw;
            }

            directories.ForEach(directoryPath => {
                var subItem = new TreeViewItem();

                subItem.Header = Path.GetFileName(directoryPath);
                subItem.Tag = directoryPath;
                

                subItem.Items.Add(null);

                subItem.Expanded += Folder_Expanded;

                item.Items.Add(subItem);
            });
            #endregion

            #region Get files

            var files = new List<string>();

            try
            {
                var fs = Directory.GetFiles(fullPath);

                if (fs.Length > 0)
                {
                    files.AddRange(fs);
                }
            }
            catch (Exception)
            {

                throw;
            }

            files.ForEach(filePath => {
                var subItem = new TreeViewItem()
                {
                    Header = Path.GetFileName(filePath),
                    Tag = filePath
                };

                item.Items.Add(subItem);
            });

            #endregion
        }

        #endregion


        #endregion

        public static string GetFilesFoldersName(string path)
        {
            if(string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            var nomarlizedPath = path.Replace("/", "\\");

            var lastIndex = nomarlizedPath.LastIndexOf("\\");

            if (lastIndex <= 0)
            {
                return path;
            }

            return path.Substring(lastIndex + 1);
        }
    }
}
