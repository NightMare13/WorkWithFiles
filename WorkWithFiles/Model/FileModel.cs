using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithFiles.Model
{
    public class FileModel
    {
        public string Name { get; set; }

        public string FullPath { get; set; }

        private /*readonly*/ List<FileModel> _subFiles = new List<FileModel>();

        public IList<FileModel> Files => _subFiles;
    }
}
