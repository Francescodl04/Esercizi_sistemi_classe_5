/*
 * Francesco Di Lena, classe 5F
 * 7-01-2022
 * Realizzazione di un software per la gestione degli indirizzi IP
 * Sistemi e reti
 * A.S. 2022-2023
 * Classe Program: contiene tutti i metodi relativi alle visualizzazioni video
*/

using System;
using System.IO;
using System.Collections;
using System.Dynamic;
using System.Net;
using System.Net.Sockets;

namespace IP_calculator
{
    internal class Program
    {
        static readonly string[] menuText = new string[] { "ottieni l'indirizzo di rete a partire da un indirizzo host", "ottieni la subnet mask da una notazione CIDR ", "ottieni la notazione CIDR da una subnet mask", "ottieni le informazioni di una rete attraverso l'indirizzo di un host", "esci dal programma" };

        #region Metodi di uso generale

        static private void LogoScreen()
        {
            Console.Clear();
            Console.WriteLine("    ________     ______      __           __      __            \r\n   /  _/ __ \\   / ____/___ _/ /______  __/ /___ _/ /_____  _____\r\n   / // /_/ /  / /   / __ `/ / ___/ / / / / __ `/ __/ __ \\/ ___/\r\n _/ // ____/  / /___/ /_/ / / /__/ /_/ / / /_/ / /_/ /_/ / /    \r\n/___/_/       \\____/\\__,_/_/\\___/\\__,_/_/\\__,_/\\__/\\____/_/     \r\n                                                                ");
        }

        static private IPAddress IPInsert()
        {
            Console.WriteLine("Inserisci l'indirizzo IP (nella forma x.y.z.h):");
            IPAddress ipAddress;
            do
            {
                string ipAddressString = Console.ReadLine();
                try
                {
                    ipAddress = IPAddress.Parse(ipAddressString);
                    ipAddress = ipAddress.MapToIPv4();
                }
                catch (FormatException)
                {
                    Console.WriteLine("Non è stato inserito un indirizzo IP corretto, riprova...");
                    continue;
                }
                break;
            } while (true);
            return ipAddress;
        }

        static private IPAddress SubnetMaskInsert()
        {
            Console.WriteLine("Inserisci la subnet mask (nella forma x.y.z.h):");
            IPAddress subnetMask;
            do
            {
                string subnetMaskString = Console.ReadLine();
                try
                {
                    subnetMask = IPAddress.Parse(subnetMaskString);
                    subnetMask = subnetMask.MapToIPv4();
                }
                catch (FormatException)
                {
                    Console.WriteLine("Non è stata inserita una subnet mask corretta, riprova...");
                    continue;
                }
                break;
            } while (true);
            return subnetMask;
        }

        static private int CIDRInsert()
        {
            Console.WriteLine("Inserisci la notazione CIDR dell'indirizzo IP (solamente il numero):");
            int cidr;
            do
            {
                bool test = int.TryParse(Console.ReadLine(), out cidr);
                if (test == false || cidr < 8 || cidr > 32)
                {
                    Console.WriteLine("Non hai inserito una notazione corretta. Riprova...");
                    continue;
                }
                break;
            } while (true);
            return cidr;
        }

        static private void MenuReturn()
        {
            Console.WriteLine("\nPremi un tasto qualsiasi per tornare alla schermata iniziale...");
            Console.ReadKey();
            Main();
        }

        #endregion

        static private void Main()
        {
            LogoScreen();
            Console.Beep();
            Console.WriteLine("Benvenuto nel software per calcolare tutti i dettagli sugli indirizzi IPv4! Scegli una delle opzioni del menu:\n");
            for (int i = 0; i < menuText.Length; i++)
            {
                Console.WriteLine($"{i + 1}) {menuText[i]}");
            }
            do
            {
                switch (Console.ReadKey(true).KeyChar)
                {
                    case '1':
                        NetworkIpScreen();
                        break;
                    case '2':
                        SubnetMaskScreen();
                        break;
                    case '3':
                        CIDRScreen();
                        break;
                    case '4':
                        HostScreen();
                        break;
                    case '5':
                        ExitScreen();
                        break;
                    default:
                        Console.WriteLine($"Devi inserire un numero compreso fra 1 e {menuText.Length}, riprova...");
                        continue;
                }
                break;
            } while (true);
        }

        #region Metodi per la visualizzazione dei menu

        static private void NetworkIpScreen()
        {
            LogoScreen();
            Console.WriteLine("Desideri inserire la notazione decimal dotted o CIDR per la subnet mask?\nInserisci \"d\" per la prima, \"c\" per la seconda:");
            IPAddress networkAddress;
            do
            {
                switch (Console.ReadKey(true).KeyChar)
                {
                    case 'd':
                        Console.Write("Hai scelto di inserire la subnet mask. ");
                        IPv4 ipv4 = new IPv4(IPInsert(), SubnetMaskInsert());
                        networkAddress = ipv4.GetNetworkAddress();
                        break;
                    case 'c':
                        Console.Write("Hai scelto di inserire la notazione CIDR. ");
                        ipv4 = new IPv4(IPInsert(), CIDRInsert());
                        networkAddress = ipv4.GetNetworkAddress();
                        break;
                    default:
                        Console.WriteLine("Non hai inserito un carattere corretto, riprova...");
                        continue;
                }
                break;
            } while (true);
            Console.WriteLine($"L'indirizzo di rete è \"{networkAddress}\"");
            MenuReturn();
        }

        static private void SubnetMaskScreen()
        {
            LogoScreen();
            IPv4 ipv4 = new IPv4(CIDRInsert());
            Console.WriteLine($"La subnet-mask richiesta è \"{ipv4.GetSubnetMaskFromCIDR()}\"");
            MenuReturn();
        }

        static private void CIDRScreen()
        {
            LogoScreen();
            IPv4 ipv4 = new IPv4(SubnetMaskInsert());
            Console.WriteLine($"La denominazione CIDR della subnet mask inserita è \"\\{ipv4.GetCIDR()}\"");
            MenuReturn();
        }

        static private void HostScreen()
        {
            LogoScreen();
            IPv4 ipv4 = new IPv4(IPInsert(), SubnetMaskInsert());
            Console.WriteLine(ipv4.ToString());
            MenuReturn();
        }

        static private void ExitScreen()
        {
            Console.Clear();
            Environment.Exit(0);
        }
        #endregion
    }
}