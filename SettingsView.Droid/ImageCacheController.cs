using System;

namespace AiForms.Renderers.Droid
{
    public static class ImageCacheController
    {
        public static MemoryLimitedLruCache Instance
        {
            get {
                if (_CacheInstance == null) {
                    _CacheInstance = new MemoryLimitedLruCache(CacheSize);
                    SettingsView._clearCache = Clear;
                }
                return _CacheInstance;
            }
        }

        public static void Clear()
        {
            _CacheInstance?.EvictAll();
            _CacheInstance?.Dispose();
            _CacheInstance = null;
            SettingsView._clearCache = null;
        }

        static readonly int CacheSize = (int)(Java.Lang.Runtime.GetRuntime().MaxMemory() / 1024 / 8);
        static MemoryLimitedLruCache _CacheInstance;
    }
}
