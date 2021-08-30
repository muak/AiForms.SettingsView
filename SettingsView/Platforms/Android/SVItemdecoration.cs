using System.Linq;
using Android.Graphics;
using Android.Graphics.Drawables;
using AndroidX.RecyclerView.Widget;
using Xamarin.Forms;
using Rect = Android.Graphics.Rect;
using View = Android.Views.View;

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
            var holder = parent.GetChildViewHolder(view) as ViewHolder;
            if (!CellIsVisible(holder?.RowInfo.Cell))
            {
                return;
            }

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
                   !SectionIsVisible(holder?.RowInfo.Section) ||
                   !CellIsVisible(holder?.RowInfo.Cell))
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

        private bool SectionIsVisible(Section section)
        {
            return section?.IsVisible ?? true;
        }
        private bool CellIsVisible(Cell cell)
        {
            return (cell as CellBase)?.IsVisible ?? true;
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
