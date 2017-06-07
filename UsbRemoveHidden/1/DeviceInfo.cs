using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace UsbRemoveHidden
{
    ///<summary>
    /// 设备详细
    /// </summary>
    public class DeviceInfo
    {
        private const int DIGCF_ALLCLASSES = (0x00000004);
        private const int DIGCF_PRESENT = (0x00000002);

        private const int MAX_DEV_LEN = 1000;//返回值最大长度

        private const int SPDRP_DEVICEDESC = (0x00000000);// DeviceDesc (R/W)
        private const int SPDRP_HARDWAREID = (0x00000001);// HardwareID (R/W)
        private const int SPDRP_COMPATIBLEIDS = (0x00000002);// CompatibleIDs (R/W)
        private const int SPDRP_UNUSED0 = (0x00000003);// unused
        private const int SPDRP_SERVICE = (0x00000004);// Service (R/W)
        private const int SPDRP_UNUSED1 = (0x00000005);// unused
        private const int SPDRP_UNUSED2 = (0x00000006);// unused
        private const int SPDRP_CLASS = (0x00000007);// Class (R--tied to ClassGUID)
        private const int SPDRP_CLASSGUID = (0x00000008);// ClassGUID (R/W)
        private const int SPDRP_DRIVER = (0x00000009);// Driver (R/W)
        private const int SPDRP_CONFIGFLAGS = (0x0000000A);// ConfigFlags (R/W)
        private const int SPDRP_MFG = (0x0000000B);// Mfg (R/W)
        private const int SPDRP_FRIENDLYNAME = (0x0000000C);// FriendlyName (R/W)
        private const int SPDRP_LOCATION_INFORMATION = (0x0000000D);// LocationInformation (R/W)
        private const int SPDRP_PHYSICAL_DEVICE_OBJECT_NAME = (0x0000000E);// PhysicalDeviceObjectName (R)
        private const int SPDRP_CAPABILITIES = (0x0000000F);// Capabilities (R)
        private const int SPDRP_UI_NUMBER = (0x00000010);// UiNumber (R)
        private const int SPDRP_UPPERFILTERS = (0x00000011);// UpperFilters (R/W)
        private const int SPDRP_LOWERFILTERS = (0x00000012);// LowerFilters (R/W)
        private const int SPDRP_BUSTYPEGUID = (0x00000013);// BusTypeGUID (R)
        private const int SPDRP_LEGACYBUSTYPE = (0x00000014);// LegacyBusType (R)
        private const int SPDRP_BUSNUMBER = (0x00000015);// BusNumber (R)
        private const int SPDRP_ENUMERATOR_NAME = (0x00000016);// Enumerator Name (R)
        private const int SPDRP_SECURITY = (0x00000017);// Security (R/W, binary form)
        private const int SPDRP_SECURITY_SDS = (0x00000018);// Security=(W, SDS form)
        private const int SPDRP_DEVTYPE = (0x00000019);// Device Type (R/W)
        private const int SPDRP_EXCLUSIVE = (0x0000001A);// Device is exclusive-access (R/W)
        private const int SPDRP_CHARACTERISTICS = (0x0000001B);// Device Characteristics (R/W)
        private const int SPDRP_ADDRESS = (0x0000001C);// Device Address (R)
        private const int SPDRP_UI_NUMBER_DESC_FORMAT = (0X0000001D);// UiNumberDescFormat (R/W)
        private const int SPDRP_DEVICE_POWER_DATA = (0x0000001E);// Device Power Data (R)
        private const int SPDRP_REMOVAL_POLICY = (0x0000001F);// Removal Policy (R)
        private const int SPDRP_REMOVAL_POLICY_HW_DEFAULT = (0x00000020);// Hardware Removal Policy (R)
        private const int SPDRP_REMOVAL_POLICY_OVERRIDE = (0x00000021);// Removal Policy Override (RW)
        private const int SPDRP_INSTALL_STATE = (0x00000022);// Device Install State (R)
        private const int SPDRP_LOCATION_PATHS = (0x00000023);// Device Location Paths (R)
        private const int SPDRP_BASE_CONTAINERID = (0x00000024);// Base ContainerID (R)

        private const int SPDRP_MAXIMUM_PROPERTY = (0x00000025);// Upper bound on ordinals

        [StructLayout(LayoutKind.Sequential)]
        private class SP_DEVINFO_DATA
        {
            public int cbSize;
            public Guid ClassGuid;
            public int DevInst;
            public ulong Reserved;
        };
        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiClassGuidsFromNameA(string ClassN, ref Guid guids, UInt32 ClassNameSize, ref UInt32 ReqSize);

        [DllImport("setupapi.dll")]
        private static extern IntPtr SetupDiGetClassDevsA(ref Guid ClassGuid, UInt32 Enumerator, IntPtr hwndParent, UInt32 Flags);

        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, UInt32 MemberIndex, SP_DEVINFO_DATA DeviceInfoData);

        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiGetDeviceRegistryPropertyA(IntPtr DeviceInfoSet, SP_DEVINFO_DATA DeviceInfoData, UInt32 Property, UInt32 PropertyRegDataType, StringBuilder PropertyBuffer, UInt32 PropertyBufferSize, IntPtr RequiredSize);

        /// <summary>
        /// 通过设备类型枚举设备信息
        /// </summary>
        /// <param name="DeviceIndex"></param>
        /// <param name="ClassName"></param>
        /// <param name="DeviceName"></param>
        /// <returns></returns>
        public static int EnumerateDevices(UInt32 DeviceIndex, string ClassName, StringBuilder DeviceName, StringBuilder DeviceID, StringBuilder Mfg, StringBuilder IsInstallDrivers)
        {
            UInt32 RequiredSize = 0;
            Guid guid = Guid.Empty;
            Guid[] guids = new Guid[1];
            IntPtr NewDeviceInfoSet;
            SP_DEVINFO_DATA DeviceInfoData = new SP_DEVINFO_DATA();


            bool res = SetupDiClassGuidsFromNameA(ClassName, ref guids[0], RequiredSize, ref RequiredSize);
            if (RequiredSize == 0)
            {
                //类型不正确
                DeviceName = new StringBuilder("");
                return -2;
            }

            if (res == false)
            {
                guids = new Guid[RequiredSize];
                res = SetupDiClassGuidsFromNameA(ClassName, ref guids[0], RequiredSize, ref RequiredSize);

                if (!res || RequiredSize == 0)
                {
                    //类型不正确
                    DeviceName = new StringBuilder("");
                    return -2;
                }
            }

            //通过类型获取设备信息
            NewDeviceInfoSet = SetupDiGetClassDevsA(ref guids[0], 0, IntPtr.Zero, DIGCF_PRESENT);
            if (NewDeviceInfoSet.ToInt32() == -1)
            {
                //设备不可用
                DeviceName = new StringBuilder("");
                return -3;
            }

            DeviceInfoData.cbSize = 28;
            //正常状态
            DeviceInfoData.DevInst = 0;
            DeviceInfoData.ClassGuid = System.Guid.Empty;
            DeviceInfoData.Reserved = 0;

            res = SetupDiEnumDeviceInfo(NewDeviceInfoSet,
                   DeviceIndex, DeviceInfoData);
            if (!res)
            {
                //没有设备
                SetupDiDestroyDeviceInfoList(NewDeviceInfoSet);
                DeviceName = new StringBuilder("");
                return -1;
            }



            DeviceName.Capacity = MAX_DEV_LEN;
            DeviceID.Capacity = MAX_DEV_LEN;
            Mfg.Capacity = MAX_DEV_LEN;
            IsInstallDrivers.Capacity = MAX_DEV_LEN;
            if (!SetupDiGetDeviceRegistryPropertyA(NewDeviceInfoSet, DeviceInfoData,
            SPDRP_FRIENDLYNAME, 0, DeviceName, MAX_DEV_LEN, IntPtr.Zero))
            {
                res = SetupDiGetDeviceRegistryPropertyA(NewDeviceInfoSet,
                 DeviceInfoData, SPDRP_DEVICEDESC, 0, DeviceName, MAX_DEV_LEN, IntPtr.Zero);
                if (!res)
                {
                    //类型不正确
                    SetupDiDestroyDeviceInfoList(NewDeviceInfoSet);
                    DeviceName = new StringBuilder("");
                    return -4;
                }
            }
            //设备ID
            bool resHardwareID = SetupDiGetDeviceRegistryPropertyA(NewDeviceInfoSet,
             DeviceInfoData, SPDRP_HARDWAREID, 0, DeviceID, MAX_DEV_LEN, IntPtr.Zero);
            if (!resHardwareID)
            {
                //设备ID未知
                DeviceID = new StringBuilder("");
                DeviceID.Append("未知");
            }
            //设备供应商
            bool resMfg = SetupDiGetDeviceRegistryPropertyA(NewDeviceInfoSet,
             DeviceInfoData, SPDRP_MFG, 0, Mfg, MAX_DEV_LEN, IntPtr.Zero);
            if (!resMfg)
            {
                //设备供应商未知
                Mfg = new StringBuilder("");
                Mfg.Append("未知");
            }
            //设备是否安装驱动
            bool resIsInstallDrivers = SetupDiGetDeviceRegistryPropertyA(NewDeviceInfoSet,
             DeviceInfoData, SPDRP_DRIVER, 0, IsInstallDrivers, MAX_DEV_LEN, IntPtr.Zero);
            if (!resIsInstallDrivers)
            {
                //设备是否安装驱动
                IsInstallDrivers = new StringBuilder("");
            }
            //释放当前设备占用内存
            SetupDiDestroyDeviceInfoList(NewDeviceInfoSet);
            return 0;
        }
        /// <summary>
        /// 获取未知设备信息
        /// </summary>
        /// <param name="DeviceIndex"></param>
        /// <param name="ClassName"></param>
        /// <param name="DeviceName"></param>
        /// <returns></returns>
        public static int EnumerateDevices(List<string> NameList, List<string> IDList, List<string> MfgList, List<string> TypeList, List<string> IsInstallDriversList)
        {
            Guid myGUID = System.Guid.Empty;
            IntPtr hDevInfo = SetupDiGetClassDevsA(ref myGUID, 0, IntPtr.Zero, DIGCF_ALLCLASSES);

            if (hDevInfo.ToInt32() == -1)
            {
                //设备不可用

                return -3;
            }
            SP_DEVINFO_DATA DeviceInfoData = new SP_DEVINFO_DATA();
            DeviceInfoData.cbSize = 28;
            //正常状态
            DeviceInfoData.DevInst = 0;
            DeviceInfoData.ClassGuid = System.Guid.Empty;
            DeviceInfoData.Reserved = 0;
            UInt32 i;
            for (i = 0; SetupDiEnumDeviceInfo(hDevInfo, i, DeviceInfoData); i++)
            {
                //设备名称
                StringBuilder DeviceName = new StringBuilder("");
                //设备ID
                StringBuilder DeviceID = new StringBuilder("");
                //设备供应商
                StringBuilder Mfg = new StringBuilder("");
                //设备类型
                StringBuilder DeviceType = new StringBuilder("");
                //设备类型
                StringBuilder IsInstallDrivers = new StringBuilder("");
                DeviceName.Capacity = MAX_DEV_LEN;
                DeviceID.Capacity = MAX_DEV_LEN;
                DeviceType.Capacity = MAX_DEV_LEN;
                Mfg.Capacity = MAX_DEV_LEN;
                IsInstallDrivers.Capacity = MAX_DEV_LEN;
                bool resName = SetupDiGetDeviceRegistryPropertyA(hDevInfo, DeviceInfoData, SPDRP_DEVICEDESC, 0, DeviceName, MAX_DEV_LEN, IntPtr.Zero);
                if (!resName)
                {
                    //设备名称未知
                    DeviceName = new StringBuilder("");
                }
                bool resClass = SetupDiGetDeviceRegistryPropertyA(hDevInfo, DeviceInfoData, SPDRP_CLASS, 0, DeviceType, MAX_DEV_LEN, IntPtr.Zero);
                if (!resClass)
                {
                    //设备类型未知
                    DeviceType = new StringBuilder("");
                }
                //设备ID
                bool resHardwareID = SetupDiGetDeviceRegistryPropertyA(hDevInfo,
                 DeviceInfoData, SPDRP_HARDWAREID, 0, DeviceID, MAX_DEV_LEN, IntPtr.Zero);
                if (!resHardwareID)
                {
                    //设备ID未知
                    DeviceID = new StringBuilder("");
                }

                //设备供应商
                bool resMfg = SetupDiGetDeviceRegistryPropertyA(hDevInfo,
                 DeviceInfoData, SPDRP_MFG, 0, Mfg, MAX_DEV_LEN, IntPtr.Zero);
                if (!resMfg)
                {
                    //设备供应商未知
                    Mfg = new StringBuilder("");
                }

                bool resIsInstallDrivers = SetupDiGetDeviceRegistryPropertyA(hDevInfo,
                 DeviceInfoData, SPDRP_DRIVER, 0, IsInstallDrivers, MAX_DEV_LEN, IntPtr.Zero);
                if (!resIsInstallDrivers)
                {
                    //设备是否安装驱动
                    IsInstallDrivers = new StringBuilder("");
                }

                if (string.IsNullOrEmpty(DeviceType.ToString()))
                {
                    if (!string.IsNullOrEmpty(DeviceName.ToString()) && !string.IsNullOrEmpty(DeviceID.ToString()))
                    {
                        TypeList.Add("其它设备");
                        NameList.Add(DeviceName.ToString());
                        IDList.Add(DeviceID.ToString());
                        MfgList.Add(Mfg.ToString());
                        IsInstallDriversList.Add(IsInstallDrivers.ToString());
                    }

                }
            }
            //释放当前设备占用内存
            SetupDiDestroyDeviceInfoList(hDevInfo);
            return 0;
        }
    }
}
