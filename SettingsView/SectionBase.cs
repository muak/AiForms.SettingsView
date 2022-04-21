using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AiForms.Renderers
{
	public abstract class SectionBase : Element, IList<Cell>, INotifyCollectionChanged
	{
		public static readonly BindableProperty TitleProperty = BindableProperty.Create("Title", typeof(string), typeof(TableSectionBase), null);
		public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TableSectionBase), Color.Default);


		readonly ObservableCollection<Cell> _children = new ObservableCollection<Cell>();

		/// <summary>
		///     Constructs a Section without an empty header.
		/// </summary>
		protected SectionBase()
		{
			_children.CollectionChanged += OnChildrenChanged;
		}

		/// <summary>
		///     Constructs a Section with the specified header.
		/// </summary>
		protected SectionBase(string title)
		{
			if (title == null)
				throw new ArgumentNullException("title");

			Title = title;
			_children.CollectionChanged += OnChildrenChanged;
		}

		public string Title {
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public Color TextColor {
			get { return (Color)GetValue(TextColorProperty); }
			set { SetValue(TextColorProperty, value); }
		}

		public void Add(Cell item)
		{
			_children.Add(item);
		}

		public void Clear()
		{
			_children.Clear();
		}

		public bool Contains(Cell item)
		{
			return _children.Contains(item);
		}

		public void CopyTo(Cell[] array, int arrayIndex)
		{
			_children.CopyTo(array, arrayIndex);
		}

		public int Count {
			get { return _children.Count; }
		}

		bool ICollection<Cell>.IsReadOnly {
			get { return false; }
		}

		public bool Remove(Cell item)
		{
			return _children.Remove(item);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<Cell> GetEnumerator()
		{
			return _children.GetEnumerator();
		}

		public int IndexOf(Cell item)
		{
			return _children.IndexOf(item);
		}

		public void Insert(int index, Cell item)
		{
			_children.Insert(index, item);
		}

		public Cell this[int index] {
			get { return _children[index]; }
			set { _children[index] = value; }
		}

		public void RemoveAt(int index)
		{
			_children.RemoveAt(index);
		}

		public event NotifyCollectionChangedEventHandler CollectionChanged {
			add { _children.CollectionChanged += value; }
			remove { _children.CollectionChanged -= value; }
		}

		public void Add(IEnumerable<Cell> items)
		{
			items.ForEach(_children.Add);
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			object bc = BindingContext;
			foreach (Cell child in _children)
				SetInheritedBindingContext(child, bc);
		}

		void OnChildrenChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			// We need to hook up the binding context for new items.
			if (notifyCollectionChangedEventArgs.NewItems == null)
				return;
			object bc = BindingContext;
			foreach (BindableObject item in notifyCollectionChangedEventArgs.NewItems)
			{
				SetInheritedBindingContext(item, bc);
			}
		}
	}
}
