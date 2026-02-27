using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace MetadataEditor.Behaviors
{
    public class ScrollSelectedItemIntoViewBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
            base.OnDetaching();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = AssociatedObject;
            if (listBox?.SelectedItem == null)
            {
                return;
            }

            // 仮想化対策
            listBox.UpdateLayout();
            listBox.ScrollIntoView(listBox.SelectedItem);
        }
    }
}