using System;
using Xamarin.Forms;
using GravityFlags = Android.Views.GravityFlags;

namespace AiForms.Renderers.Droid.Extensions
{
    /// <summary>
    /// Text alignment extensions.
    /// </summary>
    [Android.Runtime.Preserve(AllMembers = true)]
    public static class TextAlignmentExtensions
    {
        /// <summary>
        /// Tos the gravity flags.
        /// </summary>
        /// <returns>The gravity flags.</returns>
        /// <param name="forms">Forms.</param>
        public static GravityFlags ToGravityFlags(this TextAlignment forms)
        {
			return forms switch
			{
				TextAlignment.Start => GravityFlags.Left | GravityFlags.CenterVertical,
				TextAlignment.Center => GravityFlags.Center | GravityFlags.CenterVertical,
				TextAlignment.End => GravityFlags.Right | GravityFlags.CenterVertical,
				_ => GravityFlags.Right | GravityFlags.CenterVertical,
			};
		}
    }
}
