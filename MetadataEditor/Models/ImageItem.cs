using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

            // Load image into memory without locking the file
            ImageSource = LoadBitmapImage(path);

            MetadataText = PngMetadataReader.ReadPngTextMetadata(path);
        }

        public string FullPath { get; set; }

        public string FileName { get; }

        public ImageSource ImageSource { get; set; }

        public string MetadataText { get => metadataText; set => SetProperty(ref metadataText, value); }

        private static ImageSource LoadBitmapImage(string path)
        {
            // Use FileShare.ReadWrite and BitmapCacheOption.OnLoad to avoid locking the file
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad; // fully load into memory
            bitmap.StreamSource = fs;
            bitmap.EndInit();
            bitmap.Freeze(); // make it cross-thread accessible and immutable
            return bitmap;
        }
    }
}