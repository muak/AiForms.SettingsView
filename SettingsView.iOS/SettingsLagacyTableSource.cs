using System;
using Foundation;
using UIKit;
using CoreGraphics;

namespace AiForms.Renderers.iOS
{
    public class SettingsLagacyTableSource : SettingsTableSource
    {
        public SettingsLagacyTableSource(SettingsView settingsView) : base(settingsView)
        {
        }

        public override bool CanMoveRow(UITableView tableView, NSIndexPath indexPath)
        {
            var section = _settingsView.Model.GetSection(indexPath.Section);
            return section.UseDragSort;
        }

        public override void MoveRow(UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
        {
            if(sourceIndexPath.Section != destinationIndexPath.Section){
                _tableView.ReloadData();
                return;
            }

            var section = _settingsView.Model.GetSection(sourceIndexPath.Section);

            if (section.ItemsSource == null)
            {
                var tmp = section[sourceIndexPath.Row];
                section.RemoveAt(sourceIndexPath.Row);
                section.Insert(destinationIndexPath.Row, tmp);
            }
            else
            {
                var tmp = section.ItemsSource[sourceIndexPath.Row];
                section.ItemsSource.RemoveAt(sourceIndexPath.Row);
                section.ItemsSource.Insert(destinationIndexPath.Row, tmp);
            }
        }

        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableViewCellEditingStyle.None;
        }

        public override bool ShouldIndentWhileEditing(UITableView tableView, NSIndexPath indexPath)
        {
            return false;
        }
    }
}
