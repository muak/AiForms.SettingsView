using System;
using System.ComponentModel;
using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.Content;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using XF = Xamarin.Forms;

[assembly: XF.ExportRenderer(typeof(RadioCell), typeof(RadioCellRenderer))]
namespace AiForms.Renderers.Droid
{
    /// <summary>
    /// Radio cell renderer.
    /// </summary>
    [Android.Runtime.Preserve(AllMembers = true)]
    public class RadioCellRenderer : CellBaseRenderer<RadioCellView> { }

    /// <summary>
    /// Radio cell view.
    /// </summary>
    [Android.Runtime.Preserve(AllMembers = true)]
    public class RadioCellView:CellBaseView
    {
        SimpleCheck _simpleCheck;
        RadioCell _radioCell => Cell as RadioCell;

        private object SelectedValue {
            get {
                return RadioCell.GetSelectedValue(_radioCell.Section) ?? RadioCell.GetSelectedValue(CellParent);
            }
            set {
                if (RadioCell.GetSelectedValue(_radioCell.Section) != null) {
                    RadioCell.SetSelectedValue(_radioCell.Section, value);
                }
                else {
                    RadioCell.SetSelectedValue(CellParent, value);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.RadioCellView"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="cell">Cell.</param>
        public RadioCellView(Context context, XF.Cell cell):base(context,cell)
        {
            _simpleCheck = new SimpleCheck(context);
            _simpleCheck.Focusable = false;

            var lparam = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent) {
                Width = (int)context.ToPixels(30),
                Height = (int)context.ToPixels(30)
            };

            using (lparam) {
                AccessoryStack.AddView(_simpleCheck, lparam);
            }
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                _simpleCheck.RemoveFromParent();
                _simpleCheck.Dispose();
                _simpleCheck = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Updates the cell.
        /// </summary>
        public override void UpdateCell()
        {
            UpdateAccentColor();
            UpdateSelectedValue();
            base.UpdateCell();
        }

        /// <summary>
        /// Cells the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == CheckboxCell.AccentColorProperty.PropertyName) {
                UpdateAccentColor();
                _simpleCheck.Invalidate();
            }
        }

        /// <summary>
        /// Parents the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void ParentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.ParentPropertyChanged(sender, e);
            if (e.PropertyName == SettingsView.CellAccentColorProperty.PropertyName) {
                UpdateAccentColor();
                _simpleCheck.Invalidate();
            }
            else if (e.PropertyName == RadioCell.SelectedValueProperty.PropertyName) {
                UpdateSelectedValue();
            }
        }

        /// <summary>
        /// Sections the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void SectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.SectionPropertyChanged(sender, e);
            if (e.PropertyName == RadioCell.SelectedValueProperty.PropertyName) {
                UpdateSelectedValue();
            }
        }

        /// <summary>
        /// Rows the selected.
        /// </summary>
        /// <param name="adapter">Adapter.</param>
        /// <param name="position">Position.</param>
        public override void RowSelected(SettingsViewRecyclerAdapter adapter,int position)
        {
            if(!_simpleCheck.Selected)
            {
                SelectedValue = _radioCell.Value;
            }
        }

        void UpdateSelectedValue()
        {
            bool result;
            if (_radioCell.Value.GetType().IsValueType) {
                result = object.Equals(_radioCell.Value, SelectedValue);
            }
            else {
                result = object.ReferenceEquals(_radioCell.Value, SelectedValue);
            }

            _simpleCheck.Selected = result;
        }

        void UpdateAccentColor()
        {
            if (!_radioCell.AccentColor.IsDefault) {
                _simpleCheck.Color = _radioCell.AccentColor.ToAndroid();
            }
            else if (CellParent != null && !CellParent.CellAccentColor.IsDefault) {
                _simpleCheck.Color = CellParent.CellAccentColor.ToAndroid();
            }
        }
    }
}
