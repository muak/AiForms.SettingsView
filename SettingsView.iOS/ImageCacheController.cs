using System;
using Foundation;
namespace AiForms.Renderers.iOS
{
    /// <summary>
    /// Image cache controller.
    /// </summary>
    public static class ImageCacheController
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
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

        /// <summary>
        /// Clear this instance.
        /// </summary>
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
