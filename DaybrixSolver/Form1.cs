using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DaybrixSolver
{
	public partial class Form1 : Form
	{
		static long CurrentTime => DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

		private static readonly Color blankColor = Color.Transparent;
		private static readonly Color bottomColor = Color.Gray;
		private static readonly Color filledColor = Color.OrangeRed;
		private static readonly Color prevMoveColor = Color.LawnGreen;
		private int pictureBoxCounter = 0;
		private PictureBox[,] pictureGrid;
		private Board prevBoard;
		private Board startBoard;
		private bool showPrev;
		private const Keys showPrevKey = Keys.ShiftKey;

		private HttpListener? server;
		private Thread? serverThread;
		private CancellationTokenSource? serverCTS;

		public Form1()
		{
			InitializeComponent();

			pictureGrid = new PictureBox[Board.BOARD_WIDTH, Board.BOARD_HEIGHT];
			prevBoard = Board.Empty;
			startBoard = Board.Empty;
			showPrev = false;
		}

		private void DoSolve(object sender, EventArgs e)
		{
			//lblOutput.Text = "...";
			panel1.Controls.Clear();
			string boardStr = "xxxxxxx xxxxxxx xxxxxxx xx  x   x       x       x       x       x       x       x       x       x";
			long start = CurrentTime;
			//string boardStr = "x       x       x       x       x       x       x       x       x       x       x       x       x";
			//string boardStr = "xxxxxxx xxxxxxx xxxxxxx xx x  x";
			//string boardStr = new string(' ', 8 * 9) + "x x x  x";
			startBoard = MakeBoard(boardStr, (int)numScore.Value, 0, (int)numShape1.Value, (int)numShape2.Value);
			int i = 0;

			int maxDepth = (int)numMaxDepth.Value;
			panel1.SuspendLayout();
			Stack<PieceWeight> stack = new(startBoard.GetBestMoves((int)numDepth.Value).Select(pair => pair.Value).OrderByDescending(pw => pw.Shape));
			long t1 = CurrentTime;
			while (stack.Count > 0)
			{
				PieceWeight pieceWeight = stack.Pop();
				var label = new SolverResult(pieceWeight.Board, new string('-', pieceWeight.Board.Depth - 1) + pieceWeight.ToString(1, 0), DisplayBoard, DisplayDefaultBoard)
				{
					Location = new Point(0, i * 20),
					Size = new Size(panel1.Width, 20)
				};
				panel1.Controls.Add(label);
				i++;
				if (pieceWeight.Board.Depth < maxDepth)
				{
					foreach (var pw in pieceWeight.BestNextPieces.OrderByDescending(p => p.Shape))
					{
						if (pw.Shape != 0)
							stack.Push(pw);
					}
				}
			}
			panel1.ResumeLayout();
			DisplayBoard(startBoard);
			long t2 = CurrentTime;
			Debug.WriteLine("{0}ms calc\n{1}ms draw\n{2}ms total", t1 - start, t2 - t1, t2 - start);
			//lblOutput.Text = string.Join(Environment.NewLine, Enumerable.Range(1, 6).Select(x => string.Join(";", Board.GetShapeMoveCombinations(x).Count())));
		}

		private PictureBox MakePictureBox()
		{
			pictureBoxCounter++;
			PictureBox pictureBox = new()
			{
				BackColor = blankColor,
				Dock = DockStyle.Fill,
				Location = new Point(1, 1),
				Margin = new Padding(0),
				Name = "pictureBox" + pictureBoxCounter,
				Size = new Size(30, 30),
				TabIndex = 0,
				TabStop = false
			};
			return pictureBox;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			gameContainer.SuspendLayout();
			for (int y = 0; y < Board.BOARD_HEIGHT; y++)
			{
				for (int x = 0; x < Board.BOARD_WIDTH; x++)
				{
					var pictureBox = pictureGrid[x, y] = MakePictureBox();
					gameContainer.Controls.Add(pictureBox, x, Board.BOARD_HEIGHT - y - 1);
				}
			}
			gameContainer.ResumeLayout();
		}

		public void DisplayBoard(Board board)
		{
			DisplayBoard(board, false);
		}
		public void DisplayBoard(Board board, bool forceRefresh)
		{
			if (board == prevBoard && !forceRefresh)
				return;
			prevBoard = board;

			bool shouldShowPrev = showPrev && board.PreviousBoard != null;
			Board boardToShow = shouldShowPrev ? board.PreviousBoard : board;

			for (int y = 0; y < boardToShow.BottomRow; y++)
			{
				for (int x = 0; x < Board.BOARD_WIDTH; x++)
					pictureGrid[x, y].BackColor = bottomColor;
			}
			for (int y = boardToShow.BottomRow; y < Board.BOARD_HEIGHT; y++)
			{
				for (int x = 0; x < Board.BOARD_WIDTH; x++)
				{
					pictureGrid[x, y].BackColor = boardToShow.BoardValues[x, y] ? filledColor : blankColor;
				}
			}
			//foreach (Vector2i point in board.PreviousMovePoints)
			if (shouldShowPrev)
				foreach (Vector2i point in Board.GetPieceCoords(board.CurrentMove))
				{
					pictureGrid[point.x, point.y].BackColor = prevMoveColor;
				}
		}

		public void DisplayDefaultBoard(Board previousBoard)
		{
			if (prevBoard != previousBoard)
				return;

			DisplayBoard(startBoard);
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			if ((e.KeyData & showPrevKey) == showPrevKey && !showPrev)
			{
				showPrev = true;
				DisplayBoard(prevBoard, true);
			}
		}

		private void Form1_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyData == showPrevKey && showPrev)
			{
				showPrev = false;
				DisplayBoard(prevBoard, true);
			}
		}

		private void btnServer_Click(object sender, EventArgs e)
		{
			if (server == null)
			{
				btnServer.Text = "Stop Server";
				server = new();
				server.Prefixes.Add(@"http://127.0.0.1:6969/");
				serverCTS = new CancellationTokenSource();
				bool running = true;
				serverThread = new Thread(() =>
				{
					server.Start();
					serverCTS.Token.Register(() =>
					{
						server.Stop();
						server = null;
						serverCTS = null;
						serverThread = null;
						running = false;
					});
					while (running)
					{
						try
						{
							HttpListenerContext context = server.GetContext();
							var outputStream = context.Response.OutputStream;
							Uri? url = context.Request.Url;
							if (url != null && url.LocalPath == "/daybrix")
							{
								context.Response.ContentType = "application/json";
								context.Response.AddHeader("Access-Control-Allow-Headers", "*");
								context.Response.AddHeader("Access-Control-Allow-Origin", "*");
								context.Response.AddHeader("Access-Control-Allow-Methods", "GET");
								context.Response.AddHeader("Access-Control-Max-Age", "86400");
								context.Response.AddHeader("Keep-Alive", "timeout=5, max=100");
								if (context.Request.HttpMethod == "GET")
								{
									var queryParams = context.Request.QueryString;
									int shape1 = int.Parse(queryParams["shape1"]), shape2 = 0;
									int.TryParse(queryParams["shape2"], out shape2);
									int[] nextShapes = new int[2] { shape1, shape2 };
									var bestMoves = MakeBoard(queryParams["board"] ?? "", int.Parse(queryParams["score"] ?? "0"), 0, nextShapes)
										.GetBestMoves(int.Parse(queryParams["depth"] ?? Board.DEPTH.ToString()));
									Move bestMove = bestMoves[shape1].BestMove;
									string message = $"{{\"x\":{bestMove.PosX},\"r\":{bestMove.Rotation}}}";
									outputStream.Write(Encoding.UTF8.GetBytes(message));
								}
							}
							outputStream.Close();
						}
						catch (Exception _)
						{
							// closed
							Debug.WriteLine(_.ToString());
						}
					}
				});
				serverThread.Start();
			}
			else
			{
				btnServer.Text = "Start Server";
				serverCTS?.Cancel();
			}
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			serverCTS?.Cancel();
		}

		private Board MakeBoard(string boardStr, int score = 0, int numMoves = 0, params int[] shapes)
		{
			return cbGlitched.Checked ? new GlitchedBoard(boardStr, score, numMoves, shapes) : new Board(boardStr, score, numMoves, shapes);
		}
	}
}