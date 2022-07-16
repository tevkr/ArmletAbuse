using System;
using System.Threading.Tasks;

namespace ArmletAbuse
{
    internal class Config
    {
        public static event Action<bool> dota2OpenStateChanged;
        public static event Action<bool> inGameChanged;
        public static event Action<bool> enabledChanged;
        public static event Action<int> minHPChanged;

        private static bool _isDota2Opened = false;
        public static bool isDota2Opened
        {
            get { return _isDota2Opened; }
            set
            {
                if (value != _isDota2Opened)
                {
                    _isDota2Opened = value;
                    dota2OpenStateChanged?.Invoke(_isDota2Opened);
                }
            }
        }
        private static bool _isInGame = false;
        public static bool isInGame
        {
            get { return _isInGame; }
            set
            {
                if (value != _isInGame)
                {
                    _isInGame = value;
                    inGameChanged?.Invoke(_isInGame);
                }
            }
        }
        private static bool _enabled = false;
        public static bool enabled
        {
            get { return _enabled; }
            set
            {
                if (value != _enabled)
                {
                    _enabled = value;
                    enabledChanged?.Invoke(_enabled);
                }
            }
        }
        private static int _minHP = 350;
        public static int minHP
        {
            get { return _minHP; }
            set
            {
                if (value != _minHP)
                {
                    _minHP = value;
                    minHPChanged?.Invoke(_minHP);
                }
            }
        }
        public Config()
        {
            dota2OpenStateChanged += Dota2OpenStateChangedHandler;
            inGameChanged += InGameChangedHandler;
        }
        public Task Start()
        {
            return Task.Run(() =>
            {
                Dota2MemoryManager.WaitForDota2Process();
            });
        }
        private void Dota2OpenStateChangedHandler(bool openState)
        {
            if (openState)
            {
                Task.Run(() =>
                {
                    Dota2MemoryManager.InGameChecker();
                });
            }
            else 
            {
                Task.Run(() =>
                {
                    Dota2MemoryManager.WaitForDota2Process();
                });
            }
        }
        private void InGameChangedHandler(bool inGame)
        {
            if (inGame)
            {
                Dota2MemoryManager.SetUp();
            }
            else
            {
                Dota2MemoryManager.Clear();
            }
        }
    }
}
