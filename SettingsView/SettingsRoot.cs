using System;
using System.Collections.Specialized;
using System.ComponentModel;
using Xamarin.Forms;

namespace AiForms.Renderers
{
    /// <summary>
    /// Settings root.
    /// </summary>
    public class SettingsRoot : TableSectionBase<Section>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.SettingsRoot"/> class.
        /// </summary>
        public SettingsRoot()
        {
            SetupEvents();
        }

        /// <summary>
        /// Occurs when section collection changed.
        /// </summary>
        public event EventHandler<NotifyCollectionChangedEventArgs> SectionCollectionChanged;
        /// <summary>
        /// Occurs when section property changed.
        /// </summary>
        public event PropertyChangedEventHandler SectionPropertyChanged;
        /// <summary>
        /// Occurs when cell property changed.
        /// </summary>
        public event EventHandler<CellPropertyChangedEventArgs> CellPropertyChanged;
       
        void ChildCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {           
            SectionCollectionChanged?.Invoke(sender, notifyCollectionChangedEventArgs);
        }

        void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SectionPropertyChanged?.Invoke(sender, e);
        }

        void OnCellPropertyChanged(object sender, CellPropertyChangedEventArgs e)
        {
            CellPropertyChanged?.Invoke(sender, e);
        }

        void SetupEvents()
        {
            CollectionChanged += (sender, args) =>
            {
                if (args.NewItems != null) {
                    foreach (Section section in args.NewItems) {
                        section.SectionCollectionChanged += ChildCollectionChanged;
                        section.SectionPropertyChanged += ChildPropertyChanged;
                        section.CellPropertyChanged += OnCellPropertyChanged;
                    }
                }

                if (args.OldItems != null) {
                    foreach (Section section in args.OldItems) {
                        section.SectionCollectionChanged -= ChildCollectionChanged;
                        section.SectionPropertyChanged -= ChildPropertyChanged;
                        section.CellPropertyChanged -= OnCellPropertyChanged;
                    }
                }
            };
        }
    }
}
