﻿namespace DaybrixSolver
{
	partial class SolverResult
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
			label1 = new Label();
			SuspendLayout();
			// 
			// label1
			// 
			label1.Dock = DockStyle.Fill;
			label1.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point);
			label1.Location = new Point(0, 0);
			label1.Margin = new Padding(0);
			label1.Name = "label1";
			label1.Size = new Size(200, 20);
			label1.TabIndex = 0;
			label1.Text = "label1";
			label1.TextAlign = ContentAlignment.MiddleLeft;
			label1.MouseEnter += label1_MouseEnter;
			label1.MouseLeave += label1_MouseLeave;
			// 
			// SolverResult
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(label1);
			Margin = new Padding(0);
			Name = "SolverResult";
			Size = new Size(200, 20);
			ResumeLayout(false);
		}

		#endregion

		private Label label1;
	}
}
