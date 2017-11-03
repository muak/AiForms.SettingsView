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
        static readonly BindableProperty PathProperty = BindableProperty.Create("Path", typeof(Tuple<int, int>), typeof(Cell), null);

        SettingsRoot _root;
        IEnumerable<Section> _visibleSections;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AiForms.Renderers.SettingsModel"/> class.
        /// </summary>
        /// <param name="settingsRoot">Settings root.</param>
        public SettingsModel(SettingsRoot settingsRoot)
        {
            _root = settingsRoot;
            _visibleSections = _root.Where(x => x.IsVisible);
        }

        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="section">Section.</param>
        /// <param name="row">Row.</param>
        public override Cell GetCell(int section, int row)
        {
            var cell = (Cell)GetItem(section, row);
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
            return _visibleSections.ElementAt(section)[row];
        }

        /// <summary>
        /// Gets the row count.
        /// </summary>
        /// <returns>The row count.</returns>
        /// <param name="section">Section.</param>
        public override int GetRowCount(int section)
        {
            return _visibleSections.ElementAt(section).Count;
        }

        /// <summary>
        /// Gets the section count.
        /// </summary>
        /// <returns>The section count.</returns>
        public override int GetSectionCount()
        {
            return _visibleSections.Count();
        }

        /// <summary>
        /// Gets the section title.
        /// </summary>
        /// <returns>The section title.</returns>
        /// <param name="section">Section.</param>
        public override string GetSectionTitle(int section)
        {
            return _visibleSections.ElementAt(section).Title;
        }

        /// <summary>
        /// Gets the footer text.
        /// </summary>
        /// <returns>The footer text.</returns>
        /// <param name="section">Section.</param>
        public virtual string GetFooterText(int section)
        {
            return _visibleSections.ElementAt(section).FooterText;
        }

        /// <summary>
        /// Ons the row selected.
        /// </summary>
        /// <param name="item">Item.</param>
        protected override void OnRowSelected(object item)
        {
            base.OnRowSelected(item);

            (item as CellBase)?.OnTapped();
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
