/*
 * Francesco Di Lena, classe 5F
 * 7-01-2022
 * Realizzazione di un software per la gestione degli indirizzi IP
 * Sistemi e reti
 * A.S. 2022-2023
 * Classe IPv4: contiene tutti i metodi per svolgere le operazioni su IPv4
*/

using System.Net;

namespace IP_calculator
{
    public class IPv4
    {
        IPAddress ipAddress;
        IPAddress subnetMask;
        int cidr = 0;

        #region Metodi costruttori

        //Primo costruttore

        public IPv4(IPAddress ipAddress, IPAddress subnetMask)
        {
            this.ipAddress = ipAddress;
            this.subnetMask = subnetMask;
        }

        //Secondo costruttore

        public IPv4(IPAddress subnetMask)
        {
            this.subnetMask = subnetMask;
        }

        //Terzo costruttore

        public IPv4(IPAddress ipAddress, int cidr)
        {
            this.ipAddress = ipAddress;
            this.cidr = cidr;
        }

        public IPAddress GetIP()
        {
            return ipAddress;
        }

        #endregion

        public void SetIPAddress(IPAddress ipAddress)
        {
            this.ipAddress = ipAddress;
        }

        #region Metodi relativi agli indirizzi IP di rete

        public IPAddress GetNetworkAddress()
        {
            if (subnetMask == null)
            {
                subnetMask = GetSubnetMaskFromCIDR();
            }
            byte[] hostAddressBytes = ipAddress.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();
            byte[] networkAddressBytes = new byte[4];
            string networkAddress = "";
            for (int i = 0; i < 4; i++)
            {
                networkAddressBytes[i] = (byte)(hostAddressBytes[i] & subnetMaskBytes[i]);
                if (i == 0)
                {
                    networkAddress = Convert.ToString(networkAddressBytes[i]);
                }
                else
                {
                    networkAddress = string.Concat(networkAddress, '.', networkAddressBytes[i]);
                }
            }
            return IPAddress.Parse(networkAddress);
        }
        #endregion


        #region Metodi relativi al CIDR, alle subnet e wildcard mask

        public IPAddress GetSubnetMask()
        {
            return subnetMask;
        }

        public IPAddress GetWildCardMask()
        {
            return null;
        }

        public void SetSubnetMask(IPAddress subnetMask)
        {
            this.subnetMask = subnetMask;
        }

        public IPAddress GetSubnetMaskFromCIDR()
        {
            string binarySubnetMask = "";
            string subnetMask = "";
            for (int i = 0; i <= 32; i++)
            {
                if (i < cidr)
                {
                    binarySubnetMask = string.Concat(binarySubnetMask, '1');
                }
                else
                {
                    binarySubnetMask = string.Concat(binarySubnetMask, '0');
                }
            }
            for (int i = 0; i < 4; i++)
            {
                string temp = "";
                for (int j = 0; j < 8; j++)
                {
                    temp = string.Concat(temp, binarySubnetMask[j + i * 8]);
                }
                if (i == 0)
                {
                    subnetMask = Convert.ToInt32(temp, 2).ToString();
                }
                else
                {
                    subnetMask = string.Concat(subnetMask, '.', Convert.ToInt32(temp, 2).ToString());
                }
            }
            return IPAddress.Parse(subnetMask);
        }

        public int GetCIDR()
        {
            string binarySubnetMask = "";
            int cidr = 0;
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();
            for (int i = 0; i < subnetMaskBytes.Length; i++)
            {
                binarySubnetMask = string.Concat(binarySubnetMask, Convert.ToString(subnetMaskBytes[i], 2));
            }
            for (int i = 0; i < binarySubnetMask.Length; i++)
            {
                if (binarySubnetMask[i] == '1')
                {
                    cidr++;
                }
                else
                {
                    break;
                }
            }
            return cidr;
        }

        #endregion


        #region Metodi relativi al range di host di una rete

        public IPAddress GetFirstHost()
        {
            byte[] networkBytes = GetNetworkAddress().GetAddressBytes();
            networkBytes[3]++;
            string firstHost = "";
            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
                {
                    firstHost = networkBytes[i].ToString();
                }
                else
                {
                    firstHost = string.Concat(firstHost, '.', networkBytes[i]);
                }
            }
            return IPAddress.Parse(firstHost);
        }

        public IPAddress GetLastHost()
        {
            byte[] broadcastBytes = GetBroadcastAddress().GetAddressBytes();
            broadcastBytes[3]--;
            string lastHost = "";
            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
                {
                    lastHost = broadcastBytes[i].ToString();
                }
                else
                {
                    lastHost = string.Concat(lastHost, '.', broadcastBytes[i]);
                }
            }
            return IPAddress.Parse(lastHost);
        }

        public IPAddress GetBroadcastAddress()
        {
            byte[] ipAddressBytes = ipAddress.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();
            byte[] broadcastBytes = new byte[4];
            string broadcastAddress = "";
            for (int i = 0; i < 4; i++)
            {
                broadcastBytes[i] = (byte)(ipAddressBytes[i] | subnetMaskBytes[i]);
                if (i == 0)
                {
                    broadcastAddress = broadcastBytes[i].ToString();
                }
                else
                {
                    broadcastAddress = string.Concat(broadcastAddress, '.', broadcastBytes[i]);
                }
            }
            return IPAddress.Parse(broadcastAddress);
        }

        public double GetTotalNumberHost()
        {
            if (cidr == 0)
            {
                cidr = GetCIDR();
            }
            int hostBits = 32 - cidr;
            return Math.Pow(2, hostBits) - 2;
        }

        public IPAddress[] GetUsableHost()
        {
            IPAddress[] addresses = new IPAddress[] { GetFirstHost(), GetLastHost() };
            return addresses;
        }

        #endregion 


        public override string ToString()
        {
            return base.ToString();
        }
    }
}