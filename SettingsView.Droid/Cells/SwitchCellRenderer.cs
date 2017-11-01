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
    public class SwitchCellRenderer:CellBaseRenderer<SwitchCellView>{}

    public class SwitchCellView : CellBaseView, CompoundButton.IOnCheckedChangeListener,ICheckableCell
    {
        SwitchCompat _switch { get; set; }
        SwitchCell _SwitchCell => Cell as SwitchCell;

        public SwitchCellView(Context context, Cell cell) : base(context, cell) {
            
            _switch = new SwitchCompat(context);

            _switch.SetOnCheckedChangeListener(this);
            _switch.Gravity = Android.Views.GravityFlags.Right;// | Android.Views.GravityFlags.Top;

            var switchParam = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent) {
            };

            using(switchParam){
                AccessoryStack.AddView(_switch,switchParam);
            }

            _switch.Focusable = false;
            Focusable = false;
            DescendantFocusability = Android.Views.DescendantFocusability.AfterDescendants;
        }

        public void CheckChange(){
            _switch.Checked = !_switch.Checked;
        }

        public override void UpdateCell()
        {
            UpdateAccentColor();
            UpdateOn();
            base.UpdateCell();
        }

        public override void CellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.CellPropertyChanged(sender, e);
            if(e.PropertyName == SwitchCell.AccentColorProperty.PropertyName){
                UpdateAccentColor();
            }
            if(e.PropertyName ==SwitchCell.OnProperty.PropertyName){
                UpdateOn();
            }
        }

        public override void ParentPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.ParentPropertyChanged(sender, e);
            if(e.PropertyName == SettingsView.CellAccentColorProperty.PropertyName){
                UpdateAccentColor();
            }
        }

        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked) {
            _SwitchCell.On = isChecked;
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing){
                _switch.SetOnCheckedChangeListener(null);
                _switch.Dispose();
                _switch = null;
            }
            base.Dispose(disposing);
        }

        void UpdateOn()
        {
            _switch.Checked = _SwitchCell.On;
        }

        void UpdateAccentColor(){
            if (_SwitchCell.AccentColor != Xamarin.Forms.Color.Default)
            {
                ChangeSwitchColor(_SwitchCell.AccentColor.ToAndroid());
            }
            else if (CellParent != null && CellParent.CellAccentColor != Xamarin.Forms.Color.Default)
            {
               ChangeSwitchColor(CellParent.CellAccentColor.ToAndroid());
            }
        }

        void ChangeSwitchColor(Android.Graphics.Color accent){
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
