using System;
using Foundation;
namespace AiForms.Renderers.iOS
{
    public static class ImageCacheController
    {
        public static NSCache Instance
        {
            get {
                if (_CacheInstance == null) {
                    _CacheInstance = new NSCache();
                    _CacheInstance.CountLimit = CacheCountLimit;
                    SettingsView._clearCache = Clear;
                }

                return _CacheInstance;
            }
        }

        public static void Clear()
        {
            _CacheInstance?.RemoveAllObjects();
            _CacheInstance?.Dispose();
            _CacheInstance = null;
            SettingsView._clearCache = null;
        }

        static readonly nuint CacheCountLimit = 30;
        static NSCache _CacheInstance;
    }
}
