using System;
using Android.Runtime;
using Android.Util;
using Android.Graphics;

namespace AiForms.Renderers.Droid
{
    public class MemoryLimitedLruCache:LruCache
    {
        public MemoryLimitedLruCache(int size):base(size){}

        protected override int SizeOf(Java.Lang.Object key, Java.Lang.Object value)
        {
            return (value as Bitmap).ByteCount;
        }
    }
}
