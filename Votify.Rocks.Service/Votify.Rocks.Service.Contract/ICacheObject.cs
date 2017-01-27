using System;

namespace Votify.Rocks.Service
{
    public interface ICacheObject
    {
        T GetCachedObject<T>(string key, T defaultValue);
        void SetCachedObject<T>(string key, T item);
        void SetCachedObject<T>(string key, T item, TimeSpan expiry);
    }
}