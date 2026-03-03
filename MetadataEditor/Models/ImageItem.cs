using System.Collections.ObjectModel;
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
        private bool isModified;
        private string caption = string.Empty;

        public ImageItem(string path)
        {
            FullPath = path;
            FileName = Path.GetFileName(path);

            // Load image into memory without locking the file
            ImageSource = LoadBitmapImage(path);

            MetadataText = PngMetadataReader.ReadPngTextMetadata(path);
            IsModified = false;
        }

        public string FullPath { get; set; }

        public string FileName { get; }

        public ImageSource ImageSource { get; set; }

        public string MetadataText
        {
            get => metadataText;
            set
            {
                if (SetProperty(ref metadataText, value))
                {
                    IsModified = true;
                }
            }
        }

        public bool IsModified
        {
            get => isModified;
            private set => SetProperty(ref isModified, value);
        }

        public string Caption { get => caption; set => SetProperty(ref caption, value); }

        public ObservableCollection<Diff> Diffs { get; set; } = new ();

        public void MarkAsSaved()
        {
            IsModified = false;
        }

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