using System;
using System.ComponentModel;
using AiForms.Renderers;
using AiForms.Renderers.Droid;
using Android.Content;
using Android.Text;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LabelCell), typeof(LabelCellRenderer))]
namespace AiForms.Renderers.Droid
{
    public class LabelCellRenderer:CellBaseRenderer<LabelCellView>{}

    public class LabelCellView : CellBaseView
    {
        LabelCell _LabelCell => Cell as LabelCell;
        public TextView ValueLabel { get; set; }

        public LabelCellView(Context context, Cell cell) : base(context, cell) {
           
            ValueLabel = new TextView(context);
            ValueLabel.SetSingleLine(true);
            ValueLabel.Ellipsize = TextUtils.TruncateAt.End;
            ValueLabel.Gravity = GravityFlags.Right;

            var textParams = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent) {

            };
            using (textParams) {
                ContentStack.AddView(ValueLabel, textParams);
            }

        }

        public override void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);

            if (e.PropertyName == LabelCell.ValueTextProperty.PropertyName)
            {
                UpdateValueText();
            }
            else if (e.PropertyName == LabelCell.ValueTextFontSizeProperty.PropertyName)
            {
                UpdateValueTextFontSize();
            }
            else if (e.PropertyName == LabelCell.ValueTextColorProperty.PropertyName)
            {
                UpdateValueTextColor();
            }
        }

        public override void ParentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.ParentPropertyChanged(sender, e);

            if (e.PropertyName == SettingsView.CellValueTextColorProperty.PropertyName)
            {
                UpdateValueTextColor();
            }
            else if (e.PropertyName == SettingsView.CellValueTextFontSizeProperty.PropertyName)
            {
                UpdateValueTextFontSize();
            }
        }

        public override void UpdateCell()
        {
            base.UpdateCell();
            UpdateValueText();
            UpdateValueTextColor();
            UpdateValueTextFontSize();
        }

        void UpdateValueText()
        {
            ValueLabel.Text = _LabelCell.ValueText;
        }

        void UpdateValueTextFontSize()
        {
            if (_LabelCell.ValueTextFontSize > 0)
            {
                ValueLabel.SetTextSize(Android.Util.ComplexUnitType.Sp,(float)_LabelCell.ValueTextFontSize);
            }
            else if (CellParent != null)
            {
                ValueLabel.SetTextSize(Android.Util.ComplexUnitType.Sp,(float)CellParent.CellValueTextFontSize);
            }
            Invalidate();
        }

        void UpdateValueTextColor()
        {
            if (_LabelCell.ValueTextColor != Xamarin.Forms.Color.Default)
            {
                ValueLabel.SetTextColor(_LabelCell.ValueTextColor.ToAndroid());
            }
            else if (CellParent != null && CellParent.CellValueTextColor != Xamarin.Forms.Color.Default)
            {
                ValueLabel.SetTextColor(CellParent.CellValueTextColor.ToAndroid());
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
