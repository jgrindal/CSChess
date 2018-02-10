using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using ChessLibrary;

namespace Chess
{
	/// <summary>
	/// Summary description for Images.
	/// </summary>
	public class Images
	{
		private ArrayList imageList;		// store list of image list

		public Images()
		{
			imageList = new ArrayList();
		}

		public void LoadImages(string SourceDir)
		{

			// Read and store the image black and white image paths
			imageList.Add(System.Drawing.Image.FromFile(SourceDir+"Black.jpg"));
			imageList.Add(System.Drawing.Image.FromFile(SourceDir+"White.jpg"));
			// Read and store the white pieces images
			imageList.Add(System.Drawing.Image.FromFile(SourceDir+"king.gif"));
			imageList.Add(System.Drawing.Image.FromFile(SourceDir+"queen.gif"));
			imageList.Add(System.Drawing.Image.FromFile(SourceDir+"bishop.gif"));
			imageList.Add(System.Drawing.Image.FromFile(SourceDir+"knight.gif"));
			imageList.Add(System.Drawing.Image.FromFile(SourceDir+"rook.gif"));
			imageList.Add(System.Drawing.Image.FromFile(SourceDir+"pawn.gif"));
			// Read and store the black pieces images
			imageList.Add(System.Drawing.Image.FromFile(SourceDir+"king_2.gif"));
			imageList.Add(System.Drawing.Image.FromFile(SourceDir+"queen_2.gif"));
			imageList.Add(System.Drawing.Image.FromFile(SourceDir+"bishop_2.gif"));
			imageList.Add(System.Drawing.Image.FromFile(SourceDir+"knight_2.gif"));
			imageList.Add(System.Drawing.Image.FromFile(SourceDir+"rook_2.gif"));
			imageList.Add(System.Drawing.Image.FromFile(SourceDir+"pawn_2.gif"));
			// Read and store the image black and white image paths
			imageList.Add(System.Drawing.Image.FromFile(SourceDir+"Black_2.jpg"));
			imageList.Add(System.Drawing.Image.FromFile(SourceDir+"White_2.jpg"));
		}

		// Get Image by name i.e. White or Black
		public Image this[string strName]
		{
			get 
			{
				switch (strName)	// check string type
				{
					case "White":
						return (Image)imageList[0];
					case "Black":
						return (Image)imageList[1];
					case "White2":
						return (Image)imageList[14];
					case "Black2":
						return (Image)imageList[15];
					default:
						return null;

				}
				
			}
		}

		// Return image for the given piece type
		public Image GetImageForPiece(Piece Piece)
		{
			// Not a valid chess piece
			if (Piece == null || Piece.Type == Piece.PieceType.Empty )
				return null;

			// check and return the white piece image
			if (Piece.Side.isWhite())
				switch(Piece.Type)
				{
					case Piece.PieceType.King:
						return (Image)imageList[2];
					case Piece.PieceType.Queen:
						return (Image)imageList[3];
					case Piece.PieceType.Bishop:
						return (Image)imageList[4];
					case Piece.PieceType.Knight:
						return (Image)imageList[5];
					case Piece.PieceType.Rook:
						return (Image)imageList[6];
					case Piece.PieceType.Pawn:
						return (Image)imageList[7];
					default:
						return null;
				}
			else
				switch(Piece.Type)
				{
					case Piece.PieceType.King:
						return (Image)imageList[8];
					case Piece.PieceType.Queen:
						return (Image)imageList[9];
					case Piece.PieceType.Bishop:
						return (Image)imageList[10];
					case Piece.PieceType.Knight:
						return (Image)imageList[11];
					case Piece.PieceType.Rook:
						return (Image)imageList[12];
					case Piece.PieceType.Pawn:
						return (Image)imageList[13];
					default:
						return null;
				}
		}
	}
}
