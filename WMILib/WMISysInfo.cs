using System;
using System.Management;
using System.Text.RegularExpressions;

namespace WMILib
{
    public class SystemInformation
    {
        private static string getWMItem(string a, string b)
        {
            try
            {
                // a - query on language WQL, b - name of the required param
                SelectQuery q = new SelectQuery(a);
                ManagementObjectSearcher response = new ManagementObjectSearcher(q);
                foreach (ManagementObject i in response.Get())
                {
                    if (i.GetPropertyValue(b) != null)
                    {
                        return i.GetPropertyValue(b).ToString().TrimStart().TrimEnd();
                    }
                    else
                    {
                        return "";
                    }
                }
                return "Response is not a iterable object";
            }
            catch (ArgumentException err)
            {
                return err.Message;
            }
            catch (ManagementException err)
            {
                return err.Message;
            }
            finally
            {
                //Debug output
                //Console.WriteLine("test2");
            }
        }

        public static string getUname()
        {
            string result = getWMItem("SELECT * FROM Win32_ComputerSystem", "username");
            //return result;
            if (result == "Response is not a iterable object")
            {
                return result;
            }
            else
            {
                Regex p = new Regex("^.{1,}\\\\", RegexOptions.IgnoreCase);
                string username = p.Replace(result, "");
                if (username.Length == 0)
                {
                    return "Username not found";
                }
                else
                    return username;
            }
        }

        public static string getLocale()
        {
            string result = getWMItem("SELECT * FROM Win32_OperatingSystem", "locale");
            if (result == "Response is not a iterable object")
            {
                return result;
            }
            else
            {
                return result;
            }
        }
    }
}

