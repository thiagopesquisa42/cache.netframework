using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;

namespace SimpleMemoryCache
{
    public static class Cache
    {
        #region [Get cache item]
        public static ReturnType GetCacheItem<ReturnType>(int regionKey, Func<ReturnType> method)
        {
            string key = CreateKeyRegionMethodParameters(method, regionKey);
            return GetCacheItem(key, method, cacheItemDefaultPolicy);
        }

        public static ReturnType GetCacheItem<Parameter1Type, ReturnType>(int regionKey, 
            Func<Parameter1Type, ReturnType> method, Parameter1Type parameter)
        {
            var key = CreateKeyRegionMethodParameters(method.Method.Name, regionKey, 
                new List<object>() { parameter });
            return GetCacheItem(key,
                () =>
                {
                    return (ReturnType)method.DynamicInvoke(parameter);
                }, cacheItemDefaultPolicy);
        }

        public static ReturnType GetCacheItem<Parameter1Type, Parameter2Type, ReturnType>(
            int regionKey, Func<Parameter1Type, Parameter2Type, ReturnType> method, 
            params object[] parameters)
        {
            var key = CreateKeyRegionMethodParameters(method.Method.Name, regionKey, 
                parameters.ToList());
            return GetCacheItem(key,
                () =>
                {
                    return (ReturnType)method.DynamicInvoke(parameters);
                }, cacheItemDefaultPolicy);
        }

        public static ReturnType GetCacheItem<Parameter1Type, Parameter2Type, Parameter3Type, ReturnType>(
            int regionKey, Func<Parameter1Type, Parameter2Type, Parameter3Type, ReturnType> method,
            params object[] parameters)
        {
            var key = CreateKeyRegionMethodParameters(method.Method.Name, regionKey, parameters.ToList());
            return GetCacheItem(key,
                () =>
                {
                    return (ReturnType)method.DynamicInvoke(parameters);
                }, cacheItemDefaultPolicy);
        }

        public static ReturnType GetCacheItem<Parameter1Type, Parameter2Type, Parameter3Type, Parameter4Type, ReturnType>(
            int regionKey, Func<Parameter1Type, Parameter2Type, Parameter3Type, Parameter4Type, ReturnType> method,
            params object[] parameters)
        {
            var key = CreateKeyRegionMethodParameters(method.Method.Name, regionKey, parameters.ToList());
            return GetCacheItem(key,
                () =>
                {
                    return (ReturnType)method.DynamicInvoke(parameters);
                }, cacheItemDefaultPolicy);
        }
        #endregion

        #region [Set and Get cache item]
        public static ReturnType SetGetCacheItem<ReturnType>(int regionKey, Func<ReturnType> method)
        {
            string key = CreateKeyRegionMethodParameters(method, regionKey);
            return SetGetCacheItem(key, method, cacheItemDefaultPolicy);
        }

        public static ReturnType SetGetCacheItem<Parameter1Type, ReturnType>(
            int regionKey, Func<Parameter1Type, ReturnType> method, Parameter1Type parameter)
        {
            var key = CreateKeyRegionMethodParameters(method.Method.Name, regionKey, new List<object>() { parameter });
            return SetGetCacheItem(key,
                () =>
                {
                    return (ReturnType)method.DynamicInvoke(parameter);
                }, cacheItemDefaultPolicy);
        }

        public static ReturnType SetGetCacheItem<Parameter1Type, Parameter2Type, ReturnType>(
            int regionKey, Func<Parameter1Type, Parameter2Type, ReturnType> method, params object[] parameters)
        {
            var key = CreateKeyRegionMethodParameters(method.Method.Name, regionKey, parameters.ToList());
            return SetGetCacheItem(key,
                () =>
                {
                    return (ReturnType)method.DynamicInvoke(parameters);
                }, cacheItemDefaultPolicy);
        }

