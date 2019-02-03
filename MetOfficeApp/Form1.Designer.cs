namespace MetOfficeApp
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Daily_Forecast = new System.Windows.Forms.Button();
            this.outputBox = new System.Windows.Forms.TextBox();
            this.postcodeBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ThreeHourlyFC = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataPointAPIKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Daily_Forecast
            // 
            this.Daily_Forecast.Location = new System.Drawing.Point(652, 106);
            this.Daily_Forecast.Name = "Daily_Forecast";
            this.Daily_Forecast.Size = new System.Drawing.Size(106, 23);
            this.Daily_Forecast.TabIndex = 1;
            this.Daily_Forecast.Text = "Daily Forecast";
            this.Daily_Forecast.UseVisualStyleBackColor = true;
            this.Daily_Forecast.Click += new System.EventHandler(this.DailyForecast_Click);
            // 
            // outputBox
            // 
            this.outputBox.Location = new System.Drawing.Point(12, 28);
            this.outputBox.Multiline = true;
            this.outputBox.Name = "outputBox";
            this.outputBox.ReadOnly = true;
            this.outputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputBox.Size = new System.Drawing.Size(622, 410);
            this.outputBox.TabIndex = 2;
            // 
            // postcodeBox
            // 
            this.postcodeBox.AcceptsReturn = true;
            this.postcodeBox.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.postcodeBox.Location = new System.Drawing.Point(652, 44);
            this.postcodeBox.Name = "postcodeBox";
            this.postcodeBox.Size = new System.Drawing.Size(106, 20);
            this.postcodeBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(655, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Enter your Postcode";
            // 
            // ThreeHourlyFC
            // 
            this.ThreeHourlyFC.Location = new System.Drawing.Point(652, 77);
            this.ThreeHourlyFC.Name = "ThreeHourlyFC";
            this.ThreeHourlyFC.Size = new System.Drawing.Size(106, 23);
            this.ThreeHourlyFC.TabIndex = 5;
            this.ThreeHourlyFC.Text = "3-Hourly Forecast";
            this.ThreeHourlyFC.UseVisualStyleBackColor = true;
            this.ThreeHourlyFC.Click += new System.EventHandler(this.ThreeHourlyFC_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataPointAPIKeyToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // dataPointAPIKeyToolStripMenuItem
            // 
            this.dataPointAPIKeyToolStripMenuItem.Name = "dataPointAPIKeyToolStripMenuItem";
            this.dataPointAPIKeyToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.dataPointAPIKeyToolStripMenuItem.Text = "DataPoint API Key";
            this.dataPointAPIKeyToolStripMenuItem.Click += new System.EventHandler(this.dataPointAPIKeyToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.ThreeHourlyFC);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.postcodeBox);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.Daily_Forecast);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(800, 500);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "Form1";
            this.Text = "Met Office Weather";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Daily_Forecast;
        private System.Windows.Forms.TextBox outputBox;
        private System.Windows.Forms.TextBox postcodeBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ThreeHourlyFC;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataPointAPIKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}

