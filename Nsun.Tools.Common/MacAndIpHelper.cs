using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nsun.Tools.Common
{
    public class MacAndIpHelper
    {
        public static string IP
        {
            get;
            private set;
        }

        public static string MAC
        {
            get;
            private set;
        }


        //获取本机MAC地址
        public static string GetMACAddress()
        {
            string MoAddress = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                    MoAddress = mo["MacAddress"].ToString();
                mo.Dispose();
            }
            return MoAddress;
        }

        public static string GetIp()
        {
            string strHostName = Dns.GetHostName(); //得到本机的主机名
            IPHostEntry ipEntry = Dns.GetHostByName(strHostName); //取得本机IP
            IP = ipEntry.AddressList[0].ToString();
            return IP;
        }


        [DllImport("Iphlpapi.dll")]
        private static extern int SendARP(Int32 dest, Int32 host, byte[] mac, ref Int32 length);


        [DllImport("Ws2_32.dll")]
        private static extern Int32 inet_addr(string IP);


        public static string GetMacAddress(string ip)
        {
            try
            {
                byte[] aa = new byte[6];

                Int32 ldest = inet_addr(ip); //目的地的ip 

                Int64 macinfo = new Int64();
                Int32 len = 6;
                int res = SendARP(ldest, 0, aa, ref len);

                MAC= BitConverter.ToString(aa, 0, 6); 
                return MAC;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
