using System.Linq;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace AiForms.Renderers.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class SVItemdecoration : RecyclerView.ItemDecoration
    {
        Drawable _drawable;
        SettingsView _settingsView;

        public SVItemdecoration(Drawable drawable,SettingsView settingsView)
        {
            _drawable = drawable;
            _settingsView = settingsView;
        }

        public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
        {
            outRect.Set(0, _drawable.IntrinsicHeight, 0, 0);
        }

        public override void OnDraw(Canvas c, RecyclerView parent, RecyclerView.State state)
        {
            var left = parent.Left;
            var right = parent.Right;

            var childCount = parent.ChildCount;
            ViewHolder prevHolder = null;
            for(var i = 0; i < childCount; i++)
            {
                var child = parent.GetChildAt(i);
                var holder = parent.GetChildViewHolder(child) as ViewHolder;

                if(prevHolder != null && prevHolder is IHeaderViewHolder && !_settingsView.ShowSectionTopBottomBorder ||
                   holder is IFooterViewHolder && !_settingsView.ShowSectionTopBottomBorder ||
                   holder is IFooterViewHolder && !holder.RowInfo.Section.Any() ||
                   holder is IHeaderViewHolder ||
                   !holder.RowInfo.Section.IsVisible)
                {
                    prevHolder = holder;
                    continue;
                }

                var bottom = child.Top;
                var top = bottom - _drawable.IntrinsicHeight;
                _drawable.SetBounds(left, top, right, bottom);
                _drawable.Draw(c);

                prevHolder = holder;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                _settingsView = null;
                _drawable = null;
            }
            base.Dispose(disposing);
        }
    }
}
