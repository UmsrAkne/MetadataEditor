using System.Collections.ObjectModel;
using MetadataEditor.Models;
using MetadataEditor.Utils;
using Prism.Mvvm;

namespace MetadataEditor.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly AppVersionInfo appVersionInfo = new();
    private ImageItem selectedImageItem;

    public string Title => appVersionInfo.Title;

    public ObservableCollection<ImageItem> ImageItems { get; set; } = new ();

    public ImageItem SelectedImageItem
    {
        get => selectedImageItem;
        set => SetProperty(ref selectedImageItem, value);
    }

    public void Add(string path)
    {
        ImageItems.Add(new ImageItem(path));
    }
}