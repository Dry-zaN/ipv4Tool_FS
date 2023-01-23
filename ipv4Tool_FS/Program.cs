using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;

internal class Program
{
    public static void Main(string[] args)

    {
        DisplayIPAddresses();
        Console.ReadKey();
    }

    public static void Menu(string[] args)
    {
        Console.Clear();
        Console.WriteLine("1. Set a new IP address");
        Console.WriteLine("2. Unrestrict access to censored websites (i.e. thepiratebay.org");
        Console.WriteLine("3. Connect to your router");
        Console.WriteLine("4. FAQ");
        Console.ReadKey();
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

                    int vpnDeactivate = 0;
                    int vpnActivated = 0;

                    //if test contain
                    if (vpnCheck.Contains("utun"))
                    {
                        vpnDeactivate = 1;
                        vpnActivated = 5;
                        vpnCheck = vpnCheck.Replace("utun3", "a VPN").ToString();
                    }
                    else if (vpnCheck.Contains("ipsec"))
                    {
                        vpnDeactivate = 1;
                        vpnActivated = 5;
                        vpnCheck = vpnCheck.Replace("ipsec0", "a VPN").ToString();
                    }
                    else if (vpnCheck.Contains("en"))
                    {
                        vpnDeactivate = 2;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("PROBLEM");
                    }

                    if (vpnDeactivate == 1 && vpnActivated == 5)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("This application does not support the use of a Virtual Private Network (VPN)\nPlease disable your VPN and come back once it has been disabled.");
                        Console.WriteLine("Press any key to exit the application.");
                        //Waits for a key press and then shuts down the application.
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                    else if (vpnDeactivate == 2 && vpnActivated != 5)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(vpnCheck + " " + vpnDeactivate);

                        Console.WriteLine("Your Local IP address is: {0}\nYou are using a wireless interface to achieve this connection called: {1}.\n", address.Address, vpnCheck, network.Description);
                        Console.ReadKey();
                        Console.Clear();
                        Console.WriteLine("1. Set a new IP address");
                        Console.WriteLine("2. Unrestrict access to censored websites (i.e. thepiratebay.org");
                        Console.WriteLine("3. Connect to your router");
                        Console.WriteLine("4. FAQ");
                        Console.ReadKey();

                    }
                    else if (vpnDeactivate == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("You shouldn't have ended up here.");
                        Console.WriteLine("You may be missing an active connection to the internet.");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Next key press will close the application.\nPlease try again!");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }

            }
        }
        return returnAddress;
    }
                    
}
