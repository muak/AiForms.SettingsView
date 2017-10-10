using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using System;
using System.Threading.Tasks;
using System.Threading;
using Foundation;

namespace AiForms.Renderers.iOS
{
    public class CellBaseView : CellTableViewCell
    {
        public UILabel ErrorLabel { get; private set; }
        public UILabel TitleLabel { get; private set; }
        public UILabel DescriptionLabel { get; private set; }
        public UIImageView IconView { get; private set; }

        protected CellBase CellBase => Cell as CellBase;
        public SettingsView CellParent => Cell.Parent as SettingsView;

        protected UIStackView ContentStack { get; private set; }

        UIStackView _stackH;

        Size _iconSize;
        NSLayoutConstraint _iconConstraintHeight;
        NSLayoutConstraint _iconConstraintWidth;
        CancellationTokenSource _iconTokenSource;

        public CellBaseView(Cell formsCell) : base(UIKit.UITableViewCellStyle.Default, formsCell.GetType().FullName)
        {
            Cell = formsCell;
            //選択時背景色
            SelectionStyle = UITableViewCellSelectionStyle.None;
            SetErrorMessageLabel();
            SetContentView();
        }

        public virtual void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CellBase.TitleProperty.PropertyName)
            {
                UpdateTitleText();
            }
            else if (e.PropertyName == CellBase.TitleColorProperty.PropertyName)
            {
                UpdateTitleColor();
            }
            else if (e.PropertyName == CellBase.TitleFontSizeProperty.PropertyName)
            {
                UpdateWithForceLayout(UpdateTitleFontSize);
            }
            else if (e.PropertyName == CellBase.ErrorMessageProperty.PropertyName)
            {
                UpdateWithForceLayout(UpdateErrorMessage);
            }
            else if (e.PropertyName == CellBase.DescriptionProperty.PropertyName)
            {
                UpdateWithForceLayout(UpdateDescriptionText);
            }
            else if (e.PropertyName == CellBase.DescriptionFontSizeProperty.PropertyName)
            {
                UpdateWithForceLayout(UpdateDescriptionFontSize);
            }
            else if (e.PropertyName == CellBase.DescriptionColorProperty.PropertyName)
            {
                UpdateDescriptionFontSize();
            }
            else if (e.PropertyName == CellBase.IconSourceProperty.PropertyName)
            {
                UpdateIcon();
            }
            else if (e.PropertyName == CellBase.BackgroundColorProperty.PropertyName)
            {
                UpdateBackgroundColor();
            }
        }

        public virtual void ParentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == SettingsView.CellTitleColorProperty.PropertyName)
            {
                UpdateTitleColor();
            }
            else if (e.PropertyName == SettingsView.CellTitleFontSizeProperty.PropertyName)
            {
                UpdateWithForceLayout(UpdateTitleFontSize);
            }
            else if (e.PropertyName == SettingsView.CellDescriptionColorProperty.PropertyName)
            {
                UpdateDescriptionColor();
            }
            else if (e.PropertyName == SettingsView.CellDescriptionFontSizeProperty.PropertyName)
            {
                UpdateWithForceLayout(UpdateDescriptionFontSize);
            }
            else if (e.PropertyName == SettingsView.CellBackgroundColorProperty.PropertyName)
            {
                UpdateBackgroundColor();
            }
        }

        protected void UpdateWithForceLayout(System.Action updateAction)
        {
            updateAction();
            SetNeedsLayout();
        }

        void UpdateBackgroundColor()
        {
            if (CellBase.BackgroundColor != Xamarin.Forms.Color.Default)
            {
                BackgroundColor = CellBase.BackgroundColor.ToUIColor();
            }
            else if (CellParent != null && CellParent.CellBackgroundColor != Xamarin.Forms.Color.Default)
            {
                BackgroundColor = CellParent.CellBackgroundColor.ToUIColor();
            }
        }

        void UpdateTitleText()
        {
            TitleLabel.Text = CellBase.Title;
        }

        void UpdateTitleColor()
        {
            if (CellBase.TitleColor != Xamarin.Forms.Color.Default)
            {
                TitleLabel.TextColor = CellBase.TitleColor.ToUIColor();
            }
            else if (CellParent != null && CellParent.CellTitleColor != Xamarin.Forms.Color.Default)
            {
                TitleLabel.TextColor = CellParent.CellTitleColor.ToUIColor();
            }
        }

        void UpdateTitleFontSize()
        {
            if (CellBase.TitleFontSize > 0)
            {
                TitleLabel.Font = TitleLabel.Font.WithSize((nfloat)CellBase.TitleFontSize);
            }
            else if (CellParent != null)
            {
                TitleLabel.Font = TitleLabel.Font.WithSize((nfloat)CellParent.CellTitleFontSize);
            }
        }

        void UpdateDescriptionText()
        {
            DescriptionLabel.Text = CellBase.Description;
        }

        void UpdateDescriptionFontSize()
        {
            if (CellBase.DescriptionFontSize > 0)
            {
                DescriptionLabel.Font = DescriptionLabel.Font.WithSize((nfloat)CellBase.DescriptionFontSize);
            }
            else if (CellParent != null)
            {
                DescriptionLabel.Font = DescriptionLabel.Font.WithSize((nfloat)CellParent.CellDescriptionFontSize);
            }
        }

        void UpdateDescriptionColor()
        {
            if (CellBase.DescriptionColor != Xamarin.Forms.Color.Default)
            {
                DescriptionLabel.TextColor = CellBase.DescriptionColor.ToUIColor();
            }
            else if (CellParent != null && CellParent.CellDescriptionColor != Xamarin.Forms.Color.Default)
            {
                DescriptionLabel.TextColor = CellParent.CellDescriptionColor.ToUIColor();
            }
        }


        //TODO:今の所PropertyChangedには非対応（アイコン自体が変わった時には呼ばれる）
        void UpdateIconSize()
        {
            Size size;
            if (CellBase.IconSize != default(Size))
            {
                size = CellBase.IconSize;
            }
            else if (CellParent != null && CellParent.CellIconSize != default(Size))
            {
                size = CellParent.CellIconSize;
            }
            else
            {
                size = new Size(50, 50);
            }

            //前のサイズと変わらなければ何もしない
            if (size == _iconSize)
            {
                return;
            }

            if (_iconSize != default(Size))
            {
                //前の制約を解除
                _iconConstraintHeight.Active = false;
                _iconConstraintWidth.Active = false;
                _iconConstraintHeight?.Dispose();
                _iconConstraintWidth?.Dispose();
            }

            _iconConstraintHeight = IconView.HeightAnchor.ConstraintEqualTo((nfloat)size.Height);
            _iconConstraintWidth = IconView.WidthAnchor.ConstraintEqualTo((nfloat)size.Width);

            _iconConstraintHeight.Active = true;
            _iconConstraintWidth.Active = true;

            IconView.UpdateConstraints();

            _iconSize = size;
        }


        void UpdateIcon()
        {

            if (_iconTokenSource != null && !_iconTokenSource.IsCancellationRequested)
            {
                //前のがキャンセルされてなければとりあえずキャンセル
                _iconTokenSource.Cancel();
            }

            UpdateIconSize();

            if (IconView.Image != null)
            {
                //IconView.Image.Dispose();
                IconView.Image = null;
            }

            if (CellBase.IconSource != null)
            {
                //image未設定の時はhiddenにしないとstackviewのDistributionが機能しなくなる
                IconView.Hidden = false;

                var cache = ImageCacheController.Instance.ObjectForKey(FromObject(CellBase.IconSource.GetHashCode())) as UIImage;
                if(cache != null){
                    IconView.Image = cache;
                    return;
                }

                var handler = Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IImageSourceHandler>(CellBase.IconSource.GetType());
                LoadIconImage(handler, CellBase.IconSource);
            }
            else
            {
                IconView.Hidden = true;
            }
        }

        void LoadIconImage(IImageSourceHandler handler, ImageSource source)
        {
            _iconTokenSource = new CancellationTokenSource();
            var token = _iconTokenSource.Token;
            UIImage image = null;

            var scale = (float)UIScreen.MainScreen.Scale;
            Task.Run(async () => {
                image = await handler.LoadImageAsync(source, token, scale: scale);
                token.ThrowIfCancellationRequested();
            }, token).ContinueWith(t => {
                if (t.IsCompleted)
                {
                    ImageCacheController.Instance.SetObjectforKey(image,FromObject(CellBase.IconSource.GetHashCode()));
                    BeginInvokeOnMainThread(() => {
                        IconView.Image = image;
                        SetNeedsLayout();
                    });
                }
            });
        }

        void UpdateErrorMessage()
        {
            ErrorLabel.Text = CellBase.ErrorMessage;
            ErrorLabel.Hidden = string.IsNullOrEmpty(CellBase.ErrorMessage);
        }

        public virtual void UpdateCell()
        {
            UpdateBackgroundColor();
            UpdateTitleText();
            UpdateTitleColor();
            UpdateTitleFontSize();
            UpdateDescriptionText();
            UpdateDescriptionColor();
            UpdateDescriptionFontSize();
            UpdateErrorMessage();

            UpdateIcon();

            SetNeedsLayout();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CellBase.PropertyChanged -= CellPropertyChanged;
                CellParent.PropertyChanged -= ParentPropertyChanged;

                Device.BeginInvokeOnMainThread(() => {
                    ErrorLabel.RemoveFromSuperview();
                    ErrorLabel.Dispose();
                    ErrorLabel = null;
                    TitleLabel.Dispose();
                    TitleLabel = null;
                    DescriptionLabel.Dispose();
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
                    ContentStack.RemoveFromSuperview();
                    ContentStack.Dispose();
                    ContentStack = null;
                    Cell = null;

                    _stackH.RemoveFromSuperview();
                    _stackH.Dispose();
                });
            }

            base.Dispose(disposing);
        }

        void SetErrorMessageLabel()
        {
            var hoge = this.Subviews;
            ErrorLabel = new UILabel();
            ErrorLabel.LineBreakMode = UILineBreakMode.Clip;
            ErrorLabel.Lines = 1;
            //ErrorLabel.BackgroundColor = UIColor.FromWhiteAlpha(1, .5f);
            ErrorLabel.TextColor = UIColor.Red.ColorWithAlpha(0.8f);
            ErrorLabel.TintAdjustmentMode = UIViewTintAdjustmentMode.Automatic;
            ErrorLabel.AdjustsFontSizeToFitWidth = true;
            ErrorLabel.BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
            ErrorLabel.TextAlignment = UITextAlignment.Right;
            ErrorLabel.AdjustsLetterSpacingToFitWidth = true;
            ErrorLabel.Font = ErrorLabel.Font.WithSize(10);

            ContentView.AddSubview(ErrorLabel);

            ErrorLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            ErrorLabel.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, 2).Active = true;
            ErrorLabel.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor).Active = true;
            ErrorLabel.HeightAnchor.ConstraintEqualTo(14).Active = true;
            ErrorLabel.WidthAnchor.ConstraintEqualTo(ContentView.WidthAnchor).Active = true;

            ErrorLabel.SizeToFit();
        }

        protected void SetRightMarginZero()
        {
            _stackH.LayoutMargins = new UIEdgeInsets(6, 16, 6, 0);
        }

        void SetContentView()
        {
            //備え付け部品を剥がす
            ImageView.RemoveFromSuperview();
            TextLabel.RemoveFromSuperview();
            ImageView.Hidden = true;
            TextLabel.Hidden = true;

            //外側のHoriontalStackView
            _stackH = new UIStackView {
                Axis = UILayoutConstraintAxis.Horizontal,
                Alignment = UIStackViewAlignment.Center,
                Spacing = 4,
                Distribution = UIStackViewDistribution.Fill
            };
            //マージン設定
            _stackH.LayoutMargins = new UIEdgeInsets(6, 16, 6, 10);
            _stackH.LayoutMarginsRelativeArrangement = true;

            IconView = new UIImageView();
            //IconView.ContentMode = UIViewContentMode.ScaleAspectFit;

            //角丸対応
            IconView.ClipsToBounds = true;
            IconView.Layer.CornerRadius = 6;

            _stackH.AddArrangedSubview(IconView);

            UpdateIconSize();

            //右に配置するVerticalStackView（LabelTextとValueTextとDetailTextを格納）
            var stackV = new UIStackView {
                Axis = UILayoutConstraintAxis.Vertical,
                Alignment = UIStackViewAlignment.Fill,
                Spacing = 0,
                Distribution = UIStackViewDistribution.Fill,
            };

            //右側上段に配置するHorizontalStackView(LabelTextとValueTextを格納）
            ContentStack = new UIStackView {
                Axis = UILayoutConstraintAxis.Horizontal,
                Alignment = UIStackViewAlignment.Fill,
                Spacing = 0,
                Distribution = UIStackViewDistribution.Fill,
            };

            TitleLabel = new UILabel();
            DescriptionLabel = new UILabel();

            DescriptionLabel.Lines = 0;
            DescriptionLabel.LineBreakMode = UILineBreakMode.CharacterWrap;

            ContentStack.AddArrangedSubview(TitleLabel);

            stackV.AddArrangedSubview(ContentStack);
            stackV.AddArrangedSubview(DescriptionLabel);

            _stackH.AddArrangedSubview(stackV);

            //余った領域を広げる優先度の設定（低いものが優先して拡大する）
            IconView.SetContentHuggingPriority(999f, UILayoutConstraintAxis.Horizontal); //極力広げない
            stackV.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);
            ContentStack.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);
            TitleLabel.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);
            DescriptionLabel.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);


            //縮まりやすさの設定（低いものが優先して縮まる）
            IconView.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal); //極力縮めない
            stackV.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);
            ContentStack.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);
            TitleLabel.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);
            DescriptionLabel.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);

            //ContentStack.SetContentHuggingPriority(100f, UILayoutConstraintAxis.Vertical);
            //DescriptionLabel.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Vertical);
            //DescriptionLabel.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Vertical);

            IconView.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Vertical);
            IconView.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Vertical);
            stackV.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Vertical);
            stackV.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Vertical);

            ContentView.AddSubview(_stackH);

            _stackH.TranslatesAutoresizingMaskIntoConstraints = false;
            _stackH.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
            _stackH.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor).Active = true;
            _stackH.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;
            _stackH.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor).Active = true;
            _stackH.HeightAnchor.ConstraintGreaterThanOrEqualTo(44f).Active = true;  //min height
        }

    }

}
