using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DaybrixSolver
{
	public partial class SolverResult : UserControl
	{
		public delegate void ResultHoverEvent(Board board);
		private readonly ResultHoverEvent onHover;
		private readonly ResultHoverEvent onHoverEnded;

		Board boardRef;

		public SolverResult()
		{
			InitializeComponent();

			boardRef = Board.Empty;
			label1.Text = "SolverResult1SolverResult2SolverResult3SolverResult4SolverResult5";
			onHover = (_) => { };
			onHoverEnded = (_) => { };
		}

		public SolverResult(Board board, string text, ResultHoverEvent callback, ResultHoverEvent leaveCallback)
		{
			InitializeComponent();

			boardRef = board;
			label1.Text = text;
			onHover = callback;
			onHoverEnded = leaveCallback;
		}

		private void label1_MouseEnter(object sender, EventArgs e)
		{
			onHover(boardRef);
		}

		private void label1_MouseLeave(object sender, EventArgs e)
		{
			onHoverEnded(boardRef);
		}
	}
}
