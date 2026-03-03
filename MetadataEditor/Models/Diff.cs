using Prism.Mvvm;

namespace MetadataEditor.Models
{
    public class Diff : BindableBase
    {
        private string key = string.Empty;
        private string value = string.Empty;
        private bool enabled = true;

        public string Key { get => key; set => SetProperty(ref key, value); }

        public string Value { get => value; set => SetProperty(ref this.value, value); }

        public bool Enabled { get => enabled; set => SetProperty(ref enabled, value); }
    }
}