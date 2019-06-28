namespace Alturos.ImageAnnotation.CustomControls
{
    partial class AnnotationDrawControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.LegendItem legendItem1 = new System.Windows.Forms.DataVisualization.Charting.LegendItem();
            System.Windows.Forms.DataVisualization.Charting.LegendItem legendItem2 = new System.Windows.Forms.DataVisualization.Charting.LegendItem();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.contextMenuStripPicture = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearAnnotationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.legendsChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.rotatingPictureBox = new Alturos.ImageAnnotation.CustomControls.RotatingPictureBox();
            this.contextMenuStripPicture.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.legendsChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotatingPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStripPicture
            // 
            this.contextMenuStripPicture.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearAnnotationsToolStripMenuItem});
            this.contextMenuStripPicture.Name = "contextMenuStripPicture";
            this.contextMenuStripPicture.Size = new System.Drawing.Size(170, 26);
            // 
            // clearAnnotationsToolStripMenuItem
            // 
            this.clearAnnotationsToolStripMenuItem.Image = global::Alturos.ImageAnnotation.Properties.Resources.asterisk_yellow;
            this.clearAnnotationsToolStripMenuItem.Name = "clearAnnotationsToolStripMenuItem";
            this.clearAnnotationsToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.clearAnnotationsToolStripMenuItem.Text = "&Clear Annotations";
            this.clearAnnotationsToolStripMenuItem.Click += new System.EventHandler(this.ClearAnnotationsToolStripMenuItem_Click);
            // 
            // legendsChart
            // 
            this.legendsChart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.legendsChart.BackColor = System.Drawing.Color.Transparent;
            legend1.Alignment = System.Drawing.StringAlignment.Center;
            legend1.BackColor = System.Drawing.Color.Transparent;
            legendItem1.Color = System.Drawing.Color.Aqua;
            legendItem1.Name = "Legend 1";
            legendItem2.Color = System.Drawing.Color.Red;
            legendItem2.Name = "Legend 2";
            legend1.CustomItems.Add(legendItem1);
            legend1.CustomItems.Add(legendItem2);
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.IsDockedInsideChartArea = false;
            legend1.Name = "Legend1";
            this.legendsChart.Legends.Add(legend1);
            this.legendsChart.Location = new System.Drawing.Point(0, 287);
            this.legendsChart.Name = "legendsChart";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.legendsChart.Series.Add(series1);
            this.legendsChart.Size = new System.Drawing.Size(624, 49);
            this.legendsChart.TabIndex = 2;
            this.legendsChart.Text = "chart1";
            // 
            // rotatingPictureBox
            // 
            this.rotatingPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rotatingPictureBox.Image = null;
            this.rotatingPictureBox.Location = new System.Drawing.Point(0, 0);
            this.rotatingPictureBox.Name = "rotatingPictureBox";
            this.rotatingPictureBox.Size = new System.Drawing.Size(624, 336);
            this.rotatingPictureBox.TabIndex = 3;
            this.rotatingPictureBox.TabStop = false;
            this.rotatingPictureBox.ContextMenuStrip = this.contextMenuStripPicture;
            this.rotatingPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.rotatingPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox1_Paint);
            this.rotatingPictureBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseDoubleClick);
            this.rotatingPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseDown);
            this.rotatingPictureBox.MouseEnter += new System.EventHandler(this.PictureBox1_MouseEnter);
            this.rotatingPictureBox.MouseLeave += new System.EventHandler(this.PictureBox1_MouseLeave);
            this.rotatingPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseMove);
            this.rotatingPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseUp);
            // 
            // AnnotationDrawControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rotatingPictureBox);
            this.Controls.Add(this.legendsChart);
            this.Controls.Add(this.rotatingPictureBox);
            this.Name = "AnnotationDrawControl";
            this.Size = new System.Drawing.Size(624, 336);
            ((System.ComponentModel.ISupportInitialize)(this.rotatingPictureBox)).EndInit();
            this.contextMenuStripPicture.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.legendsChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotatingPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStripPicture;
        private System.Windows.Forms.ToolStripMenuItem clearAnnotationsToolStripMenuItem;
        private System.Windows.Forms.DataVisualization.Charting.Chart legendsChart;
        private RotatingPictureBox rotatingPictureBox;
    }
}
