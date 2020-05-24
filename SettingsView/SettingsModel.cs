using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Linq;
using System.Collections.Generic;

namespace AiForms.Renderers
{
	/// <summary>
	/// Settings model.
	/// </summary>
	public class SettingsModel : TableModel
	{
		static readonly BindableProperty _pathProperty = BindableProperty.Create("Path", typeof(Tuple<int, int>), typeof(Cell), null);

		SettingsRoot _root;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:AiForms.Renderers.SettingsModel"/> class.
		/// </summary>
		/// <param name="settingsRoot">Settings root.</param>
		public SettingsModel(SettingsRoot settingsRoot)
		{
			_root = settingsRoot;
		}

		/// <summary>
		/// Gets the cell.
		/// </summary>
		/// <returns>The cell.</returns>
		/// <param name="section">Section.</param>
		/// <param name="row">Row.</param>
		public override Cell GetCell(int section, int row)
		{
			var cell = (Cell) GetItem(section, row);
			SetPath(cell, new Tuple<int, int>(section, row));
			return cell;
		}

		/// <summary>
		/// Gets the item.
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="section">Section.</param>
		/// <param name="row">Row.</param>
		public override object GetItem(int section, int row)
		{
			return _root.ElementAt(section)[row];
		}

		/// <summary>
		/// Gets the row count.
		/// </summary>
		/// <returns>The row count.</returns>
		/// <param name="section">Section.</param>
		public override int GetRowCount(int section)
		{
			return _root.ElementAt(section).Count;
		}

		/// <summary>
		/// Gets the section count.
		/// </summary>
		/// <returns>The section count.</returns>
		public override int GetSectionCount()
		{
			return _root.Count();
		}

		/// <summary>
		/// Gets the section.
		/// </summary>
		/// <returns>The section.</returns>
		/// <param name="section">Section.</param>
		public virtual Section GetSection(int section)
		{
			return _root.ElementAtOrDefault(section);
		}

		/// <summary>
		/// Gets the section from cell.
		/// </summary>
		/// <returns>The section from cell.</returns>
		/// <param name="cell">Cell.</param>
		public virtual Section GetSectionFromCell(Cell cell)
		{
			return _root.FirstOrDefault(x => x.Contains(cell));
		}

		/// <summary>
		/// Gets the index of the section.
		/// </summary>
		/// <returns>The section index.</returns>
		/// <param name="section">Section.</param>
		public virtual int GetSectionIndex(Section section)
		{
			return _root.IndexOf(section);
		}

		/// <summary>
		/// Gets the section title.
		/// </summary>
		/// <returns>The section title.</returns>
		/// <param name="section">Section.</param>
		public override string GetSectionTitle(int section)
		{
			return _root.ElementAt(section).Title;
		}


		/// <summary>
		/// Gets the section header view.
		/// </summary>
		/// <returns>The section header view.</returns>
		/// <param name="section">Section.</param>
		public virtual View GetSectionHeaderView(int section)
		{
			return _root.ElementAt(section).HeaderView;
		}

		/// <summary>
		/// Gets the footer text.
		/// </summary>
		/// <returns>The footer text.</returns>
		/// <param name="section">Section.</param>
		public virtual string GetFooterText(int section)
		{
			return _root.ElementAt(section).FooterText;
		}

		/// <summary>
		/// Gets the section footer view.
		/// </summary>
		/// <returns>The section footer view.</returns>
		/// <param name="section">Section.</param>
		public virtual View GetSectionFooterView(int section)
		{
			return _root.ElementAt(section).FooterView;
		}

		/// <summary>
		/// Ons the row selected.
		/// </summary>
		/// <param name="item">Item.</param>
		protected override void OnRowSelected(object item)
		{
			base.OnRowSelected(item);

			( item as CellBase )?.OnTapped();
		}

		/// <summary>
		/// Gets the height of the header.
		/// </summary>
		/// <returns>The header height.</returns>
		/// <param name="section">Section.</param>
		public virtual double GetHeaderHeight(int section)
		{
			return _root.ElementAt(section).HeaderHeight;
		}


		// this method no longer uses except for iOS.CellBaseRenderer.
		internal static Tuple<int, int> GetPath(Cell item)
		{
			if ( item == null )
				throw new ArgumentNullException("item");

			return (Tuple<int, int>) item.GetValue(_pathProperty);
		}


		static void SetPath(Cell item, Tuple<int, int> index)
		{
			if ( item == null )
				return;

			item.SetValue(_pathProperty, index);
		}
	}
}
