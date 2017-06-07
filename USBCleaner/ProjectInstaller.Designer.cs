namespace USBCleaner
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.USBCleanerProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.USBCleanerInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // USBCleanerProcessInstaller
            // 
            this.USBCleanerProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.USBCleanerProcessInstaller.Password = null;
            this.USBCleanerProcessInstaller.Username = null;
            // 
            // USBCleanerInstaller
            // 
            this.USBCleanerInstaller.Description = "USB 设备使用记录清除。";
            this.USBCleanerInstaller.DisplayName = "USB Cleaner";
            this.USBCleanerInstaller.ServiceName = "USBCleaner";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.USBCleanerProcessInstaller,
            this.USBCleanerInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller USBCleanerProcessInstaller;
        private System.ServiceProcess.ServiceInstaller USBCleanerInstaller;
    }
}