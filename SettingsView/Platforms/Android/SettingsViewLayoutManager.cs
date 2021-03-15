using System.Collections.Generic;
using System.Linq;
using AndroidX.RecyclerView.Widget;
using Xamarin.Forms.Platform.Android;

namespace AiForms.Renderers.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class SettingsViewLayoutManager:LinearLayoutManager
    {
        SettingsView _settingsView;
        Android.Content.Context _context;
        Dictionary<Android.Views.View, int> ItemHeights = new Dictionary<Android.Views.View, int>();

        public SettingsViewLayoutManager(Android.Content.Context context,SettingsView settingsView):base(context)
        {
            _context = context;
            _settingsView = settingsView;
        }        

        public override int GetDecoratedMeasuredHeight(Android.Views.View child)
        {
            var height =  base.GetDecoratedMeasuredHeight(child);
            ItemHeights[child] = height;
            return height;
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                ItemHeights.Clear();
                ItemHeights = null;
                _context = null;
                _settingsView = null;
            }
            base.Dispose(disposing);
        }

        public override void OnLayoutCompleted(RecyclerView.State state)
        {
            base.OnLayoutCompleted(state);

            var total = ItemHeights.Sum(x => x.Value);

            _settingsView.VisibleContentHeight = _context.FromPixels(total);              
        }
    }
}
