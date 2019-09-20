using System;
using Xamarin.Forms;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

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
            CollectionChanged += OnCollectionChanged;
            PropertyChanged += OnPropertyChanged;
        }

        public void MoveSourceItemWithoutNotify(int from, int to)
        {
            CollectionChanged -= OnCollectionChanged;
            var tmp = ItemsSource[from];
            ItemsSource.RemoveAt(from);
            ItemsSource.Insert(to, tmp);
            CollectionChanged += OnCollectionChanged;
        }

        public void MoveCellWithoutNotify(int from, int to)
        {
            CollectionChanged -= OnCollectionChanged;
            var tmp = this[from];
            this.RemoveAt(from);
            this.Insert(to, tmp);
            CollectionChanged += OnCollectionChanged;
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SectionCollectionChanged?.Invoke(this, e);
        }

        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SectionPropertyChanged?.Invoke(this, e);
        }


        public event NotifyCollectionChangedEventHandler SectionCollectionChanged;
        public event PropertyChangedEventHandler SectionPropertyChanged;

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

        /// <summary>
        /// The item template property.
        /// </summary>
        public static BindableProperty ItemTemplateProperty =
            BindableProperty.Create(
                nameof(ItemTemplate),
                typeof(DataTemplate),
                typeof(Section),
                default(DataTemplate),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the item template.
        /// </summary>
        /// <value>The item template.</value>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// The items source property.
        /// </summary>
        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IList),
                typeof(Section),
                default(IList),
                defaultBindingMode: BindingMode.OneWay,
                propertyChanged: ItemsChanged
            );

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// The header height property.
        /// </summary>
        public static BindableProperty HeaderHeightProperty =
            BindableProperty.Create(
                nameof(HeaderHeight),
                typeof(double),
                typeof(Section),
                -1d,
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the height of the header.
        /// </summary>
        /// <value>The height of the header.</value>
        public double HeaderHeight
        {
            get { return (double)GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }

        public static BindableProperty HeaderViewProperty =
            BindableProperty.Create(
                nameof(HeaderView),
                typeof(View),
                typeof(Section),
                default(View),
                defaultBindingMode: BindingMode.OneWay
            );

        public View HeaderView {
            get { return (View)GetValue(HeaderViewProperty); }
            set { SetValue(HeaderViewProperty, value); }
        }

        public static BindableProperty FooterViewProperty =
            BindableProperty.Create(
                nameof(FooterView),
                typeof(View),
                typeof(Section),
                default(View),
                defaultBindingMode: BindingMode.OneWay
            );

        public View FooterView {
            get { return (View)GetValue(FooterViewProperty); }
            set { SetValue(FooterViewProperty, value); }
        }

        /// <summary>
        /// The use drag sort property.
        /// </summary>
        public static BindableProperty UseDragSortProperty =
            BindableProperty.Create(
                nameof(UseDragSort),
                typeof(bool),
                typeof(Section),
                default(bool),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:AiForms.Renderers.Section"/> use drag sort.
        /// </summary>
        /// <value><c>true</c> if use drag sort; otherwise, <c>false</c>.</value>
        public bool UseDragSort
        {
            get { return (bool)GetValue(UseDragSortProperty); }
            set { SetValue(UseDragSortProperty, value); }
        }

        static void ItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var section = (Section)bindable;

            if (section.ItemTemplate == null)
            {
                return;
            }

            IList newValueAsEnumerable;
            try
            {
                newValueAsEnumerable = newValue as IList;
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
                    for (var i = 0; i < e.NewItems.Count; i++)
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
