using System;
using System.Drawing;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace ChessLibrary
{
	/// <summary>
	/// The class stores info about the chess player. Like 
	/// his type, name, image and side.
	/// </summary>
    [Serializable]
	public class Player
	{
		private Type type;		// Type of the player i.e. computer or human
		private Side side;		// Player side i.e. white or black
		private string name;		// Name of the Player
		private Image image;		// Image of the player
		private Rules rules;		// A reference to the chess rules
		private TimeSpan maxThinkTime;		// Maximum think time in seconds

		private TimeSpan totalThinkTime;	// Stores total think time of the player
		private DateTime startTime;		// User turn time starts
		private int	totalMovesAnalyzed;	// Total no. of moves analzyed by the player
		private bool gameNearEnd;			// True when the game is near the end
		public enum Type{Human, Computer};

        // Empty Constructor for XML serialization
        internal Player()
        {
            side = PlayerSide;
            type = PlayerType;
            maxThinkTime = new TimeSpan(0, 0, 4);	// maximum think for 4 seconds
            totalThinkTime = (DateTime.Now - DateTime.Now);	// Reset the time
        }

		// Constructor for the new playe
		public Player(Side PlayerSide, Type PlayerType)
		{
			side=PlayerSide;
			type=PlayerType;
			maxThinkTime = new TimeSpan(0,0,4);	// maximum think for 4 seconds
			totalThinkTime = (DateTime.Now - DateTime.Now);	// Reset the time
		}

		// Constructor for the new playe
		public Player(Side PlayerSide, Type PlayerType, Rules rules) : this(PlayerSide,PlayerType)
		{
			rules=rules;	
		}

		// User turn/thinking time starts
		public void TimeStart()
		{
			startTime=DateTime.Now;
		}

		// User turn/thinking time ends
		public void TimeEnd()
		{
			totalThinkTime+= (DateTime.Now - startTime);
		}

		// Update the user time
		public void UpdateTime()
		{
		}

		// Returns true if the current player is computer
		public bool IsComputer()
		{
			return (type==Type.Computer);
		}

		// Reset the player timers
		public void ResetTime()
		{
			totalThinkTime = (DateTime.Now - DateTime.Now);	// Reset time
		}

        // Get or Set the game rules
        [XmlIgnore]
        internal Rules GameRules
        {
            set { rules = value; }
        }

		// Get the best move available to the player
		public Move GetFixBestMove()
		{
			int alpha, beta;
			int depth;					// depth to which to do the search
			TimeSpan ElapsedTime= new TimeSpan(1);		// Total elpased time
			Move BestMove=null;		// The best move for the current position

			// Initialize constants
			const int MIN_SCORE= -1000000;		// Minimum limit of negative for integer
			const int MAX_SCORE= 1000000;		// Maximum limit of positive integer

			ArrayList TotalMoves=rules.GenerateAllLegalMoves(side); // Get all the legal moves for the current side
			ArrayList PlayerCells = rules.ChessBoard.GetSideCell(side.type);

			alpha = MIN_SCORE;	// The famous Alpha & Beta are set to their initial values
			beta  = MAX_SCORE;	// at the start of each increasing search depth iteration

			depth=3;

			// Loop through all the legal moves and get the one with best score
			foreach (Move move in TotalMoves)
			{
				// Now to get the effect of this move; execute this move and analyze the board
				rules.ExecuteMove(move);
				move.Score = -AlphaBeta(rules.ChessGame.EnemyPlayer(side).PlayerSide,depth - 1, -beta, -alpha);
				rules.UndoMove(move);	// undo the move

				// If the score of the move we just tried is better than the score of the best move we had 
				// so far, at this depth, then make this the best move.
				if (move.Score > alpha)
				{
					BestMove = move;
					alpha = move.Score;
				}
			}		
			return BestMove;
		}


		// Get the best move available to the player
		public Move GetBestMove()
		{
			int alpha, beta;
			int depth;					// depth to which to do the search
			TimeSpan ElapsedTime= new TimeSpan(1);		// Total elpased time
			Move BestMove=null;		// The best move for the current position

			// Initialize constants
			const int MIN_SCORE= -10000000;		// Minimum limit of negative for integer
			const int MAX_SCORE= 10000000;		// Maximum limit of positive integer

			ArrayList TotalMoves=rules.GenerateAllLegalMoves(side); // Get all the legal moves for the current side

			// Now we use the Iterative deepening technique to search the best move
			// The idea is just simple, we will keep searching in the more and more depth
			// as long as we don't time out.
			// So, it means that when we have less moves, we can search more deeply and which means
			// better chess game.
			DateTime ThinkStartTime=DateTime.Now;
			int MoveCounter;
			Random RandGenerator= new Random();

			// Game is near the end, or the current player is under check
			if (rules.ChessBoard.GetSideCell(side.type).Count<=5 || TotalMoves.Count <= 5 )
				gameNearEnd = true;

			// Game is near the end, or the Enemy player is under check
			Side EnemySide;

			if (side.isBlack())
				EnemySide = rules.ChessGame.WhitePlayer.PlayerSide;
			else
				EnemySide = rules.ChessGame.BlackPlayer.PlayerSide;

			if (rules.ChessBoard.GetSideCell(side.Enemy()).Count<=5 || rules.GenerateAllLegalMoves(EnemySide).Count <= 5 )
				gameNearEnd = true;

			totalMovesAnalyzed=0;		// Reset the total moves anazlye counter

			for (depth = 1;; depth++)	// Keep doing a depth search 
			{
				alpha = MIN_SCORE;	// The famous Alpha & Beta are set to their initial values
				beta  = MAX_SCORE;	// at the start of each increasing search depth iteration
				MoveCounter = 0;	// Initialize the move counter variable

				// Loop through all the legal moves and get the one with best score
				foreach (Move move in TotalMoves)
				{
					MoveCounter++;

					// Now to get the effect of this move; execute this move and analyze the board
					rules.ExecuteMove(move);
					move.Score = -AlphaBeta(rules.ChessGame.EnemyPlayer(side).PlayerSide,depth - 1, -beta, -alpha);
					totalMovesAnalyzed++;	// Increment move counter
					rules.UndoMove(move);	// undo the move

					// If the score of the move we just tried is better than the score of the best move we had 
					// so far, at this depth, then make this the best move.
					if (move.Score > alpha)
					{
						BestMove = move;
						alpha = move.Score;
					}

					rules.ChessGame.NotifyComputerThinking(depth, MoveCounter, TotalMoves.Count,totalMovesAnalyzed, BestMove );

					// Check if the user time has expired
					ElapsedTime=DateTime.Now - ThinkStartTime;
					if ( ElapsedTime.Ticks > (maxThinkTime.Ticks) )	// More than 75 percent time is available
						break;							// Force break the loop
				}

				// Check if the user time has expired
				ElapsedTime=DateTime.Now - ThinkStartTime;
				if ( ElapsedTime.Ticks > (maxThinkTime.Ticks*0.25))	// More than 75 percent time is available
					break;							// Force break the loop
			}
		
			rules.ChessGame.NotifyComputerThinking(depth, MoveCounter, TotalMoves.Count,totalMovesAnalyzed, BestMove );
			return BestMove;
		}

		// Alpha and beta search to recursively travers the tree to calculate the best move
		private int AlphaBeta(Side PlayerSide, int depth, int alpha, int beta)
		{
			int val;
			System.Windows.Forms.Application.DoEvents();

			// Before we do anything, let's try the null move. It's like giving the opponent
			// a free shot and see if he can damage us. If he can't, we are in a better position and 
			// can nock down him

			// "Adaptive" Null-move forward pruning
			int R = (depth>6 ) ? 3 : 2; //  << This is the "adaptive" bit
			// The rest is normal Null-move forward pruning
			if (depth >= 2 && !gameNearEnd && rules.ChessGame.DoNullMovePruning)	// disable null move for now
			{
				val = -AlphaBeta(rules.ChessGame.EnemyPlayer(PlayerSide).PlayerSide,depth  - R - 1, -beta, -beta + 1); // Try a Null Move
				if (val >= beta) // All the moves can be skipped, i.e. cut-off is possible
					return beta;
			}

			// This variable is set to true when we have found at least one Principle variation node.
			// Principal variation (PV) node is the one where One or more of the moves will return a score greater than alpha (a PV move), but none will return a score greater than or equal to beta. 
			bool bFoundPv = false;

			// Check if we have reached at the end of the search
			if (depth <= 0)
			{
				// Check if need to do queiscent search to avoid horizontal effect
				if (rules.ChessGame.DoQuiescentSearch)
					return QuiescentSearch(PlayerSide, alpha, beta);
				else
					return rules.Evaluate(PlayerSide);	// evaluate the current board position
			}	
			// Get all the legal moves for the current side
			ArrayList TotalMoves=rules.GenerateAllLegalMoves(PlayerSide); 

			// Loop through all the legal moves and get the one with best score
			foreach (Move move in TotalMoves)
			{
				// Now to get the effect of this move; execute this move and analyze the board
				rules.ExecuteMove(move);

				// Principle variation node is found
				if (bFoundPv && rules.ChessGame.DoPrincipleVariation) 
				{
					val = -AlphaBeta(rules.ChessGame.EnemyPlayer(PlayerSide).PlayerSide, depth - 1, -alpha - 1, -alpha);
					if ((val > alpha) && (val < beta)) // Check for failure.
						val=-AlphaBeta(rules.ChessGame.EnemyPlayer(PlayerSide).PlayerSide,depth - 1, -beta, -alpha); // Do normal Alpha beta pruning
				} 
				else
					val = -AlphaBeta(rules.ChessGame.EnemyPlayer(PlayerSide).PlayerSide,depth - 1, -beta, -alpha); // Do normal Alpha beta pruning

				totalMovesAnalyzed++;	// Increment move counter
				rules.UndoMove(move);	// undo the move
			
				// This move will never played by the opponent, as he has already better options
				if (val >= beta)
					return beta;
				// This is the best move for the current side (found so far)
				if (val > alpha)
				{
					alpha = val;
					bFoundPv = true;		// we have found a principle variation node
				}
			}
			return alpha;			
		}


		// Do the queiscent search to avoid horizontal effect
		int QuiescentSearch(Side PlayerSide, int alpha, int beta)
		{
			int val = rules.Evaluate(PlayerSide);

			if (val >= beta) // We have reached beta cutt off
				return beta;
			
			if (val > alpha) // found alpha cut-off
				alpha = val;

			// Get all the legal moves for the current side
			ArrayList TotalMoves=rules.GenerateGoodCaptureMoves(PlayerSide); 

			// Loop through all the legal moves and get the one with best score
			foreach (Move move in TotalMoves)
			{
				// Now to get the effect of this move; execute this move and analyze the board
				rules.ExecuteMove(move);
				val = -QuiescentSearch(rules.ChessGame.EnemyPlayer(PlayerSide).PlayerSide, -beta, -alpha);
				rules.UndoMove(move);	// undo the move

				if (val >= beta) // We have reached beta cutt off
					return beta;
			
				if (val > alpha) // found alpha cut-off
					alpha = val;
			}

			return alpha;
		}


		//--------------------------------------------------
		#region Properties for the player class
        [XmlAttribute("Type=PlayerType")]
		public Type PlayerType
		{
			get
			{
				return type;
			}
			set
			{
				type=value;
			}
		}
		//--------------------------------------------------
		public Side PlayerSide
		{
			get
			{
				return side;
			}
			set
			{
				side=value;
			}
		}
		//--------------------------------------------------
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name=value;
			}
		}
		//--------------------------------------------------
        [XmlIgnore]
		public Image Image
		{
			get
			{
				return image;
			}
			set
			{
				image=value;
			}
		}

		// Set and Get the total think time. Used for the computer player
		public int TotalThinkTime
		{
			get
			{
				return maxThinkTime.Seconds;	// returns back total think time of the user
			}
			set
			{
				maxThinkTime=new TimeSpan(0,0,value);	// returns back total think time of the user
			}
		}

		// Get the time used by player to make a move
        [XmlIgnore]
		public TimeSpan ThinkSpanTime
		{
			get
			{
				return totalThinkTime;	// returns back total think time of the user
			}
            set
            {
                totalThinkTime = value;
            }
		}

        /// <summary>
        /// Get or set the total think time in seconds
        /// </summary>
        public long ThinkSpanTimeInSeconds
        {
            get
            {
                return (long)totalThinkTime.TotalSeconds;	// returns back total think time of the user
            }
            set
            {
                totalThinkTime = new TimeSpan(0,0, (int)value);
            }
        }

		// Get user total think time in time format
		public string ThinkTime
		{
			get
			{
				string strThinkTime;

                // If the Start time is not yet set, initialize it
                if (startTime.Year == 1)
                    startTime = DateTime.Now;

				TimeSpan timespan = totalThinkTime+(DateTime.Now - startTime);
				strThinkTime =  timespan.Hours.ToString("00")+":"+timespan.Minutes.ToString("00")+":"+timespan.Seconds.ToString("00");
				return strThinkTime;	// returns back total think time of the user
			}
		}
		#endregion
	}
}
