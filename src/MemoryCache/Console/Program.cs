using static MemoryCacheCustom.MemoryCacheCustom;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var valor = ObterItemCache(42, TestFunction);
            System.Console.WriteLine($"{valor.GetType().Name}: {valor}");
            valor = ObterItemCache(42, TestFunction);
            System.Console.WriteLine($"{valor.GetType().Name}: {valor}");
            valor = ObterAtualizarItemCache(42, TestFunction);
            System.Console.WriteLine($"{valor.GetType().Name}: {valor}");
            valor = ObterItemCache(42, TestFunction);
            System.Console.WriteLine($"{valor.GetType().Name}: {valor}");
            System.Console.ReadLine();
        }

        static int TestFunction()
        {
            System.Console.WriteLine("TestFunction Executed");
            var random = new System.Random();
            return random.Next();
        }
    }
}
