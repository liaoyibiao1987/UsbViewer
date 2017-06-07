using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Windows.Forms;

namespace USBViewer
{
    public static class ServiceHelper
    {
        private const string SERVICE_NAME = "USBCleaner";
        private const string SERVICE_FILE_NAME = "USBCleaner.exe";
        private const string APPLICATION_TITLE = "USB 存储设备使用痕迹检测";
        private const string SERVICE_FILE_NOT_EXIST = "注册表删除服务文件不存在。";
        private const string INSTALL_SERVICE_ERROR = "安装注册表删除服务时发生错误。";
        private const string UNINSTALL_SERVICE_ERROR = "卸载注册表删除服务时发生错误。";

        /// <summary>
        /// 获取服务文件的路径。
        /// </summary>
        /// <returns></returns>
        private static string GetServiceFilePath()
        {
            return Path.Combine(Application.StartupPath, SERVICE_FILE_NAME);
        }

        /// <summary>
        /// 检测相应的服务文件是否存在。
        /// </summary>
        /// <returns></returns>
        private static bool IsServiceFileExists()
        {
            return File.Exists(GetServiceFilePath());
        }

        /// <summary>
        /// 显示错误信息。
        /// </summary>
        /// <param name="errMsg"></param>
        private static void OOPS(string errMsg)
        {
            MessageBox.Show(errMsg, APPLICATION_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 启动服务。
        /// </summary>
        public static bool StartService()
        {
            if (!IsWindowsServiceInstalled(SERVICE_NAME))
            {
                if (InstallService() == false)
                    return false;
            }

            ServiceController sc = new ServiceController(SERVICE_NAME);
            if (sc.Status != ServiceControllerStatus.Running)
                sc.Start();

            return true;
        }

        /// <summary>
        /// 停止服务。
        /// </summary>
        private static void StopService()
        {
            if (!IsWindowsServiceInstalled(SERVICE_NAME))
            {
                if (InstallService() == false)
                    return;
            }

            ServiceController sc = new ServiceController(SERVICE_NAME);
            if (sc.Status != ServiceControllerStatus.Stopped)
                sc.Stop();
        }

        /// <summary>
        /// 检测服务是否安装。
        /// </summary>
        /// <param name="serviceName">要查询的服务名。</param>
        /// <returns></returns>
        private static bool IsWindowsServiceInstalled(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController service in services)
            {
                if (string.Equals(service.ServiceName, serviceName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 安装服务。
        /// </summary>
        /// <returns></returns>
        private static bool InstallService()
        {
            if (!IsServiceFileExists())
            {
                OOPS(SERVICE_FILE_NOT_EXIST);
                return false;
            }

            try
            {
                string[] cmdline = {};
                TransactedInstaller transactedInstaller = new TransactedInstaller();
                AssemblyInstaller assemblyInstaller = new AssemblyInstaller(GetServiceFilePath(), cmdline);
                transactedInstaller.Installers.Add(assemblyInstaller);
                transactedInstaller.Install(new System.Collections.Hashtable());
            }
            catch (Exception ex)
            {
                OOPS(INSTALL_SERVICE_ERROR + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 卸载服务。
        /// </summary>
        /// <returns></returns>
        public static bool UninstallService()
        {
            // 检测服务是否存在。
            if (!IsWindowsServiceInstalled(SERVICE_NAME))
                return true;

            // 在卸载服务前需要停止服务。
            StopService();

            // 检测相应的服务文件是否存在。
            if (!IsServiceFileExists())
            {
                OOPS(SERVICE_FILE_NOT_EXIST);
                return false;
            }

            try
            {
                string[] cmdline = {};
                TransactedInstaller transactedInstaller = new TransactedInstaller();
                AssemblyInstaller assemblyInstaller = new AssemblyInstaller(GetServiceFilePath(), cmdline);
                transactedInstaller.Installers.Add(assemblyInstaller);
                transactedInstaller.Uninstall(null);
            }
            catch (Exception ex)
            {
                OOPS(UNINSTALL_SERVICE_ERROR + ex.Message);
                return false;
            }

            return true;
        }
    }
}
