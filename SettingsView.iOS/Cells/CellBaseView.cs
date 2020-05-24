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
		public UILabel HintLabel { get; private set; }
		public UILabel TitleLabel { get; private set; }
		public UILabel DescriptionLabel { get; private set; }
		public UIImageView IconView { get; private set; }


		protected CellBase _CellBase => Cell as CellBase;
		public SettingsView CellParent => Cell.Parent as SettingsView;


		protected UIStackView _ContentStack { get; private set; }

		protected UIStackView _StackH { get; set; }
		protected UIStackView _StackV { get; set; }

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

			TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			HintLabel.LineBreakMode = UILineBreakMode.WordWrap;
			DescriptionLabel.LineBreakMode = UILineBreakMode.WordWrap;
		}

		/// <summary>
		/// Cells the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public virtual void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if ( e.PropertyName == CellBase.TitleProperty.PropertyName )
			{
				UpdateTitleText();
			}
			else if ( e.PropertyName == CellBase.TitleColorProperty.PropertyName )
			{
				UpdateTitleColor();
			}
			else if ( e.PropertyName == CellBase.TitleFontSizeProperty.PropertyName )
			{
				UpdateWithForceLayout(UpdateTitleFontSize);
			}
			else if ( e.PropertyName == CellBase.DescriptionProperty.PropertyName )
			{
				UpdateWithForceLayout(UpdateDescriptionText);
			}
			else if ( e.PropertyName == CellBase.DescriptionFontSizeProperty.PropertyName )
			{
				UpdateWithForceLayout(UpdateDescriptionFontSize);
			}
			else if ( e.PropertyName == CellBase.DescriptionColorProperty.PropertyName )
			{
				UpdateDescriptionColor();
			}
			else if ( e.PropertyName == CellBase.IconSourceProperty.PropertyName )
			{
				UpdateIcon();
			}
			else if ( e.PropertyName == CellBase.BackgroundColorProperty.PropertyName )
			{
				UpdateBackgroundColor();
			}
			else if ( e.PropertyName == CellBase.HintTextProperty.PropertyName )
			{
				UpdateWithForceLayout(UpdateHintText);
			}
			else if ( e.PropertyName == CellBase.HintTextColorProperty.PropertyName )
			{
				UpdateHintTextColor();
			}
			else if ( e.PropertyName == CellBase.HintFontSizeProperty.PropertyName )
			{
				UpdateWithForceLayout(UpdateHintFontSize);
			}
			else if ( e.PropertyName == CellBase.IconSizeProperty.PropertyName )
			{
				UpdateWithForceLayout(UpdateIconSize);
			}
			else if ( e.PropertyName == CellBase.IconRadiusProperty.PropertyName )
			{
				UpdateWithForceLayout(UpdateIconRadius);
			}
			else if ( e.PropertyName == Cell.IsEnabledProperty.PropertyName )
			{
				UpdateIsEnabled();
			}
			else if ( e.PropertyName == CellBase.AllowMultiLineProperty.PropertyName )
			{
				UpdateAllowMultiLine();
			}
			else if ( e.PropertyName == CellBase.HintTextAlignmentProperty.PropertyName )
			{
				UpdateHintTextAlignment();
			}
			else if ( e.PropertyName == CellBase.TitleTextAlignmentProperty.PropertyName )
			{
				UpdateTitleTextAlignment();
			}
			else if ( e.PropertyName == CellBase.DescriptionTextAlignmentProperty.PropertyName )
			{
				UpdateDescriptionTextAlignment();
			}
			else if ( e.PropertyName == CellBase.MaxLinesProperty.PropertyName )
			{
				UpdateMaxLines();
			}
		}

		/// <summary>
		/// Parents the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public virtual void ParentPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if ( e.PropertyName == SettingsView.CellTitleColorProperty.PropertyName )
			{
				UpdateTitleColor();
			}
			else if ( e.PropertyName == SettingsView.CellTitleFontSizeProperty.PropertyName )
			{
				UpdateWithForceLayout(UpdateTitleFontSize);
			}
			else if ( e.PropertyName == SettingsView.CellDescriptionColorProperty.PropertyName )
			{
				UpdateDescriptionColor();
			}
			else if ( e.PropertyName == SettingsView.CellDescriptionFontSizeProperty.PropertyName )
			{
				UpdateWithForceLayout(UpdateDescriptionFontSize);
			}
			else if ( e.PropertyName == SettingsView.CellBackgroundColorProperty.PropertyName )
			{
				UpdateBackgroundColor();
			}
			else if ( e.PropertyName == SettingsView.CellHintTextColorProperty.PropertyName )
			{
				UpdateHintTextColor();
			}
			else if ( e.PropertyName == SettingsView.CellHintFontSizeProperty.PropertyName )
			{
				UpdateWithForceLayout(UpdateHintFontSize);
			}
			else if ( e.PropertyName == SettingsView.CellIconSizeProperty.PropertyName )
			{
				UpdateWithForceLayout(UpdateIconSize);
			}
			else if ( e.PropertyName == SettingsView.CellIconRadiusProperty.PropertyName )
			{
				UpdateWithForceLayout(UpdateIconRadius);
			}
			else if ( e.PropertyName == SettingsView.SelectedColorProperty.PropertyName )
			{
				UpdateSelectedColor();
			}
			else if ( e.PropertyName == TableView.RowHeightProperty.PropertyName )
			{
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
		public virtual void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
		}

		public virtual bool RowLongPressed(UITableView tableView, NSIndexPath indexPath)
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


		protected virtual void UpdateAllowMultiLine()
		{
			TitleLabel.Lines = _CellBase.AllowMultiLine ? 1 : _CellBase.MaxLines;
			HintLabel.Lines = _CellBase.AllowMultiLine ? 1 : _CellBase.MaxLines;
			DescriptionLabel.Lines = _CellBase.AllowMultiLine ? 1 : _CellBase.MaxLines;
		}
		protected virtual void UpdateMaxLines()
		{
			TitleLabel.Lines = _CellBase.MaxLines;
			HintLabel.Lines = _CellBase.MaxLines;
			DescriptionLabel.Lines = _CellBase.MaxLines;
		}

		void UpdateHintTextAlignment()
		{
			HintLabel.TextAlignment = GetTextAllignment(_CellBase.HintTextAlignment);
		}
		void UpdateTitleTextAlignment()
		{
			TitleLabel.TextAlignment = GetTextAllignment(_CellBase.TitleTextAlignment);
		}
		void UpdateDescriptionTextAlignment()
		{
			DescriptionLabel.TextAlignment = GetTextAllignment(_CellBase.DescriptionTextAlignment);
		}
		internal static UITextAlignment GetTextAllignment(Xamarin.Forms.TextAlignment alignment)
		{
			switch ( alignment )
			{
				case TextAlignment.Start:
					return UITextAlignment.Left;

				case TextAlignment.Center:
					return UITextAlignment.Center;

				case TextAlignment.End:
					return UITextAlignment.Right;

				default:
					throw new ArgumentOutOfRangeException(nameof(alignment), alignment, "alignment must be a member of the enum Xamarin.Forms.TextAlignment.");
			}
		}




		void UpdateSelectedColor()
		{
			if ( CellParent != null && CellParent.SelectedColor != Xamarin.Forms.Color.Default )
			{
				if ( SelectedBackgroundView != null )
				{
					SelectedBackgroundView.BackgroundColor = CellParent.SelectedColor.ToUIColor();
				}
				else
				{
					SelectedBackgroundView = new UIView { BackgroundColor = CellParent.SelectedColor.ToUIColor() };
				}
			}
		}

		void UpdateBackgroundColor()
		{
			if ( _CellBase.BackgroundColor != Xamarin.Forms.Color.Default )
			{
				BackgroundColor = _CellBase.BackgroundColor.ToUIColor();
			}
			else if ( CellParent != null && CellParent.CellBackgroundColor != Xamarin.Forms.Color.Default )
			{
				BackgroundColor = CellParent.CellBackgroundColor.ToUIColor();
			}
		}

		void UpdateHintText()
		{
			HintLabel.Text = _CellBase.HintText;
			HintLabel.Hidden = string.IsNullOrEmpty(_CellBase.HintText);
		}

		void UpdateHintTextColor()
		{
			if ( _CellBase.HintTextColor != Xamarin.Forms.Color.Default )
			{
				HintLabel.TextColor = _CellBase.HintTextColor.ToUIColor();
			}
			else if ( CellParent != null && CellParent.CellHintTextColor != Xamarin.Forms.Color.Default )
			{
				HintLabel.TextColor = CellParent.CellHintTextColor.ToUIColor();
			}
		}

		void UpdateHintFontSize()
		{
			if ( _CellBase.HintFontSize > 0 )
			{
				HintLabel.Font = HintLabel.Font.WithSize((nfloat) _CellBase.HintFontSize);
			}
			else if ( CellParent != null )
			{
				HintLabel.Font = HintLabel.Font.WithSize((nfloat) CellParent.CellHintFontSize);
			}
		}

		void UpdateTitleText()
		{
			TitleLabel.Text = _CellBase.Title;
			//Since Layout breaks when text empty, prevent Label height from resizing 0.
			if ( string.IsNullOrEmpty(TitleLabel.Text) )
			{
				TitleLabel.Hidden = true;
				TitleLabel.Text = " ";
			}
			else
			{
				TitleLabel.Hidden = false;
			}

		}

		void UpdateTitleColor()
		{
			if ( _CellBase.TitleColor != Xamarin.Forms.Color.Default )
			{
				TitleLabel.TextColor = _CellBase.TitleColor.ToUIColor();
			}
			else if ( CellParent != null && CellParent.CellTitleColor != Xamarin.Forms.Color.Default )
			{
				TitleLabel.TextColor = CellParent.CellTitleColor.ToUIColor();
			}
		}

		void UpdateTitleFontSize()
		{
			if ( _CellBase.TitleFontSize > 0 )
			{
				TitleLabel.Font = TitleLabel.Font.WithSize((nfloat) _CellBase.TitleFontSize);
			}
			else if ( CellParent != null )
			{
				TitleLabel.Font = TitleLabel.Font.WithSize((nfloat) CellParent.CellTitleFontSize);
			}
		}

		void UpdateDescriptionText()
		{
			DescriptionLabel.Text = _CellBase.Description;
			//layout break because of StackView spacing.DescriptionLabel hidden to fix it. 
			DescriptionLabel.Hidden = string.IsNullOrEmpty(DescriptionLabel.Text);
		}

		void UpdateDescriptionFontSize()
		{
			if ( _CellBase.DescriptionFontSize > 0 )
			{
				DescriptionLabel.Font = DescriptionLabel.Font.WithSize((nfloat) _CellBase.DescriptionFontSize);
			}
			else if ( CellParent != null )
			{
				DescriptionLabel.Font = DescriptionLabel.Font.WithSize((nfloat) CellParent.CellDescriptionFontSize);
			}
		}

		void UpdateDescriptionColor()
		{
			if ( _CellBase.DescriptionColor != Xamarin.Forms.Color.Default )
			{
				DescriptionLabel.TextColor = _CellBase.DescriptionColor.ToUIColor();
			}
			else if ( CellParent != null && CellParent.CellDescriptionColor != Xamarin.Forms.Color.Default )
			{
				DescriptionLabel.TextColor = CellParent.CellDescriptionColor.ToUIColor();
			}
		}

		/// <summary>
		/// Updates the is enabled.
		/// </summary>
		protected virtual void UpdateIsEnabled()
		{
			SetEnabledAppearance(_CellBase.IsEnabled);
		}

		/// <summary>
		/// Sets the enabled appearance.
		/// </summary>
		/// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
		protected virtual void SetEnabledAppearance(bool isEnabled)
		{
			if ( isEnabled )
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
			if ( _CellBase.IconSize != default(Size) )
			{
				size = _CellBase.IconSize;
			}
			else if ( CellParent != null && CellParent.CellIconSize != default(Size) )
			{
				size = CellParent.CellIconSize;
			}
			else
			{
				size = new Size(32, 32);
			}

			//do nothing when current size is previous size
			if ( size == _iconSize )
			{
				return;
			}

			if ( _iconSize != default(Size) )
			{
				//remove previous constraint
				_iconConstraintHeight.Active = false;
				_iconConstraintWidth.Active = false;
				_iconConstraintHeight?.Dispose();
				_iconConstraintWidth?.Dispose();
			}

			_iconConstraintHeight = IconView.HeightAnchor.ConstraintEqualTo((nfloat) size.Height);
			_iconConstraintWidth = IconView.WidthAnchor.ConstraintEqualTo((nfloat) size.Width);

			_iconConstraintHeight.Priority = 999f; // fix warning-log:Unable to simultaneously satisfy constraints.
			_iconConstraintHeight.Active = true;
			_iconConstraintWidth.Active = true;

			IconView.UpdateConstraints();

			_iconSize = size;
		}

		void UpdateIconRadius()
		{
			if ( _CellBase.IconRadius >= 0 )
			{
				IconView.Layer.CornerRadius = (float) _CellBase.IconRadius;
			}
			else if ( CellParent != null )
			{
				IconView.Layer.CornerRadius = (float) CellParent.CellIconRadius;
			}

		}

		void UpdateIcon()
		{

			if ( _iconTokenSource != null && !_iconTokenSource.IsCancellationRequested )
			{
				_iconTokenSource.Cancel();
			}

			UpdateIconSize();

			if ( IconView.Image != null )
			{
				IconView.Image = null;
			}

			if ( _CellBase.IconSource != null )
			{
				//image未設定の時はhiddenにしないとstackviewのDistributionが機能しなくなる
				//hide IconView because UIStackView Distribution won't work when a image isn't set.
				IconView.Hidden = false;

				var cache = ImageCacheController.Instance.ObjectForKey(FromObject(_CellBase.IconSource.GetHashCode())) as UIImage;
				if ( cache != null )
				{
					IconView.Image = cache;
					return;
				}

				var handler = Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IImageSourceHandler>(_CellBase.IconSource.GetType());
				LoadIconImage(handler, _CellBase.IconSource);
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

			var scale = (float) UIScreen.MainScreen.Scale;
			Task.Run(async () =>
			{
				image = await handler.LoadImageAsync(source, token, scale: scale);
				token.ThrowIfCancellationRequested();
			}, token).ContinueWith(t =>
			{
				if ( t.IsCompleted )
				{
					ImageCacheController.Instance.SetObjectforKey(image, FromObject(_CellBase.IconSource.GetHashCode()));
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
			if ( _minheightConstraint != null )
			{
				_minheightConstraint.Active = false;
				_minheightConstraint.Dispose();
				_minheightConstraint = null;
			}

			if ( CellParent.HasUnevenRows )
			{
				_minheightConstraint = _StackH.HeightAnchor.ConstraintGreaterThanOrEqualTo(CellParent.RowHeight);
				_minheightConstraint.Priority = 999f;
				_minheightConstraint.Active = true;

			}

			_StackH.UpdateConstraints();
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
			if ( disposing )
			{
				_CellBase.PropertyChanged -= CellPropertyChanged;
				CellParent.PropertyChanged -= ParentPropertyChanged;

				if ( _CellBase.Section != null )
				{
					_CellBase.Section.PropertyChanged -= SectionPropertyChanged;
					_CellBase.Section = null;
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
					_ContentStack?.RemoveFromSuperview();
					_ContentStack?.Dispose();
					_ContentStack = null;
					Cell = null;

					_StackV?.RemoveFromSuperview();
					_StackV?.Dispose();
					_StackV = null;

					_StackH.RemoveFromSuperview();
					_StackH.Dispose();
					_StackH = null;

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
			if ( UIDevice.CurrentDevice.CheckSystemVersion(11, 0) )
			{
				_StackH.LayoutMargins = new UIEdgeInsets(6, 16, 6, 0);
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
			_StackH = new UIStackView
			{
				Axis = UILayoutConstraintAxis.Horizontal,
				Alignment = UIStackViewAlignment.Center,
				Spacing = 16,
				Distribution = UIStackViewDistribution.Fill
			};
			//set margin
			_StackH.LayoutMargins = new UIEdgeInsets(6, 16, 6, 16);
			_StackH.LayoutMarginsRelativeArrangement = true;

			IconView = new UIImageView();

			//round corners
			IconView.ClipsToBounds = true;

			_StackH.AddArrangedSubview(IconView);

			UpdateIconSize();

			//右に配置するVerticalStackView（コアの部品とDescriptionを格納）
			//VerticalStackView that is arranged at right. ( put main parts and Description ) 
			_StackV = new UIStackView
			{
				Axis = UILayoutConstraintAxis.Vertical,
				Alignment = UIStackViewAlignment.Fill,
				Spacing = 4,
				Distribution = UIStackViewDistribution.Fill,
			};

			//右側上段に配置するHorizontalStackView(LabelTextとValueTextを格納）
			//HorizontalStackView that is arranged at upper in right.(put LabelText and ValueText)
			_ContentStack = new UIStackView
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

			_ContentStack.AddArrangedSubview(TitleLabel);

			_StackV.AddArrangedSubview(_ContentStack);
			_StackV.AddArrangedSubview(DescriptionLabel);

			_StackH.AddArrangedSubview(_StackV);

			//余った領域を広げる優先度の設定（低いものが優先して拡大する）
			IconView.SetContentHuggingPriority(999f, UILayoutConstraintAxis.Horizontal); //if possible, not to expand. 極力広げない
			_StackV.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);
			_ContentStack.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);
			TitleLabel.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);
			DescriptionLabel.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Horizontal);


			//縮まりやすさの設定（低いものが優先して縮まる）
			IconView.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal); //if possible, not to shrink. 極力縮めない
			_StackV.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);
			_ContentStack.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);
			TitleLabel.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);
			DescriptionLabel.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Horizontal);

			IconView.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Vertical);
			IconView.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Vertical);
			_StackV.SetContentCompressionResistancePriority(999f, UILayoutConstraintAxis.Vertical);
			_StackV.SetContentHuggingPriority(1f, UILayoutConstraintAxis.Vertical);

			ContentView.AddSubview(_StackH);

			_StackH.TranslatesAutoresizingMaskIntoConstraints = false;
			_StackH.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
			_StackH.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor).Active = true;
			_StackH.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;
			_StackH.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor).Active = true;


			var minHeight = Math.Max(CellParent?.RowHeight ?? 44, SettingsViewRenderer.MinRowHeight);
			_minheightConstraint = _StackH.HeightAnchor.ConstraintGreaterThanOrEqualTo(minHeight);
			// fix warning-log:Unable to simultaneously satisfy constraints.
			_minheightConstraint.Priority = 999f; // this is superior to any other view.
			_minheightConstraint.Active = true;
		}

	}

}
