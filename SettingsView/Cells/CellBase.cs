using System;
using System.Globalization;
using Xamarin.Forms;

namespace AiForms.Renderers
{
	/// <summary>
	/// Cell base.
	/// </summary>
	public class CellBase : Cell
	{
		/// <summary>
		/// Occurs when tapped.
		/// </summary>
		public new event EventHandler Tapped;
		internal new void OnTapped()
		{
			Tapped?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		public string Title
		{
			get { return (string) GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		/// <summary>
		/// The title property.
		/// </summary>
		public static BindableProperty TitleProperty =
			BindableProperty.Create(
				nameof(Title),
				typeof(string),
				typeof(CellBase),
				default(string),
				defaultBindingMode: BindingMode.OneWay
			);


		/// <summary>
		/// Gets or sets the title Text Alignment.
		/// </summary>
		/// <value>The title.</value>
		public TextAlignment TitleTextAlignment
		{
			get { return (TextAlignment) GetValue(TitleTextAlignmentProperty); }
			set { SetValue(TitleTextAlignmentProperty, value); }
		}

		/// <summary>
		/// The title property.
		/// </summary>
		public static BindableProperty TitleTextAlignmentProperty =
			BindableProperty.Create(
				nameof(TitleTextAlignment),
				typeof(TextAlignment),
				typeof(CellBase),
				default(TextAlignment),
				defaultBindingMode: BindingMode.OneWay
			);


		/// <summary>
		/// The title color property.
		/// </summary>
		public static BindableProperty TitleColorProperty =
			BindableProperty.Create(
				nameof(TitleColor),
				typeof(Color),
				typeof(CellBase),
				default(Color),
				defaultBindingMode: BindingMode.OneWay
			);

		/// <summary>
		/// Gets or sets the color of the title.
		/// </summary>
		/// <value>The color of the title.</value>
		public Color TitleColor
		{
			get { return (Color) GetValue(TitleColorProperty); }
			set { SetValue(TitleColorProperty, value); }
		}

		/// <summary>
		/// The title font size property.
		/// </summary>
		public static BindableProperty TitleFontSizeProperty =
			BindableProperty.Create(
				nameof(TitleFontSize),
				typeof(double),
				typeof(CellBase),
				-1.0,
				defaultBindingMode: BindingMode.OneWay
			);

		/// <summary>
		/// Gets or sets the size of the title font.
		/// </summary>
		/// <value>The size of the title font.</value>
		[TypeConverter(typeof(FontSizeConverter))]
		public double TitleFontSize
		{
			get { return (double) GetValue(TitleFontSizeProperty); }
			set { SetValue(TitleFontSizeProperty, value); }
		}

		/// <summary>
		/// The description property.
		/// </summary>
		public static BindableProperty DescriptionProperty =
			BindableProperty.Create(
				nameof(Description),
				typeof(string),
				typeof(CellBase),
				default(string),
				defaultBindingMode: BindingMode.OneWay
			);

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
		public string Description
		{
			get { return (string) GetValue(DescriptionProperty); }
			set { SetValue(DescriptionProperty, value); }
		}

		/// <summary>
		/// The description color property.
		/// </summary>
		public static BindableProperty DescriptionColorProperty =
			BindableProperty.Create(
				nameof(DescriptionColor),
				typeof(Color),
				typeof(CellBase),
				default(Color),
				defaultBindingMode: BindingMode.OneWay
			);

		/// <summary>
		/// Gets or sets the Description Text Alignment.
		/// </summary>
		/// <value>The title.</value>
		public TextAlignment DescriptionTextAlignment
		{
			get { return (TextAlignment) GetValue(DescriptionTextAlignmentProperty); }
			set { SetValue(DescriptionTextAlignmentProperty, value); }
		}

		/// <summary>
		/// The title property.
		/// </summary>
		public static BindableProperty DescriptionTextAlignmentProperty =
			BindableProperty.Create(
				nameof(DescriptionTextAlignment),
				typeof(TextAlignment),
				typeof(CellBase),
				default(TextAlignment),
				defaultBindingMode: BindingMode.OneWay
			);

		/// <summary>
		/// Gets or sets the color of the description.
		/// </summary>
		/// <value>The color of the description.</value>
		public Color DescriptionColor
		{
			get { return (Color) GetValue(DescriptionColorProperty); }
			set { SetValue(DescriptionColorProperty, value); }
		}

		/// <summary>
		/// The description font size property.
		/// </summary>
		public static BindableProperty DescriptionFontSizeProperty =
			BindableProperty.Create(
				nameof(DescriptionFontSize),
				typeof(double),
				typeof(CellBase),
				-1.0d,
				defaultBindingMode: BindingMode.OneWay
			);

		/// <summary>
		/// Gets or sets the size of the description font.
		/// </summary>
		/// <value>The size of the description font.</value>
		[TypeConverter(typeof(FontSizeConverter))]
		public double DescriptionFontSize
		{
			get { return (double) GetValue(DescriptionFontSizeProperty); }
			set { SetValue(DescriptionFontSizeProperty, value); }
		}

		/// <summary>
		/// The hint text property.
		/// </summary>
		public static BindableProperty HintTextProperty =
			BindableProperty.Create(
				nameof(HintText),
				typeof(string),
				typeof(CellBase),
				default(string),
				defaultBindingMode: BindingMode.OneWay
			);

		/// <summary>
		/// Gets or sets the hint text.
		/// </summary>
		/// <value>The hint text.</value>
		public string HintText
		{
			get { return (string) GetValue(HintTextProperty); }
			set { SetValue(HintTextProperty, value); }
		}

		/// <summary>
		/// The hint text color property.
		/// </summary>
		public static BindableProperty HintTextColorProperty =
			BindableProperty.Create(
				nameof(HintTextColor),
				typeof(Color),
				typeof(CellBase),
				default(Color),
				defaultBindingMode: BindingMode.OneWay
			);

		/// <summary>
		/// Gets or sets the Hint Text Alignment.
		/// </summary>
		/// <value>The title.</value>
		public TextAlignment HintTextAlignment
		{
			get { return (TextAlignment) GetValue(HintTextAlignmentProperty); }
			set { SetValue(HintTextAlignmentProperty, value); }
		}

		/// <summary>
		/// The title Text Alignment property.
		/// </summary>
		public static BindableProperty HintTextAlignmentProperty =
			BindableProperty.Create(
				nameof(HintTextAlignment),
				typeof(TextAlignment),
				typeof(CellBase),
				TextAlignment.End,
				defaultBindingMode: BindingMode.OneWay
			);

		/// <summary>
		/// Gets or sets the color of the hint text.
		/// </summary>
		/// <value>The color of the hint text.</value>
		public Color HintTextColor
		{
			get { return (Color) GetValue(HintTextColorProperty); }
			set { SetValue(HintTextColorProperty, value); }
		}

		/// <summary>
		/// The hint font size property.
		/// </summary>
		public static BindableProperty HintFontSizeProperty =
			BindableProperty.Create(
				nameof(HintFontSize),
				typeof(double),
				typeof(CellBase),
				-1.0d,
				defaultBindingMode: BindingMode.OneWay
			);

		/// <summary>
		/// Gets or sets the size of the hint font.
		/// </summary>
		/// <value>The size of the hint font.</value>
		[TypeConverter(typeof(FontSizeConverter))]
		public double HintFontSize
		{
			get { return (double) GetValue(HintFontSizeProperty); }
			set { SetValue(HintFontSizeProperty, value); }
		}

		/// <summary>
		/// The background color property.
		/// </summary>
		public static BindableProperty BackgroundColorProperty =
			BindableProperty.Create(
				nameof(BackgroundColor),
				typeof(Color),
				typeof(CellBase),
				default(Color),
				defaultBindingMode: BindingMode.OneWay
			);

		/// <summary>
		/// Gets or sets the color of the background.
		/// </summary>
		/// <value>The color of the background.</value>
		public Color BackgroundColor
		{
			get { return (Color) GetValue(BackgroundColorProperty); }
			set { SetValue(BackgroundColorProperty, value); }
		}

		/// <summary>
		/// The icon source property.
		/// </summary>
		public static BindableProperty IconSourceProperty =
			BindableProperty.Create(
				nameof(IconSource),
				typeof(ImageSource),
				typeof(CellBase),
				default(ImageSource),
				defaultBindingMode: BindingMode.OneWay
			);

		/// <summary>
		/// Gets or sets the icon source.
		/// </summary>
		/// <value>The icon source.</value>
		[TypeConverter(typeof(ImageSourceConverter))]
		public ImageSource IconSource
		{
			get { return (ImageSource) GetValue(IconSourceProperty); }
			set { SetValue(IconSourceProperty, value); }
		}

		/// <summary>
		/// The icon size property.
		/// </summary>
		public static BindableProperty IconSizeProperty =
			BindableProperty.Create(
				nameof(IconSize),
				typeof(Size),
				typeof(CellBase),
				default(Size),
				defaultBindingMode: BindingMode.OneWay
			);

		/// <summary>
		/// Gets or sets the size of the icon.
		/// </summary>
		/// <value>The size of the icon.</value>
		[TypeConverter(typeof(SizeConverter))]
		public Size IconSize
		{
			get { return (Size) GetValue(IconSizeProperty); }
			set { SetValue(IconSizeProperty, value); }
		}

		/// <summary>
		/// The icon radius property.
		/// </summary>
		public static BindableProperty IconRadiusProperty =
			BindableProperty.Create(
				nameof(IconRadius),
				typeof(double),
				typeof(CellBase),
				-1.0d,
				defaultBindingMode: BindingMode.OneWay
			);

		/// <summary>
		/// Gets or sets the ValueText Text Alignment.
		/// </summary>
		/// <value>The title.</value>
		public TextAlignment ValueTextAlignment
		{
			get { return (TextAlignment) GetValue(ValueTextAlignmentProperty); }
			set { SetValue(ValueTextAlignmentProperty, value); }
		}

		/// <summary>
		/// The ValueText Text Alignment property.
		/// </summary>
		public static BindableProperty ValueTextAlignmentProperty =
			BindableProperty.Create(
				nameof(ValueTextAlignment),
				typeof(TextAlignment),
				typeof(CellBase),
				default(TextAlignment),
				defaultBindingMode: BindingMode.OneWay
			);

		public bool AllowMultiLine
		{
			get { return (bool) GetValue(AllowMultiLineProperty); }
			set
			{
				SetValue(AllowMultiLineProperty, value);
				if ( value is false )
				{
					MaxLines = 1;
					MinLines = 1;
				}
			}
		}

		public static BindableProperty AllowMultiLineProperty =
			BindableProperty.Create(
				nameof(AllowMultiLine),
				typeof(bool),
				typeof(CellBase),
				default(bool),
				defaultBindingMode: BindingMode.OneWay
			);

		public int MaxLines
		{
			get { return (int) GetValue(MaxLinesProperty); }
			set { SetValue(MaxLinesProperty, value); }
		}

		public static BindableProperty MaxLinesProperty =
			BindableProperty.Create(
				nameof(MaxLines),
				typeof(int),
				typeof(CellBase),
				1,
				defaultBindingMode: BindingMode.OneWay
			);

		
		/// <summary>
		/// Android Only.
		/// </summary>
		public int MinLines
		{
			get { return (int) GetValue(MinLinesProperty); }
			set { SetValue(MinLinesProperty, value); }
		}

		public static BindableProperty MinLinesProperty =
			BindableProperty.Create(
				nameof(MinLines),
				typeof(int),
				typeof(CellBase),
				1,
				defaultBindingMode: BindingMode.OneWay
			);

		/// <summary>
		/// Gets or sets the icon radius.
		/// </summary>
		/// <value>The icon radius.</value>
		public double IconRadius
		{
			get { return (double) GetValue(IconRadiusProperty); }
			set { SetValue(IconRadiusProperty, value); }
		}

		/// <summary>
		/// Gets or sets the section.
		/// </summary>
		/// <value>The section.</value>
		public Section Section { get; set; }
	}

}
