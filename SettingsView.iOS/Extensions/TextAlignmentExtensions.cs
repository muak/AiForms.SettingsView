using System;
using UIKit;
using Xamarin.Forms;
namespace AiForms.Renderers.iOS.Extensions
{
	/// <summary>
	/// Text alignment extensions.
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
	public static class TextAlignmentExtensions
	{
		/// <summary>
		/// To the UITextalignment.
		/// </summary>
		/// <returns>The UIT ext alignment.</returns>
		/// <param name="forms">Forms.</param>
		public static UITextAlignment ToUITextAlignment(this TextAlignment forms)
		{
			switch ( forms )
			{
				case TextAlignment.Start:
					return UITextAlignment.Left;

				case TextAlignment.Center:
					return UITextAlignment.Center;

				case TextAlignment.End:
					return UITextAlignment.Right;

				default:
					return UITextAlignment.Right;
			}
		}
	}
}
