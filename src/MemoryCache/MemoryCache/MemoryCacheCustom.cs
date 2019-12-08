using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;

namespace MemoryCacheCustom
{
    public static class MemoryCacheCustom
    {
        #region [Obter item-cache]
        public static TRetorno ObterItemCache<TRetorno>(
            int regiaoId, Func<TRetorno> metodo)
        {
            string chave = GerarChave(metodo, regiaoId);
            return ObterItemCache(chave, metodo, politicaItemCachePadrao);
        }

        public static TRetorno ObterItemCache<TRetorno, TParam1>(
            int regiaoId, Func<TParam1, TRetorno> metodo, TParam1 parametro)
        {
            var chave = GerarChave(metodo.Method.Name, regiaoId, new List<object>() { parametro });
            return ObterItemCache(chave,
                () =>
                {
                    return (TRetorno)metodo.DynamicInvoke(parametro);
                }, politicaItemCachePadrao);
        }

        public static TRetorno ObterItemCache<TRetorno, TParam1, TParam2>(
            int regiaoId, Func<TParam1, TParam2, TRetorno> metodo, params object[] parametros)
        {
            var chave = GerarChave(metodo.Method.Name, regiaoId, parametros.ToList());
            return ObterItemCache(chave,
                () =>
                {
                    return (TRetorno)metodo.DynamicInvoke(parametros);
                }, politicaItemCachePadrao);
        }

        public static TRetorno ObterItemCache<TRetorno, TParam1, TParam2, TParam3>(
            int regiaoId, Func<TParam1, TParam2, TParam3, TRetorno> metodo,
            params object[] parametros)
        {
            var chave = GerarChave(metodo.Method.Name, regiaoId, parametros.ToList());
            return ObterItemCache(chave,
                () =>
                {
                    return (TRetorno)metodo.DynamicInvoke(parametros);
                }, politicaItemCachePadrao);
        }

        public static TRetorno ObterItemCache<TRetorno, TParam1, TParam2, TParam3, TParam4>(
            int regiaoId, Func<TParam1, TParam2, TParam3, TParam4, TRetorno> metodo,
            params object[] parametros)
        {
            var chave = GerarChave(metodo.Method.Name, regiaoId, parametros.ToList());
            return ObterItemCache(chave,
                () =>
                {
                    return (TRetorno)metodo.DynamicInvoke(parametros);
                }, politicaItemCachePadrao);
        }
        #endregion

        #region [Atualizar o item-cache e Obter item-cache]
        public static TRetorno ObterAtualizarItemCache<TRetorno>(int regiaoId, Func<TRetorno> metodo)
        {
            string chave = GerarChave(metodo, regiaoId);
            return ObterAtualizarItemCache(chave, metodo, politicaItemCachePadrao);
        }

        public static TRetorno ObterAtualizarItemCache<TRetorno, TParam1>(
            int regiaoId, Func<TParam1, TRetorno> metodo, TParam1 parametro)
        {
            var chave = GerarChave(metodo.Method.Name, regiaoId, new List<object>() { parametro });
            return ObterAtualizarItemCache(chave,
                () =>
                {
                    return (TRetorno)metodo.DynamicInvoke(parametro);
                }, politicaItemCachePadrao);
        }

        public static TRetorno ObterAtualizarItemCache<TRetorno, TParam1, TParam2>(
            int regiaoId, Func<TParam1, TParam2, TRetorno> metodo, params object[] parametros)
        {
            var chave = GerarChave(metodo.Method.Name, regiaoId, parametros.ToList());
            return ObterAtualizarItemCache(chave,
                () =>
                {
                    return (TRetorno)metodo.DynamicInvoke(parametros);
                }, politicaItemCachePadrao);
        }

        public static TRetorno ObterAtualizarItemCache<TRetorno, TParam1, TParam2, TParam3>(
            int regiaoId, Func<TParam1, TParam2, TParam3, TRetorno> metodo,
            params object[] parametros)
        {
            var chave = GerarChave(metodo.Method.Name, regiaoId, parametros.ToList());
            return ObterAtualizarItemCache(chave,
                () =>
                {
                    return (TRetorno)metodo.DynamicInvoke(parametros);
                }, politicaItemCachePadrao);
        }

