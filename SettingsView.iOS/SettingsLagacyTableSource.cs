using System;
using Foundation;
using UIKit;
using CoreGraphics;

namespace AiForms.Renderers.iOS
{
    /// <summary>
    /// Settings lagacy table source.
    /// </summary>
    public class SettingsLagacyTableSource : SettingsTableSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.SettingsLagacyTableSource"/> class.
        /// </summary>
        /// <param name="settingsView">Settings view.</param>
        public SettingsLagacyTableSource(SettingsView settingsView) : base(settingsView)
        {
        }

        /// <summary>
        /// Cans the move row.
        /// </summary>
        /// <returns><c>true</c>, if move row was caned, <c>false</c> otherwise.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override bool CanMoveRow(UITableView tableView, NSIndexPath indexPath)
        {
            var section = _settingsView.Model.GetSection(indexPath.Section);
            return section.UseDragSort;
        }

        /// <summary>
        /// Moves the row.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="sourceIndexPath">Source index path.</param>
        /// <param name="destinationIndexPath">Destination index path.</param>
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

        /// <summary>
        /// Editings the style for row.
        /// </summary>
        /// <returns>The style for row.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableViewCellEditingStyle.None;
        }

        /// <summary>
        /// Shoulds the indent while editing.
        /// </summary>
        /// <returns><c>true</c>, if indent while editing was shoulded, <c>false</c> otherwise.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override bool ShouldIndentWhileEditing(UITableView tableView, NSIndexPath indexPath)
        {
            return false;
        }
    }
}
