using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using CoreGraphics;

namespace AiForms.Renderers.iOS
{
    /// <summary>
    /// Cell base view.
    /// </summary>
    [Foundation.Preserve(AllMembers = true)]
    public class CellBaseView : CellTableViewCell
    {
        /// <summary>
        /// Gets the hint label.
        /// </summary>
        /// <value>The hint label.</value>
        public UILabel HintLabel { get; private set; }
        /// <summary>
        /// Gets the title label.
        /// </summary>
        /// <value>The title label.</value>
        public UILabel TitleLabel { get; private set; }
        /// <summary>
        /// Gets the description label.
        /// </summary>
        /// <value>The description label.</value>
        public UILabel DescriptionLabel { get; private set; }
        /// <summary>
        /// Gets the icon view.
        /// </summary>
        /// <value>The icon view.</value>
        public UIImageView IconView { get; private set; }

        /// <summary>
        /// Gets the cell base.
        /// </summary>
        /// <value>The cell base.</value>
        protected CellBase CellBase => Cell as CellBase;

        /// <summary>
        /// Gets the cell parent.
        /// </summary>
        /// <value>The cell parent.</value>
        public SettingsView CellParent => Cell.Parent as SettingsView;

        /// <summary>
        /// Gets the content stack.
        /// </summary>
        /// <value>The content stack.</value>
        protected UIStackView ContentStack { get; private set; }

        protected UIStackView StackH;
        protected UIStackView StackV;

        Size _iconSize;
        NSLayoutConstraint _iconConstraintHeight;
        NSLayoutConstraint _iconConstraintWidth;
        NSLayoutConstraint _minheightConstraint;
        CancellationTokenSource _iconTokenSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.iOS.CellBaseView"/> class.
        /// </summary>
        /// <param name="formsCell">Forms cell.</param>
        public CellBaseView(Cell formsCell) : base(UIKit.UITableViewCellStyle.Default, formsCell.GetType().FullName)
        {
            Cell = formsCell;

            SelectionStyle = UITableViewCellSelectionStyle.None;
            SetUpHintLabel();
            SetUpContentView();

            UpdateSelectedColor();
        }

