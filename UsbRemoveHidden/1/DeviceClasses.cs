using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace UsbRemoveHidden
{
    ///<summary>
    /// 设备类型
    /// </summary>
    public class DeviceClasses
    {
        public static Guid ClassesGuid;
        public const int MAX_SIZE_DEVICE_DESCRIPTION = 1000;
        public const int CR_SUCCESS = 0x00000000;
        public const int CR_NO_SUCH_VALUE = 0x00000025;
        public const int CR_INVALID_DATA = 0x0000001F;
        private const int DIGCF_PRESENT = 0x00000002;
        private const int DIOCR_INSTALLER = 0x00000001;
        private const int MAXIMUM_ALLOWED = 0x02000000;
        public const int DMI_MASK = 0x00000001;
        public const int DMI_BKCOLOR = 0x00000002;
        public const int DMI_USERECT = 0x00000004;

        /// <summary>
        /// 定义了一个设备实例就是一个设备信息集合的成员
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        class SP_DEVINFO_DATA
        {
            public int cbSize;
            public Guid ClassGuid;
            public int DevInst;
            public ulong Reserved;
        }
        /// <summary>
        /// 提供每个类的GUID枚举本地机器安装的设备类
        /// </summary>
        /// <param name="ClassIndex"></param>
        /// <param name="ClassGuid"></param>
        /// <param name="Params"></param>
        /// <returns></returns>
        [DllImport("cfgmgr32.dll")]
        private static extern UInt32 CM_Enumerate_Classes(UInt32 ClassIndex, ref Guid ClassGuid, UInt32 Params);
        /// <summary>
        /// 检索与类GUID相关联的类名
        /// </summary>
        /// <param name="ClassGuid"></param>
        /// <param name="ClassName"></param>
        /// <param name="ClassNameSize"></param>
        /// <param name="RequiredSize"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiClassNameFromGuidA(ref Guid ClassGuid, StringBuilder ClassName, UInt32 ClassNameSize, ref UInt32 RequiredSize);
        /// <summary>
        /// 获取一个指定类别或全部类别的所有已安装设备的信息
        /// </summary>
        /// <param name="ClassGuid"></param>
        /// <param name="Enumerator"></param>
        /// <param name="hwndParent"></param>
        /// <param name="Flags"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll")]
        private static extern IntPtr SetupDiGetClassDevsA(ref Guid ClassGuid, UInt32 Enumerator, IntPtr hwndParent, UInt32 Flags);
        /// <summary>
        /// 销毁一个设备信息集合,并且释放所有关联的内存
        /// </summary>
        /// <param name="DeviceInfoSet"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);
        /// <summary>
        /// 打开设备安装程序类的注册表项，设备接口类的注册表项，或一个特定类的子项。此函数打开本地计算机或远程计算机上指定的键。
        /// </summary>
        /// <param name="ClassGuid"></param>
        /// <param name="samDesired"></param>
        /// <param name="Flags"></param>
        /// <param name="MachineName"></param>
        /// <param name="Reserved"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll")]
        private static extern IntPtr SetupDiOpenClassRegKeyExA(ref Guid ClassGuid, UInt32 samDesired, int Flags, IntPtr MachineName, UInt32 Reserved);
        /// <summary>
        /// 获取设备信息集合的设备信息元素。
        /// </summary>
        /// <param name="DeviceInfoSet"></param>
        /// <param name="MemberIndex"></param>
        /// <param name="DeviceInfoData"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, UInt32 MemberIndex, SP_DEVINFO_DATA DeviceInfoData);
        /// <summary>
        /// 取得指定项或子项的默认（未命名）值 
        /// </summary>
        /// <param name="KeyClass"></param>
        /// <param name="SubKey"></param>
        /// <param name="ClassDescription"></param>
        /// <param name="sizeB"></param>
        /// <returns></returns>
        [DllImport("advapi32.dll")]
        private static extern UInt32 RegQueryValueA(IntPtr KeyClass, UInt32 SubKey, StringBuilder ClassDescription, ref UInt32 sizeB);

        /// <summary>
        /// 设备类型图标信息
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class SP_CLASSIMAGELIST_DATA
        {
            public int cbSize;
            public string ImageList;
            public ulong Reserved;
        }
        public struct RECT
        {
            long left;
            long top;
            long right;
            long bottom;
        }

        /// <summary>
        /// 载入图片
        /// </summary>
        /// <param name="hInstance"></param>
        /// <param name="Reserved"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int LoadBitmapW(int hInstance, ulong Reserved);

        /// <summary>
        /// 获取图标
        /// </summary>
        /// <param name="ClassImageListData"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll")]
        public static extern Boolean SetupDiGetClassImageList(out SP_CLASSIMAGELIST_DATA ClassImageListData);
        /// <summary>
        /// 绘制小图标
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="rc"></param>
        /// <param name="MiniIconIndex"></param>
        /// <param name="Flags"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll")]
        public static extern int SetupDiDrawMiniIcon(Graphics hdc, RECT rc, int MiniIconIndex, int Flags);
        /// <summary>
        /// 检索指定类提供的小图标的索引。
        /// </summary>
        /// <param name="ClassGuid"></param>
        /// <param name="MiniIconIndex"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll")]
        public static extern bool SetupDiGetClassBitmapIndex(Guid ClassGuid, out int MiniIconIndex);
        /// <summary>
        /// 加载小图标
        /// </summary>
        /// <param name="classGuid"></param>
        /// <param name="hIcon"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll")]
        public static extern int SetupDiLoadClassIcon(ref Guid classGuid, out IntPtr hIcon, out int index);

        /// <summary>
        /// 枚举设备类型
        /// </summary>
        /// <param name="ClassIndex"></param>
        /// <param name="ClassName">设备类型名称</param>
        /// <param name="ClassDescription">设备类型说明</param>
        /// <param name="DevicePresent"></param>
        /// <returns></returns>
        public static int EnumerateClasses(UInt32 ClassIndex, StringBuilder ClassName, StringBuilder ClassDescription, ref bool DevicePresent)
        {
            Guid ClassGuid = Guid.Empty;
            IntPtr NewDeviceInfoSet;
            UInt32 result;
            SP_DEVINFO_DATA DeviceInfoData = new SP_DEVINFO_DATA();
            bool resNam = false;
            UInt32 RequiredSize = 0;
            result = CM_Enumerate_Classes(ClassIndex, ref ClassGuid, 0);
            DevicePresent = false;
            SP_CLASSIMAGELIST_DATA imagelist = new SP_CLASSIMAGELIST_DATA();
            if (result != CR_SUCCESS)
            {
                return (int)result;
            }
            resNam = SetupDiClassNameFromGuidA(ref ClassGuid, ClassName, RequiredSize, ref RequiredSize);
            if (RequiredSize > 0)
            {
                ClassName.Capacity = (int)RequiredSize;
                resNam = SetupDiClassNameFromGuidA(ref ClassGuid, ClassName, RequiredSize, ref RequiredSize);
            }
            NewDeviceInfoSet = SetupDiGetClassDevsA(ref ClassGuid, 0, IntPtr.Zero, DIGCF_PRESENT);
            if (NewDeviceInfoSet.ToInt32() == -1)
            {
                DevicePresent = false;
                return 0;
            }

            UInt32 numD = 0;
            DeviceInfoData.cbSize = 28;
            DeviceInfoData.DevInst = 0;
            DeviceInfoData.ClassGuid = System.Guid.Empty;
            DeviceInfoData.Reserved = 0;

            Boolean res1 = SetupDiEnumDeviceInfo(
            NewDeviceInfoSet,
            numD,
            DeviceInfoData);

            if (!res1)
            {
                DevicePresent = false;
                return 0;
            }
            SetupDiDestroyDeviceInfoList(NewDeviceInfoSet);
            IntPtr KeyClass = SetupDiOpenClassRegKeyExA(
                ref ClassGuid, MAXIMUM_ALLOWED, DIOCR_INSTALLER, IntPtr.Zero, 0);
            if (KeyClass.ToInt32() == -1)
            {
                DevicePresent = false;
                return 0;
            }
            UInt32 sizeB = MAX_SIZE_DEVICE_DESCRIPTION;
            ClassDescription.Capacity = MAX_SIZE_DEVICE_DESCRIPTION;
            UInt32 res = RegQueryValueA(KeyClass, 0, ClassDescription, ref sizeB);
            if (res != 0) ClassDescription = new StringBuilder(""); //No device description
            DevicePresent = true;
            ClassesGuid = DeviceInfoData.ClassGuid;
            return 0;
        }
    }
}