        public static TRetorno ObterAtualizarItemCache<TRetorno, TParam1, TParam2, TParam3, TParam4>(
            int regiaoId, Func<TParam1, TParam2, TParam3, TParam4, TRetorno> metodo,
            params object[] parametros)
        {
            var chave = GerarChave(metodo.Method.Name, regiaoId, parametros.ToList());
            return ObterAtualizarItemCache(chave,
                () =>
                {
                    return (TRetorno)metodo.DynamicInvoke(parametros);
                }, politicaItemCachePadrao);
        }
        #endregion

        #region [Propriedades]
        private static MemoryCache cachePadrao = MemoryCache.Default;
        private static CacheItemPolicy politicaItemCachePadrao = ObterPoliticaItemCachePadrao();
        private static TimeSpan TEMPO_EXPIRAR_CACHE_PADRAO
        {
            get
            {
                string tempoMinutosString = ConfigurationManager.AppSettings["TEMPO_MINUTOS_EXPIRAR_CACHE"];
                if (int.TryParse(tempoMinutosString, out int tempoMinutos))
                {
                    return new TimeSpan(hours: 0, minutes: tempoMinutos, seconds: 0);
                }
                else
                {
                    return new TimeSpan(hours: 0, minutes: 30, seconds: 0);
                }
            }
        }
        #endregion

        #region [Métodos privados]
        private static CacheItemPolicy ObterPoliticaItemCachePadrao()
        {
            return ObterPoliticaItemCacheCustomizada(TEMPO_EXPIRAR_CACHE_PADRAO);
        }

        private static CacheItemPolicy ObterPoliticaItemCachePadraoCustomizada(int tempoMinutosExpirarCache)
        {
            if (tempoMinutosExpirarCache <= 0)
            {
                tempoMinutosExpirarCache = (int)TEMPO_EXPIRAR_CACHE_PADRAO.TotalMinutes;
            }
            var tempoExpirarCache = new TimeSpan(hours: 0, minutes: tempoMinutosExpirarCache, seconds: 0);
            return ObterPoliticaItemCacheCustomizada(tempoExpirarCache);
        }

        private static CacheItemPolicy ObterPoliticaItemCacheCustomizada(TimeSpan tempoExpirarCache)
        {
            var dataAtual = DateTime.Now;
            var cachePolicy = new CacheItemPolicy();
            cachePolicy.AbsoluteExpiration = dataAtual.Add(tempoExpirarCache);
            return cachePolicy;
        }

        private static T ObterAtualizarItemCache<T>(string chave, Func<T> metodo, CacheItemPolicy politicaItemCache)
        {
            var objeto = metodo();
            var cacheItem = new CacheItem(key: chave, value: objeto);
            cachePadrao.Set(cacheItem, politicaItemCache);
            return objeto;
        }

        private static T ObterItemCache<T>(string chave, Func<T> metodo, CacheItemPolicy politicaItemCache)
        {
            var objeto = cachePadrao.Get(key: chave);
            if (objeto == null)
            {
                objeto = metodo();
                var cacheItem = new CacheItem(key: chave, value: objeto);
                cachePadrao.Set(cacheItem, politicaItemCache);
            }
            return (T)objeto;
        }

        private static string GerarChave<T>(Func<T> metodo, int regiaoId)
        {
            return GerarChave(metodo.Method.Name, regiaoId, new List<object>());
        }

        private static string GerarChave(string metodoNome, int regiaoId, List<object> parametros)
        {
            List<object> itemsAgregar = new List<object> {
                regiaoId, metodoNome
            };
            if (parametros != null)
            {
                itemsAgregar.AddRange(parametros);
            }
            var chave = string.Join("_", itemsAgregar);
            return chave;
        }
        #endregion
    }
}