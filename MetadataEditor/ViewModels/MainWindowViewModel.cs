using System.Collections.ObjectModel;
using MetadataEditor.Utils;
using Prism.Mvvm;

namespace MetadataEditor.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly AppVersionInfo appVersionInfo = new();

    public string Title => appVersionInfo.Title;

    public ObservableCollection<string> Paths { get; set; } = new ();
}