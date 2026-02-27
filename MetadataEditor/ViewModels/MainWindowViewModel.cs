using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using MetadataEditor.Core;
using MetadataEditor.Models;
using MetadataEditor.Utils;
using Prism.Commands;
using Prism.Mvvm;

namespace MetadataEditor.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly AppVersionInfo appVersionInfo = new();
    private ImageItem selectedImageItem;

    public MainWindowViewModel()
    {
        AddDummy();
    }

    public string Title => appVersionInfo.Title;

    public ObservableCollection<ImageItem> ImageItems { get; set; } = new ();

    public ImageItem SelectedImageItem
    {
        get => selectedImageItem;
        set => SetProperty(ref selectedImageItem, value);
    }

    public DelegateCommand SaveMetadataCommand => new DelegateCommand(() =>
    {
        if (SelectedImageItem == null)
        {
            return;
        }

        MetadataWriter.Write(SelectedImageItem.FullPath, SelectedImageItem.MetadataText);
    });

    public DelegateCommand CopyToClipboardCommand => new DelegateCommand(() =>
    {
        if (SelectedImageItem == null)
        {
            return;
        }

        string[] fileNames = { SelectedImageItem.FullPath, };
        var data = new DataObject(DataFormats.FileDrop, fileNames);
        Clipboard.SetDataObject(data);
    });

    public void Add(string path)
    {
        ImageItems.Add(new ImageItem(path));
    }

    [Conditional("DEBUG")]
    private void AddDummy()
    {
        var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var testDataPath = "myFiles\\Tests\\RiderProjects\\MetadataEditor\\test1";
        var path = Path.Combine(desktop, testDataPath);
        var files = Directory.GetFiles(path, "*.png");
        foreach (var file in files)
        {
            Add(file);
        }
    }
}