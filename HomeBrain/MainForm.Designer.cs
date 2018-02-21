namespace HomeBrain
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._tray = new System.Windows.Forms.NotifyIcon(this.components);
            this._timer_alive = new System.Windows.Forms.Timer(this.components);
            this._menu_tray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._menu_tray.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tray
            // 
            this._tray.BalloonTipTitle = "Home Brain";
            this._tray.ContextMenuStrip = this._menu_tray;
            this._tray.Icon = ((System.Drawing.Icon)(resources.GetObject("_tray.Icon")));
            this._tray.Text = "Home Brain";
            this._tray.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this._tray_MouseDoubleClick);
            // 
            // _timer_alive
            // 
            this._timer_alive.Interval = 10000;
            this._timer_alive.Tick += new System.EventHandler(this._timer_alive_Tick);
            // 
            // _menu_tray
            // 
            this._menu_tray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this._menu_tray.Name = "_menu_tray";
            this._menu_tray.Size = new System.Drawing.Size(93, 26);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 400);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Home Brain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this._menu_tray.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon _tray;
        private System.Windows.Forms.Timer _timer_alive;
        private System.Windows.Forms.ContextMenuStrip _menu_tray;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}

