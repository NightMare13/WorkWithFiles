using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WorkWithFiles.Model
{
    public static class HelpClass
    {
        public static async Task<FileModel> GetCatalog(string fullpath = "")
        {
            var rootDirectory = new FileModel()
            {
                Name = fullpath
            };

            if (fullpath == "")
            {
                rootDirectory = new FileModel()
                {
                    Name = "Empty..."
                };
            }

            await Task.Run(() =>
            {
                if (fullpath != "")
                {
                    var directoryInfo = new DirectoryInfo(fullpath);
                    RunThroughAllDirectories(directoryInfo, rootDirectory);
                }
            });


            return rootDirectory;
        }

        private static void RunThroughAllDirectories(DirectoryInfo directoryInfo,
            FileModel directory)
        {

            try
            {
                var dirs = from dir in directoryInfo.EnumerateDirectories()
                           select new
                           {
                               ProgDir = dir,
                           };
                foreach (var di in dirs)
                {
                    var newFolder = new FileModel { Name = di.ProgDir.Name, FullPath = di.ProgDir.FullName };
                    directory.Files.Add(newFolder);
                    int positionOfFolderToFill = directory.Files.IndexOf(newFolder);
                    RunThroughAllDirectories(di.ProgDir, directory.Files[positionOfFolderToFill]);
                }
                var files = from file in directoryInfo.EnumerateFiles()
                            select new
                            {
                                ProgFile = file,
                            };
                foreach (var f in files)
                {
                    directory.Files.Add(new FileModel() { Name = f.ProgFile.Name, FullPath = f.ProgFile.FullName });
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("UnauthorizedAccessException");
            }
        }
    }
}
