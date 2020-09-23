using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AiForms.Renderers.Droid
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public static class FontUtility
    {
		static readonly ConcurrentDictionary<Tuple<string, FontAttributes>, Typeface> Typefaces = new ConcurrentDictionary<Tuple<string, FontAttributes>, Typeface>();

		static Typeface s_defaultTypeface;

		public static Typeface CreateTypeface(string fontFamily, FontAttributes fontAttributes = FontAttributes.None)
		{
			if (fontAttributes == FontAttributes.None && string.IsNullOrEmpty(fontFamily))
			{
				return s_defaultTypeface ?? (s_defaultTypeface = Typeface.Default);
			}

			return ToTypeface(fontFamily, fontAttributes);
		}

		static Typeface ToTypeface(string fontFamily, FontAttributes fontAttributes)
		{
			fontFamily ??= String.Empty;
			return Typefaces.GetOrAdd(new Tuple<string, FontAttributes>(fontFamily, fontAttributes), CreateTypeface);
		}

		static Typeface CreateTypeface(Tuple<string, FontAttributes> key)
		{
			Typeface result;
			var fontFamily = key.Item1;
			var fontAttribute = key.Item2;

			if (String.IsNullOrWhiteSpace(fontFamily))
			{
				var style = ToTypefaceStyle(fontAttribute);
				result = Typeface.Create(Typeface.Default, style);
			}
			else if (IsAssetFontFamily(fontFamily))
			{
				result = Typeface.CreateFromAsset(Android.App.Application.Context.Assets, FontNameToFontFile(fontFamily));
			}
			else
			{
				result = fontFamily.ToTypeFace(fontAttribute);
			}

			return result;
		}

		static Typeface ToTypeFace(this string fontfamily, FontAttributes attr = FontAttributes.None)
		{
			fontfamily ??= String.Empty;
			var result = fontfamily.TryGetFromAssets();
			if (result.success)
			{
				return result.typeface;
			}
			else
			{
				var style = ToTypefaceStyle(attr);
				return Typeface.Create(fontfamily, style);
			}

		}

		static (bool success, Typeface typeface) TryGetFromAssets(this string fontName)
		{
			//First check Alias
			var (hasFontAlias, fontPostScriptName) = FontRegistrar.HasFont(fontName);
			if (hasFontAlias)
				return (true, Typeface.CreateFromFile(fontPostScriptName));

			var isAssetFont = IsAssetFontFamily(fontName);
			if (isAssetFont)
			{
				return LoadTypefaceFromAsset(fontName);
			}

			var folders = new[]
			{
				"",
				"Fonts/",
				"fonts/",
			};


			//copied text
			var fontFile = FontFile.FromString(fontName);

			if (!string.IsNullOrWhiteSpace(fontFile.Extension))
			{
				var (hasFont, fontPath) = FontRegistrar.HasFont(fontFile.FileNameWithExtension());
				if (hasFont)
				{
					return (true, Typeface.CreateFromFile(fontPath));
				}
			}
			else
			{
				foreach (var ext in FontFile.Extensions)
				{
					var formated = fontFile.FileNameWithExtension(ext);
					var (hasFont, fontPath) = FontRegistrar.HasFont(formated);
					if (hasFont)
					{
						return (true, Typeface.CreateFromFile(fontPath));
					}

					foreach (var folder in folders)
					{
						formated = $"{folder}{fontFile.FileNameWithExtension()}#{fontFile.PostScriptName}";
						var result = LoadTypefaceFromAsset(formated);
						if (result.success)
							return result;
					}

				}
			}

			return (false, null);
		}

		static (bool success, Typeface typeface) LoadTypefaceFromAsset(string fontfamily)
		{
			try
			{
				var result = Typeface.CreateFromAsset(Android.App.Application.Context.Assets, FontNameToFontFile(fontfamily));
				return (true, result);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				return (false, null);
			}
		}

		static bool IsAssetFontFamily(string name)
		{
			return name != null && (name.Contains(".ttf#") || name.Contains(".otf#"));
		}

		static TypefaceStyle ToTypefaceStyle(FontAttributes attrs)
		{
			var style = TypefaceStyle.Normal;
			if ((attrs & (FontAttributes.Bold | FontAttributes.Italic)) == (FontAttributes.Bold | FontAttributes.Italic))
				style = TypefaceStyle.BoldItalic;
			else if ((attrs & FontAttributes.Bold) != 0)
				style = TypefaceStyle.Bold;
			else if ((attrs & FontAttributes.Italic) != 0)
				style = TypefaceStyle.Italic;
			return style;
		}

		static string FontNameToFontFile(string fontFamily)
		{
			fontFamily ??= String.Empty;
			int hashtagIndex = fontFamily.IndexOf('#');
			if (hashtagIndex >= 0)
				return fontFamily.Substring(0, hashtagIndex);

			throw new InvalidOperationException($"Can't parse the {nameof(fontFamily)} {fontFamily}");
		}
	}
}
