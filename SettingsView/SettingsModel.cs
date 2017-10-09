using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Linq;
using System.Collections.Generic;

namespace AiForms.Renderers
{
	public class SettingsModel:TableModel
	{
		static readonly BindableProperty PathProperty = BindableProperty.Create("Path", typeof(Tuple<int, int>), typeof(Cell), null);

		SettingsRoot _root;
        IEnumerable<Section> _visibleSections;

		public SettingsModel(SettingsRoot settingsRoot)
		{
			_root = settingsRoot;
            _visibleSections = _root.Where(x=>x.IsVisible);
		}

		public override Cell GetCell(int section, int row)
		{
			var cell = (Cell)GetItem(section, row);
			SetPath(cell, new Tuple<int, int>(section, row));
			return cell;
		}

		public override object GetItem(int section, int row)
		{
            return _visibleSections.ElementAt(section)[row];
		}

		public override int GetRowCount(int section)
		{
            return _visibleSections.ElementAt(section).Count;
		}

		public override int GetSectionCount()
		{
            return _visibleSections.Count();
		}

		public override string GetSectionTitle(int section)
		{
            return _visibleSections.ElementAt(section).Title;
		}

        public virtual string GetFooterText(int section)
        {
            return _visibleSections.ElementAt(section).FooterText;
        }

		protected override void OnRowSelected(object item)
		{
			base.OnRowSelected(item);

			//((Cell)item).OnTapped();
		}

		internal static Tuple<int, int> GetPath(Cell item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			return (Tuple<int, int>)item.GetValue(PathProperty);
		}


		static void SetPath(Cell item, Tuple<int, int> index)
		{
			if (item == null)
				return;

			item.SetValue(PathProperty, index);
		}
	}
}
