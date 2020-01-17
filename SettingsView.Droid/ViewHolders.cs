using System;
using Android.Support.V7.Widget;
using Android.Widget;
using Xamarin.Forms;
using AView = Android.Views.View;
using Android.Views;
using Xamarin.Forms.Platform.Android;

namespace AiForms.Renderers.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    internal class ViewHolder : RecyclerView.ViewHolder
    {
        public RowInfo RowInfo { get; set; }

        public ViewHolder(AView view) : base(view) { }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ItemView?.Dispose();
                ItemView = null;
            }
            base.Dispose(disposing);
        }
    }

    [Android.Runtime.Preserve(AllMembers = true)]
    internal interface IHeaderViewHolder
    { 
    }

    [Android.Runtime.Preserve(AllMembers = true)]
    internal interface IFooterViewHolder
    {
    }


    [Android.Runtime.Preserve(AllMembers = true)]
    internal class HeaderViewHolder :ViewHolder, IHeaderViewHolder
    {
        public TextView TextView { get; private set; }

        public HeaderViewHolder(AView view) : base(view)
        {
            TextView = view.FindViewById<TextView>(Resource.Id.HeaderCellText);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                TextView?.Dispose();
                TextView = null;
            }
            base.Dispose(disposing);
        }
    }

    [Android.Runtime.Preserve(AllMembers = true)]
    internal class FooterViewHolder :ViewHolder, IFooterViewHolder
    {
        public TextView TextView { get; private set; }

        public FooterViewHolder(AView view) : base(view)
        {
            TextView = view.FindViewById<TextView>(Resource.Id.FooterCellText);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                TextView?.Dispose();
                TextView = null;
            }
            base.Dispose(disposing);
        }
    }

    [Android.Runtime.Preserve(AllMembers = true)]
    internal class CustomHeaderViewHolder :ViewHolder, IHeaderViewHolder
    {
        public CustomHeaderViewHolder(AView view) : base(view)
        {
            view.LayoutParameters = new ViewGroup.LayoutParams(-1, -2);
        }       
    }

    [Android.Runtime.Preserve(AllMembers = true)]
    internal class CustomFooterViewHolder :ViewHolder, IFooterViewHolder
    {
        public CustomFooterViewHolder(AView view) : base(view)
        {
            view.LayoutParameters = new ViewGroup.LayoutParams(-1, -2);
        }
    }

    [Android.Runtime.Preserve(AllMembers = true)]
    internal class ContentViewHolder : ViewHolder
    {
        public LinearLayout Body { get; private set; }
        //public RowInfo RowInfo { get; set; }

        public ContentViewHolder(AView view) : base(view)
        {
            Body = view.FindViewById<LinearLayout>(Resource.Id.ContentCellBody);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var nativeCell = Body.GetChildAt(0);
                if (nativeCell is INativeElementView nativeElementView)
                {
                    // If a ViewCell is used, it stops the ViewCellContainer from executing the dispose method.
                    // Because if the AiForms.Effects is used and a ViewCellContainer is disposed, it crashes.
                    if (!(nativeElementView.Element is ViewCell))
                    {
                        nativeCell?.Dispose();
                    }
                }
                Body?.Dispose();
                Body = null;
                ItemView.SetOnClickListener(null);
                ItemView.SetOnLongClickListener(null);
            }
            base.Dispose(disposing);
        }
    }
}
