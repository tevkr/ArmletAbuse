using System.Threading.Tasks;

namespace ArmletAbuse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleInfo consoleInfo = new ConsoleInfo();
            Config config = new Config();
            Abuser abuser = new Abuser();
            Task.WaitAll(consoleInfo.Start(), config.Start(), abuser.Start());
        }
    }
}
