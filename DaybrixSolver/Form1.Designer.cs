namespace DaybrixSolver
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			btnSolve = new Button();
			groupBox1 = new GroupBox();
			panel1 = new Panel();
			numDepth = new NumericUpDown();
			label1 = new Label();
			label2 = new Label();
			numMaxDepth = new NumericUpDown();
			gameContainer = new TableLayoutPanel();
			numShape1 = new NumericUpDown();
			label4 = new Label();
			numShape2 = new NumericUpDown();
			label5 = new Label();
			numScore = new NumericUpDown();
			label6 = new Label();
			btnServer = new Button();
			cbGlitched = new CheckBox();
			groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)numDepth).BeginInit();
			((System.ComponentModel.ISupportInitialize)numMaxDepth).BeginInit();
			((System.ComponentModel.ISupportInitialize)numShape1).BeginInit();
			((System.ComponentModel.ISupportInitialize)numShape2).BeginInit();
			((System.ComponentModel.ISupportInitialize)numScore).BeginInit();
			SuspendLayout();
			// 
			// btnSolve
			// 
			btnSolve.Anchor = AnchorStyles.Top;
			btnSolve.Location = new Point(259, 125);
			btnSolve.Margin = new Padding(5);
			btnSolve.Name = "btnSolve";
			btnSolve.Size = new Size(100, 40);
			btnSolve.TabIndex = 0;
			btnSolve.Text = "Solve";
			btnSolve.UseVisualStyleBackColor = true;
			btnSolve.Click += DoSolve;
			// 
			// groupBox1
			// 
			groupBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			groupBox1.Controls.Add(panel1);
			groupBox1.Location = new Point(15, 177);
			groupBox1.Margin = new Padding(6);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new Size(313, 467);
			groupBox1.TabIndex = 1;
			groupBox1.TabStop = false;
			groupBox1.Text = "Output";
			// 
			// panel1
			// 
			panel1.AutoScroll = true;
			panel1.Dock = DockStyle.Fill;
			panel1.Location = new Point(3, 23);
			panel1.Name = "panel1";
			panel1.Padding = new Padding(3);
			panel1.Size = new Size(307, 441);
			panel1.TabIndex = 0;
			// 
			// numDepth
			// 
			numDepth.Anchor = AnchorStyles.Top;
			numDepth.Location = new Point(239, 16);
			numDepth.Margin = new Padding(5);
			numDepth.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
			numDepth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			numDepth.Name = "numDepth";
			numDepth.Size = new Size(46, 27);
			numDepth.TabIndex = 2;
			numDepth.Value = new decimal(new int[] { 3, 0, 0, 0 });
			// 
			// label1
			// 
			label1.Anchor = AnchorStyles.Top;
			label1.Location = new Point(180, 16);
			label1.Margin = new Padding(5);
			label1.Name = "label1";
			label1.Size = new Size(53, 25);
			label1.TabIndex = 3;
			label1.Text = "Depth";
			label1.TextAlign = ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			label2.Anchor = AnchorStyles.Top;
			label2.Location = new Point(295, 18);
			label2.Margin = new Padding(5);
			label2.Name = "label2";
			label2.Size = new Size(105, 25);
			label2.TabIndex = 5;
			label2.Text = "Display Depth";
			label2.TextAlign = ContentAlignment.MiddleRight;
			// 
			// numMaxDepth
			// 
			numMaxDepth.Anchor = AnchorStyles.Top;
			numMaxDepth.Location = new Point(406, 16);
			numMaxDepth.Margin = new Padding(5);
			numMaxDepth.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
			numMaxDepth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			numMaxDepth.Name = "numMaxDepth";
			numMaxDepth.Size = new Size(46, 27);
			numMaxDepth.TabIndex = 4;
			numMaxDepth.Value = new decimal(new int[] { 2, 0, 0, 0 });
			// 
			// gameContainer
			// 
			gameContainer.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			gameContainer.BackColor = SystemColors.Control;
			gameContainer.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
			gameContainer.ColumnCount = 8;
			gameContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));
			gameContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));
			gameContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));
			gameContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));
			gameContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));
			gameContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));
			gameContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));
			gameContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));
			gameContainer.Location = new Point(347, 177);
			gameContainer.Name = "gameContainer";
			gameContainer.RowCount = 15;
			gameContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			gameContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			gameContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			gameContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			gameContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			gameContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			gameContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			gameContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			gameContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			gameContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			gameContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			gameContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			gameContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			gameContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			gameContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
			gameContainer.Size = new Size(249, 466);
			gameContainer.TabIndex = 6;
			// 
			// numShape1
			// 
			numShape1.Anchor = AnchorStyles.Top;
			numShape1.Location = new Point(239, 90);
			numShape1.Margin = new Padding(5);
			numShape1.Maximum = new decimal(new int[] { 6, 0, 0, 0 });
			numShape1.Name = "numShape1";
			numShape1.Size = new Size(46, 27);
			numShape1.TabIndex = 7;
			// 
			// label4
			// 
			label4.Anchor = AnchorStyles.Top;
			label4.Location = new Point(202, 53);
			label4.Margin = new Padding(5);
			label4.Name = "label4";
			label4.Size = new Size(105, 25);
			label4.TabIndex = 10;
			label4.Text = "Score";
			label4.TextAlign = ContentAlignment.MiddleRight;
			// 
			// numShape2
			// 
			numShape2.Anchor = AnchorStyles.Top;
			numShape2.Location = new Point(406, 90);
			numShape2.Margin = new Padding(5);
			numShape2.Maximum = new decimal(new int[] { 6, 0, 0, 0 });
			numShape2.Name = "numShape2";
			numShape2.Size = new Size(46, 27);
			numShape2.TabIndex = 9;
			// 
			// label5
			// 
			label5.Anchor = AnchorStyles.Top;
			label5.Location = new Point(295, 90);
			label5.Margin = new Padding(5);
			label5.Name = "label5";
			label5.Size = new Size(105, 25);
			label5.TabIndex = 14;
			label5.Text = "Next Shape";
			label5.TextAlign = ContentAlignment.MiddleRight;
			// 
			// numScore
			// 
			numScore.Anchor = AnchorStyles.Top;
			numScore.Location = new Point(313, 53);
			numScore.Margin = new Padding(5);
			numScore.Maximum = new decimal(new int[] { 150, 0, 0, 0 });
			numScore.Name = "numScore";
			numScore.Size = new Size(46, 27);
			numScore.TabIndex = 13;
			// 
			// label6
			// 
			label6.Anchor = AnchorStyles.Top;
			label6.Location = new Point(128, 88);
			label6.Margin = new Padding(5);
			label6.Name = "label6";
			label6.Size = new Size(105, 25);
			label6.TabIndex = 12;
			label6.Text = "Shape";
			label6.TextAlign = ContentAlignment.MiddleRight;
			// 
			// btnServer
			// 
			btnServer.Location = new Point(18, 18);
			btnServer.Margin = new Padding(5);
			btnServer.Name = "btnServer";
			btnServer.Size = new Size(100, 40);
			btnServer.TabIndex = 15;
			btnServer.Text = "Start Server";
			btnServer.UseVisualStyleBackColor = true;
			btnServer.Click += btnServer_Click;
			// 
			// cbGlitched
			// 
			cbGlitched.AutoSize = true;
			cbGlitched.Location = new Point(19, 66);
			cbGlitched.Name = "cbGlitched";
			cbGlitched.Size = new Size(86, 24);
			cbGlitched.TabIndex = 16;
			cbGlitched.Text = "Glitched";
			cbGlitched.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(608, 658);
			Controls.Add(cbGlitched);
			Controls.Add(btnServer);
			Controls.Add(label5);
			Controls.Add(numScore);
			Controls.Add(label6);
			Controls.Add(label4);
			Controls.Add(numShape2);
			Controls.Add(numShape1);
			Controls.Add(gameContainer);
			Controls.Add(label2);
			Controls.Add(numMaxDepth);
			Controls.Add(label1);
			Controls.Add(numDepth);
			Controls.Add(groupBox1);
			Controls.Add(btnSolve);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			KeyPreview = true;
			MaximizeBox = false;
			Name = "Form1";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Daybrix Solver";
			FormClosed += Form1_FormClosed;
			Load += Form1_Load;
			KeyDown += Form1_KeyDown;
			KeyUp += Form1_KeyUp;
			groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)numDepth).EndInit();
			((System.ComponentModel.ISupportInitialize)numMaxDepth).EndInit();
			((System.ComponentModel.ISupportInitialize)numShape1).EndInit();
			((System.ComponentModel.ISupportInitialize)numShape2).EndInit();
			((System.ComponentModel.ISupportInitialize)numScore).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button btnSolve;
		private GroupBox groupBox1;
		private NumericUpDown numDepth;
		private Label label1;
		private Label label2;
		private NumericUpDown numMaxDepth;
		private TableLayoutPanel gameContainer;
		private Panel panel1;
		private NumericUpDown numShape1;
		private Label label4;
		private NumericUpDown numShape2;
		private Label label5;
		private NumericUpDown numScore;
		private Label label6;
		private Button btnServer;
		private CheckBox cbGlitched;
	}
}