using MetadataEditor.Utils;
using Prism.Mvvm;

namespace MetadataEditor.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly AppVersionInfo appVersionInfo = new();

    public string Title => appVersionInfo.Title;
}