        public static ReturnType SetGetCacheItem<Parameter1Type, Parameter2Type, Parameter3Type, ReturnType>(
            int regionKey, Func<Parameter1Type, Parameter2Type, Parameter3Type, ReturnType> method,
            params object[] parameters)
        {
            var key = CreateKeyRegionMethodParameters(method.Method.Name, regionKey, parameters.ToList());
            return SetGetCacheItem(key,
                () =>
                {
                    return (ReturnType)method.DynamicInvoke(parameters);
                }, cacheItemDefaultPolicy);
        }

        public static ReturnType SetGetCacheItem<Parameter1Type, Parameter2Type, Parameter3Type, Parameter4Type, ReturnType>(
            int regionKey, Func<Parameter1Type, Parameter2Type, Parameter3Type, Parameter4Type, ReturnType> method,
            params object[] parameters)
        {
            var key = CreateKeyRegionMethodParameters(method.Method.Name, regionKey, parameters.ToList());
            return SetGetCacheItem(key,
                () =>
                {
                    return (ReturnType)method.DynamicInvoke(parameters);
                }, cacheItemDefaultPolicy);
        }
        #endregion

        #region [Propriedades]
        private static readonly MemoryCache memoryCache = MemoryCache.Default;
        private static readonly CacheItemPolicy cacheItemDefaultPolicy = GetCacheItemDefaultPolicy();
        #endregion

        #region [Métodos privados]
        private static CacheItemPolicy GetCacheItemDefaultPolicy()
        {
            return CreateCacheItemPolicy(GetAbsoluteExpirationItemCacheDefault());
        }

        //TODO: always use this, only set time before call this...
        private static CacheItemPolicy GetCustomCacheItemPolicy(int minutesAbsoluteExpiration)
        {
            if (minutesAbsoluteExpiration <= 0)
            {
                minutesAbsoluteExpiration = (int)GetAbsoluteExpirationItemCacheDefault().TotalMinutes;
            }
            var expirationTimeSpan = new TimeSpan(hours: 0, minutes: minutesAbsoluteExpiration, seconds: 0);
            return CreateCacheItemPolicy(expirationTimeSpan);
        }

        private static TimeSpan GetAbsoluteExpirationItemCacheDefault()
        {
            string minutesString = ConfigurationManager.AppSettings["SimpleMemoryCache:AbsoluteExpirationDefault"];
            if (int.TryParse(minutesString, out int minutesInt))
            {
                return new TimeSpan(hours: 0, minutes: minutesInt, seconds: 0);
            }
            else
            {
                return new TimeSpan(hours: 0, minutes: 30, seconds: 0);
            }
        }

        private static CacheItemPolicy CreateCacheItemPolicy(TimeSpan expirationTimeSpan)
        {
            var now = DateTime.Now;
            var cachePolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = now.Add(expirationTimeSpan)
            };
            return cachePolicy;
        }

        private static T SetGetCacheItem<T>(string key, Func<T> method, CacheItemPolicy cacheItemPolicy)
        {
            var methodResult = method();
            var cacheItem = new CacheItem(key: key, value: methodResult);
            memoryCache.Set(cacheItem, cacheItemPolicy);
            return methodResult;
        }

        private static T GetCacheItem<T>(string key, Func<T> method, CacheItemPolicy cacheItemPolicy)
        {
            var methodResult = memoryCache.Get(key: key);
            if (methodResult == null)
            {
                methodResult = method();
                var cacheItem = new CacheItem(key: key, value: methodResult);
                memoryCache.Set(cacheItem, cacheItemPolicy);
            }
            return (T)methodResult;
        }

        private static string CreateKeyRegionMethodParameters<T>(Func<T> method, int regionKey)
        {
            return CreateKeyRegionMethodParameters(method.Method.Name, regionKey, new List<object>());
        }

        //TODO: receive Func<T> only...
        private static string CreateKeyRegionMethodParameters(string methodNome, int regionKey, List<object> parameters)
        {
            List<object> keyComponents = new List<object> {
                regionKey, methodNome
            };
            if (parameters != null)
            {
                keyComponents.AddRange(parameters);
            }
            var key = string.Join("_", keyComponents);
            return key;
        }
        #endregion
    }
}
