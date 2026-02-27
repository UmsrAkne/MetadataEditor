using System.IO;
using System.Windows.Media;
using MetadataEditor.Core;
using Prism.Mvvm;

namespace MetadataEditor.Models
{
    public class ImageItem : BindableBase
    {
        private string metadataText;

        public ImageItem(string path)
        {
            FullPath = path;
            FileName = Path.GetFileName(path);

            MetadataText = PngMetadataReader.ReadPngTextMetadata(path);
        }

        public string FullPath { get; set; }

        public string FileName { get; }

        public ImageSource ImageSource { get; set; }

        public string MetadataText { get => metadataText; set => SetProperty(ref metadataText, value); }
    }
}