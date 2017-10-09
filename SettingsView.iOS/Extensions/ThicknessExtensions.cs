using System;
using UIKit;
using Xamarin.Forms;
namespace AiForms.Renderers.iOS.Extensions
{
    public static class ThicknessExtensions
    {
        public static UIEdgeInsets ToUIEdgeInsets(this Thickness forms)
        {
            return new UIEdgeInsets(
                (nfloat)forms.Top,
                (nfloat)forms.Left,
                (nfloat)forms.Bottom,
                (nfloat)forms.Right
            );
        }
    }
}
