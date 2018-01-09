using System;
using Xamarin.Forms;
using System.Collections;
using System.Collections.Specialized;

namespace AiForms.Renderers
{
    /// <summary>
    /// Section.
    /// </summary>
    public class Section : TableSectionBase<Cell>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Section"/> class.
        /// </summary>
        public Section()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Section"/> class.
        /// </summary>
        /// <param name="title">Title.</param>
        public Section(string title) : base(title)
        {

        }

        /// <summary>
        /// The is visible property.
        /// </summary>
        public static BindableProperty IsVisibleProperty =
            BindableProperty.Create(
                nameof(IsVisible),
                typeof(bool),
                typeof(Section),
                true,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AiForms.Renderers.Section"/> is visible.
        /// </summary>
        /// <value><c>true</c> if is visible; otherwise, <c>false</c>.</value>
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// The footer text property.
        /// </summary>
        public static BindableProperty FooterTextProperty =
            BindableProperty.Create(
                nameof(FooterText),
                typeof(string),
                typeof(Section),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the footer text.
        /// </summary>
        /// <value>The footer text.</value>
        public string FooterText
        {
            get { return (string)GetValue(FooterTextProperty); }
            set { SetValue(FooterTextProperty, value); }
        }

        public static BindableProperty ItemTemplateProperty =
            BindableProperty.Create(
                nameof(ItemTemplate),
                typeof(DataTemplate),
                typeof(Section),
                default(DataTemplate),
                defaultBindingMode: BindingMode.OneWay
            );

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(Section),
                default(IEnumerable),
                defaultBindingMode: BindingMode.OneWay,
                propertyChanged: ItemsChanged
            );

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        static void ItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var section = (Section)bindable;

            if (section.ItemTemplate == null)
            {
                return;
            }

            IEnumerable newValueAsEnumerable;
            try
            {
                newValueAsEnumerable = newValue as IEnumerable;
            }
            catch (Exception e)
            {
                throw e;
            }

            var oldObservableCollection = oldValue as INotifyCollectionChanged;

            if (oldObservableCollection != null)
            {
                oldObservableCollection.CollectionChanged -= section.OnItemsSourceCollectionChanged;
            }

            var newObservableCollection = newValue as INotifyCollectionChanged;

            if (newObservableCollection != null)
            {
                newObservableCollection.CollectionChanged += section.OnItemsSourceCollectionChanged;
            }

            section.Clear();

            if (newValueAsEnumerable != null)
            {
                foreach (var item in newValueAsEnumerable)
                {
                    var view = CreateChildViewFor(section.ItemTemplate, item, section);

                    section.Add(view);
                }
            }
        }

        void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {

                this.RemoveAt(e.OldStartingIndex);

                var item = e.NewItems[e.NewStartingIndex];
                var view = CreateChildViewFor(this.ItemTemplate, item, this);

                this.Insert(e.NewStartingIndex, view);
            }

            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    for (var i = 0; i < e.NewItems.Count; ++i)
                    {
                        var item = e.NewItems[i];
                        var view = CreateChildViewFor(this.ItemTemplate, item, this);

                        this.Insert(i + e.NewStartingIndex, view);
                    }
                }
            }

            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    this.RemoveAt(e.OldStartingIndex);
                }
            }

            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                this.Clear();
            }

            else
            {
                return;
            }

        }


        static Cell CreateChildViewFor(DataTemplate template, object item, BindableObject container)
        {
            var selector = template as DataTemplateSelector;

            if (selector != null)
            {
                template = selector.SelectTemplate(item, container);
            }

            //Binding context
            template.SetValue(BindableObject.BindingContextProperty, item);

            return (Cell)template.CreateContent();
        }
    }
}
