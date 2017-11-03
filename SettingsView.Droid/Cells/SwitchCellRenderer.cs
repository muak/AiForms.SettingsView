using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AiForms.Renderers.SwitchCell), typeof(AiForms.Renderers.Droid.SwitchCellRenderer))]
namespace AiForms.Renderers.Droid
{
    /// <summary>
    /// Switch cell renderer.
    /// </summary>
    public class SwitchCellRenderer : CellBaseRenderer<SwitchCellView> { }

    /// <summary>
    /// Switch cell view.
    /// </summary>
    public class SwitchCellView : CellBaseView, CompoundButton.IOnCheckedChangeListener, ICheckableCell
    {
        SwitchCompat _switch { get; set; }
        SwitchCell _SwitchCell => Cell as SwitchCell;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.SwitchCellView"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="cell">Cell.</param>
        public SwitchCellView(Context context, Cell cell) : base(context, cell)
        {

            _switch = new SwitchCompat(context);

            _switch.SetOnCheckedChangeListener(this);
            _switch.Gravity = Android.Views.GravityFlags.Right;

            var switchParam = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent)
            {
            };

            using (switchParam) {
                AccessoryStack.AddView(_switch, switchParam);
            }

            _switch.Focusable = false;
            Focusable = false;
            DescendantFocusability = Android.Views.DescendantFocusability.AfterDescendants;
        }

        /// <summary>
        /// Checks the change.
        /// </summary>
        public void CheckChange()
        {
            _switch.Checked = !_switch.Checked;
        }

        /// <summary>
        /// Updates the cell.
        /// </summary>
        public override void UpdateCell()
        {
            UpdateAccentColor();
            UpdateOn();
            base.UpdateCell();
        }

        /// <summary>
        /// Cells the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if (e.PropertyName == SwitchCell.AccentColorProperty.PropertyName) {
                UpdateAccentColor();
            }
            if (e.PropertyName == SwitchCell.OnProperty.PropertyName) {
                UpdateOn();
            }
        }

        /// <summary>
        /// Parents the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void ParentPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.ParentPropertyChanged(sender, e);
            if (e.PropertyName == SettingsView.CellAccentColorProperty.PropertyName) {
                UpdateAccentColor();
            }
        }

        /// <summary>
        /// Ons the checked changed.
        /// </summary>
        /// <param name="buttonView">Button view.</param>
        /// <param name="isChecked">If set to <c>true</c> is checked.</param>
        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        {
            _SwitchCell.On = isChecked;
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <returns>The dispose.</returns>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                _switch.SetOnCheckedChangeListener(null);
                _switch.Background?.Dispose();
                _switch.Background = null;
                _switch.ThumbDrawable?.Dispose();
                _switch.ThumbDrawable = null;
                _switch.Dispose();
                _switch = null;
            }
            base.Dispose(disposing);
        }

        void UpdateOn()
        {
            _switch.Checked = _SwitchCell.On;
        }

        void UpdateAccentColor()
        {
            if (_SwitchCell.AccentColor != Xamarin.Forms.Color.Default) {
                ChangeSwitchColor(_SwitchCell.AccentColor.ToAndroid());
            }
            else if (CellParent != null && CellParent.CellAccentColor != Xamarin.Forms.Color.Default) {
                ChangeSwitchColor(CellParent.CellAccentColor.ToAndroid());
            }
        }

        void ChangeSwitchColor(Android.Graphics.Color accent)
        {
            var trackColors = new ColorStateList(new int[][]
                  {
                            new int[]{global::Android.Resource.Attribute.StateChecked},
                            new int[]{-global::Android.Resource.Attribute.StateChecked},
                  },
                 new int[] {
                            Android.Graphics.Color.Argb(76,accent.R,accent.G,accent.B),
                            Android.Graphics.Color.Argb(76, 117, 117, 117)
                  });


            _switch.TrackDrawable.SetTintList(trackColors);

            var thumbColors = new ColorStateList(new int[][]
                 {
                            new int[]{global::Android.Resource.Attribute.StateChecked},
                            new int[]{-global::Android.Resource.Attribute.StateChecked},
                 },
                new int[] {
                            accent,
                            Android.Graphics.Color.Argb(255, 244, 244, 244)
                 });

            _switch.ThumbDrawable.SetTintList(thumbColors);

            var ripple = _switch.Background as RippleDrawable;
            ripple.SetColor(trackColors);
        }
    }
}
