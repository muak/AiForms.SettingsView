using System;
using Foundation;

namespace AiForms.Renderers.iOS.Extensions
{
    public static class NSObjectExtension
    {
        /// <summary>
        /// https://stackoverflow.com/questions/25532870/xamarin-ios-memory-leaks-everywhere
        /// </summary>
        /// <param name="target"></param>
        public static void DisposeIfDisposable(this NSObject target)
        {
            if(target == null)
            {
                return;
            }

            if(!target.IsDisposed())
            {
                target.Dispose();
            }
        }

        public static bool IsDisposed(this NSObject target)
        {
            return target.Handle == IntPtr.Zero;
        }
    }
}
