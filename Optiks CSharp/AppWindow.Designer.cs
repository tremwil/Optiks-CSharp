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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.baseContainer = new System.Windows.Forms.TableLayoutPanel();
            this.logoContainer = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bodiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.raysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.angleClippingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.raySurfaceAnglesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.antiAliasingAAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.physicsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.diffractionNYEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.canvas = new Optiks_CSharp.DrawingPanel();
            this.tabs.SuspendLayout();
            this.treeViewPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.treeViewPanel.Controls.Add(this.groupBox1);
            this.treeViewPanel.Location = new System.Drawing.Point(4, 22);
            this.treeViewPanel.Name = "treeViewPanel";
            this.treeViewPanel.Padding = new System.Windows.Forms.Padding(3);
            this.treeViewPanel.Size = new System.Drawing.Size(187, 435);
            this.treeViewPanel.TabIndex = 1;
            this.treeViewPanel.Text = "Settings";
            this.treeViewPanel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(175, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Clipping Angle";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(65, 60);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 5;
            this.textBox2.Text = "5";
            this.textBox2.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.textBox2_PreviewKeyDown);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(65, 34);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "22.5";
            this.textBox1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.textBox1_PreviewKeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Sensivity:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Angle size:";
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
            // editToolStripMenuItem1
            // 
            this.editToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.deleteAllToolStripMenuItem,
            this.angleClippingToolStripMenuItem});
            this.editToolStripMenuItem1.Name = "editToolStripMenuItem1";
            this.editToolStripMenuItem1.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem1.Text = "Edit";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // deleteAllToolStripMenuItem
            // 
            this.deleteAllToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bodiesToolStripMenuItem,
            this.raysToolStripMenuItem});
            this.deleteAllToolStripMenuItem.Name = "deleteAllToolStripMenuItem";
            this.deleteAllToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Del";
            this.deleteAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.deleteAllToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.deleteAllToolStripMenuItem.Text = "Delete all...";
            this.deleteAllToolStripMenuItem.Click += new System.EventHandler(this.deleteAllToolStripMenuItem_Click);
            // 
            // bodiesToolStripMenuItem
            // 
            this.bodiesToolStripMenuItem.Name = "bodiesToolStripMenuItem";
            this.bodiesToolStripMenuItem.RightToLeftAutoMirrorImage = true;
            this.bodiesToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.bodiesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.D0)));
            this.bodiesToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.bodiesToolStripMenuItem.Text = "Bodies";
            this.bodiesToolStripMenuItem.Click += new System.EventHandler(this.bodiesToolStripMenuItem_Click);
            // 
            // raysToolStripMenuItem
            // 
            this.raysToolStripMenuItem.Name = "raysToolStripMenuItem";
            this.raysToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.raysToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.D9)));
            this.raysToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.raysToolStripMenuItem.Text = "Rays";
            this.raysToolStripMenuItem.Click += new System.EventHandler(this.raysToolStripMenuItem_Click);
            // 
            // angleClippingToolStripMenuItem
            // 
            this.angleClippingToolStripMenuItem.Checked = true;
            this.angleClippingToolStripMenuItem.CheckOnClick = true;
            this.angleClippingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.angleClippingToolStripMenuItem.Name = "angleClippingToolStripMenuItem";
            this.angleClippingToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.angleClippingToolStripMenuItem.Text = "Angle clipping";
            this.angleClippingToolStripMenuItem.Click += new System.EventHandler(this.angleClippingToolStripMenuItem_Click);
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
            this.rayNormalsToolStripMenuItem,
            this.raySurfaceAnglesToolStripMenuItem,
            this.antiAliasingAAToolStripMenuItem});
            this.creditsToolStripMenuItem.Name = "creditsToolStripMenuItem";
            this.creditsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.creditsToolStripMenuItem.Text = "View";
            // 
            // resetViewToolStripMenuItem
            // 
            this.resetViewToolStripMenuItem.Name = "resetViewToolStripMenuItem";
            this.resetViewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.resetViewToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
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
            this.graduationsToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
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
            this.axesToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.axesToolStripMenuItem.Text = "Axes";
            this.axesToolStripMenuItem.Click += new System.EventHandler(this.axesToolStripMenuItem_Click);
            // 
            // pointsToolStripMenuItem
            // 
            this.pointsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pointToolStripMenuItem,
            this.crossToolStripMenuItem});
            this.pointsToolStripMenuItem.Name = "pointsToolStripMenuItem";
            this.pointsToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
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
            this.rayNormalsToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.rayNormalsToolStripMenuItem.Text = "Ray normals";
            this.rayNormalsToolStripMenuItem.Click += new System.EventHandler(this.rayNormalsToolStripMenuItem_Click);
            // 
            // raySurfaceAnglesToolStripMenuItem
            // 
            this.raySurfaceAnglesToolStripMenuItem.CheckOnClick = true;
            this.raySurfaceAnglesToolStripMenuItem.Name = "raySurfaceAnglesToolStripMenuItem";
            this.raySurfaceAnglesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.raySurfaceAnglesToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.raySurfaceAnglesToolStripMenuItem.Text = "Ray surface angles";
            this.raySurfaceAnglesToolStripMenuItem.Click += new System.EventHandler(this.raySurfaceAnglesToolStripMenuItem_Click);
            // 
            // antiAliasingAAToolStripMenuItem
            // 
            this.antiAliasingAAToolStripMenuItem.Checked = true;
            this.antiAliasingAAToolStripMenuItem.CheckOnClick = true;
            this.antiAliasingAAToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.antiAliasingAAToolStripMenuItem.Name = "antiAliasingAAToolStripMenuItem";
            this.antiAliasingAAToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.antiAliasingAAToolStripMenuItem.Size = new System.Drawing.Size(245, 22);
            this.antiAliasingAAToolStripMenuItem.Text = "Anti-Aliasing (AA)";
            this.antiAliasingAAToolStripMenuItem.Click += new System.EventHandler(this.antiAliasingAAToolStripMenuItem_Click);
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
            this.diffractionNYEToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.diffractionNYEToolStripMenuItem.Text = "Diffraction";
            this.diffractionNYEToolStripMenuItem.Click += new System.EventHandler(this.diffractionNYEToolStripMenuItem_Click);
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
            this.treeViewPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem deleteAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bodiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem raysToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem raySurfaceAnglesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem angleClippingToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem antiAliasingAAToolStripMenuItem;
    }
}

