using System;
using System.Linq;
using AiForms.Renderers.iOS.Extensions;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AiForms.Renderers.iOS
{
    /// <summary>
    /// Settings table source.
    /// </summary>
    [Foundation.Preserve(AllMembers = true)]
    public class SettingsTableSource : UITableViewSource
    {
        /// <summary>
        /// The table view.
        /// </summary>
        protected UITableView _tableView;
        /// <summary>
        /// The settings view.
        /// </summary>
        protected SettingsView _settingsView;

        bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.SettingsTableSource"/> class.
        /// </summary>
        /// <param name="settingsView">Settings view.</param>
        public SettingsTableSource(SettingsView settingsView)
        {
            _settingsView = settingsView;
            _settingsView.ModelChanged += (sender, e) => {
                if (_tableView != null) {
                    _tableView.ReloadData();
                }
            };

        }

        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {

            //get forms cell
            var cell = _settingsView.Model.GetCell(indexPath.Section, indexPath.Row);

            var id = cell.GetType().FullName;

            var renderer = (CellRenderer)Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IRegisterable>(cell.GetType());

            //get recycle cell
            var reusableCell = tableView.DequeueReusableCell(id);
            //get native cell
            var nativeCell = renderer.GetCell(cell, reusableCell, tableView);

            var cellWithContent = nativeCell;

            // Sometimes iOS for returns a dequeued cell whose Layer is hidden. 
            // This prevents it from showing up, so lets turn it back on!
            if (cellWithContent.Layer.Hidden)
                cellWithContent.Layer.Hidden = false;

            // Because the layer was hidden we need to layout the cell by hand
            if (cellWithContent != null)
                cellWithContent.LayoutSubviews();

            //selected background
            if (!(nativeCell is CellBaseView)) {
                nativeCell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }

            return nativeCell;
        }

        /// <summary>
        /// Gets the height for row.
        /// </summary>
        /// <returns>The height for row.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (!_settingsView.HasUnevenRows) {
                return tableView.EstimatedRowHeight;
            }

            var cell = _settingsView.Model.GetCell(indexPath.Section, indexPath.Row);
            var h = cell.Height;

            if (h == -1) {
                //automatic height
                return tableView.RowHeight;
            }

            //individual height
            return (nfloat)h;
        }

        /// <summary>
        /// section header height
        /// </summary>
        /// <returns>The height for header.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="section">Section.</param>
        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            var individualHeight = _settingsView.Model.GetHeaderHeight((int)section);

            if(individualHeight > 0d){
                return (nfloat)individualHeight;
            }
            if (_settingsView.HeaderHeight == -1d) {
                return _tableView.EstimatedSectionHeaderHeight;
            }

            return (nfloat)_settingsView.HeaderHeight;
        }

        /// <summary>
        /// Gets the view for header.
        /// </summary>
        /// <returns>The view for header.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="section">Section.</param>
        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var title = TitleForHeader(tableView, section);

            var container = new HeaderView(_settingsView.HeaderPadding.ToUIEdgeInsets(),
                                           _settingsView.HeaderTextVerticalAlign);

            container.BackgroundColor = _settingsView.HeaderBackgroundColor.ToUIColor();

            var label = container.Label;

            label.Text = title;
            label.TextColor = _settingsView.HeaderTextColor == Color.Default ?
                UIColor.Gray : _settingsView.HeaderTextColor.ToUIColor();
            label.Font = UIFont.SystemFontOfSize((nfloat)_settingsView.HeaderFontSize);

            return container;
        }

        /// <summary>
        /// section footer height
        /// </summary>
        /// <returns>The height for footer.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="section">Section.</param>
        public override nfloat GetHeightForFooter(UITableView tableView, nint section)
        {
            var footerText = _settingsView.Model.GetFooterText((int)section);

            if (string.IsNullOrEmpty(footerText)) {
                //hide footer
                return nfloat.Epsilon; // must not zero
            }

            return UITableView.AutomaticDimension;
        }

        /// <summary>
        /// Gets the view for footer.
        /// </summary>
        /// <returns>The view for footer.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="section">Section.</param>
        public override UIView GetViewForFooter(UITableView tableView, nint section)
        {
            var text = TitleForFooter(tableView, section);

            if (string.IsNullOrEmpty(text)) {
                return new UIView(CGRect.Empty);
            }

            var container = new FooterView(_settingsView.FooterPadding.ToUIEdgeInsets());
            container.BackgroundColor = _settingsView.FooterBackgroundColor.ToUIColor();

            var label = container.Label;

            label.Text = text;
            label.TextColor = _settingsView.FooterTextColor == Color.Default ?
                UIColor.Gray : _settingsView.FooterTextColor.ToUIColor();
            label.Font = UIFont.SystemFontOfSize((nfloat)_settingsView.FooterFontSize);

            return container;
        }

        /// <summary>
        /// section footer text
        /// </summary>
        /// <returns>The for footer.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="section">Section.</param>
        public override string TitleForFooter(UITableView tableView, nint section)
        {
            return _settingsView.Model.GetFooterText((int)section);
        }

        /// <summary>
        /// Numbers the of sections.
        /// </summary>
        /// <returns>The of sections.</returns>
        /// <param name="tableView">Table view.</param>
        public override nint NumberOfSections(UITableView tableView)
        {
            _tableView = tableView;
            return _settingsView.Model.GetSectionCount();
        }

        /// <summary>
        /// Rowses the in section.
        /// </summary>
        /// <returns>The in section.</returns>
        /// <param name="tableview">Tableview.</param>
        /// <param name="section">Section.</param>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _settingsView.Model.GetRowCount((int)section);
        }

        /// <summary>
        /// Title text string array (unknown what to do ) 
        /// </summary>
        /// <returns>The index titles.</returns>
        /// <param name="tableView">Table view.</param>
        public override string[] SectionIndexTitles(UITableView tableView)
        {
            return _settingsView.Model.GetSectionIndexTitles();
        }

        /// <summary>
        /// section header title
        /// </summary>
        /// <returns>The for header.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="section">Section.</param>
        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return _settingsView.Model.GetSectionTitle((int)section);
        }

      
        /// <summary>
        /// processing when row is selected.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {         
            _settingsView.Model.RowSelected(indexPath.Section,indexPath.Row);

            var cell = tableView.CellAt(indexPath) as CellBaseView;
            if(cell == null)
            {
                return;
            }

            cell.RowSelected(tableView, indexPath);
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed){
                _settingsView = null;
                _tableView = null;               
            }

            _disposed = true;

            base.Dispose(disposing);
        }

        class HeaderView : UIView
        {
            public UILabel Label { get; set; }


            public HeaderView(UIEdgeInsets padding, LayoutAlignment align)
            {
                Label = new UILabel();
                Label.Lines = 1;
                Label.LineBreakMode = UILineBreakMode.TailTruncation;
                Label.TranslatesAutoresizingMaskIntoConstraints = false;

                this.AddSubview(Label);

                Label.LeftAnchor.ConstraintEqualTo(this.LeftAnchor, padding.Left).Active = true;
                Label.RightAnchor.ConstraintEqualTo(this.RightAnchor, -padding.Right).Active = true;

                if (align == LayoutAlignment.Start) {
                    Label.TopAnchor.ConstraintEqualTo(this.TopAnchor, padding.Top).Active = true;
                }
                else if (align == LayoutAlignment.End) {
                    Label.BottomAnchor.ConstraintEqualTo(this.BottomAnchor, -padding.Bottom).Active = true;
                }
                else {
                    Label.CenterYAnchor.ConstraintEqualTo(this.CenterYAnchor, 0).Active = true;
                }

            }
        }

        class FooterView : UIView
        {
            public UILabel Label { get; set; }

            public FooterView(UIEdgeInsets padding)
            {
                Label = new UILabel();
                Label.Lines = 0;
                Label.LineBreakMode = UILineBreakMode.WordWrap;
                Label.TranslatesAutoresizingMaskIntoConstraints = false;

                this.AddSubview(Label);

                Label.TopAnchor.ConstraintEqualTo(this.TopAnchor, padding.Top).Active = true;
                Label.LeftAnchor.ConstraintEqualTo(this.LeftAnchor, padding.Left).Active = true;
                Label.RightAnchor.ConstraintEqualTo(this.RightAnchor, -padding.Right).Active = true;
                Label.BottomAnchor.ConstraintEqualTo(this.BottomAnchor, -padding.Bottom).Active = true;
            }
        }
    }
}
