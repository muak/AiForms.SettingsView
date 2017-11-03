using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using UIKit;
using CoreGraphics;
using System.Reflection;
using System.Linq.Expressions;

namespace AiForms.Renderers.iOS
{
    /// <summary>
    /// Cell base renderer.
    /// </summary>
    public class CellBaseRenderer<TnativeCell> : CellRenderer where TnativeCell : CellBaseView
    {
        /// <summary>
        /// Refer to 
        /// http://qiita.com/Temarin/items/d6f00428743b0971ec95
        /// http://neue.cc/2014/09/16_478.html
        /// </summary>
        internal static class InstanceCreator<T1, TInstance>
        {
            public static Func<T1, TInstance> Create { get; } = CreateInstance();

            private static Func<T1, TInstance> CreateInstance()
            {
                var constructor = typeof(TInstance).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder,
                    new[] { typeof(T1) }, null);
                var arg1 = Expression.Parameter(typeof(T1));
                return Expression.Lambda<Func<T1, TInstance>>(Expression.New(constructor, arg1), arg1).Compile();
            }
        }

        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        /// <param name="item">Item.</param>
        /// <param name="reusableCell">Reusable cell.</param>
        /// <param name="tv">Tv.</param>
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            TnativeCell nativeCell = reusableCell as TnativeCell;
            if (nativeCell == null) {
                nativeCell = InstanceCreator<Cell, TnativeCell>.Create(item);
            }
            nativeCell.Cell = item;

            SetUpPropertyChanged(nativeCell);

            nativeCell.UpdateCell();

            return nativeCell;
        }

        /// <summary>
        /// Sets up property changed.
        /// </summary>
        /// <param name="nativeCell">Native cell.</param>
        protected void SetUpPropertyChanged(CellBaseView nativeCell)
        {
            var formsCell = nativeCell.Cell as CellBase;
            var parentElement = formsCell.Parent as SettingsView;

            formsCell.PropertyChanged -= nativeCell.CellPropertyChanged;
            formsCell.PropertyChanged += nativeCell.CellPropertyChanged;

            if (parentElement != null) {
                parentElement.PropertyChanged -= nativeCell.ParentPropertyChanged;
                parentElement.PropertyChanged += nativeCell.ParentPropertyChanged;
            }
        }
    }
}
