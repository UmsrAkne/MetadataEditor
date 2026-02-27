using System.IO;
using System.Windows.Media;

namespace MetadataEditor.Models
{
    public class ImageItem
    {
        public ImageItem(string path)
        {
            FullPath = path;
            FileName = Path.GetFileName(path);
        }

        public string FullPath { get; set; }

        public string FileName { get; }

        public ImageSource ImageSource { get; set; }

        public string MetadataText { get; }
    }
}