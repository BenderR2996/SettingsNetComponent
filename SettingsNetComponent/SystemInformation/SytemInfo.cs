using System;
using System.Collections.Generic;
using System.Linq;

namespace SettingsNetComponent.SystemInformation
{
    /// <summary>
    /// Системная информация
    /// </summary>
    /// <remarks>
    /// Сведения о клиентской машине
    /// </remarks>
    internal class SystemInfo : ISystemInfo
    {
        #region IP & MAC
        public struct MacIpPair
        {
            public string MacAddress;
            public string IpAddress;
        }

        /// <summary>
        /// Получить все пары (ip,mac) адресов существующих на хосте сетевых интерфейсов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MacIpPair> GetIPMacPairs()
        {
            var nacs = new Win32_SystemParameters<Win32_NetworkAdapterConfiguration>().GetInfo();
            foreach (Win32_NetworkAdapterConfiguration nac in nacs)
            {
                if (nac.IPAddress != null)
                    yield return new MacIpPair { IpAddress = nac.IPAddress.First(), MacAddress = nac.MACAddress };
            }
        }
        /// <summary>
        /// Возвращает mac-адрес по ip-адресу, если такой есть.
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public string GetMacByIp(string ip)
        {
            foreach (var pairIpMac in GetIPMacPairs())
            {
                if (pairIpMac.IpAddress == ip)
                {
                    return pairIpMac.MacAddress;
                }
            }
            return null;
        }

        #endregion

        /// <summary>
        /// Имя компьютера
        /// </summary>
        public string HostName => Environment.MachineName;

        /// <summary>
        /// Возвращает ip-адрес хоста
        /// </summary>
        /// <remarks>первый по списку из сетевых интерфейсов</remarks>
        public string IpAddress
        {
            get
            {
                var result = GetIPMacPairs()?.First().IpAddress;
                return result;
            }
        }
        /// <summary>
        /// Возвращает mac-адрес хоста
        /// </summary>
        /// <remarks>первый по списку из сетевых интерфейсов</remarks>
        public string MacAddress
        {
            get => GetIPMacPairs()?.First().MacAddress;
        }
    }
}
