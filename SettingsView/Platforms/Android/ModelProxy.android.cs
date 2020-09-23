using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Collections.Specialized;
#if ANDROIDX
using AndroidX.RecyclerView.Widget;
#else
using Android.Support.V7.Widget;
#endif
using Android.OS;

namespace AiForms.Renderers.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public enum ViewType
    {
        TextHeader,
        TextFooter,
        CustomHeader,
        CustomFooter,
    }

    [Android.Runtime.Preserve(AllMembers = true)]
    public class ModelProxy:List<RowInfo>,IDisposable
    {
        public Dictionary<Type, int> ViewTypes { get; private set; }

        SettingsModel _model;
        SettingsRoot _root;
        SettingsViewRecyclerAdapter _adapter;
        RecyclerView _recyclerView;

        public ModelProxy(SettingsView settingsView,SettingsViewRecyclerAdapter adapter,RecyclerView recyclerView)
        {
            _model = settingsView.Model;
            _root = settingsView.Root;
            _adapter = adapter;
            _recyclerView = recyclerView;

            _root.SectionCollectionChanged += OnRootSectionCollectionChanged;
            _root.CollectionChanged += OnRootCollectionChanged;

            FillProxy();
        }       

        public void Dispose()
        {
            _root.SectionCollectionChanged -= OnRootSectionCollectionChanged;
            _root.CollectionChanged -= OnRootCollectionChanged;
            _model = null;
            _root = null;
            _adapter = null;
            _recyclerView = null;
            this?.Clear();
            ViewTypes?.Clear();
            ViewTypes = null;
        }

        void OnRootCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex == -1 || e.NewItems == null)
                    {
                        goto case NotifyCollectionChangedAction.Reset;
                    }
                    AddSection(e);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex == -1)
                    {
                        goto case NotifyCollectionChangedAction.Reset;
                    }
                    RemoveSection(e);
                    break;
                case NotifyCollectionChangedAction.Replace: // no support on Android.
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Reset:
                    FillProxy();
                    _adapter.NotifyDataSetChanged();
                    break;
            }
        }


        void OnRootSectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex == -1 || e.NewItems == null)
                    {
                        goto case NotifyCollectionChangedAction.Reset;
                    }
                    AddCell(sender, e);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex == -1)
                    {
                        goto case NotifyCollectionChangedAction.Reset;
                    }
                    RemoveCell(sender, e);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex == -1)
                    {
                        goto case NotifyCollectionChangedAction.Reset;
                    }
                    ReplaceCell(sender, e);
                    break;

                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Reset:
                    FillProxy();
                    _adapter.NotifyDataSetChanged();
                    break;
            }
        }

        void AddSection(NotifyCollectionChangedEventArgs e)
        {
            // regard as coming only one item.
            var section = e.NewItems[0] as Section;
            var startIndex = RowIndexFromParentCollection(e.NewStartingIndex);

            this.Insert(startIndex,new RowInfo {
                Section = section,
                ViewType = section.HeaderView == null ? ViewType.TextHeader : ViewType.CustomHeader,
            });

            for (var i = 0; i < section.Count; i++)
            {
                var cell = section[i];
                if(!ViewTypes.ContainsKey(cell.GetType()))
                {
                    ViewTypes.Add(cell.GetType(), GetNextTypeIndex());
                }

                var rowInfo = new RowInfo {
                    Section = section,
                    Cell = cell,
                    ViewType = (ViewType)ViewTypes[cell.GetType()],
                };
                this.Insert(i + 1 + startIndex, rowInfo);
            }

            this.Insert(startIndex + section.Count() + 1,new RowInfo {
                Section = section,
                ViewType = section.FooterView == null ? ViewType.TextFooter : ViewType.CustomFooter,
            });

            _adapter.NotifyItemRangeInserted(startIndex, section.Count + 2); // add a header and footer
        }

        void RemoveSection(NotifyCollectionChangedEventArgs e)
        {
            // regard as coming only one item.
            var section = e.OldItems[0] as Section;
            var startIndex = RowIndexFromParentCollection(e.OldStartingIndex);

            this.RemoveRange(startIndex, section.Count + 2);

            _adapter.NotifyItemRangeRemoved(startIndex, section.Count + 2);
        }


        void AddCell(object sender, NotifyCollectionChangedEventArgs e)
        {
            var section = sender as Section;
            var startIndex = RowIndexFromChildCollection(section, e.NewStartingIndex);
            var newCells = e.NewItems.Cast<Cell>().ToList();
            for (var i = 0; i < newCells.Count; i++)
            {
                var cell = newCells[i];
                if(!ViewTypes.ContainsKey(cell.GetType()))
                {
                    ViewTypes.Add(cell.GetType(), GetNextTypeIndex());
                }

                var rowInfo = new RowInfo {
                    Section = section,
                    Cell = cell,
                    ViewType = (ViewType)ViewTypes[cell.GetType()],
                };
                this.Insert(i + startIndex,rowInfo);
            }

            _adapter.NotifyItemRangeInserted(startIndex, newCells.Count);
        }

        void RemoveCell(object sender, NotifyCollectionChangedEventArgs e)
        {
            var section = sender as Section;
            var startIndex = RowIndexFromChildCollection(section, e.OldStartingIndex);
            this.RemoveAt(startIndex);

            _adapter.NotifyItemRangeRemoved(startIndex, 1);
        }

        void ReplaceCell(object sender, NotifyCollectionChangedEventArgs e)
        {
            var section = sender as Section;
            var startIndex = RowIndexFromChildCollection(section, e.OldStartingIndex);
            var repCell = e.NewItems[0] as Cell;
            this[startIndex] = new RowInfo {
                Section = section,
                Cell = repCell,
                ViewType = (ViewType)ViewTypes[repCell.GetType()],
            };

            // Stop animation.
            (_recyclerView.GetItemAnimator() as DefaultItemAnimator).SupportsChangeAnimations = false;

            _adapter.NotifyItemRangeChanged(startIndex, 1);

            new Handler().PostDelayed(() =>
            {
                // Restart animation.
                (_recyclerView.GetItemAnimator() as DefaultItemAnimator).SupportsChangeAnimations = true;
            },100);
        }

        int RowIndexFromChildCollection(object sender,int index)
        {
            var section = sender as Section;
            var targetSectionIndex = this.IndexOf(x => x.Section == section);

            if (targetSectionIndex < 0) return -1;

            return index + targetSectionIndex + 1;
        }

        int RowIndexFromParentCollection(int index)
        {
            var groups = this.Select((x, idx) => new { index = idx, x.Section }).GroupBy(x => x.Section);
            var match = groups.ElementAtOrDefault(index)?.Min(x => x.index);

            return match.HasValue ? match.Value : this.Count;
        }

        int GetNextTypeIndex()
        {
            var idx = ViewTypes.Values.LastOrDefault();            
            return idx == 0 ? Enum.GetNames(typeof(ViewType)).Length : idx + 1;
        }

        public void FillProxy()
        {
            this.Clear();

            ViewTypes = _root.SelectMany(x => x).Select(x => x.GetType()).Distinct()
                .Select((type,idx) => new { type,index = idx })
                .ToDictionary(key =>key.type , val => val.index + Enum.GetNames(typeof(ViewType)).Length);

            int sectionCount = _model.GetSectionCount();

            for (var sectionIndex = 0; sectionIndex < sectionCount; sectionIndex++)
            {
                var sectionRowCount = _model.GetRowCount(sectionIndex);
                var isTextHeader = _model.GetSectionHeaderView(sectionIndex) == null;
                var curSection = _model.GetSection(sectionIndex);

                this.Add(new RowInfo {
                    Section = curSection,
                    ViewType = isTextHeader ? ViewType.TextHeader : ViewType.CustomHeader,
                });

                for (int i = 0; i < sectionRowCount; i++)
                {
                    var cell = _model.GetCell(sectionIndex, i);
                    this.Add(new RowInfo {
                        Section = curSection,
                        Cell = cell,
                        ViewType = (ViewType)ViewTypes[cell.GetType()],
                    });
                }

                var isTextFooter = _model.GetSectionFooterView(sectionIndex) == null;

                this.Add(new RowInfo {
                    Section = curSection,
                    ViewType = isTextFooter ? ViewType.TextFooter : ViewType.CustomFooter,
                });
            }
        }


    }

    [Android.Runtime.Preserve(AllMembers = true)]
    public class RowInfo
    {
        public Section Section { get; set; }
        public Cell Cell { get; set; }
        public ViewType ViewType { get; set; }
    }
}
