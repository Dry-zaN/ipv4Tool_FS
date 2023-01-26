using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;

internal class Program
{
    public static void Main(string[] args)

    {
        Console.Title = "DryzaN IPv4Tool";
        bool vpnOn;
        Console.ReadKey();
        DisplayIPAddresses();
        Menu();
    }

    public static string DisplayIPAddresses()
    {
        string returnAddress = String.Empty;
        

        // Get a list of all network interfaces (usually one per network card, dialup, and VPN connection)
        NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface network in networkInterfaces)
        {
            // Read the IP configuration for each network
            IPInterfaceProperties properties = network.GetIPProperties();

            if (network.OperationalStatus == OperationalStatus.Up &&
                   !network.Description.ToLower().Contains("virtual") &&
                   !network.Description.ToLower().Contains("pseudo"))
            {
                // Each network interface may have multiple IP addresses
                foreach (IPAddressInformation address in properties.UnicastAddresses)
                {
                    // For IPv4 addresses
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;

                    // Ignore loopback addresses (e.g., 127.0.0.1)
                    if (IPAddress.IsLoopback(address.Address))
                        continue;

                    returnAddress = address.Address.ToString();
                    
                    string vpnCheck = network.Name.ToString();

                    Console.WriteLine(returnAddress);
                    Console.ReadKey();

                    //if test contain
                    if (vpnCheck.Contains("ipsec") || vpnCheck.Contains("utun3"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("This application does not support the use of a Virtual Private Network (VPN)\nPlease disable your VPN and come back once it has been disabled.");
                        Console.WriteLine("Press any key to exit the application.");
                        Console.ForegroundColor = ConsoleColor.White;
                        //Waits for a key press and then shuts down the application.
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                    

                }

            }
        }
        return returnAddress;
        


    }

    //Menus are still WIP

    public static void Menu()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Welcome to DryzaN's IPv4 Tool\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Clear();
            Console.WriteLine("1. Set a new IP address");
            Console.WriteLine("2. Unrestrict access to censored websites (i.e. thepiratebay.org");
            Console.WriteLine("3. Connect to your router");
            Console.WriteLine("4. FAQ");
            Console.WriteLine("5. Show external IP address");
            Console.WriteLine("6. Menú en Español");

            Console.ForegroundColor = ConsoleColor.White;
            string input = Console.ReadLine();
            input.ToLower();

            if (input != "1" && input != "2" && input != "3" && input != "4" && input != "5" && input != "6" && input != "exit")
            {
            Console.WriteLine("wrong answer motherfucker");
            Console.ReadKey();
            Menu();
            }
            else if (input.Equals("exit"))
            {
                Environment.Exit(0);
            }
            else if (input.Equals("5"))
            {
                showExternalIP();
            }
            else if (input.Equals("6"))
            {
                spanishMenu();
            }   


        Console.ReadKey();
       
    }

    public static void spanishMenu()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Bievenido para herramienta IPv4 de DryzaN\n");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Clear();
        Console.WriteLine("1. Configurado IP nuevo");
        Console.WriteLine("2. Unrestrict access to censored websites (i.e. thepiratebay.org");
        Console.WriteLine("3. Connect to your router");
        Console.WriteLine("4. FAQ");
        Console.WriteLine("5. Mostrar IP externa");
        Console.WriteLine("6. Menú en Inglés");

        Console.ForegroundColor = ConsoleColor.White;
        string input = Console.ReadLine(); input.ToLower();

        if (input != "1" && input != "2" && input != "3" && input != "4" && input != "5" && input != "6" && input != "exit")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("¡Respuesta incorrecta!");
            Console.ReadKey();
            Menu();
        }
        else if (input.Equals("5"))
        {
            showExternalIP();
        }
        else if (input.Equals("6"))
        {
            Menu();
        }
        else if (input.Equals("exit"))
        {
            Environment.Exit(0);
        }


        Console.ReadKey();
    }

    private static void showExternalIP()
    {
        string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
        var externalIp = IPAddress.Parse(externalIpString);
        Console.WriteLine(externalIp.ToString());
        Console.ReadKey();
        Menu();

    }
}
