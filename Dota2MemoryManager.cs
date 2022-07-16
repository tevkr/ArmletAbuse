using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ArmletAbuse
{
    internal class Dota2MemoryManager
    {
        private const int _itemArmletId = 151;
        private static int _armletItemsTabIndex = -1;
        public enum ItemArmletStates
        {
            Disabled = 6,
            Enabled = 13
        }
        private static Process _dota2Process;
        private static IntPtr _dota2HProc;
        private static List<Dota2Offset> dota2Offsets;
        private static IntPtr _hpAddress;
        private static int _itemIdsAddressesStep = 0x280;
        private static List<IntPtr> _itemIdsAddresses;
        private static int _itemStatesAddressesStep = 0x68;
        private static List<IntPtr> _itemStatesAddresses;
        private static int _buttonsAddressesStep = 0xC0;
        private static List<IntPtr> _buttonsAddresses;
        static Dota2MemoryManager()
        {
            dota2Offsets = new List<Dota2Offset>();
            _itemIdsAddresses = new List<IntPtr>();
            _itemStatesAddresses = new List<IntPtr>();
            _buttonsAddresses = new List<IntPtr>();
            dota2Offsets.Add(new Dota2Offset("hp", "server.dll", 0x03627228, new List<int> { 0x0, 0x38, 0x30, 0x2A8 }));
            dota2Offsets.Add(new Dota2Offset("firstItemId", "engine2.dll", 0x0050ACA0, new List<int> { 0x220, 0x10, 0x2D8, 0x40, 0x8, 0x78, 0xE0 }));
            dota2Offsets.Add(new Dota2Offset("firstItemButton", "client.dll", 0x03A92B10, new List<int> { 0x0, 0x0 }));
        }
        public static void SetUp()
        {
            FindAndSetHpAddress();
            FindAndSetItemIdsAddresses();
            FindAndSetItemStatesAddresses();
            FindAndSetButtonsAddresses();
        }
        public static void Clear()
        {
            _hpAddress = IntPtr.Zero;
            _itemIdsAddresses = new List<IntPtr>();
            _itemStatesAddresses = new List<IntPtr>();
            _buttonsAddresses = new List<IntPtr>();
        }
        public static void InGameChecker()
        {
            while (Config.isDota2Opened)
            {
                Thread.Sleep(1000);
                FindAndSetHpAddress();
                Dota2Offset hpData = dota2Offsets.Find(o => o.name == "hp");
                Config.isInGame = _hpAddress != IntPtr.Zero && _hpAddress != (IntPtr)hpData.offsets.Last();
            }
        }
        public static void WaitForDota2Process()
        {
            while (!Config.isDota2Opened)
            {
                _dota2Process = Dota2MemoryManager.GetDota2Process();
                if (_dota2Process != null)
                {
                    _dota2HProc = MemoryManager.OpenProcess(MemoryManager.ProcessAccessFlags.All, false, _dota2Process.Id);
                    Config.isDota2Opened = true;
                    _dota2Process.EnableRaisingEvents = true;
                    _dota2Process.Exited += new EventHandler(Dota2Exited);
                }
            }
        }
        private static void Dota2Exited(object sender, System.EventArgs e)
        {
            Config.isDota2Opened = false;
            _dota2Process = null;
        }
        private static void FindAndSetHpAddress()
        {
            Dota2Offset hpData = dota2Offsets.Find(o => o.name == "hp");
            if (hpData.moduleAddress == IntPtr.Zero)
            {
                IntPtr moduleAddress = MemoryManager.GetModuleBaseAddress(_dota2Process, hpData.moduleName);
                hpData.moduleAddress = moduleAddress;
            }
            _hpAddress = MemoryManager.FindDMAAddy(_dota2HProc, (IntPtr)(hpData.moduleAddress + hpData.moduleOffset), hpData.offsets.ToArray());
        }
        private static void FindAndSetItemIdsAddresses()
        {
            Dota2Offset firstItemIdData = dota2Offsets.Find(o => o.name == "firstItemId");
            if (firstItemIdData.moduleAddress == IntPtr.Zero)
            {
                IntPtr moduleAddress = MemoryManager.GetModuleBaseAddress(_dota2Process, firstItemIdData.moduleName);
                firstItemIdData.moduleAddress = moduleAddress;
            }
            IntPtr firstItemIdAddress = MemoryManager.FindDMAAddy(_dota2HProc, (IntPtr)(firstItemIdData.moduleAddress + firstItemIdData.moduleOffset), firstItemIdData.offsets.ToArray());
            _itemIdsAddresses.Clear();
            for (int i = 0, step = 0x0; i < 6; i++, step += _itemIdsAddressesStep)
            {
                _itemIdsAddresses.Add(firstItemIdAddress + step);
            }
        }
        private static void FindAndSetItemStatesAddresses()
        {
            _itemStatesAddresses.Clear();
            for (int i = 0; i < 6; i++)
            {
                _itemStatesAddresses.Add(_itemIdsAddresses[i] - _itemStatesAddressesStep);
            }
        }
        private static void FindAndSetButtonsAddresses()
        {
            Dota2Offset firstItemButtonData = dota2Offsets.Find(o => o.name == "firstItemButton");
            if (firstItemButtonData.moduleAddress == IntPtr.Zero)
            {
                IntPtr moduleAddress = MemoryManager.GetModuleBaseAddress(_dota2Process, firstItemButtonData.moduleName);
                firstItemButtonData.moduleAddress = moduleAddress;
            }
            IntPtr firstItemButtonAddress = MemoryManager.FindDMAAddy(_dota2HProc, (IntPtr)(firstItemButtonData.moduleAddress + firstItemButtonData.moduleOffset), firstItemButtonData.offsets.ToArray());
            _buttonsAddresses.Clear();
            for (int i = 0, step = 0x0; i < 6; i++, step += _buttonsAddressesStep)
            {
                _buttonsAddresses.Add(firstItemButtonAddress + step);
            }
        }
        private static Process GetDota2Process()
        {
            return Process.GetProcessesByName("dota2").FirstOrDefault();
        }
        public static int GetHP()
        {
            if (_hpAddress == IntPtr.Zero) return -1;
            byte[] buffer = new byte[4];
            MemoryManager.ReadProcessMemory(_dota2HProc, _hpAddress, buffer, buffer.Length, out _);
            int hp = BitConverter.ToInt32(buffer, 0);
            return hp;
        }
        public static void FindAndSetArmletItemsTabIndex()
        {
            for (int i = 0; i < _itemIdsAddresses.Count; i++)
            {
                if (_itemIdsAddresses[i] == IntPtr.Zero)
                {
                    _armletItemsTabIndex = -1;
                    break;
                }
                byte[] buffer = new byte[4];
                MemoryManager.ReadProcessMemory(_dota2HProc, _itemIdsAddresses[i], buffer, buffer.Length, out _);
                int itemId = BitConverter.ToInt32(buffer, 0);
                if (itemId == _itemArmletId)
                {
                    _armletItemsTabIndex = i;
                    break;
                }
            }
        }
        public static int GetArmletState()
        {
            if (_armletItemsTabIndex == -1) return -1;
            IntPtr armletStateAddress = _itemStatesAddresses.ElementAt(_armletItemsTabIndex);
            byte[] buffer = new byte[4];
            MemoryManager.ReadProcessMemory(_dota2HProc, armletStateAddress, buffer, buffer.Length, out _);
            int armletState = BitConverter.ToInt32(buffer, 0);
            return armletState;
        }
        public static string GetArmletButton()
        {
            if (_armletItemsTabIndex == -1) return "Error";
            IntPtr armletButtonAddress = _buttonsAddresses.ElementAt(_armletItemsTabIndex);
            byte[] buffer = new byte[8];
            MemoryManager.ReadProcessMemory(_dota2HProc, armletButtonAddress, buffer, buffer.Length, out _);
            string armletButton = Encoding.ASCII.GetString(buffer);
            armletButton = armletButton.Replace("\0", string.Empty);
            return armletButton;
        }
        private class Dota2Offset
        {
            public Dota2Offset(string name, string moduleName, int moduleOffset, List<int> offsets)
            {
                this.name = name;
                this.moduleName = moduleName;
                this.moduleAddress = IntPtr.Zero;
                this.moduleOffset = moduleOffset;
                this.offsets = offsets;
            }
            public Dota2Offset(string name, string moduleName, IntPtr moduleAddress, int moduleOffset, List<int> offsets)
            {
                this.name = name;
                this.moduleName = moduleName;
                this.moduleAddress = moduleAddress;
                this.moduleOffset = moduleOffset;
                this.offsets = offsets;
            }
            public string name { get; set; }
            public string moduleName { get; set; }
            public IntPtr moduleAddress { get; set; }
            public int moduleOffset { get; set; }
            public List<int> offsets { get; set; }
        }
    }
}
