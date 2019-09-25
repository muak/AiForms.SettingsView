using System;
using AiForms.Renderers;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    /// <summary>
    /// Custom cell.
    /// </summary>
    [ContentProperty("Content")]
    public class CustomCell:CommandCell
    {
        /// <summary>
        /// The show arrow indicator property.
        /// </summary>
        public static BindableProperty ShowArrowIndicatorProperty =
            BindableProperty.Create(
                nameof(ShowArrowIndicator),
                typeof(bool),
                typeof(CustomCell),
                default(bool),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AiForms.Renderers.CustomCell"/> show arrow indicator.
        /// </summary>
        /// <value><c>true</c> if show arrow indicator; otherwise, <c>false</c>.</value>
        public bool ShowArrowIndicator {
            get { return (bool)GetValue(ShowArrowIndicatorProperty); }
            set { SetValue(ShowArrowIndicatorProperty, value); }
        }

        /// <summary>
        /// The content property.
        /// </summary>
        public static BindableProperty ContentProperty =
            BindableProperty.Create(
                nameof(Content),
                typeof(View),
                typeof(CustomCell),
                default(View),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public View Content {
            get { return (View)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// The is selectable property.
        /// </summary>
        public static BindableProperty IsSelectableProperty =
            BindableProperty.Create(
                nameof(IsSelectable),
                typeof(bool),
                typeof(CustomCell),
                true,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AiForms.Renderers.CustomCell"/> is selectable.
        /// </summary>
        /// <value><c>true</c> if is selectable; otherwise, <c>false</c>.</value>
        public bool IsSelectable {
            get { return (bool)GetValue(IsSelectableProperty); }
            set { SetValue(IsSelectableProperty, value); }
        }

        /// <summary>
        /// The is measure once property.
        /// </summary>
        public static BindableProperty IsMeasureOnceProperty =
            BindableProperty.Create(
                nameof(IsMeasureOnce),
                typeof(bool),
                typeof(CustomCell),
                default(bool),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AiForms.Renderers.CustomCell"/> is measure once.
        /// </summary>
        /// <value><c>true</c> if is measure once; otherwise, <c>false</c>.</value>
        public bool IsMeasureOnce {
            get { return (bool)GetValue(IsMeasureOnceProperty); }
            set { SetValue(IsMeasureOnceProperty, value); }
        }

        /// <summary>
        /// The use full size property.
        /// </summary>
        public static BindableProperty UseFullSizeProperty =
            BindableProperty.Create(
                nameof(UseFullSize),
                typeof(bool),
                typeof(CustomCell),
                default(bool),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AiForms.Renderers.CustomCell"/> use full size.
        /// </summary>
        /// <value><c>true</c> if use full size; otherwise, <c>false</c>.</value>
        public bool UseFullSize {
            get { return (bool)GetValue(UseFullSizeProperty); }
            set { SetValue(UseFullSizeProperty, value); }
        }

        /// <summary>
        /// Ons the binding context changed.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if(Content != null)
            {
                Content.BindingContext = BindingContext;
            }
        }

        /// <summary>
        /// Ons the parent set.
        /// </summary>
        protected override void OnParentSet()
        {
            base.OnParentSet();
            if(Content != null)
            {
                Content.Parent = Parent;
            }
        }
    }
}
