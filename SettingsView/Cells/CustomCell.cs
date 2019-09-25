using System;
using AiForms.Renderers;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    [ContentProperty("Content")]
    public class CustomCell:CommandCell
    {
        public static BindableProperty ShowArrowIndicatorProperty =
            BindableProperty.Create(
                nameof(ShowArrowIndicator),
                typeof(bool),
                typeof(CustomCell),
                default(bool),
                defaultBindingMode: BindingMode.OneWay
            );

        public bool ShowArrowIndicator {
            get { return (bool)GetValue(ShowArrowIndicatorProperty); }
            set { SetValue(ShowArrowIndicatorProperty, value); }
        }

        public static BindableProperty ContentProperty =
            BindableProperty.Create(
                nameof(Content),
                typeof(View),
                typeof(CustomCell),
                default(View),
                defaultBindingMode: BindingMode.OneWay
            );

        public View Content {
            get { return (View)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static BindableProperty IsSelectableProperty =
            BindableProperty.Create(
                nameof(IsSelectable),
                typeof(bool),
                typeof(CustomCell),
                true,
                defaultBindingMode: BindingMode.OneWay
            );

        public bool IsSelectable {
            get { return (bool)GetValue(IsSelectableProperty); }
            set { SetValue(IsSelectableProperty, value); }
        }

        public static BindableProperty IsMeasureOnceProperty =
            BindableProperty.Create(
                nameof(IsMeasureOnce),
                typeof(bool),
                typeof(CustomCell),
                default(bool),
                defaultBindingMode: BindingMode.OneWay
            );

        public bool IsMeasureOnce {
            get { return (bool)GetValue(IsMeasureOnceProperty); }
            set { SetValue(IsMeasureOnceProperty, value); }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if(Content != null)
            {
                Content.BindingContext = BindingContext;
            }
        }

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
