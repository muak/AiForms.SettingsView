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
        public event EventHandler<EventArgs> SectionCollectionChanged;

        void ChildCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            SectionCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TitleProperty.PropertyName) {
                OnPropertyChanged(TitleProperty.PropertyName);
            }
            else if (e.PropertyName == Section.FooterTextProperty.PropertyName) {
                OnPropertyChanged(Section.FooterTextProperty.PropertyName);
            }
            else if (e.PropertyName == Section.IsVisibleProperty.PropertyName) {
                OnPropertyChanged(Section.IsVisibleProperty.PropertyName);
            }
        }

        void SetupEvents()
        {
            CollectionChanged += (sender, args) =>
            {
                if (args.NewItems != null) {
                    foreach (Section section in args.NewItems) {
                        section.CollectionChanged += ChildCollectionChanged;
                        section.PropertyChanged += ChildPropertyChanged;
                    }
                }

                if (args.OldItems != null) {
                    foreach (Section section in args.OldItems) {
                        section.CollectionChanged -= ChildCollectionChanged;
                        section.PropertyChanged -= ChildPropertyChanged;
                    }
                }
            };
        }
    }
}
