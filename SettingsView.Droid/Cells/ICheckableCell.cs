using System;
namespace AiForms.Renderers.Droid
{
    /// <summary>
    /// Checkable cell.
    /// </summary>
    [Android.Runtime.Preserve(AllMembers = true)]
    public interface ICheckableCell
    {
        /// <summary>
        /// Checks the change.
        /// </summary>
        void CheckChange();
    }
}
