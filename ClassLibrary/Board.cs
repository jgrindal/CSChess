using System;
using System.Collections;
using System.Xml;

namespace ChessLibrary
{
	/// <summary>
	/// he main chess board. Board contain the chess cell
	/// which will contain the chess pieces. Board also contains the methods
	/// to get and set the user moves.
	/// </summary>
    [Serializable]
	public class Board
	{
		private Side whiteSide, blackSide;	// Chess board site object 
		private Cells cells;	// collection of cells in the board

		public Board()
		{
            whiteSide = new Side(Side.SideType.White);	// Makde white side
            blackSide = new Side(Side.SideType.Black);	// Makde white side

			cells = new Cells();					// Initialize the chess cells collection
		}

		// Initialize the chess board and place piece on thier initial positions
		public void Init()
		{
			cells.Clear();		// Remove any existing chess cells

			// Build the 64 chess board cells
			for (int row=1; row<=8; row++)
				for (int col=1; col<=8; col++)
				{
					cells.Add(new Cell(row,col));	// Initialize and add the new chess cell
				}

			// Now setup the board for black side
			cells["a1"].piece = new Piece(Piece.PieceType.Rook,blackSide);
			cells["h1"].piece = new Piece(Piece.PieceType.Rook,blackSide);
			cells["b1"].piece = new Piece(Piece.PieceType.Knight,blackSide);
			cells["g1"].piece = new Piece(Piece.PieceType.Knight,blackSide);
			cells["c1"].piece = new Piece(Piece.PieceType.Bishop,blackSide);
			cells["f1"].piece = new Piece(Piece.PieceType.Bishop,blackSide);
			cells["e1"].piece = new Piece(Piece.PieceType.King,blackSide);
			cells["d1"].piece = new Piece(Piece.PieceType.Queen,blackSide);
			for (int col=1; col<=8; col++)
				cells[2, col].piece = new Piece(Piece.PieceType.Pawn,blackSide);

			// Now setup the board for white side
			cells["a8"].piece = new Piece(Piece.PieceType.Rook,whiteSide);
			cells["h8"].piece = new Piece(Piece.PieceType.Rook,whiteSide);
			cells["b8"].piece = new Piece(Piece.PieceType.Knight,whiteSide);
			cells["g8"].piece = new Piece(Piece.PieceType.Knight,whiteSide);
			cells["c8"].piece = new Piece(Piece.PieceType.Bishop,whiteSide);
			cells["f8"].piece = new Piece(Piece.PieceType.Bishop,whiteSide);
			cells["e8"].piece = new Piece(Piece.PieceType.King,whiteSide);
			cells["d8"].piece = new Piece(Piece.PieceType.Queen,whiteSide);
			for (int col=1; col<=8; col++)
				cells[7, col].piece = new Piece(Piece.PieceType.Pawn,whiteSide);
		}

		// get the new item by rew and column
		public Cell this[int row, int col]
		{
			get
			{
				return cells[row, col];
			}
		}

		// get the new item by string location
		public Cell this[string strloc]
		{
			get
			{
				return cells[strloc];	
			}
		}

		// get the chess cell by given cell
		public Cell this[Cell cellobj]
		{
			get
			{
				return cells[cellobj.ToString()];	
			}
		}

        /// <summary>
        /// Serialize the Game object as XML String
        /// </summary>
        /// <returns>XML containing the Game object state XML</returns>
        public XmlNode XmlSerialize(XmlDocument xmlDoc)
        {
            XmlElement xmlBoard = xmlDoc.CreateElement("Board");

            // Append game state attributes
            xmlBoard.AppendChild(whiteSide.XmlSerialize(xmlDoc));
            xmlBoard.AppendChild(blackSide.XmlSerialize(xmlDoc));

            xmlBoard.AppendChild(cells.XmlSerialize(xmlDoc));

            // Return this as String
            return xmlBoard;
        }

        /// <summary>
        /// DeSerialize the Board object from XML String
        /// </summary>
        /// <returns>XML containing the Board object state XML</returns>
        public void XmlDeserialize(XmlNode xmlBoard)
        {
            // Deserialize the Sides XML
            XmlNode side = XMLHelper.GetFirstNodeByName(xmlBoard, "Side");

            // Deserialize the XML nodes
            whiteSide.XmlDeserialize(side);
            blackSide.XmlDeserialize(side.NextSibling);

            // Deserialize the Cells
            XmlNode xmlCells = XMLHelper.GetFirstNodeByName(xmlBoard, "Cells");
            cells.XmlDeserialize(xmlCells);
        }

		// get all the cell locations on the chess board
		public ArrayList GetAllCells()
		{
			ArrayList CellNames = new ArrayList();

			// Loop all the squars and store them in Array List
			for (int row=1; row<=8; row++)
				for (int col=1; col<=8; col++)
				{
					CellNames.Add(this[row,col].ToString()); // append the cell name to list
				}

			return CellNames;
		}

		// get all the cell containg pieces of given side
        public ArrayList GetSideCell(Side.SideType PlayerSide)
		{
			ArrayList CellNames = new ArrayList();

			// Loop all the squars and store them in Array List
			for (int row=1; row<=8; row++)
				for (int col=1; col<=8; col++)
				{
					// check and add the current type cell
					if (this[row,col].piece!=null && !this[row,col].IsEmpty() && this[row,col].piece.Side.type == PlayerSide)
						CellNames.Add(this[row,col].ToString()); // append the cell name to list
				}

			return CellNames;
		}

		// Returns the cell on the top of the given cell
		public Cell TopCell(Cell cell)
		{
			return this[cell.row-1, cell.col];
		}

		// Returns the cell on the left of the given cell
		public Cell LeftCell(Cell cell)
		{
			return this[cell.row, cell.col-1];
		}

		// Returns the cell on the right of the given cell
		public Cell RightCell(Cell cell)
		{
			return this[cell.row, cell.col+1];
		}

		// Returns the cell on the bottom of the given cell
		public Cell BottomCell(Cell cell)
		{
			return this[cell.row+1, cell.col];
		}

		// Returns the cell on the top-left of the current cell
		public Cell TopLeftCell(Cell cell)
		{
			return this[cell.row-1, cell.col-1];
		}

		// Returns the cell on the top-right of the current cell
		public Cell TopRightCell(Cell cell)
		{
			return this[cell.row-1, cell.col+1];
		}

		// Returns the cell on the bottom-left of the current cell
		public Cell BottomLeftCell(Cell cell)
		{
			return this[cell.row+1, cell.col-1];
		}

		// Returns the cell on the bottom-right of the current cell
		public Cell BottomRightCell(Cell cell)
		{
			return this[cell.row+1, cell.col+1];
		}
	}
}
