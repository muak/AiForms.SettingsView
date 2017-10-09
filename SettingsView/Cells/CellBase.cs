using System;
using System.Globalization;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    public class CellBase:Cell
    {
        public CellBase() {
        }

        public static BindableProperty TitleProperty =
            BindableProperty.Create(
                nameof(Title),
                typeof(string),
                typeof(CellBase),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        public string Title {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static BindableProperty TitleColorProperty =
            BindableProperty.Create(
                nameof(TitleColor),
                typeof(Color),
                typeof(CellBase),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        public Color TitleColor {
            get { return (Color)GetValue(TitleColorProperty); }
            set { SetValue(TitleColorProperty, value); }
        }

        public static BindableProperty TitleFontSizeProperty =
            BindableProperty.Create(
                nameof(TitleFontSize),
                typeof(double),
                typeof(CellBase),
                -1.0,
                defaultBindingMode: BindingMode.OneWay
            );

        [TypeConverter(typeof(FontSizeConverter))]
        public double TitleFontSize {
            get { return (double)GetValue(TitleFontSizeProperty); }
            set { SetValue(TitleFontSizeProperty, value); }
        }

        public static BindableProperty DescriptionProperty =
            BindableProperty.Create(
                nameof(Description),
                typeof(string),
                typeof(CellBase),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        public string Description {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static BindableProperty DescriptionColorProperty =
            BindableProperty.Create(
                nameof(DescriptionColor),
                typeof(Color),
                typeof(CellBase),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        public Color DescriptionColor {
            get { return (Color)GetValue(DescriptionColorProperty); }
            set { SetValue(DescriptionColorProperty, value); }
        }

        public static BindableProperty DescriptionFontSizeProperty =
            BindableProperty.Create(
                nameof(DescriptionFontSize),
                typeof(double),
                typeof(CellBase),
                -1.0d,
                defaultBindingMode: BindingMode.OneWay
            );

        [TypeConverter(typeof(FontSizeConverter))]
        public double DescriptionFontSize {
            get { return (double)GetValue(DescriptionFontSizeProperty); }
            set { SetValue(DescriptionFontSizeProperty, value); }
        }

        public static BindableProperty ErrorMessageProperty =
            BindableProperty.Create(
                nameof(ErrorMessage),
                typeof(string),
                typeof(CellBase),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        public string ErrorMessage {
            get { return (string)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }

        public static BindableProperty BackgroundColorProperty =
            BindableProperty.Create(
                nameof(BackgroundColor),
                typeof(Color),
                typeof(CellBase),
                default(Color),
                defaultBindingMode: BindingMode.OneWay
            );

        public Color BackgroundColor {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static BindableProperty IconSourceProperty =
            BindableProperty.Create(
                nameof(IconSource),
                typeof(ImageSource),
                typeof(CellBase),
                default(ImageSource),
                defaultBindingMode: BindingMode.OneWay
            );

        public ImageSource IconSource {
            get { return (ImageSource)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }

        public static BindableProperty IconSizeProperty =
            BindableProperty.Create(
                nameof(IconSize),
                typeof(Size),
                typeof(CellBase),
                default(Size),
                defaultBindingMode: BindingMode.OneWay
            );

        [TypeConverter(typeof(SizeConverter))]
        public Size IconSize {
            get { return (Size)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        //public static BindableProperty IconResourceProperty =
            //BindableProperty.Create(
            //    nameof(IconResource),
            //    typeof(string),
            //    typeof(CellBase),
            //    default(string),
            //    defaultBindingMode: BindingMode.OneWay
            //);

        //public string IconResource {
        //    get { return (string)GetValue(IconResourceProperty); }
        //    set { SetValue(IconResourceProperty, value); }
        //}

        //public static BindableProperty IconColorProperty =
        //    BindableProperty.Create(
        //        nameof(IconColor),
        //        typeof(Color),
        //        typeof(CellBase),
        //        default(Color),
        //        defaultBindingMode: BindingMode.OneWay
        //    );

        //public Color IconColor {
        //    get { return (Color)GetValue(IconColorProperty); }
        //    set { SetValue(IconColorProperty, value); }
        //}

        //private NGraphics.IImage _Image;
        //public NGraphics.IImage Image {
        //    get {
        //        if (string.IsNullOrEmpty(IconResource)) return null;
        //        if (_Image == null) {
        //            var g = SvgLoader.GetResourceAndLoadSvg(IconResource);
        //            var sv = DependencyService.Get<ISvgService>();
        //            Device.OnPlatform(
        //                iOS: () => { _Image = sv.GetCanvas(g, 25, 25,IconColor);},
        //                Android: () => { _Image = sv.GetCanvas(g, 30, 30,IconColor); }
        //            );
        //        }

        //        return _Image;
        //    }
        //}

    }

}
