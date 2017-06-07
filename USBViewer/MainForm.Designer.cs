namespace USBViewer
{
    partial class MainForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStripMainForm = new System.Windows.Forms.ToolStrip();
            this.tsbDetect = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.tsbQuit = new System.Windows.Forms.ToolStripButton();
            this.lvwList = new System.Windows.Forms.ListView();
            this.clnIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clnUsbstor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clnUsbstorName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageListMainForm = new System.Windows.Forms.ImageList(this.components);
            this.toolStripMainForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMainForm
            // 
            this.toolStripMainForm.AutoSize = false;
            this.toolStripMainForm.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripMainForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbDetect,
            this.tsbDelete,
            this.tsbQuit});
            this.toolStripMainForm.Location = new System.Drawing.Point(0, 0);
            this.toolStripMainForm.Name = "toolStripMainForm";
            this.toolStripMainForm.Size = new System.Drawing.Size(784, 64);
            this.toolStripMainForm.TabIndex = 0;
            this.toolStripMainForm.Text = "toolStrip1";
            // 
            // tsbDetect
            // 
            this.tsbDetect.AutoSize = false;
            this.tsbDetect.Image = global::USBViewer.Properties.Resources.search;
            this.tsbDetect.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbDetect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDetect.Name = "tsbDetect";
            this.tsbDetect.Size = new System.Drawing.Size(61, 61);
            this.tsbDetect.Text = "检测";
            this.tsbDetect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbDetect.Click += new System.EventHandler(this.tsbDetect_Click);
            // 
            // tsbDelete
            // 
            this.tsbDelete.AutoSize = false;
            this.tsbDelete.Image = global::USBViewer.Properties.Resources.delete;
            this.tsbDelete.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(61, 61);
            this.tsbDelete.Text = "删除";
            this.tsbDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // tsbQuit
            // 
            this.tsbQuit.AutoSize = false;
            this.tsbQuit.Image = global::USBViewer.Properties.Resources.quit;
            this.tsbQuit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbQuit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbQuit.Name = "tsbQuit";
            this.tsbQuit.Size = new System.Drawing.Size(61, 61);
            this.tsbQuit.Text = "退出";
            this.tsbQuit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbQuit.Click += new System.EventHandler(this.tsbQuit_Click);
            // 
            // lvwList
            // 
            this.lvwList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clnIndex,
            this.clnUsbstor,
            this.clnUsbstorName});
            this.lvwList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwList.FullRowSelect = true;
            this.lvwList.GridLines = true;
            this.lvwList.Location = new System.Drawing.Point(0, 64);
            this.lvwList.Name = "lvwList";
            this.lvwList.Size = new System.Drawing.Size(784, 498);
            this.lvwList.SmallImageList = this.imageListMainForm;
            this.lvwList.TabIndex = 1;
            this.lvwList.UseCompatibleStateImageBehavior = false;
            this.lvwList.View = System.Windows.Forms.View.Details;
            // 
            // clnIndex
            // 
            this.clnIndex.Text = "";
            this.clnIndex.Width = 40;
            // 
            // clnUsbstor
            // 
            this.clnUsbstor.Text = "USB 存储设备标记";
            this.clnUsbstor.Width = 400;
            // 
            // clnUsbstorName
            // 
            this.clnUsbstorName.Text = "USB 存储设备 / 厂商名称";
            this.clnUsbstorName.Width = 400;
            // 
            // imageListMainForm
            // 
            this.imageListMainForm.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMainForm.ImageStream")));
            this.imageListMainForm.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMainForm.Images.SetKeyName(0, "usb.png");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.lvwList);
            this.Controls.Add(this.toolStripMainForm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "USB 存储设备使用痕迹检测";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStripMainForm.ResumeLayout(false);
            this.toolStripMainForm.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMainForm;
        private System.Windows.Forms.ListView lvwList;
        private System.Windows.Forms.ColumnHeader clnIndex;
        private System.Windows.Forms.ImageList imageListMainForm;
        private System.Windows.Forms.ToolStripButton tsbDetect;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.ToolStripButton tsbQuit;
        private System.Windows.Forms.ColumnHeader clnUsbstor;
        private System.Windows.Forms.ColumnHeader clnUsbstorName;
    }
}

