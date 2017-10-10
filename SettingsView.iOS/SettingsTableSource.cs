using System;
using AiForms.Renderers.iOS.Extensions;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AiForms.Renderers.iOS
{
    public class SettingsTableSource : UITableViewSource
    {
        UITableView _tableView;
        SettingsView _settingsView;
        PickerTableViewController _pickerVC;

        bool _disposed;

        public SettingsTableSource(SettingsView settingsView)
        {
            _settingsView = settingsView;
            _settingsView.ModelChanged += (sender, e) => {
                if (_tableView != null) {
                    _tableView.ReloadData();
                }
            };

        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {

            //FormsCell取得
            var cell = _settingsView.Model.GetCell(indexPath.Section, indexPath.Row);

            var id = cell.GetType().FullName;

            var renderer = (CellRenderer)Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IRegisterable>(cell.GetType());

            //リサイクルCell取得
            var reusableCell = tableView.DequeueReusableCell(id);
            //NativeCell取得
            var nativeCell = renderer.GetCell(cell, reusableCell, tableView);

            var cellWithContent = nativeCell;

            // Sometimes iOS for returns a dequeued cell whose Layer is hidden. 
            // This prevents it from showing up, so lets turn it back on!
            if (cellWithContent.Layer.Hidden)
                cellWithContent.Layer.Hidden = false;

            // Because the layer was hidden we need to layout the cell by hand
            if (cellWithContent != null)
                cellWithContent.LayoutSubviews();

            //選択時背景色
            if (!(nativeCell is CellBaseView)) {
                nativeCell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }

            return nativeCell;
        }

        //行の高さ
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (!_settingsView.HasUnevenRows) {
                return tableView.EstimatedRowHeight;
            }

            var cell = _settingsView.Model.GetCell(indexPath.Section, indexPath.Row);
            var h = cell.Height;

            if (h == -1) {
                //高さ自動
                return tableView.RowHeight;
            }

            //個別の高さが指定されていたらその高さにする
            return (nfloat)h;
        }


        //ヘッダーの高さ
        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            if (_settingsView.HeaderHeight == -1d) {
                return _tableView.EstimatedSectionHeaderHeight;
            }

            return (nfloat)_settingsView.HeaderHeight;
        }

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

        //フッターの高さ
        public override nfloat GetHeightForFooter(UITableView tableView, nint section)
        {
            var footerText = _settingsView.Model.GetFooterText((int)section);

            if (string.IsNullOrEmpty(footerText)) {
                //テキスト未指定だったら非表示にする
                return nfloat.Epsilon;
            }

            return UITableView.AutomaticDimension;
        }

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

        //フッターのテキスト
        public override string TitleForFooter(UITableView tableView, nint section)
        {
            return _settingsView.Model.GetFooterText((int)section);
        }

        //セクションの数
        public override nint NumberOfSections(UITableView tableView)
        {
            _tableView = tableView;
            return _settingsView.Model.GetSectionCount();
        }

        //1セクションの行数
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _settingsView.Model.GetRowCount((int)section);
        }

        //セクションのタイトル文字列の配列（何のためか不明）
        public override string[] SectionIndexTitles(UITableView tableView)
        {
            return _settingsView.Model.GetSectionIndexTitles();
        }


        //セクションタイトル
        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return _settingsView.Model.GetSectionTitle((int)section);
        }

        //行が選択された時の処理
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {         
            var cell = tableView.CellAt(indexPath);

            _settingsView.Model.RowSelected(indexPath.Section,indexPath.Row);

            if (cell is CommandCellView) {
                var cmdCell = cell as CommandCellView;
                cmdCell?.Execute?.Invoke();
                if (!(cmdCell.Cell as CommandCell).KeepSelectedUntilBack){
                    tableView.DeselectRow(indexPath, true);
                }
            }
            else if(cell is PickerCellView){
                var pickerCell = (cell as PickerCellView).Cell as PickerCell;
                //TODO: これでどんなパターンでも大丈夫なのか要検証
                var naviCtrl = UIApplication.SharedApplication.KeyWindow.RootViewController.ChildViewControllers[0] as UINavigationController;

                _pickerVC?.Dispose();
                _pickerVC = new PickerTableViewController((PickerCellView)cell);
                naviCtrl.PushViewController(_pickerVC, true);

                if(!pickerCell.KeepSelectedUntilBack){
                    tableView.DeselectRow(indexPath, true);
                }
            }
            else if(cell is IPickerCell){
                tableView.DeselectRow(indexPath, true);
                var pCell = cell as IPickerCell;
                pCell.DummyField.BecomeFirstResponder();
            }
            else if(cell is EntryCellView){
                var eCell = cell as EntryCellView;
                eCell.ValueField.BecomeFirstResponder();
            }

        }


        protected override void Dispose(bool disposing)
        {
            if (!_disposed){
                _settingsView = null;
                _tableView = null;
                _pickerVC?.Dispose();
                _pickerVC = null;
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
