using System;
using System.Threading;
using System.Threading.Tasks;

namespace ArmletAbuse
{
    internal class Abuser
    {
        public Abuser()
        {
            Config.dota2OpenStateChanged += ConfigEventHandler;
            Config.inGameChanged += ConfigEventHandler;
            Config.enabledChanged += ConfigEventHandler;
        }
        public Task Start()
        {
            return Task.Run(() =>
            {
                Abuse();
            });
        }
        private static void Abuse()
        {
            int prevHp = 1000000;
            while(Config.isDota2Opened && Config.isInGame && Config.enabled)
            {
                try
                {
                    int hp = Dota2MemoryManager.GetHP();
                    if (hp >= prevHp)
                    {
                        prevHp = hp;
                        continue;
                    }
                    if (hp > 0 && hp < Config.minHP)
                    {
                        Dota2MemoryManager.FindAndSetArmletItemsTabIndex();
                        int armletState = Dota2MemoryManager.GetArmletState();
                        string armletButton = Dota2MemoryManager.GetArmletButton();
                        if (armletState == (int)Dota2MemoryManager.ItemArmletStates.Enabled)
                        {
                            KeyboardManager.PressButton(armletButton, 2);
                        }
                        else if (armletState == (int)Dota2MemoryManager.ItemArmletStates.Disabled)
                        {
                            KeyboardManager.PressButton(armletButton, 1);
                        }
                        Thread.Sleep(700);
                    }
                    prevHp = hp;
                }
                catch { break; }
            }
        }
        
        private static void ConfigEventHandler(bool state)
        {
            if (Config.isDota2Opened && Config.isInGame && Config.enabled)
            {
                Task.Run(() =>
                {
                    Abuse();
                });
            }
        }
    }
}
