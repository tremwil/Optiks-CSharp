namespace Optiks_CSharp
{
    partial class AppWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppWindow));
            this.unfocusLabel = new System.Windows.Forms.Label();
            this.tabs = new System.Windows.Forms.TabControl();
            this.valueEdit = new System.Windows.Forms.TabPage();
            this.treeViewPanel = new System.Windows.Forms.TabPage();
            this.baseContainer = new System.Windows.Forms.TableLayoutPanel();
            this.logoContainer = new System.Windows.Forms.PictureBox();
            this.canvas = new Optiks_CSharp.DrawingPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simulationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runSimulationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.creditsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graduationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.axesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.crossToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rayNormalsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.physicsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.diffractionNYEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabs.SuspendLayout();
            this.baseContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoContainer)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // unfocusLabel
            // 
            this.unfocusLabel.AutoSize = true;
            this.unfocusLabel.Location = new System.Drawing.Point(0, 0);
            this.unfocusLabel.Name = "unfocusLabel";
            this.unfocusLabel.Size = new System.Drawing.Size(0, 13);
            this.unfocusLabel.TabIndex = 2;
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.valueEdit);
            this.tabs.Controls.Add(this.treeViewPanel);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(3, 3);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(195, 461);
            this.tabs.TabIndex = 3;
            // 
            // valueEdit
            // 
            this.valueEdit.Location = new System.Drawing.Point(4, 22);
            this.valueEdit.Name = "valueEdit";
            this.valueEdit.Padding = new System.Windows.Forms.Padding(3);
            this.valueEdit.Size = new System.Drawing.Size(187, 435);
            this.valueEdit.TabIndex = 0;
            this.valueEdit.Text = "Value editing";
            this.valueEdit.UseVisualStyleBackColor = true;
            // 
            // treeViewPanel
            // 
            this.treeViewPanel.Location = new System.Drawing.Point(4, 22);
            this.treeViewPanel.Name = "treeViewPanel";
            this.treeViewPanel.Padding = new System.Windows.Forms.Padding(3);
            this.treeViewPanel.Size = new System.Drawing.Size(187, 435);
            this.treeViewPanel.TabIndex = 1;
            this.treeViewPanel.Text = "Advanced edtition";
            this.treeViewPanel.UseVisualStyleBackColor = true;
            // 
            // baseContainer
            // 
            this.baseContainer.AutoSize = true;
            this.baseContainer.ColumnCount = 2;
            this.baseContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.baseContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.baseContainer.Controls.Add(this.tabs, 0, 0);
            this.baseContainer.Controls.Add(this.logoContainer, 0, 1);
            this.baseContainer.Controls.Add(this.canvas, 1, 0);
            this.baseContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.baseContainer.Location = new System.Drawing.Point(0, 24);
            this.baseContainer.Name = "baseContainer";
            this.baseContainer.RowCount = 2;
            this.baseContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.baseContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.baseContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.baseContainer.Size = new System.Drawing.Size(804, 537);
            this.baseContainer.TabIndex = 4;
            // 
            // logoContainer
            // 
            this.baseContainer.SetColumnSpan(this.logoContainer, 2);
            this.logoContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logoContainer.Image = ((System.Drawing.Image)(resources.GetObject("logoContainer.Image")));
            this.logoContainer.Location = new System.Drawing.Point(3, 470);
            this.logoContainer.Name = "logoContainer";
            this.logoContainer.Size = new System.Drawing.Size(798, 64);
            this.logoContainer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoContainer.TabIndex = 4;
            this.logoContainer.TabStop = false;
            // 
            // canvas
            // 
            this.canvas.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvas.Location = new System.Drawing.Point(204, 3);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(597, 461);
            this.canvas.TabIndex = 5;
            this.canvas.SizeChanged += new System.EventHandler(this.canvas_SizeChanged);
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.canvas_Paint);
            this.canvas.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseDoubleClick);
            this.canvas.MouseEnter += new System.EventHandler(this.canvas_MouseEnter);
            this.canvas.MouseLeave += new System.EventHandler(this.canvas_MouseLeave);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.editToolStripMenuItem1,
            this.simulationToolStripMenuItem,
            this.creditsToolStripMenuItem,
            this.physicsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(804, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFileToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "File";
            // 
            // newFileToolStripMenuItem
            // 
            this.newFileToolStripMenuItem.Name = "newFileToolStripMenuItem";
            this.newFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newFileToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.newFileToolStripMenuItem.Text = "New File";
            this.newFileToolStripMenuItem.Click += new System.EventHandler(this.newFileToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveToolStripMenuItem.Text = "Save...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // simulationToolStripMenuItem
            // 
            this.simulationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.runSimulationToolStripMenuItem});
            this.simulationToolStripMenuItem.Name = "simulationToolStripMenuItem";
            this.simulationToolStripMenuItem.Size = new System.Drawing.Size(76, 20);
            this.simulationToolStripMenuItem.Text = "Simulation";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.editToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // runSimulationToolStripMenuItem
            // 
            this.runSimulationToolStripMenuItem.Name = "runSimulationToolStripMenuItem";
            this.runSimulationToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.runSimulationToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.runSimulationToolStripMenuItem.Text = "Run Simulation";
            this.runSimulationToolStripMenuItem.Click += new System.EventHandler(this.runSimulationToolStripMenuItem_Click);
            // 
            // creditsToolStripMenuItem
            // 
            this.creditsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetViewToolStripMenuItem,
            this.graduationsToolStripMenuItem,
            this.axesToolStripMenuItem,
            this.pointsToolStripMenuItem,
            this.rayNormalsToolStripMenuItem});
            this.creditsToolStripMenuItem.Name = "creditsToolStripMenuItem";
            this.creditsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.creditsToolStripMenuItem.Text = "View";
            // 
            // resetViewToolStripMenuItem
            // 
            this.resetViewToolStripMenuItem.Name = "resetViewToolStripMenuItem";
            this.resetViewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.resetViewToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.resetViewToolStripMenuItem.Text = "Reset View";
            this.resetViewToolStripMenuItem.Click += new System.EventHandler(this.resetViewToolStripMenuItem_Click);
            // 
            // graduationsToolStripMenuItem
            // 
            this.graduationsToolStripMenuItem.Checked = true;
            this.graduationsToolStripMenuItem.CheckOnClick = true;
            this.graduationsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.graduationsToolStripMenuItem.Name = "graduationsToolStripMenuItem";
            this.graduationsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.graduationsToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.graduationsToolStripMenuItem.Text = "Graduations";
            this.graduationsToolStripMenuItem.Click += new System.EventHandler(this.graduationsToolStripMenuItem_Click);
            // 
            // axesToolStripMenuItem
            // 
            this.axesToolStripMenuItem.Checked = true;
            this.axesToolStripMenuItem.CheckOnClick = true;
            this.axesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.axesToolStripMenuItem.Name = "axesToolStripMenuItem";
            this.axesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.axesToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.axesToolStripMenuItem.Text = "Axes";
            this.axesToolStripMenuItem.Click += new System.EventHandler(this.axesToolStripMenuItem_Click);
            // 
            // pointsToolStripMenuItem
            // 
            this.pointsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pointToolStripMenuItem,
            this.crossToolStripMenuItem});
            this.pointsToolStripMenuItem.Name = "pointsToolStripMenuItem";
            this.pointsToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.pointsToolStripMenuItem.Text = "Points";
            // 
            // pointToolStripMenuItem
            // 
            this.pointToolStripMenuItem.Checked = true;
            this.pointToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pointToolStripMenuItem.Name = "pointToolStripMenuItem";
            this.pointToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.pointToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.pointToolStripMenuItem.Text = "Circle";
            this.pointToolStripMenuItem.Click += new System.EventHandler(this.pointToolStripMenuItem_Click);
            // 
            // crossToolStripMenuItem
            // 
            this.crossToolStripMenuItem.Name = "crossToolStripMenuItem";
            this.crossToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
            this.crossToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.crossToolStripMenuItem.Text = "Cross";
            this.crossToolStripMenuItem.Click += new System.EventHandler(this.crossToolStripMenuItem_Click);
            // 
            // rayNormalsToolStripMenuItem
            // 
            this.rayNormalsToolStripMenuItem.CheckOnClick = true;
            this.rayNormalsToolStripMenuItem.Name = "rayNormalsToolStripMenuItem";
            this.rayNormalsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
            this.rayNormalsToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.rayNormalsToolStripMenuItem.Text = "Ray normals";
            this.rayNormalsToolStripMenuItem.Click += new System.EventHandler(this.rayNormalsToolStripMenuItem_Click);
            // 
            // physicsToolStripMenuItem
            // 
            this.physicsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.diffractionNYEToolStripMenuItem});
            this.physicsToolStripMenuItem.Name = "physicsToolStripMenuItem";
            this.physicsToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.physicsToolStripMenuItem.Text = "Physics";
            // 
            // diffractionNYEToolStripMenuItem
            // 
            this.diffractionNYEToolStripMenuItem.CheckOnClick = true;
            this.diffractionNYEToolStripMenuItem.Name = "diffractionNYEToolStripMenuItem";
            this.diffractionNYEToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.D)));
            this.diffractionNYEToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.diffractionNYEToolStripMenuItem.Text = "Diffraction (NE)";
            this.diffractionNYEToolStripMenuItem.Click += new System.EventHandler(this.diffractionNYEToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem1
            // 
            this.editToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.editToolStripMenuItem1.Name = "editToolStripMenuItem1";
            this.editToolStripMenuItem1.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem1.Text = "Edit";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // AppWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 561);
            this.Controls.Add(this.baseContainer);
            this.Controls.Add(this.unfocusLabel);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(816, 600);
            this.Name = "AppWindow";
            this.Text = "Optiks C# V1.0";
            this.Load += new System.EventHandler(this.AppWindow_Load);
            this.tabs.ResumeLayout(false);
            this.baseContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logoContainer)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label unfocusLabel;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage valueEdit;
        private System.Windows.Forms.TabPage treeViewPanel;
        private System.Windows.Forms.TableLayoutPanel baseContainer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem simulationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runSimulationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem creditsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem axesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem crossToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pointToolStripMenuItem;
        private System.Windows.Forms.PictureBox logoContainer;
        private System.Windows.Forms.ToolStripMenuItem resetViewToolStripMenuItem;
        private DrawingPanel canvas;
        private System.Windows.Forms.ToolStripMenuItem graduationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem physicsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem diffractionNYEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rayNormalsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    }
}

