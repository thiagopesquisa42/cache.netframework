using MemoryCacheCustom;
using static MemoryCacheCustom.MemoryCacheCustom;

namespace Test
{
    internal static class MemoryCacheCustomMock
    {
        #region [Somar 2 números]
        public static float ObterSoma2Parametros(float numero1, float numero2)
        {
            return numero1 + numero2;
        }

        public static float ObterSoma2ParametrosComCache(float numero1, float numero2)
        {
            return ObterItemCache<float, float, float>(
                regiaoId: 1, ObterSoma2Parametros, numero1, numero2);
        }
        #endregion

        #region [Obter DataAtual]
        public static System.DateTime ObterDataAtual()
        {
            return System.DateTime.Now;
        }

        public static System.DateTime ObterDataAtualComCache()
        {
            return ObterItemCache(regiaoId: 1, ObterDataAtual);
        }
        #endregion

    }
}