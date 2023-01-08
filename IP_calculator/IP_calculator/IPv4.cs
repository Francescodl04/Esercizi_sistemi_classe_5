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
        private IPAddress ipAddress;
        private IPAddress subnetMask;
        private int cidr = 0;

        #region Metodi costruttori

        //Primo costruttore

        public IPv4(IPAddress subnetMask)
        {
            this.subnetMask = subnetMask;
        }

        //Secondo costruttore

        public IPv4(int cidr)
        {
            this.cidr = cidr;
        }

        //Terzo costruttore

        public IPv4(IPAddress ipAddress, int cidr)
        {
            this.ipAddress = ipAddress;
            this.cidr = cidr;
        }

        //Quarto costruttore

        public IPv4(IPAddress ipAddress, IPAddress subnetMask)
        {
            this.ipAddress = ipAddress;
            this.subnetMask = subnetMask;
        }

        public IPAddress GetIP()
        {
            return ipAddress;
        }

        #endregion

        #region Metodi di modifica attributi

        public void SetIPAddress(IPAddress ipAddress)
        {
            this.ipAddress = ipAddress;
        }

        public void SetSubnetMask(IPAddress subnetMask)
        {
            this.subnetMask = subnetMask;
        }

        public void SetCIDR(int cidr)
        {
            this.cidr = cidr;
        }

        #endregion

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


        #region Metodi per ottenere CIDR, subnet e wildcard mask

        public IPAddress GetSubnetMask()
        {
            return subnetMask;
        }

        public IPAddress GetWildCardMask()
        {
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();
            string wildCardMask = "";
            for (int i = 0; i < 4; i++)
            {
                string subnetMaskBits = Convert.ToString(subnetMaskBytes[i], 2);
                while (subnetMaskBits.Length < 8)
                {
                    subnetMaskBits = string.Concat('0', subnetMaskBits);
                }
                string wildCardBits = "";
                for (int j = 0; j < 8; j++)
                {
                    switch (subnetMaskBits[j])
                    {
                        case '1':
                            wildCardBits = string.Concat(wildCardBits, '0');
                            break;
                        case '0':
                            wildCardBits = string.Concat(wildCardBits, '1');
                            break;
                    }
                }
                if (i == 0)
                {
                    wildCardMask = Convert.ToInt32(wildCardBits, 2).ToString();
                }
                else
                {
                    wildCardMask = string.Concat(wildCardMask, '.', Convert.ToInt32(wildCardBits, 2).ToString());
                }
            }
            return IPAddress.Parse(wildCardMask);
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


        #region Metodi per ottenere range di host, indirizzo di broadcast e numero di host di una rete

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
            byte[] wildCardtMaskBytes = GetWildCardMask().GetAddressBytes();
            byte[] broadcastBytes = new byte[4];
            string broadcastAddress = "";
            for (int i = 0; i < 4; i++)
            {
                broadcastBytes[i] = (byte)(ipAddressBytes[i] | wildCardtMaskBytes[i]);
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
            return "\nOra verranno mostrate tutte le caratteristiche riguardanti la rete cui appartiene l'host:" +
            $"\n - Il primo indirizzo assegnabile ad un host è \"{GetFirstHost()}\"." +
            $"\n - L'ultimo indirizzo assegnabile ad un host è \"{GetLastHost()}\"." +
            $"\n - Il numero di indirizzi utilizzabili dagli host è {GetTotalNumberHost()}." +
            $"\n - La subnet mask della rete è \"{GetSubnetMask()}\"" +
            $"\n - La wild card mask della rete è \"{GetWildCardMask()}\"." +
            $"\n - La notazione CIDR della rete è \"\\{GetCIDR()}\"" +
            $"\n - L'indirizzo di broadcast è \"{GetBroadcastAddress()}\".";
        }
    }
}