using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaybrixSolver
{
	public class Board
	{
		public const int BOARD_WIDTH = 8;
		public const int BOARD_HEIGHT = 15;
		public const int DEPTH = 3;
		public const int SHOW_DEPTH = 3;
		protected static readonly int[] pieces = new int[] { 1, 1, 2, 2, 3, 4, 5, 6 };
		public static readonly Board Empty = new();

		public bool[,] BoardValues => board;
		public Move CurrentMove => currentMove;
		public Vector2i[] PreviousMovePoints => prevMovePoints;
		public int Depth => numMoves;
		public Board? PreviousBoard => prevBoard;
		public int BottomRow => GetBottomRow(score);
		public int[] HighestYs => highestYs;

		protected readonly int boardHashCode;
		protected readonly bool[,] board;
		protected readonly int[] highestYs;
		protected readonly int numHoles;
		protected readonly int blocksAboveHoles;
		protected readonly int score;
		protected readonly int numMoves;
		protected readonly Move currentMove;
		protected readonly Vector2i[] prevMovePoints;
		protected readonly Board? prevBoard;
		protected readonly LinkedListNode<int>? nextShape;

		public virtual float Weight => (1 + blocksAboveHoles) * (float)highestYs.Select(y => BOARD_HEIGHT / (BOARD_HEIGHT - (float)y)).Average() * MathF.Pow(1.5f, highestYs.Max() - BottomRow);

		public Board()
		{
			board = new bool[BOARD_WIDTH, BOARD_HEIGHT];
			highestYs = new int[BOARD_WIDTH];
			numHoles = 0;
			score = 0;
			numMoves = 0;
			blocksAboveHoles = 0;
			currentMove = new Move(0, -1, 0);
			prevMovePoints = Array.Empty<Vector2i>();
			prevBoard = null;
			boardHashCode = -1;
			nextShape = null;
		}

		public Board(string boardStr, int score = 0, int numMoves = 0, params int[] shapes) : this(BoardValuesFromString(boardStr), score, numMoves, shapes) {}

		public Board(bool[,] board, int score = 0, int numMoves = 0, params int[] shapes) : this(board, Move.Empty, Array.Empty<Vector2i>(), score, numMoves, null, new LinkedList<int>(shapes.Where(s => s != 0)).First) {}

		public Board(bool[,] board, Move move, Vector2i[] movePoints, int score, int numMoves, Board? prevBoard, LinkedListNode<int>? nextShape)
		{
			this.board = board;
			currentMove = move;
			this.prevBoard = prevBoard;
			prevMovePoints = movePoints;
			this.score = score;
			this.numMoves = numMoves;
			this.nextShape = nextShape;
			boardHashCode = GetBoardHashCode();
			int bottomRow = BottomRow;

			numHoles = 0;
			blocksAboveHoles = 0;
			highestYs = new int[BOARD_WIDTH];
            for (int x = 0; x < BOARD_WIDTH; x++)
			{
				int y;
				for (y = BOARD_HEIGHT - 1; y >= bottomRow; y--)
				{
					if (board[x, y])
						break;
				}
				highestYs[x] = y;
				for (y = bottomRow; y < highestYs[x]; y++)
				{
					if (!board[x, y])
					{
						numHoles++;
						if (y >= BOARD_HEIGHT || board[x, y + 1])
							blocksAboveHoles += highestYs[x] - y;
					}
				}
			}
		}

		public static bool[,] BoardValuesFromString(string boardStr)
		{
			bool[,] board = new bool[BOARD_WIDTH, BOARD_HEIGHT];
			int x = 0;
			int y = 0;
			foreach (char c in boardStr.ToCharArray())
			{
				board[x, y] = c != '_' && !char.IsWhiteSpace(c);
				x++;
				if (x == BOARD_WIDTH)
				{
					x = 0;
					y++;
				}
			}
			return board;
		}

		public static int GetBottomRow(int score)
		{
			return Math.Max(0, score / 10 - 2);
		}

		internal int GetBoardHashCode()
		{
			unchecked
			{
				int hash = 0;
				foreach (bool b in board)
				{
					hash = hash * 23 + b.GetHashCode();
				}
				return hash;
			}
		}

		public Dictionary<int, float> GetNextShapeWeights()
		{
			if (nextShape != null)
				return new() { { nextShape.Value, 1 } };
			Dictionary<int, float> dict = new()
			{
				{ 1, 2 },
				{ 2, 2 }
			};
			if (highestYs.Max() >= 9)
				return dict;

			dict.Add(5, 1);
			dict.Add(6, 1);
			
			if (currentMove.Shape == 3 || currentMove.Shape == 4)
			{
				foreach (var pair in dict)
					dict[pair.Key] = pair.Value + .25f;
			}
			else if (score >= 10)
			{
				dict.Add(3, 1);
			}
			if (score >= 20)
			{
				if (currentMove.Shape == 4)
				{
					dict.Add(3, 1);
				}
				else
				{
					dict.Add(4, 1);
				}
			}
			
			//foreach (int shape in GetNextPieces())
			//{
			//	if (!dict.TryAdd(shape, 1))
			//		dict[shape]++;
			//}
			return dict;
		}

		public virtual bool IsValid(bool[,] boardValues)
		{
			return true;
		}

		protected virtual Board? MakeNewBoard(bool[,] board, Move move, Vector2i[] movePoints, int score, int numMoves, Board? prevBoard, LinkedListNode<int>? nextShape)
		{
			return new Board(board, move, movePoints, score, numMoves, prevBoard, nextShape);
		}

		public Board? PlacePiece(Move move)
		{
			bool[,] newBoard = (bool[,])board.Clone();
			int newScore = score;
			int bottomRow = BottomRow;
			int y;
			for (y = BOARD_HEIGHT - 2; y >= 0; y--)
			{
				Vector2i[] coords = GetPieceCoords(move, y);
				bool overlap = false;
				foreach (Vector2i coord in coords)
				{
					if (coord.y < bottomRow || board[coord.x, coord.y])
					{
						overlap = true;
						break;
					}
				}
				if (overlap)
					break;
			}
			y++;
			// Fail if piece collides at start position
			if (y >= BOARD_HEIGHT - 1)
				return null;

			Vector2i[] pieceCoords = GetPieceCoords(move, y);
			foreach (Vector2i pos in pieceCoords)
			{
				newBoard[pos.x, pos.y] = true;
			}

			if (!IsValid(newBoard))
				return null;

			// Check line clear
			int offset = 0;
			for (int i = bottomRow; i < BOARD_HEIGHT; i++)
			{
				if (i + offset < BOARD_HEIGHT)
				{
					bool shouldClear;
					do
					{
						shouldClear = true;
						for (int x = 0; x < BOARD_WIDTH; x++)
						{
							if (!newBoard[x, i + offset])
							{
								shouldClear = false;
								break;
							}
						}
						if (shouldClear)
						{
							newScore++;
							offset++;
						}
					}
					while (shouldClear);
				}
				for (int x = 0; x < BOARD_WIDTH; x++)
				{
					newBoard[x, i] = i + offset < BOARD_HEIGHT && newBoard[x, i + offset];
				}
			}

			int newBottomRow = GetBottomRow(newScore);
			if (newBottomRow > bottomRow)
			{
				for (int i = BOARD_HEIGHT - 1; i >= newBottomRow; i--)
				{
					// Assume bottom can only go up by 1 at a time (cannot clear 10 rows at once)
					for (int x = 0; x < BOARD_WIDTH; x++)
						newBoard[x, i] = newBoard[x, i - 1];
				}
				for (int i = bottomRow; i < newBottomRow; i++)
				{
					for (int x = 0; x < BOARD_WIDTH; x++)
					{
						newBoard[x, i] = false;
					}
				}

			}

			return MakeNewBoard(newBoard, move, pieceCoords, newScore, numMoves + 1, this, nextShape?.Next);
		}

		public Dictionary<int, PieceWeight> GetBestMoves(int depth = DEPTH)
		{
			if (depth <= 0)
			{
				return new()
				{
					{ 0, new(Weight) }
				};
			}

			bool shouldDoParallel = depth > 1;

			Dictionary<int, PieceWeight> pieceWeights = new();
			var nextShapeWeights = GetNextShapeWeights();
			float sumShapeWeights = nextShapeWeights.Sum(pair => pair.Value);

			Action<KeyValuePair<int, float>> action = pair =>
			{
				// Find the best move
				int shape = pair.Key;
				//List<Board> topBoards = new();

				var bestMoveResult = GetShapeMoveCombinations(shape).Select(move =>
				{
					Board? board = PlacePiece(move);
					if (board == null)
						return null;
					var bestMoves = board.GetBestMoves(depth - 1);
					//Debug.WriteLineIf(board.HighestYs.Max() < 12, board + " " + string.Join("\n", bestMoves.Values));
					return Tuple.Create(board, bestMoves.Sum(pair1 => pair1.Value.AverageWeight * pair1.Value.Multiplier) / bestMoves.Sum(pair1 => pair1.Value.Multiplier), bestMoves);
				}).Where(tuple => tuple != null).OrderBy(pair => pair!.Item2).FirstOrDefault();
				if (bestMoveResult != null)
					pieceWeights.Add(shape, new PieceWeight(shape, bestMoveResult.Item1.currentMove, bestMoveResult.Item1, bestMoveResult.Item2, pair.Value, bestMoveResult.Item3.Values.ToArray()));
			};

			if (shouldDoParallel && false)
				Parallel.ForEach(nextShapeWeights, action);
			else
				foreach (var pair in nextShapeWeights)
					action(pair);

			return pieceWeights;
		}

		public override string ToString()
		{
			return string.Join("\n", Enumerable.Range(0, BOARD_HEIGHT)
				.Select(y => string.Join("", Enumerable.Range(0, BOARD_WIDTH).Select(x => board[x, BOARD_HEIGHT - y - 1]).Select(b => b ? "X" : "_"))));
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(boardHashCode, currentMove, score);
		}

		/// <summary>
		/// Get the integer coordinates of a piece, given its shape, center position, and rotation
		/// </summary>
		/// <param name="shape">Shape of the piece:<br />1: straight<br />2: L<br />3: v<br />4: diagonal<br />5: 2 left 1 up-right<br />6: 2 left 1 down-right</param>
		/// <param name="pos">Position of the center of the piece</param>
		/// <param name="rot"></param>
		/// <returns>Array of each point position</returns>
		/// <exception cref="ArgumentException"></exception>
		public static Vector2i[] GetPieceCoords(int shape, Vector2i pos, int rot)
		{
			switch (shape)
			{
				case 1:
					if (rot == 0 || rot == 2)
						return new Vector2i[] { new(pos.x, pos.y), new(pos.x - 1, pos.y), new(pos.x + 1, pos.y) };
					return new Vector2i[] { new(pos.x, pos.y), new(pos.x, pos.y + 1), new(pos.x, pos.y - 1) };
				case 2:
					if (rot == 0)
						return new Vector2i[] { new(pos.x, pos.y), new(pos.x + 1, pos.y), new(pos.x, pos.y - 1) };
					if (rot == 1)
						return new Vector2i[] { new(pos.x, pos.y), new(pos.x + 1, pos.y), new(pos.x + 1, pos.y - 1) };
					if (rot == 2)
						return new Vector2i[] { new(pos.x + 1, pos.y), new(pos.x, pos.y - 1), new(pos.x + 1, pos.y - 1) };
					return new Vector2i[] { new(pos.x, pos.y), new(pos.x, pos.y - 1), new(pos.x + 1, pos.y - 1) };
				case 3:
					if (rot == 0)
						return new Vector2i[] { new(pos.x - 1, pos.y), new(pos.x + 1, pos.y), new(pos.x, pos.y - 1) };
					if (rot == 1)
						return new Vector2i[] { new(pos.x - 1, pos.y), new(pos.x, pos.y - 1), new(pos.x, pos.y + 1) };
					if (rot == 2)
						return new Vector2i[] { new(pos.x - 1, pos.y), new(pos.x, pos.y + 1), new(pos.x + 1, pos.y) };
					return new Vector2i[] { new(pos.x, pos.y + 1), new(pos.x + 1, pos.y), new(pos.x, pos.y - 1) };
				case 4:
					if ((rot == 0) || (rot == 2))
						return new Vector2i[] { new(pos.x, pos.y), new(pos.x - 1, pos.y + 1), new(pos.x + 1, pos.y - 1) };
					return new Vector2i[] { new(pos.x, pos.y), new(pos.x + 1, pos.y + 1), new(pos.x - 1, pos.y - 1) };
				case 5:
					if (rot == 0)
						return new Vector2i[] { new(pos.x, pos.y), new(pos.x - 1, pos.y), new(pos.x + 1, pos.y + 1) };
					if (rot == 1)
						return new Vector2i[] { new(pos.x, pos.y), new(pos.x, pos.y + 1), new(pos.x + 1, pos.y - 1) };
					if (rot == 2)
						return new Vector2i[] { new(pos.x, pos.y), new(pos.x + 1, pos.y), new(pos.x - 1, pos.y - 1) };
					return new Vector2i[] { new(pos.x, pos.y), new(pos.x - 1, pos.y + 1), new(pos.x, pos.y - 1) };
				case 6:
					if (rot == 0)
						return new Vector2i[] { new(pos.x, pos.y), new(pos.x - 1, pos.y), new(pos.x + 1, pos.y - 1) };
					if (rot == 1)
						return new Vector2i[] { new(pos.x, pos.y), new(pos.x, pos.y + 1), new(pos.x - 1, pos.y - 1) };
					if (rot == 2)
						return new Vector2i[] { new(pos.x, pos.y), new(pos.x + 1, pos.y), new(pos.x - 1, pos.y + 1) };
					return new Vector2i[] { new(pos.x, pos.y), new(pos.x + 1, pos.y + 1), new(pos.x, pos.y - 1) };
				default:
					throw new ArgumentException("Invalid shape given");
			}
		}
		public static Vector2i[] GetPieceCoords(Move move, int y = BOARD_HEIGHT - 2)
		{
			return GetPieceCoords(move.Shape, new Vector2i(move.PosX, y), move.Rotation);
		}

		public static IEnumerable<Move> GetShapeMoveCombinations(int shape)
		{
			int[] rotations = GetShapeUniqueRotations(shape);
			for (int x = 0; x < BOARD_WIDTH; x++)
			{
				foreach (int rot in rotations)
				{
					Move move = new(shape, x, rot);
					if (GetPieceCoords(move).All(IsInBounds))
						yield return move;
				}
			}
		}

		public static string GetShapeName(int shape)
		{
			return shape switch
			{
				1 => "I",
				2 => "L",
				3 => "V",
				4 => "/",
				5 => "_.",
				6 => "._",
				_ => string.Empty,
			};
		}

		public static int[] GetShapeUniqueRotations(int shape)
		{
			if (shape == 1 || shape == 4)
				return new int[] { 0, 1 };
			return new int[] { 0, 1, 2, 3 };
		}
		
		public static bool IsInBounds(int x, int y = BOARD_HEIGHT - 1)
		{
			return x >= 0 && x < BOARD_WIDTH && y >= 0;
		}
		public static bool IsInBounds(Vector2i pos)
		{
			return IsInBounds(pos.x, pos.y);
		}
	}

	public readonly struct PieceWeight
	{
		public readonly int Shape;
		public readonly Move BestMove;
		public readonly Board Board;
		public readonly float AverageWeight;
		public readonly float Multiplier;
		public readonly PieceWeight[] BestNextPieces;
		public PieceWeight(int shape, Move bestMove, Board board, float averageWeight, float mult, PieceWeight[] nextMoves)
		{
			Shape = shape;
			BestMove = bestMove;
			Board = board;
			AverageWeight = averageWeight;
			Multiplier = mult;
			BestNextPieces = nextMoves;
		}
		public PieceWeight(float averageWeight)
		{
			Shape = 0;
			BestMove = Move.Empty;
			Board = Board.Empty;
			AverageWeight = averageWeight;
			Multiplier = 1;
			BestNextPieces = Array.Empty<PieceWeight>();
		}

		public string ToString(int tabs = 1, int maxDepth = Board.SHOW_DEPTH)
		{
			string str = string.Format("{0}, w={1:F3}", BestMove.ToString(), AverageWeight);
			if (tabs < maxDepth && BestNextPieces != null && BestNextPieces.Length > 0 && BestNextPieces[0].Shape > 0)
			{
				str += Environment.NewLine;
				str += string.Join(Environment.NewLine, BestNextPieces.Select(pieceWeight => new string('-', tabs) + pieceWeight.ToString(tabs + 1, maxDepth)));
			}
			return str;
		}
		public override string ToString()
		{
			return ToString();
		}
	}

	public readonly struct Move
	{
		public static readonly Move Empty = new();
		public readonly int Shape;
		public readonly int PosX;
		public readonly int Rotation;
		public Move(int shape, int pos, int rot)
		{
			Shape = shape;
			PosX = pos;
			Rotation = rot;
		}
		public override bool Equals(object? obj)
		{
			return obj != null && obj is Move other && Shape == other.Shape && PosX == other.PosX &&
				((Shape == 1 || Shape == 4) ? Rotation % 2 == other.Rotation % 2 : Rotation == other.Rotation);
		}
		public override int GetHashCode()
		{
			return HashCode.Combine(Shape, PosX, Rotation);
		}
		public override string ToString()
		{
			return string.Format("{{s:{0}, x:{1}, rot:{2}}}", Board.GetShapeName(Shape), PosX, Rotation);
		}
	}

	public struct Vector2i
	{
		public int x;
		public int y;
		public Vector2i(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}
}
