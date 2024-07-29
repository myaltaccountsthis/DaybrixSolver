using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaybrixSolver
{
	public class GlitchedBoard : Board
	{
		public GlitchedBoard(string boardStr, int score = 0, int numMoves = 0, params int[] shapes) : base(boardStr, 0, numMoves, shapes)
		{
		}

		public GlitchedBoard(bool[,] board, Move move, Vector2i[] movePoints, int score, int numMoves, Board? prevBoard, LinkedListNode<int>? nextShape) : base(board, move, movePoints, score, numMoves, prevBoard, nextShape) { }

		protected override Board? MakeNewBoard(bool[,] board, Move move, Vector2i[] movePoints, int score, int numMoves, Board? prevBoard, LinkedListNode<int>? nextShape)
		{
			return new GlitchedBoard(board, move, movePoints, score, numMoves, prevBoard, nextShape);
		}

		public override bool IsValid(bool[,] boardValues)
		{
			if (!base.IsValid(boardValues))
				return false;

			bool[] fullRows = new bool[BOARD_HEIGHT];
			for (int y = 0; y < fullRows.Length; y++)
			{
				fullRows[y] = true;
				for (int x = 0; x < BOARD_WIDTH; x++)
				{
					if (!boardValues[x, y])
					{
						fullRows[y] = false;
						break;
					}
				}
			}
			int numFullRows = fullRows.Count(b => b);
			if (numFullRows > 0 && numFullRows < 3)
				return false;

			return true;
		}

		// very hardcoded (hole must be right side)
		public override float Weight
		{
			get
			{
				float max = highestYs.Max();
				float min = highestYs.Min();
				float factor = 1f;
				if (max < 9)
				{
					factor *= 100f;
				}
				else if (max < 12)
				{
				}
				else if (prevMovePoints.FirstOrDefault(pos => pos.y == max).y == max)
				{
					factor /= 2f;
				}
				if (highestYs[BOARD_WIDTH - 1] != -1)
					factor *= 1e6f;

				int change = 0;
				for (int x = 2; x < BOARD_WIDTH - 1; x++)
				{
					change += Math.Abs(highestYs[x] - highestYs[x - 1]);
				}
				
				return (1 + numHoles * 1000f) * (1 + change / 10f) * (1 + (6 + highestYs.Sum(y => y * y) - max * max - min * min) / 10f) * factor;
			}
		}
	}
}
