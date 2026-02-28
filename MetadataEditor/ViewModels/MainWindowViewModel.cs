using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
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
    private string logMessage;

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

    public string LogMessage { get => logMessage; set => SetProperty(ref logMessage, value); }

    public DelegateCommand SaveMetadataCommand => new DelegateCommand(() =>
    {
        if (SelectedImageItem == null)
        {
            return;
        }

        MetadataWriter.Write(SelectedImageItem.FullPath, SelectedImageItem.MetadataText);
        SelectedImageItem.MarkAsSaved();
        WriteLog($"Saved metadata for '{SelectedImageItem.FullPath}'");
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
        WriteLog($"Copied '{SelectedImageItem.FullPath}' to clipboard");
    });

    // Paste image files from clipboard: create a non-conflicting copy next to the source and add to the list
    public DelegateCommand PasteFromClipboardCommand => new DelegateCommand(() =>
    {
        try
        {
            if (!Clipboard.ContainsFileDropList())
            {
                return;
            }

            var files = Clipboard.GetFileDropList();
            if (files.Count == 0)
            {
                return;
            }

            var imageExtensions = new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".tif", ".tiff", };
            var imageFiles = files.Cast<string>()
                .Where(f => !string.IsNullOrWhiteSpace(f))
                .Where(f => imageExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()))
                .ToList();

            foreach (var src in imageFiles)
            {
                try
                {
                    var dir = Path.GetDirectoryName(src);
                    var name = Path.GetFileNameWithoutExtension(src);
                    var ext = Path.GetExtension(src);

                    // Base new name: original and timestamp
                    var baseName = $"{name}_{DateTime.Now:MMdd_HHmmss_fff}";
                    var dest = Path.Combine(dir!, baseName + ext);

                    // Ensure uniqueness
                    var i = 1;
                    while (File.Exists(dest))
                    {
                        dest = Path.Combine(dir!, $"{baseName}_{i}{ext}");
                        i++;
                    }

                    File.Copy(src, dest);

                    // Add to a collection
                    Add(dest);
                }
                catch (Exception ex)
                {
                    LogWriter.Write($"PasteFromClipboard failed for '{src}': {ex.Message}");
                }
            }

            WriteLog($"Pasted {imageFiles.Count} image files from clipboard");
        }
        catch (Exception ex)
        {
            LogWriter.Write($"PasteFromClipboardCommand error: {ex.Message}");
        }
    });

    public void Add(string path)
    {
        ImageItems.Add(new ImageItem(path));
    }

    public void WriteLog(string text)
    {
        LogMessage = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss} {text}";
        LogWriter.Write(text);
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

        ImageItems[4].MetadataText = $"{DateTime.Now.ToString(CultureInfo.InvariantCulture)}";
    }
}