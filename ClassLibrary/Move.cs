using System;

namespace ChessLibrary
{
	/// <summary>
	/// This class stores info about a single chess game move.
	/// It contains source and target chess squars/cells and also the type
	/// of move and related info.
	/// </summary>
    [Serializable]
	public class Move
	{
		public enum MoveType {NormalMove, CaputreMove, TowerMove, PromotionMove, EnPassant};	// Type of the move

		private Cell startCell;	// start cell
		private Cell endCell;		// end cell
		private Piece piece;			// Piece being moved
		private Piece capturedPiece;	// Piece captured by this mov
		private Piece promoPiece;		// Piece selected after pawn promotion
		private Piece enPassantPiece;	// Piece captured during enpassant move
		private MoveType type;		// Type of the move
		private bool causeCheck;		// if cause or leave the user under check
		private int	score;			// Score of the move from the board analyze routine

        // Emptry internal constructor for XML Serialization support
        internal Move()
        {
            score = 0;
        }

		public Move(Cell begin, Cell end)
		{
			startCell=begin;
			endCell=end;
			piece=begin.piece;
			capturedPiece=end.piece;
			score=0;
		}

		// Returns the move start cell
		public Cell StartCell
		{
			get
			{
				return startCell;
			}
            set
            {
                startCell = value;
            }
		}

		// Returns the move end cell
		public Cell EndCell
		{
			get
			{
				return endCell;
			}
            set
            {
                endCell = value;
            }
		}

		// Returns the piece which was moved
		public Piece Piece
		{
			get
			{
				return piece;
			}
            set
            {
                piece = value;
            }
		}

		// Returns the captured piece 
		public Piece CapturedPiece
		{
			get
			{
				return capturedPiece;
			}
            set
            {
                capturedPiece = value;
            }
		}

		// Get and Set the move type property
		public MoveType Type
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

		// Return true if the move when executed, place or leave user under check
		public bool CauseCheck
		{
			get
			{
				return causeCheck;
			}
			set
			{
				causeCheck=value;
			}
		}

		// Set and get the promo piece
		public Piece PromoPiece
		{
			get
			{
				return promoPiece;
			}
			set
			{
				promoPiece=value;
			}
		}

		// Set and get the EnPassant piece
		public Piece EnPassantPiece
		{
			get
			{
				return enPassantPiece;
			}
			set
			{
				enPassantPiece=value;
			}
		}

		// Set and get the move Score
		public int Score
		{
			get
			{
				return score;
			}
			set
			{
				score=value;
			}
		}

		// Return true if the move was promo move
		public bool IsPromoMove()
		{
			return type==MoveType.PromotionMove;
		}

		// Return true if the move was capture move
		public bool IsCaptureMove()
		{
			return type==MoveType.CaputreMove;
		}

		//Return a descriptive move text
		public override string ToString()
		{
			if (type == Move.MoveType.CaputreMove)	// It's a capture move
				return piece + " " + startCell.ToString2() + "x" + endCell.ToString2();
			else
				return piece + " " + startCell.ToString2() + "-" + endCell.ToString2();
		}
	}

	// This class is used to compare two Move type objects
	public class MoveCompare : System.Collections.IComparer
	{
		// Empty constructore
		public MoveCompare()
		{
		}

		public int Compare(Object firstObj, Object SecondObj)
		{
			Move firstMove = (Move)firstObj;
			Move secondMove = (Move)SecondObj;

			return -firstMove.Score.CompareTo(secondMove.Score);
		}
	}
}
