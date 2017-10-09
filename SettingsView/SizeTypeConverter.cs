using System;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    public class SizeConverter : TypeConverter
    {
        public override object ConvertFromInvariantString(string value)
        {
            if (value != null) {
                var size = value.Split(',');

                switch (size.Length) {
                    case 1:
                        var w = double.Parse(size[0]);
                        return new Size(w, w);
                    case 2:
                        return new Size(double.Parse(size[0]), double.Parse(size[1]));
                }
            }
            throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Size)}");
        }
    }
}
