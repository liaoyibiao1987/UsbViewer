using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace UsbRemoveHidden
{
    public class DevManager
    { /*
            自动查找VID/PID匹配的USB设备当作Virutal COM Port口 
            输入值：   字符串，如VID_0483&PID_5740
            返回值：   0 - 没有找到对应设备
        */
        public static int GetPortNum(String vid_pid)
        {
            ManagementObjectCollection USBControllerDeviceCollection
                = new ManagementObjectSearcher("SELECT * FROM Win32_USBControllerDevice").Get();
            if (USBControllerDeviceCollection != null)
            {
                foreach (ManagementObject USBControllerDevice in USBControllerDeviceCollection)
                {
                    String Dependent = (USBControllerDevice["Dependent"] as String).Split(new Char[] { '=' })[1];

                    if (Dependent.Contains(vid_pid))
                    {
                        ManagementObjectCollection PnPEntityCollection
                            = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE DeviceID=" + Dependent).Get();
                        if (PnPEntityCollection != null)
                        {
                            foreach (ManagementObject Entity in PnPEntityCollection)
                            {
                                String DevName = Entity["Name"] as String;// 设备名称
                                String PortNum = Regex.Replace(DevName, @"[^\d.\d]", "");
                                return Convert.ToInt32(PortNum);
                            }
                        }
                    }
                }
            }

            return -1;
        }

        /*
            删除指定的usb虚拟串口 
            输入值：   设备管理器下的设备名称，不包括后面的(COMXX)
            返回值：   成功/失败
        */
        /*!!!BUG: 删除port后重新扫描，port number会加一；插拔或者通过设备管理器Uninstall port没有这个问题。!!!*/
        public static bool DeletePort(String portName = null)
        {
            bool ret = false;

            Guid classGuid = Guid.Empty;
            IntPtr hDevInfo = Win32.SetupDiGetClassDevs(ref classGuid, null, IntPtr.Zero, Win32.DIGCF_ALLCLASSES | Win32.DIGCF_PRESENT);
            if (hDevInfo.ToInt32() == Win32.INVALID_HANDLE_VALUE)
            {
                Console.WriteLine("访问硬件设备失败");
            }
            else
            {
                int i = 0;
                int selected = 0;

                StringBuilder deviceName = new StringBuilder();
                deviceName.Capacity = Win32.MAX_DEV_LEN;
                do
                {
                    UsbRemoveHidden.Win32.SP_DEVINFO_DATA devInfoData = new UsbRemoveHidden.Win32.SP_DEVINFO_DATA();
                    StringBuilder _DeviceName = new StringBuilder("");
                    _DeviceName.Capacity = 1000;

                    devInfoData.cbSize = Marshal.SizeOf(typeof(UsbRemoveHidden.Win32.SP_DEVINFO_DATA));
                    devInfoData.classGuid = Guid.Empty;
                    devInfoData.devInst = 0;
                    devInfoData.reserved = IntPtr.Zero;
                    bool result = Win32.SetupDiEnumDeviceInfo(hDevInfo, i, devInfoData);
                    if (false == result)
                    {
                        break;
                    }

                    Win32.SetupDiGetDeviceRegistryProperty(hDevInfo, devInfoData, 0, 0, _DeviceName, (uint)_DeviceName.Capacity, IntPtr.Zero);

                    if (_DeviceName.ToString().Equals(portName))
                    {
                        Debug.WriteLine("Log.Level.Operation," + _DeviceName.ToString() + ":" + Win32.GetDeviceInstanceId(hDevInfo, devInfoData).ToString());
                        selected = i;
                    }
                    ++i;
                } while (true);

                ret = Win32.Remove(selected, hDevInfo);

            }
            Win32.SetupDiDestroyDeviceInfoList(hDevInfo);

            return ret;
        }

        /*
            重新扫描硬件 
            输入值：   无
            返回值：   成功/失败
        */
        public static bool Rescan()
        {
            //UInt32 devRoot = 0;

            //if (CM_Locate_DevNode_Ex(ref devRoot, null, 0, IntPtr.Zero) != CR_SUCCESS)
            //{
            //    return false;
            //}
            //if (CM_Reenumerate_DevNode_Ex(devRoot, 0, IntPtr.Zero) != CR_SUCCESS)
            //{
            //    return false;
            //}

            return true;
        }
        public static bool DisablePort(String portName = null)
        {
            bool ret = false;

            Guid classGuid = Guid.Empty;
            IntPtr hDevInfo = Win32.SetupDiGetClassDevs(ref classGuid, null, IntPtr.Zero, Win32.DIGCF_ALLCLASSES | Win32.DIGCF_PRESENT);
            if (hDevInfo.ToInt32() == Win32.INVALID_HANDLE_VALUE)
            {
                Console.WriteLine("访问硬件设备失败");
            }
            else
            {
                int i = 0;
                int selected = 0;

                StringBuilder deviceName = new StringBuilder();
                deviceName.Capacity = Win32.MAX_DEV_LEN;
                do
                {
                    UsbRemoveHidden.Win32.SP_DEVINFO_DATA devInfoData = new UsbRemoveHidden.Win32.SP_DEVINFO_DATA();
                    StringBuilder _DeviceName = new StringBuilder("");
                    _DeviceName.Capacity = 1000;

                    devInfoData.cbSize = Marshal.SizeOf(typeof(UsbRemoveHidden.Win32.SP_DEVINFO_DATA));
                    devInfoData.classGuid = Guid.Empty;
                    devInfoData.devInst = 0;
                    devInfoData.reserved = IntPtr.Zero;
                    bool result = Win32.SetupDiEnumDeviceInfo(hDevInfo, i, devInfoData);
                    if (false == result)
                    {
                        break;
                    }

                    Win32.SetupDiGetDeviceRegistryProperty(hDevInfo, devInfoData, 0, 0, _DeviceName, (uint)_DeviceName.Capacity, IntPtr.Zero);

                    if (_DeviceName.ToString().Equals("USB Serial Port"))
                    {
                        System.Diagnostics.Debug.WriteLine("Log.Level.Operation, " + _DeviceName.ToString());
                        selected = i;
                    }
                    ++i;
                } while (true);

                ret = Win32.StateChange(false, selected, hDevInfo);

            }
            Win32.SetupDiDestroyDeviceInfoList(hDevInfo);

            return ret;
        }

    }
}