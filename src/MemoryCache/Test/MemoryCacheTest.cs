using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;

namespace Test
{
    [TestClass]
    public class MemoryCacheTest
    {
        [TestInitialize]
        public void IniciarTeste()
        {
            ConfigurationManager.AppSettings.Set("TEMPO_MINUTOS_EXPIRAR_CACHE", "2");
        }

        [TestMethod]
        public void AcessarCachePrimeiraVez()
        {
            var numero1 = 1F;
            var numero2 = 2.5F;
            var soma = MemoryCacheCustomMock.ObterSoma2Parametros(numero1, numero2);
            var primeiroVezUsandoCache = MemoryCacheCustomMock.ObterSoma2ParametrosComCache(numero1, numero2);

            Assert.AreEqual(soma, primeiroVezUsandoCache);
        }

        [TestMethod]
        public void ObterItemPelaCache()
        {
            var numero1 = 1F;
            var numero2 = 2.5F;
            var primeiroVezUsandoCache = MemoryCacheCustomMock.ObterSoma2ParametrosComCache(numero1, numero2);
            System.Threading.Thread.Sleep(500);
            var segundaVezUsandoCache = MemoryCacheCustomMock.ObterSoma2ParametrosComCache(numero1, numero2);

            Assert.AreEqual(primeiroVezUsandoCache, segundaVezUsandoCache);
        }

        [TestMethod]
        public void ObterItemCacheExpirada()
        {
            ConfigurationManager.AppSettings.Set("TEMPO_MINUTOS_EXPIRAR_CACHE", "1");
            var criandoItemNaCache = MemoryCacheCustomMock.ObterDataAtualComCache();
            System.Threading.Thread.Sleep(new TimeSpan(hours: 0, minutes: 0, seconds: 30));
            var antesExpirarCache = MemoryCacheCustomMock.ObterDataAtualComCache();
            System.Threading.Thread.Sleep(new TimeSpan(hours: 0, minutes: 0, seconds: 31));
            var aposExpirarCache = MemoryCacheCustomMock.ObterDataAtualComCache();

            Assert.AreNotEqual(antesExpirarCache, aposExpirarCache);
        }

        [TestMethod]
        public void ObterItemCacheExpirada_SincroniaOk()
        {
            ConfigurationManager.AppSettings.Set("TEMPO_MINUTOS_EXPIRAR_CACHE", "1");
            var criandoItemNaCache = MemoryCacheCustomMock.ObterDataAtualComCache();
            System.Threading.Thread.Sleep(new TimeSpan(hours: 0, minutes: 0, seconds: 30));
            var antesExpirarCache = MemoryCacheCustomMock.ObterDataAtualComCache();
            System.Threading.Thread.Sleep(new TimeSpan(hours: 0, minutes: 0, seconds: 31));
            var aposExpirarCache = MemoryCacheCustomMock.ObterDataAtualComCache();

            var diferenca = aposExpirarCache - antesExpirarCache;
            var esperadoDiferenca = new TimeSpan(hours: 0, minutes: 1, seconds: 1);
            Assert.AreEqual((int)esperadoDiferenca.TotalSeconds, (int)diferenca.TotalSeconds);
        }

        [TestMethod]
        public void ObterItemCacheSemExpirar_SincroniaOk()
        {
            ConfigurationManager.AppSettings.Set("TEMPO_MINUTOS_EXPIRAR_CACHE", "1");
            var criandoItemNaCache = MemoryCacheCustomMock.ObterDataAtualComCache();
            System.Threading.Thread.Sleep(new TimeSpan(hours: 0, minutes: 0, seconds: 30));
            var antesExpirarCache = MemoryCacheCustomMock.ObterDataAtualComCache();

            var diferenca = antesExpirarCache - criandoItemNaCache;
            var esperadoDiferenca = new TimeSpan(hours: 0, minutes: 0, seconds: 0);
            Assert.AreEqual((int)esperadoDiferenca.TotalSeconds, (int)diferenca.TotalSeconds);
        }
    }
}
