using System.IO;
using System.Windows.Media;
using MetadataEditor.Core;

namespace MetadataEditor.Models
{
    public class ImageItem
    {
        public ImageItem(string path)
        {
            FullPath = path;
            FileName = Path.GetFileName(path);

            MetadataText = PngMetadataReader.ReadPngTextMetadata(path);
        }

        public string FullPath { get; set; }

        public string FileName { get; }

        public ImageSource ImageSource { get; set; }

        public string MetadataText { get; }
    }
}