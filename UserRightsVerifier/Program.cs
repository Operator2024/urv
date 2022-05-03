using System;
using System.Collections;
using System.DirectoryServices;
using WMILib;
using System.Threading;

namespace UserRightsVerifier
{
    class Program
    {
        public static string isMember(string uname, string gname)
        {
            DirectoryEntry host = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
            DirectoryEntry g = host.Children.Find(gname, "group");
            object members = g.Invoke("Members", null);
            foreach (object i in (IEnumerable)members)
            {
                DirectoryEntry x = new DirectoryEntry(i);
                if (uname == x.Name) { return "User has admin rights"; }
            }
            return "User has not admin rights";
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string pattern = "The '{0}' method has returned the message - {1}";
            string username = SystemInformation.getUname();
            if (username == "Response is not a iterable object")
            {
                Console.WriteLine(pattern, "getUname", username);
            }
            else if (username == "Username not found")
            {
                Console.WriteLine(username);
            }
            else if (username.Length == 0)
            {
                Console.WriteLine("Username not found");
            }
            else
            {
                string locale = SystemInformation.getLocale();
                if (locale == "Response is not a iterable object")
                {
                    Console.WriteLine(pattern, "getLocale", locale);
                }
                else
                {
                    /* Supported locales: 0419 - Russian, 0409 - United States, 
                     *                    0809 - Great Britain, 0422 - Ukranian */
                    string groupname = "";
                    string resp = "";
                    if (locale == "0419")
                    {
                        groupname = "Администраторы";
                        resp = isMember(username, groupname);
                    }
                    else if (locale == "0409" || locale == "0809")
                    {
                        groupname = "Administrators";
                        resp = isMember(username, groupname);
                    }
                    else if (locale == "0422")
                    {
                        groupname = "Адміністратори";
                        resp = isMember(username, groupname);
                    }
                    else
                    {
                        Console.WriteLine("Unsupported locale - {0}", locale);
                    }
                    Console.WriteLine("UserName: {0}, Locale: {1}, Response isMember: {2}", username, locale, resp);
                }

            }
            //DirectoryEntry machine = new DirectoryEntry("WinNT://" + Environment.MachineName + ",Администраторы,group");


            /* foreach (object i in (IEnumerable)members)
             {
                 DirectoryEntry x = new DirectoryEntry(i);
                 Console.WriteLine(x.Name);
             }
             Console.WriteLine("");
             Console.WriteLine(machine.Path);
             Console.WriteLine(g.Path);
             Console.WriteLine(g.Invoke("IsMember", "WinNT://WORKGROUP/" + Environment.MachineName + "/Paddy"));
             Console.WriteLine("WinNT://" + Environment.MachineName + "/Paddy");*/
            // docs.microsoft.com/en-us/windows/win32/api/iads/nf-iads-iadsgroup-members
        }
    }
}

//$username = Get - WmiObject Win32_ComputerSystem | Select - Object username