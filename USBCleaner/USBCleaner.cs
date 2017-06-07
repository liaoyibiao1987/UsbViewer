using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Microsoft.Win32;
using System.Text;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;

namespace USBCleaner
{
    public partial class USBCleaner : ServiceBase
    {
        private string[] _keyNames = {
                                        "Enum\\USB",
                                        "Enum\\USBSTOR",
                                        "Control\\DeviceClasses\\{53f56307-b6bf-11d0-94f2-00a0c91efb8b}",
                                     };

        public USBCleaner()
        {
            InitializeComponent();
        }

        private string GetCurrentDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ");
        }

        /// <summary>
        /// 枚举以下键值：
        /// HKEY_LOCAL_MACHINE\SYSTEM\ControlSetxxx\Enum\USB
        /// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSetxxx\Enum\USB
        /// </summary>
        /// <param name="usbstorKey"></param>
        private string DeleteUsb(RegistryKey usbstorKey)
        {
            StringBuilder info = new StringBuilder();
            Regex regx = new Regex("^\\w{8}&\\w{8}$");
            foreach (string subUsbstorKey in usbstorKey.GetSubKeyNames())
            {
                if (!regx.IsMatch(subUsbstorKey.ToUpper())) continue;

                RegistryKey tempUsbKey = usbstorKey.OpenSubKey(subUsbstorKey);
                if (null == tempUsbKey) continue;
                if (tempUsbKey.SubKeyCount == 0) continue;

                RegistryKey subTempUsbKey = tempUsbKey.OpenSubKey(tempUsbKey.GetSubKeyNames()[0]);
                if (null == subTempUsbKey) continue;
                object valueService = subTempUsbKey.GetValue("Service");
                if (null != valueService)
                {
                    if (!string.Equals(valueService.ToString().ToUpper(), "USBSTOR") &&
                        !string.Equals(valueService.ToString().ToUpper(), "WUDFRD") &&
                        !string.Equals(valueService.ToString().ToUpper(), "VBOXUSB"))
                        continue;
                }
                subTempUsbKey.Close();
                tempUsbKey.Close();

                try
                {
                    usbstorKey.DeleteSubKeyTree(subUsbstorKey);
                    info.Append(GetCurrentDateTime());
                    info.Append("删除键值 " + usbstorKey.ToString() + "\\" + subUsbstorKey + " 成功。").AppendLine();
                }
                catch (Exception ex)
                {
                    info.Append(GetCurrentDateTime());
                    info.Append("删除键值 " + usbstorKey.ToString() + "\\" + subUsbstorKey + " 失败，错误消息：" + ex.Message).AppendLine();
                }
            }

            return info.ToString();
        }

        /// <summary>
        /// 枚举以下键值：
        /// HKEY_LOCAL_MACHINE\SYSTEM\ControlSetxxx\Enum\USBSTOR
        /// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSetxxx\Enum\USBSTOR
        /// HKEY_LOCAL_MACHINE\SYSTEM\ControlSetxxx\Control\DeviceClasses\{53f56307-b6bf-11d0-94f2-00a0c91efb8b}
        /// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSetxxx\Control\DeviceClasses\{53f56307-b6bf-11d0-94f2-00a0c91efb8b}
        /// </summary>
        /// <param name="usbstorKey"></param>
        private string DeleteUsbstorDeviceClasses(RegistryKey usbstorKey)
        {
            StringBuilder info = new StringBuilder();
            foreach (string subUsbstorKey in usbstorKey.GetSubKeyNames())
            {
                try
                {
                    usbstorKey.DeleteSubKeyTree(subUsbstorKey);
                    info.Append(GetCurrentDateTime());
                    info.Append("删除键值 " + usbstorKey.ToString() + "\\" + subUsbstorKey + " 成功。").AppendLine();
                }
                catch (Exception ex)
                {
                    info.Append(GetCurrentDateTime());
                    info.Append("删除键值 " + usbstorKey.ToString() + "\\" + subUsbstorKey + " 失败，错误消息：" + ex.Message).AppendLine();
                }
            }

            return info.ToString();
        }

        protected override void OnStart(string[] args)
        {
            StringBuilder info = new StringBuilder();
            info.Append(GetCurrentDateTime() + "USBCleaner 服务启动。").AppendLine();

            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey systemKey = hklm.OpenSubKey("SYSTEM");
            if (null == systemKey)
            {
                info.Append(GetCurrentDateTime());
                info.Append("打开键值 HKEY_LOCAL_MACHINE\\SYSTEM 失败。").AppendLine();
                return;
            }
            else
            {
                info.Append(GetCurrentDateTime());
                info.Append("打开键值 HKEY_LOCAL_MACHINE\\SYSTEM 成功。").AppendLine();
            }

            foreach (string sysKey in systemKey.GetSubKeyNames())
            {
                if (!sysKey.ToUpper().StartsWith("CONTROLSET") &&
                    !sysKey.ToUpper().StartsWith("CURRENTCONTROLSET"))
                    continue;

                RegistryKey subSystemKey = systemKey.OpenSubKey(sysKey);
                if (null == subSystemKey)
                    continue;

                foreach (string key in _keyNames)
                {
                    RegistryKey usbstorKey = null;
                    try
                    {
                        usbstorKey = subSystemKey.OpenSubKey(key, true);
                    }
                    catch (Exception ex)
                    {
                        info.Append(GetCurrentDateTime());
                        info.Append("打开键值 HKEY_LOCAL_MACHINE\\SYSTEM\\" + key + " 失败，错误消息：" + ex.Message).AppendLine();
                        continue;
                    }

                    if (string.Equals(key.ToUpper(), "ENUM\\USB"))
                    {
                        info.Append(DeleteUsb(usbstorKey));
                    }
                    else
                    {
                        info.Append(DeleteUsbstorDeviceClasses(usbstorKey));
                    }
                    usbstorKey.Close();
                }
                subSystemKey.Close();
            }
            systemKey.Close();

            // 写入操作日志。
            string logFileName = Assembly.GetExecutingAssembly().Location;
            logFileName = Path.Combine(Path.GetDirectoryName(logFileName), "USBCleanerLog.txt");
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(logFileName, true))
            {
                sw.Write(info.ToString());
            }

            // 执行完注册表删除操作后，自动停止服务。
            this.Stop();
        }

        protected override void OnStop()
        {
            string logFileName = Assembly.GetExecutingAssembly().Location;
            logFileName = Path.Combine(Path.GetDirectoryName(logFileName), "USBCleanerLog.txt");
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(logFileName, true))
            {
                sw.WriteLine(GetCurrentDateTime() + "USBCleaner 服务停止。");
            }

        }
    }
}
