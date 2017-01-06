/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Aliyun.Api.LOG.Utilities
{
    internal class IpUtils
    {
        public static bool IsInternalIP(IPAddress ip)
        {
            //According to RFC 1918 (http://www.faqs.org/rfcs/rfc1918.html). private IP ranges are as bellow
            //    10.0.0.0        -   10.255.255.255  (10/8 prefix)
            //    172.16.0.0      -   172.31.255.255  (172.16/12 prefix)
            //    192.168.0.0     -   192.168.255.255 (192.168/16 prefix)

            byte[] addrs = ip.GetAddressBytes();
            if ((addrs[0] == 10) ||
                 (addrs[0] == 192 && addrs[1] == 168) ||
                 (addrs[0] == 172 && (addrs[1] >= 16) && (addrs[1] < 32)))
                return true;
            else
                return false;
        }
        public static bool IsIpAddress(String str)
        {
            Regex regex = new Regex("^(\\d{1,3}\\.){3}\\d{1,3}");
            return regex.Match(str).Success;
        }
        public static string GetLocalMachinePrivateIp()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            string ip = "";
            foreach (NetworkInterface adapter in adapters)
            {
                if (adapter.OperationalStatus == OperationalStatus.Up)
                {
                    IPInterfaceProperties IPInterfaceProperties = adapter.GetIPProperties();
                    UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = IPInterfaceProperties.UnicastAddresses;
                    foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
                    {
                        IPAddress addr = UnicastIPAddressInformation.Address;

                        if (!IPAddress.IsLoopback(addr) &&
                            UnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            if (IsInternalIP(addr))
                            {
                                ip = addr.ToString();
                                break;
                            }
                        }
                    }

                    if (!String.IsNullOrEmpty(ip))
                        break;
                }
            }
            return ip;
        }
    }
}
