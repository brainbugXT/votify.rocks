﻿using System;
using System.Runtime.Caching;

namespace Votify.Rocks.Service
{
    public class MemoryCacheObject : ICacheObject
    {
        private readonly TimeSpan _expiry;
        private readonly ObjectCache _cache = MemoryCache.Default;

        public MemoryCacheObject(TimeSpan expiry)
        {
            _expiry = expiry;
        }

        public T GetCachedObject<T>(string key, T defaultValue)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                var cacheItem = _cache.GetCacheItem(key.ToLower());
                if (cacheItem?.Value != null)
                {
                    return (T)cacheItem.Value;
                }
            }
            return defaultValue;
        }

        public void SetCachedObject<T>(string key, T item)
        {
            SetCachedObject(key, item, _expiry);
        }

        public void SetCachedObject<T>(string key, T item, TimeSpan expiry)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            _cache.Set(key.ToLower(), item, new CacheItemPolicy { SlidingExpiration = _expiry });
        }
    }
}
