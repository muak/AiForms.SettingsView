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

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.Section"/> class.
        /// </summary>
        /// <param name="title">Title.</param>
        public Section(string title) :this()
        {
            Title = title;
        }

        /// <summary>
        /// Ons the binding context changed.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if(HeaderView != null)
            {
                HeaderView.BindingContext = BindingContext;
            }
            if(FooterView != null)
            {
                FooterView.BindingContext = BindingContext;
            }
        }

        /// <summary>
        /// Moves the source item without notify.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public void MoveSourceItemWithoutNotify(int from, int to)
        {
            CollectionChanged -= OnCollectionChanged;
            var notifyCollection = ItemsSource as INotifyCollectionChanged;
            if(notifyCollection != null)
            {
                notifyCollection.CollectionChanged -= OnItemsSourceCollectionChanged;
            }

            var tmp = ItemsSource[from];
            ItemsSource.RemoveAt(from);
            ItemsSource.Insert(to, tmp);

            var tmpCell = this[from];
            this.RemoveAt(from);
            this.Insert(to, tmpCell);

            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged += OnItemsSourceCollectionChanged;
            }

            CollectionChanged += OnCollectionChanged;
        }

        /// <summary>
        /// Moves the cell without notify.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public void MoveCellWithoutNotify(int from, int to)
        {
            CollectionChanged -= OnCollectionChanged;
            var tmp = this[from];
            this.RemoveAt(from);
            this.Insert(to, tmp);
            CollectionChanged += OnCollectionChanged;
        }

        public (Cell Cell,Object Item) DeleteSourceItemWithoutNotify(int from)
        {
            CollectionChanged -= OnCollectionChanged;
            var notifyCollection = ItemsSource as INotifyCollectionChanged;
            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged -= OnItemsSourceCollectionChanged;
            }

            var deletedItem = ItemsSource[from];
            ItemsSource.RemoveAt(from);

            var deletedCell = this[from];
            this.RemoveAt(from);

            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged += OnItemsSourceCollectionChanged;
            }

            CollectionChanged += OnCollectionChanged;

            return (deletedCell, deletedItem);
        }

        public void InsertSourceItemWithoutNotify(Cell cell,Object item, int to)
        {
            CollectionChanged -= OnCollectionChanged;
            var notifyCollection = ItemsSource as INotifyCollectionChanged;
            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged -= OnItemsSourceCollectionChanged;
            }

            ItemsSource.Insert(to, item);
            Insert(to, cell);

            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged += OnItemsSourceCollectionChanged;
            }

            CollectionChanged += OnCollectionChanged;
        }

        public Cell DeleteCellWithoutNotify(int from)
        {
            var deletedCell = this[from];
            CollectionChanged -= OnCollectionChanged;
            RemoveAt(from);
            CollectionChanged += OnCollectionChanged;
            return deletedCell;
        }

        public void InsertCellWithoutNotify(Cell cell,int to)
        {
            CollectionChanged -= OnCollectionChanged;
            Insert(to, cell);
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


        /// <summary>
        /// Occurs when section collection changed.
        /// </summary>
        public event NotifyCollectionChangedEventHandler SectionCollectionChanged;
        /// <summary>
        /// Occurs when section property changed.
        /// </summary>
        public event PropertyChangedEventHandler SectionPropertyChanged;


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

        /// <summary>
        /// The header view property.
        /// </summary>
        public static BindableProperty HeaderViewProperty =
            BindableProperty.Create(
                nameof(HeaderView),
                typeof(View),
                typeof(Section),
                default(View),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the header view.
        /// </summary>
        /// <value>The header view.</value>
        public View HeaderView {
            get { return (View)GetValue(HeaderViewProperty); }
            set { SetValue(HeaderViewProperty, value); }
        }

        /// <summary>
        /// The footer view property.
        /// </summary>
        public static BindableProperty FooterViewProperty =
            BindableProperty.Create(
                nameof(FooterView),
                typeof(View),
                typeof(Section),
                default(View),
                defaultBindingMode: BindingMode.OneWay
            );

        /// <summary>
        /// Gets or sets the footer view.
        /// </summary>
        /// <value>The footer view.</value>
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

        public static BindableProperty TemplateStartIndexProperty =
            BindableProperty.Create(
                nameof(TemplateStartIndex),
                typeof(int),
                typeof(Section),
                default(int),
                defaultBindingMode: BindingMode.OneWay
            );

        public int TemplateStartIndex
        {
            get { return (int)GetValue(TemplateStartIndexProperty); }
            set { SetValue(TemplateStartIndexProperty, value); }
        }

        int templatedItemsCount;

        static void ItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var section = (Section)bindable;

            if (section.ItemTemplate == null)
            {
                return;
            }

            IList oldValueAsEnumerable;
            IList newValueAsEnumerable;
            try
            {
                oldValueAsEnumerable = oldValue as IList;
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

            if (oldValueAsEnumerable != null)
            {
                for (var i = oldValueAsEnumerable.Count - 1; i >= 0; i--)
                {
                    section.RemoveAt(section.TemplateStartIndex + i);
                }
            }

            if (newValueAsEnumerable != null)
            {
                for (var i = 0; i < newValueAsEnumerable.Count; i++)
                {
                    var view = CreateChildViewFor(section.ItemTemplate, newValueAsEnumerable[i], section);
                    section.Insert(section.TemplateStartIndex + i, view);
                }
                section.templatedItemsCount = newValueAsEnumerable.Count;
            }

            var newObservableCollection = newValue as INotifyCollectionChanged;

            if (newObservableCollection != null)
            {
                newObservableCollection.CollectionChanged += section.OnItemsSourceCollectionChanged;
            }

            // Notify manually Collection Reset.
            section.SectionCollectionChanged?.Invoke(section,new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {

                //RemoveAt(e.OldStartingIndex + TemplateStartIndex);

                var item = e.NewItems[0];
                var view = CreateChildViewFor(ItemTemplate, item, this);

                //Insert(e.NewStartingIndex + TemplateStartIndex, view);
                this[e.NewStartingIndex + TemplateStartIndex] = view;
            }

            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    for (var i = 0; i < e.NewItems.Count; i++)
                    {
                        var item = e.NewItems[i];
                        var view = CreateChildViewFor(ItemTemplate, item, this);

                        Insert(i + e.NewStartingIndex + TemplateStartIndex, view);
                        templatedItemsCount++;
                    }
                }
            }

            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    RemoveAt(e.OldStartingIndex + TemplateStartIndex);
                    templatedItemsCount--;
                }
            }

            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                //this.Clear();
                IList source = ItemsSource as IList;
                for (var i = templatedItemsCount - 1; i >= 0; i--)
                {
                    RemoveAt(TemplateStartIndex + i);
                }
                templatedItemsCount = 0;
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