        /// <summary>
        /// Cells the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public virtual void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CellBase.TitleProperty.PropertyName) {
                UpdateTitleText();
            }
            else if (e.PropertyName == CellBase.TitleColorProperty.PropertyName) {
                UpdateTitleColor();
            }
            else if (e.PropertyName == CellBase.TitleFontSizeProperty.PropertyName) {
                UpdateWithForceLayout(UpdateTitleFontSize);
            }
            else if (e.PropertyName == CellBase.DescriptionProperty.PropertyName) {
                UpdateWithForceLayout(UpdateDescriptionText);
            }
            else if (e.PropertyName == CellBase.DescriptionFontSizeProperty.PropertyName) {
                UpdateWithForceLayout(UpdateDescriptionFontSize);
            }
            else if (e.PropertyName == CellBase.DescriptionColorProperty.PropertyName) {
                UpdateDescriptionColor();
            }
            else if (e.PropertyName == CellBase.IconSourceProperty.PropertyName) {
                UpdateIcon();
            }
            else if (e.PropertyName == CellBase.BackgroundColorProperty.PropertyName) {
                UpdateBackgroundColor();
            }
            else if (e.PropertyName == CellBase.HintTextProperty.PropertyName) {
                UpdateWithForceLayout(UpdateHintText);
            }
            else if (e.PropertyName == CellBase.HintTextColorProperty.PropertyName) {
                UpdateHintTextColor();
            }
            else if (e.PropertyName == CellBase.HintFontSizeProperty.PropertyName) {
                UpdateWithForceLayout(UpdateHintFontSize);
            }
            else if (e.PropertyName == CellBase.IconSizeProperty.PropertyName) {
                UpdateWithForceLayout(UpdateIconSize);
            }
            else if (e.PropertyName == CellBase.IconRadiusProperty.PropertyName) {
                UpdateWithForceLayout(UpdateIconRadius);
            }
            else if(e.PropertyName == Cell.IsEnabledProperty.PropertyName){
                UpdateIsEnabled();
            }
        }

        /// <summary>
        /// Parents the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public virtual void ParentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == SettingsView.CellTitleColorProperty.PropertyName) {
                UpdateTitleColor();
            }
            else if (e.PropertyName == SettingsView.CellTitleFontSizeProperty.PropertyName) {
                UpdateWithForceLayout(UpdateTitleFontSize);
            }
            else if (e.PropertyName == SettingsView.CellDescriptionColorProperty.PropertyName) {
                UpdateDescriptionColor();
            }
            else if (e.PropertyName == SettingsView.CellDescriptionFontSizeProperty.PropertyName) {
                UpdateWithForceLayout(UpdateDescriptionFontSize);
            }
            else if (e.PropertyName == SettingsView.CellBackgroundColorProperty.PropertyName) {
                UpdateBackgroundColor();
            }
            else if (e.PropertyName == SettingsView.CellHintTextColorProperty.PropertyName) {
                UpdateHintTextColor();
            }
            else if (e.PropertyName == SettingsView.CellHintFontSizeProperty.PropertyName) {
                UpdateWithForceLayout(UpdateHintFontSize);
            }
            else if (e.PropertyName == SettingsView.CellIconSizeProperty.PropertyName) {
                UpdateWithForceLayout(UpdateIconSize);
            }
            else if (e.PropertyName == SettingsView.CellIconRadiusProperty.PropertyName) {
                UpdateWithForceLayout(UpdateIconRadius);
            }
            else if (e.PropertyName == SettingsView.SelectedColorProperty.PropertyName) {
                UpdateSelectedColor();
            }
            else if (e.PropertyName == TableView.RowHeightProperty.PropertyName) {
                UpdateMinRowHeight();
            }
        }

        /// <summary>
        /// Sections the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public virtual void SectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// Rows the selected.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public virtual void RowSelected(UITableView tableView,NSIndexPath indexPath)
        {
        }

        public virtual bool RowLongPressed(UITableView tableView,NSIndexPath indexPath)
        {
            return false;
        }

        /// <summary>
        /// Updates the with force layout.
        /// </summary>
        /// <param name="updateAction">Update action.</param>
        protected void UpdateWithForceLayout(System.Action updateAction)
        {
            updateAction();
            SetNeedsLayout();
        }

        void UpdateSelectedColor()
        {
            if (CellParent != null && CellParent.SelectedColor != Xamarin.Forms.Color.Default) {
                if (SelectedBackgroundView != null) {
                    SelectedBackgroundView.BackgroundColor = CellParent.SelectedColor.ToUIColor();
                }
                else {
                    SelectedBackgroundView = new UIView { BackgroundColor = CellParent.SelectedColor.ToUIColor() };
                }
            }
        }

        void UpdateBackgroundColor()
        {
            if (CellBase.BackgroundColor != Xamarin.Forms.Color.Default) {
                BackgroundColor = CellBase.BackgroundColor.ToUIColor();
            }
            else if (CellParent != null && CellParent.CellBackgroundColor != Xamarin.Forms.Color.Default) {
                BackgroundColor = CellParent.CellBackgroundColor.ToUIColor();
            }
        }

        void UpdateHintText()
        {
            HintLabel.Text = CellBase.HintText;
            HintLabel.Hidden = string.IsNullOrEmpty(CellBase.HintText);
        }

        void UpdateHintTextColor()
        {
            if (CellBase.HintTextColor != Xamarin.Forms.Color.Default) {
                HintLabel.TextColor = CellBase.HintTextColor.ToUIColor();
            }
            else if (CellParent != null && CellParent.CellHintTextColor != Xamarin.Forms.Color.Default) {
                HintLabel.TextColor = CellParent.CellHintTextColor.ToUIColor();
            }
        }

        void UpdateHintFontSize()
        {
            if (CellBase.HintFontSize > 0) {
                HintLabel.Font = HintLabel.Font.WithSize((nfloat)CellBase.HintFontSize);
            }
            else if (CellParent != null) {
                HintLabel.Font = HintLabel.Font.WithSize((nfloat)CellParent.CellHintFontSize);
            }
        }

        void UpdateTitleText()
        {
            TitleLabel.Text = CellBase.Title;
            //Since Layout breaks when text empty, prevent Label height from resizing 0.
            if (string.IsNullOrEmpty(TitleLabel.Text)) {
                TitleLabel.Hidden = true;
                TitleLabel.Text = " ";
            }
            else {
                TitleLabel.Hidden = false;
            }

        }

        void UpdateTitleColor()
        {
            if (CellBase.TitleColor != Xamarin.Forms.Color.Default) {
                TitleLabel.TextColor = CellBase.TitleColor.ToUIColor();
            }
            else if (CellParent != null && CellParent.CellTitleColor != Xamarin.Forms.Color.Default) {
                TitleLabel.TextColor = CellParent.CellTitleColor.ToUIColor();
            }
        }

        void UpdateTitleFontSize()
        {
            if (CellBase.TitleFontSize > 0) {
                TitleLabel.Font = TitleLabel.Font.WithSize((nfloat)CellBase.TitleFontSize);
            }
            else if (CellParent != null) {
                TitleLabel.Font = TitleLabel.Font.WithSize((nfloat)CellParent.CellTitleFontSize);
            }
        }

        void UpdateDescriptionText()
        {
            DescriptionLabel.Text = CellBase.Description;
            //layout break because of StackView spacing.DescriptionLabel hidden to fix it. 
            DescriptionLabel.Hidden = string.IsNullOrEmpty(DescriptionLabel.Text);
        }

        void UpdateDescriptionFontSize()
        {
            if (CellBase.DescriptionFontSize > 0) {
                DescriptionLabel.Font = DescriptionLabel.Font.WithSize((nfloat)CellBase.DescriptionFontSize);
            }
            else if (CellParent != null) {
                DescriptionLabel.Font = DescriptionLabel.Font.WithSize((nfloat)CellParent.CellDescriptionFontSize);
            }
        }

        void UpdateDescriptionColor()
        {
            if (CellBase.DescriptionColor != Xamarin.Forms.Color.Default) {
                DescriptionLabel.TextColor = CellBase.DescriptionColor.ToUIColor();
            }
            else if (CellParent != null && CellParent.CellDescriptionColor != Xamarin.Forms.Color.Default) {
                DescriptionLabel.TextColor = CellParent.CellDescriptionColor.ToUIColor();
            }
        }

        /// <summary>
        /// Updates the is enabled.
        /// </summary>
        protected virtual void UpdateIsEnabled()
        {
            SetEnabledAppearance(CellBase.IsEnabled);
        }

        /// <summary>
        /// Sets the enabled appearance.
        /// </summary>
        /// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
        protected virtual void SetEnabledAppearance(bool isEnabled)
        {
            if (isEnabled)
            {
                UserInteractionEnabled = true;
                TitleLabel.Alpha = 1f;
                DescriptionLabel.Alpha = 1f;
                IconView.Alpha = 1f;
            }
            else
            {
                UserInteractionEnabled = false;
                TitleLabel.Alpha = 0.3f;
                DescriptionLabel.Alpha = 0.3f;
                IconView.Alpha = 0.3f;
            }
        }

        void UpdateIconSize()
        {
            Size size;
            if (CellBase.IconSize != default(Size)) {
                size = CellBase.IconSize;
            }
            else if (CellParent != null && CellParent.CellIconSize != default(Size)) {
                size = CellParent.CellIconSize;
            }
            else {
                size = new Size(32, 32);
            }

            //do nothing when current size is previous size
            if (size == _iconSize) {
                return;
            }

            if (_iconSize != default(Size)) {
                //remove previous constraint
                _iconConstraintHeight.Active = false;
                _iconConstraintWidth.Active = false;
                _iconConstraintHeight?.Dispose();
                _iconConstraintWidth?.Dispose();
            }

            _iconConstraintHeight = IconView.HeightAnchor.ConstraintEqualTo((nfloat)size.Height);
            _iconConstraintWidth = IconView.WidthAnchor.ConstraintEqualTo((nfloat)size.Width);

            _iconConstraintHeight.Priority = 999f; // fix warning-log:Unable to simultaneously satisfy constraints.
            _iconConstraintHeight.Active = true;
            _iconConstraintWidth.Active = true;

            IconView.UpdateConstraints();

            _iconSize = size;
        }

        void UpdateIconRadius()
        {
            if (CellBase.IconRadius >= 0) {
                IconView.Layer.CornerRadius = (float)CellBase.IconRadius;
            }
            else if (CellParent != null) {
                IconView.Layer.CornerRadius = (float)CellParent.CellIconRadius;
            }

        }

        void UpdateIcon()
        {

            if (_iconTokenSource != null && !_iconTokenSource.IsCancellationRequested) {               
                _iconTokenSource.Cancel();
            }

            UpdateIconSize();

            if (IconView.Image != null) {
                IconView.Image = null;
            }

            if (CellBase.IconSource != null) {
                //image未設定の時はhiddenにしないとstackviewのDistributionが機能しなくなる
                //hide IconView because UIStackView Distribution won't work when a image isn't set.
                IconView.Hidden = false;

                var cache = ImageCacheController.Instance.ObjectForKey(FromObject(CellBase.IconSource.GetHashCode())) as UIImage;
                if (cache != null) {
                    IconView.Image = cache;
                    return;
                }

                var handler = Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IImageSourceHandler>(CellBase.IconSource.GetType());
                LoadIconImage(handler, CellBase.IconSource);
            }
            else {
                IconView.Hidden = true;
            }
        }

        void LoadIconImage(IImageSourceHandler handler, ImageSource source)
        {
            _iconTokenSource = new CancellationTokenSource();
            var token = _iconTokenSource.Token;
            UIImage image = null;

            var scale = (float)UIScreen.MainScreen.Scale;
            Task.Run(async () =>
            {
                image = await handler.LoadImageAsync(source, token, scale: scale);
                token.ThrowIfCancellationRequested();
            }, token).ContinueWith(t =>
            {
                if (t.IsCompleted) {
                    ImageCacheController.Instance.SetObjectforKey(image, FromObject(CellBase.IconSource.GetHashCode()));
                    BeginInvokeOnMainThread(() =>
                    {
                        IconView.Image = image;
                        SetNeedsLayout();
                    });
                }
            });
        }


        void UpdateMinRowHeight()
        {
            if (_minheightConstraint != null) {
                _minheightConstraint.Active = false;
                _minheightConstraint.Dispose();
                _minheightConstraint = null;
            }

            if (CellParent.HasUnevenRows) {
                _minheightConstraint = StackH.HeightAnchor.ConstraintGreaterThanOrEqualTo(CellParent.RowHeight);
                _minheightConstraint.Priority = 999f;
                _minheightConstraint.Active = true;

            }

            StackH.UpdateConstraints();
        }

        /// <summary>
        /// Updates the cell.
        /// </summary>
        public virtual void UpdateCell()
        {
            UpdateBackgroundColor();
            UpdateTitleText();
            UpdateTitleColor();
            UpdateTitleFontSize();
            UpdateDescriptionText();
            UpdateDescriptionColor();
            UpdateDescriptionFontSize();
            UpdateHintText();
            UpdateHintTextColor();
            UpdateHintFontSize();

            UpdateIcon();
            UpdateIconRadius();

            UpdateIsEnabled();

            SetNeedsLayout();
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                CellBase.PropertyChanged -= CellPropertyChanged;
                CellParent.PropertyChanged -= ParentPropertyChanged;

                if(CellBase.Section != null)
                {
                    CellBase.Section.PropertyChanged -= SectionPropertyChanged;
                    CellBase.Section = null;
                }


                SelectedBackgroundView?.Dispose();
                SelectedBackgroundView = null;

                Device.BeginInvokeOnMainThread(() =>
                {
                    HintLabel.RemoveFromSuperview();
                    HintLabel.Dispose();
                    HintLabel = null;
                    TitleLabel?.Dispose();
                    TitleLabel = null;
                    DescriptionLabel?.Dispose();
                    DescriptionLabel = null;
                    IconView.RemoveFromSuperview();
                    IconView.Image?.Dispose();
                    IconView.Dispose();
                    IconView = null;
                    _iconTokenSource?.Dispose();
                    _iconTokenSource = null;
                    _iconConstraintWidth?.Dispose();
                    _iconConstraintHeight?.Dispose();
                    _iconConstraintHeight = null;
                    _iconConstraintWidth = null;
                    ContentStack?.RemoveFromSuperview();
                    ContentStack?.Dispose();
                    ContentStack = null;
                    Cell = null;

                    StackV?.RemoveFromSuperview();
                    StackV?.Dispose();
                    StackV = null;

                    StackH.RemoveFromSuperview();
                    StackH.Dispose();
                    StackH = null;

                });
            }

            base.Dispose(disposing);
        }

        void SetUpHintLabel()
        {
            HintLabel = new UILabel();
            HintLabel.LineBreakMode = UILineBreakMode.Clip;
            HintLabel.Lines = 0;
            HintLabel.TintAdjustmentMode = UIViewTintAdjustmentMode.Automatic;
            HintLabel.AdjustsFontSizeToFitWidth = true;
            HintLabel.BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
            HintLabel.TextAlignment = UITextAlignment.Right;
            HintLabel.AdjustsLetterSpacingToFitWidth = true;

            this.AddSubview(HintLabel);

            HintLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            HintLabel.TopAnchor.ConstraintEqualTo(this.TopAnchor, 2).Active = true;
            HintLabel.LeftAnchor.ConstraintEqualTo(this.LeftAnchor, 16).Active = true;
            HintLabel.RightAnchor.ConstraintEqualTo(this.RightAnchor, -10).Active = true;
            HintLabel.BottomAnchor.ConstraintLessThanOrEqualTo(this.BottomAnchor, -12).Active = true;

            HintLabel.SizeToFit();
            BringSubviewToFront(HintLabel);
        }

        /// <summary>
        /// Sets the right margin zero.
        /// </summary>
        protected void SetRightMarginZero()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                StackH.LayoutMargins = new UIEdgeInsets(6, 16, 6, 0);
            }
        }


        protected virtual void SetUpContentView()
        {
            //remove existing views 
            ImageView.RemoveFromSuperview();
            TextLabel.RemoveFromSuperview();
            ImageView.Hidden = true;
            TextLabel.Hidden = true;

            //外側のHorizontalStackView
            //Outer HorizontalStackView
            StackH = new UIStackView
            {
                Axis = UILayoutConstraintAxis.Horizontal,
                Alignment = UIStackViewAlignment.Center,
                Spacing = 16,
                Distribution = UIStackViewDistribution.Fill
            };
            //set margin
            StackH.LayoutMargins = new UIEdgeInsets(6, 16, 6, 16);
            StackH.LayoutMarginsRelativeArrangement = true;

            IconView = new UIImageView();

            //round corners
            IconView.ClipsToBounds = true;

            StackH.AddArrangedSubview(IconView);

            UpdateIconSize();

            //右に配置するVerticalStackView（コアの部品とDescriptionを格納）
            //VerticalStackView that is arranged at right. ( put main parts and Description ) 
            StackV = new UIStackView
            {
                Axis = UILayoutConstraintAxis.Vertical,
                Alignment = UIStackViewAlignment.Fill,
                Spacing = 4,
                Distribution = UIStackViewDistribution.Fill,
            };

            //右側上段に配置するHorizontalStackView(LabelTextとValueTextを格納）
            //HorizontalStackView that is arranged at upper in right.(put LabelText and ValueText)
            ContentStack = new UIStackView
            {
                Axis = UILayoutConstraintAxis.Horizontal,
                Alignment = UIStackViewAlignment.Fill,
                Spacing = 6,
                Distribution = UIStackViewDistribution.Fill,
            };

            TitleLabel = new UILabel();
            DescriptionLabel = new UILabel();

            DescriptionLabel.Lines = 0;
            DescriptionLabel.LineBreakMode = UILineBreakMode.WordWrap;

            ContentStack.AddArrangedSubview(TitleLabel);

            StackV.AddArrangedSubview(ContentStack);
            StackV.AddArrangedSubview(DescriptionLabel);

            StackH.AddArrangedSubview(StackV);

            //余った領域を広げる優先度の設定（低いものが優先して拡大する）
            IconView.SetContentHuggingPriority(999f, UILayoutConstraintAxis.Horizontal); //if possible, not to expand. 極力広げない
            StackV.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);
            ContentStack.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);
            TitleLabel.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);
            DescriptionLabel.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);


            //縮まりやすさの設定（低いものが優先して縮まる）
            IconView.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal); //if possible, not to shrink. 極力縮めない
            StackV.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);
            ContentStack.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);
            TitleLabel.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);
            DescriptionLabel.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);

            IconView.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Vertical);
            IconView.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Vertical);
            StackV.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Vertical);
            StackV.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Vertical);

            ContentView.AddSubview(StackH);

            StackH.TranslatesAutoresizingMaskIntoConstraints = false;
            StackH.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
            StackH.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor).Active = true;
            StackH.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;
            StackH.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor).Active = true;


            var minHeight = Math.Max(CellParent?.RowHeight ?? 44, SettingsViewRenderer.MinRowHeight);
            _minheightConstraint = StackH.HeightAnchor.ConstraintGreaterThanOrEqualTo(minHeight);
            // fix warning-log:Unable to simultaneously satisfy constraints.
            _minheightConstraint.Priority = 999f; // this is superior to any other view.
            _minheightConstraint.Active = true;
        }

    }

}
