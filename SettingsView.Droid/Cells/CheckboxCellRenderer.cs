using System;
using Xamarin.Forms;
using Android.Widget;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Xamarin.Forms.Platform.Android;
using Android.Content.Res;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(AiForms.Renderers.CheckboxCell), typeof(AiForms.Renderers.Droid.CheckboxCellRenderer))]
namespace AiForms.Renderers.Droid
{
    public class CheckboxCellRenderer:CellBaseRenderer<CheckboxCellView>{}
   
    public class CheckboxCellView:CellBaseView,CompoundButton.IOnCheckedChangeListener,ICheckableCell
    {
        AppCompatCheckBox _checkbox;
        CheckboxCell _CheckboxCell => Cell as CheckboxCell;

        public CheckboxCellView(Context context,Cell cell):base(context,cell)
        {
            _checkbox = new AppCompatCheckBox(context);
            _checkbox.SetOnCheckedChangeListener(this);
            _checkbox.Gravity = Android.Views.GravityFlags.Right;

            var lparam = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent) {
                Width = (int)context.ToPixels(30),
                Height = (int)context.ToPixels(30)
            };

            using (lparam)
            {
                AccessoryStack.AddView(_checkbox, lparam);
            }

            _checkbox.Focusable = false;
            Focusable = false;
            DescendantFocusability = Android.Views.DescendantFocusability.AfterDescendants;
        }

        public void CheckChange(){
            _checkbox.Checked = !_checkbox.Checked;
        }

        public override void UpdateCell()
        {
            UpdateAccentColor();
            UpdateChecked();
            base.UpdateCell();
        }

        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == CheckboxCell.AccentColorProperty.PropertyName)
            {
                UpdateAccentColor();
            }
            if (e.PropertyName == CheckboxCell.CheckedProperty.PropertyName)
            {
                UpdateChecked();
            }
        }

        public override void ParentPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.ParentPropertyChanged(sender, e);
            if (e.PropertyName == SettingsView.CellAccentColorProperty.PropertyName)
            {
                UpdateAccentColor();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing){
                _checkbox.SetOnCheckedChangeListener(null);
                _checkbox.Dispose();
                _checkbox = null;
            }
            base.Dispose(disposing);
        }

        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        {
            _CheckboxCell.Checked = isChecked;
            buttonView.JumpDrawablesToCurrentState();
        }

        void UpdateChecked()
        {
            _checkbox.Checked = _CheckboxCell.Checked;
        }

        void UpdateAccentColor()
        {
            if (_CheckboxCell.AccentColor != Xamarin.Forms.Color.Default)
            {
                ChangeCheckColor(_CheckboxCell.AccentColor.ToAndroid());
            }
            else if (CellParent != null && CellParent.CellAccentColor != Xamarin.Forms.Color.Default)
            {
                ChangeCheckColor(CellParent.CellAccentColor.ToAndroid());
            }
        }


        void ChangeCheckColor(Android.Graphics.Color accent){
           
            var colorList = new ColorStateList(
                new int[][]{
                    new int[]{global::Android.Resource.Attribute.StateChecked},
                    new int[]{-global::Android.Resource.Attribute.StateChecked},
                },
                new int[]{
                    accent,
                    accent
                }
            );

            _checkbox.SupportButtonTintList = colorList;

            var rippleColor = new ColorStateList(
                new int[][]{
                    new int[]{global::Android.Resource.Attribute.StateChecked},
                    new int[]{-global::Android.Resource.Attribute.StateChecked}
                },
                new int[] {
                    Android.Graphics.Color.Argb(76,accent.R,accent.G,accent.B),
                    Android.Graphics.Color.Argb(76, 117, 117, 117)
                }
            );
            var ripple = _checkbox.Background as RippleDrawable;
            ripple.SetColor(rippleColor);
            _checkbox.Background = ripple;
        }
    }
}
