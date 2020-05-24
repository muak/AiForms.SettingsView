using Android.Graphics;
using Xamarin.Forms.Platform.Android;
using AView = Android.Views.View;

namespace AiForms.Renderers.Droid
{
	/// <summary>
	/// Simple check.
	/// </summary>
	[Android.Runtime.Preserve(AllMembers = true)]
	public class SimpleCheck : AView
	{
		/// <summary>
		/// Gets or sets the color.
		/// </summary>
		/// <value>The color.</value>
		public Color Color { get; set; }
		Paint _paint = new Paint();
		Android.Content.Context _context;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:AiForms.Renderers.Droid.SimpleCheck"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public SimpleCheck(Android.Content.Context context) : base(context)
		{
			_context = context;
			SetWillNotDraw(false);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:AiForms.Renderers.Droid.SimpleCheck"/> is selected.
		/// </summary>
		/// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
		public override bool Selected
		{
			get
			{
				return base.Selected;
			}
			set
			{
				base.Selected = value;
				Invalidate();
			}
		}

		/// <summary>
		/// Ons the draw.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw(canvas);

			if ( !base.Selected )
			{
				return;
			}

			_paint.SetStyle(Paint.Style.Stroke);
			_paint.Color = Color;
			_paint.StrokeWidth = _context.ToPixels(2);
			_paint.AntiAlias = true;

			var fromX = 22f / 100f * canvas.Width;
			var fromY = 52f / 100f * canvas.Height;
			var toX = 38f / 100f * canvas.Width;
			var toY = 68f / 100f * canvas.Height;

			canvas.DrawLine(fromX, fromY, toX, toY, _paint);

			fromX = 36f / 100f * canvas.Width;
			fromY = 66f / 100f * canvas.Height;

			toX = 74f / 100f * canvas.Width;
			toY = 28f / 100f * canvas.Height;

			canvas.DrawLine(fromX, fromY, toX, toY, _paint);
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <returns>The dispose.</returns>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose(bool disposing)
		{
			if ( disposing )
			{
				_paint?.Dispose();
				_paint = null;
				_context = null;
			}
			base.Dispose(disposing);
		}
	}
}
