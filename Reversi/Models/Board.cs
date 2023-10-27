using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi.Models
{
    public class Board
    {
        // These constants represent the contents of a board square.
        public static readonly int Black = -1;  // Green
        public static readonly int Empty = 0;
        public static readonly int White = 1;
        public static readonly int GridSize = 64;

        // Internal counts.
        private int blackCount;
        private int whiteCount;
        private int emptyCount;
        private int blackFrontierCount;
        private int whiteFrontierCount;
        private int blackSafeCount;
        private int whiteSafeCount;

        // These counts reflect the current board situation.
        private List<GameSquare> gameSquares = new();
        public List<GameSquare> GameSquares
        {
            get => gameSquares;
            set => gameSquares.AddRange(value);
        }
        public int BlackCount
        {
            get { return this.blackCount; }
        }
        public int WhiteCount
        {
            get { return this.whiteCount; }
        }
        public int EmptyCount
        {
            get { return this.emptyCount; }
        }
        public int BlackFrontierCount
        {
            get { return this.blackFrontierCount; }
        }
        public int WhiteFrontierCount
        {
            get { return this.whiteFrontierCount; }
        }
        public int BlackSafeCount
        {
            get { return this.blackSafeCount; }
        }
        public int WhiteSafeCount
        {
            get { return this.whiteSafeCount; }
        }

        // This two-dimensional array tracks which discs are safe (i.e.,
        // discs that cannot be outflanked in any direction).
        private bool[,] safeDiscs;

        //
        // Creates a new, empty Board object.
        //
        public Board()
        {
            //
            // TODO: Add constructor logic here
            //

            // Create the safe disc map.
            this.safeDiscs = new bool[8, 8];

            //Initialize list of game squares
            this.gameSquares = new();

            // Clear the board and map.
            int i, j;
            for (i = 0; i < 8; i++)
                for (j = 0; j < 8; j++)
                {
                    this.safeDiscs[i, j] = false;

                    //Create square and add to list
                    GameSquare gs = new GameSquare();
                    gs.ID = calcIndex(i, j);
                    gs.BoardRow = i;
                    gs.BoardCol = j;
                    gs.BoardValue = 0;
                    this.gameSquares.Add(gs);
                }

            // Update the counts.
            this.UpdateCounts();
        }

        //
        //  Calculate index into list based on row and column
        //
        private static int calcIndex (int row, int col) 
        {
            return (row * 8) + col;
        }

        //
        // Creates a new Board object by copying an existing one.
        //
        public Board(Board board)
        {
            // Create the game square list and safe map.
            this.safeDiscs = new bool[8, 8];
            this.gameSquares = new();

            // Copy the given board.
            this.gameSquares = board.gameSquares;
            int i, j;
            for (i = 0; i < 8; i++)
                for (j = 0; j < 8; j++)
                {
                    this.safeDiscs[i, j] = board.safeDiscs[i, j];
                }

            // Copy the counts.
            this.blackCount = board.blackCount;
            this.whiteCount = board.whiteCount;
            this.emptyCount = board.emptyCount;
            this.blackSafeCount = board.blackSafeCount;
            this.whiteSafeCount = board.whiteSafeCount;
        }

        //
        // Sets a board with the initial game set-up.
        //
        public void SetForNewGame()
        {
            // Clear the board.
            this.gameSquares = new();
            int i, j;
            for (i = 0; i < 8; i++)
                for (j = 0; j < 8; j++)
                {
                    this.safeDiscs[i, j] = false;

                    GameSquare gs = new GameSquare();
                    gs.ID = calcIndex(i, j);
                    gs.BoardRow = i;
                    gs.BoardCol = j;
                    gs.BoardValue = 0;
                    this.gameSquares.Add(gs);
                }


            // Set two black and two white discs in the center.
            this.gameSquares[27].BoardValue = 1;
            this.gameSquares[28].BoardValue = -1;
            this.gameSquares[35].BoardValue = -1;
            this.gameSquares[36].BoardValue = 1;


            // Update the counts.
            this.UpdateCounts();
        }

        //
        // Returns the contents of a given board square.
        //
        public int GetSquareContents(int row, int col)
        {
            return gameSquares[calcIndex(row, col)].BoardValue;
        }

        //
        // Places a disc for the player on the board and flips any outflanked
        // opponents.
        // Note: For performance reasons, it does NOT check that the move is
        // valid.
        //
        public void MakeMove(int color, int row, int col)
        {
            this.gameSquares[calcIndex(row, col)].BoardValue = color; 

            int dr, dc;
            int r, c;
            for (dr = -1; dr <= 1; dr++)
                for (dc = -1; dc <= 1; dc++)
                    // Are there any outflanked opponents?
                    if (!(dr == 0 && dc == 0) && IsOutflanking(color, row, col, dr, dc))
                    {
                        r = row + dr;
                        c = col + dc;
                        // Flip 'em.
                        while (this.gameSquares[calcIndex(r, c)].BoardValue == -color) 
                        {
                            this.gameSquares[calcIndex(r, c)].BoardValue = color;
                            r += dr;
                            c += dc;
                        }
                    }

            // Update the counts.
            this.UpdateCounts();
        }

        //
        // Determines if the player can make any valid move on the board.
        //
        public bool HasAnyValidMove(int color)
        {
            // Check all board positions for a valid move.
            int r, c;
            for (r = 0; r < 8; r++)
                for (c = 0; c < 8; c++)
                    if (this.IsValidMove(color, r, c))
                        return true;

            // None found.
            return false;
        }

        //
        // Determines if a specific move is valid for the player.
        //
        public bool IsValidMove(int color, int row, int col)
        {
            // The square must be empty.
            //if (this.squares[row, col] != Board.Empty)
            //    return false;
            if (this.gameSquares[calcIndex(row,col)].BoardValue  != Board.Empty)
                return false;

            // Must be able to flip at least one opponent disc.
            int dr, dc;
            for (dr = -1; dr <= 1; dr++)
                for (dc = -1; dc <= 1; dc++)
                    if (!(dr == 0 && dc == 0) && this.IsOutflanking(color, row, col, dr, dc))
                        return true;

            // No opponents could be flipped.
            return false;
        }

        //
        // Returns the number of valid moves a player can make on the board.
        //
        public int GetValidMoveCount(int color)
        {
            int n = 0;

            // Check all board positions.
            int i, j;
            for (i = 0; i < 8; i++)
                for (j = 0; j < 8; j++)
                    // If the move is valid for the color, bump the count.
                    if (this.IsValidMove(color, i, j))
                        n++;
            return n;
        }

        //
        // Given a player move and a specific direction, determines if any
        // opponent discs will be outflanked.
        // Note: For performance reasons the direction values are NOT checked
        // for validity (dr and dc may be one of -1, 0 or 1 but both should
        // not be zero).
        //
        private bool IsOutflanking(int color, int row, int col, int dr, int dc)
        {
            // Move in the given direction as long as we stay on the board and
            // land on a disc of the opposite color.
            int r = row + dr;
            int c = col + dc;
            while (r >= 0 && r < 8 && c >= 0 && c < 8 && this.gameSquares[calcIndex(r,c)].BoardValue == -color)
            {
                r += dr;
                c += dc;
            }

            if (r < 0 || r > 7 || c < 0 || c > 7 || (r - dr == row && c - dc == col) || this.gameSquares[calcIndex(r, c)].BoardValue != color)
                return false;

            // Otherwise, return true;
            return true;
        }

        //
        // Updates the board counts and safe disc map.
        // Note: MUST be called after any changes to the board contents.
        //
        private void UpdateCounts()
        {
            // Reset all counts.
            this.blackCount = 0;
            this.whiteCount = 0;
            this.emptyCount = 0;
            this.blackFrontierCount = 0;
            this.whiteFrontierCount = 0;
            this.whiteSafeCount = 0;
            this.blackSafeCount = 0;

            int i, j;

            // Update the safe disc map.
            //
            // All currently unsafe discs are checked to see if they are still
            // outflankable. Those that are not are marked as safe.
            // If any new safe discs were found, the process is repeated
            // because this change may have made other discs safe as well. The
            // loop exits when no new safe discs are found.
            bool statusChanged = true;
            while (statusChanged)
            {
                statusChanged = false;
                for (i = 0; i < 8; i++)
                    for (j = 0; j < 8; j++)
                    { 
                        if (this.gameSquares[calcIndex(i,j)].BoardValue != Board.Empty && !this.safeDiscs[i, j] && !this.IsOutflankable(i, j))
                        {
                            this.safeDiscs[i, j] = true;
                            statusChanged = true;
                        }
                    }
            }

            // Tally the counts.
            int dr, dc;

            for (i = 0; i < 8; i++)
                for (j = 0; j < 8; j++)
                {
                    // If there is a disc at this position, determine if it is
                    // on the frontier (i.e., adjacent to an empty square).
                    bool isFrontier = false;
                    if (this.gameSquares[calcIndex(i,j)].BoardValue != Board.Empty)
                    {
                        for (dr = -1; dr <= 1; dr++)
                            for (dc = -1; dc <= 1; dc++)
                            {
                                
                                if (!(dr == 0 && dc == 0) && i + dr >= 0 && i + dr < 8 && j + dc >= 0 && j + dc < 8 && this.gameSquares[calcIndex(i + dr, j + dc)].BoardValue == Board.Empty)
                                    isFrontier = true;
                            }
                    }

                    // Update the counts.
                    if (this.gameSquares[calcIndex(i,j)].BoardValue == Board.Black)
                    {
                        this.blackCount++;
                        if (isFrontier)
                            this.blackFrontierCount++;
                        if (this.safeDiscs[i, j])
                            this.blackSafeCount++;
                    }
                    else if (this.gameSquares[calcIndex(i, j)].BoardValue == Board.White)
                    {
                        this.whiteCount++;
                        if (isFrontier)
                            this.whiteFrontierCount++;
                        if (this.safeDiscs[i, j])
                            this.whiteSafeCount++;
                    }
                    else
                        this.emptyCount++;
                }
        }

        //
        // Returns true if the disc at the given position can be outflanked in
        // any direction.
        // Note: For performance reasons we do NOT check that the square has a
        // disc.
        //
        private bool IsOutflankable(int row, int col)
        {
            // Get the disc color.
            int color = this.gameSquares[calcIndex(row, col)].BoardValue;

            // Check each line through the disc.
            // NOTE: A disc is outflankable if there is an empty square on
            // both sides OR if there is an empty square on one side and an
            // opponent or unsafe (outflankable) disc of the same color on the
            // other side.
            int i, j;
            bool hasSpaceSide1, hasSpaceSide2;
            bool hasUnsafeSide1, hasUnsafeSide2;

            // Check the horizontal line through the disc.
            hasSpaceSide1 = false;
            hasUnsafeSide1 = false;
            hasSpaceSide2 = false;
            hasUnsafeSide2 = false;
            // West side.
            for (j = 0; j < col && !hasSpaceSide1; j++)
            { 
                if (this.gameSquares[calcIndex(row, j)].BoardValue == Board.Empty)
                    hasSpaceSide1 = true;
                else if (this.gameSquares[calcIndex(row, j)].BoardValue != color || !this.safeDiscs[row, j])
                    hasUnsafeSide1 = true;
            }
            // East side.
            for (j = col + 1; j < 8 && !hasSpaceSide2; j++)
            {
                if (this.gameSquares[calcIndex(row, j)].BoardValue == Board.Empty)
                    hasSpaceSide2 = true;
                else if (this.gameSquares[calcIndex(row, j)].BoardValue != color || !this.safeDiscs[row, j])
                    hasUnsafeSide2 = true;
            }
            if ((hasSpaceSide1 && hasSpaceSide2) ||
                (hasSpaceSide1 && hasUnsafeSide2) ||
                (hasUnsafeSide1 && hasSpaceSide2))
                return true;

            // Check the vertical line through the disc.
            hasSpaceSide1 = false;
            hasSpaceSide2 = false;
            hasUnsafeSide1 = false;
            hasUnsafeSide2 = false;
            // North side.
            for (i = 0; i < row && !hasSpaceSide1; i++)
            { 
                if (this.gameSquares[calcIndex(i,col)].BoardValue == Board.Empty)
                    hasSpaceSide1 = true;
                else if (this.gameSquares[calcIndex(i, col)].BoardValue != color || !this.safeDiscs[i, col])
                    hasUnsafeSide1 = true;
            }
            // South side.
            for (i = row + 1; i < 8 && !hasSpaceSide2; i++)
            {
                if (this.gameSquares[calcIndex(i, col)].BoardValue == Board.Empty)
                    hasSpaceSide2 = true;
                else if (this.gameSquares[calcIndex(i, col)].BoardValue != color || !this.safeDiscs[i, col])
                    hasUnsafeSide2 = true;
            }
            if ((hasSpaceSide1 && hasSpaceSide2) ||
                (hasSpaceSide1 && hasUnsafeSide2) ||
                (hasUnsafeSide1 && hasSpaceSide2))
                return true;

            // Check the Northwest-Southeast diagonal line through the disc.
            hasSpaceSide1 = false;
            hasSpaceSide2 = false;
            hasUnsafeSide1 = false;
            hasUnsafeSide2 = false;
            // Northwest side.
            i = row - 1;
            j = col - 1;
            while (i >= 0 && j >= 0 && !hasSpaceSide1)
            {
                if (this.gameSquares[calcIndex(i,j)].BoardValue == Board.Empty)
                    hasSpaceSide1 = true;
                else if (this.gameSquares[calcIndex(i, j)].BoardValue != color || !this.safeDiscs[i, j])
                    hasUnsafeSide1 = true;
                i--;
                j--;
            }
            // Southeast side.
            i = row + 1;
            j = col + 1;
            while (i < 8 && j < 8 && !hasSpaceSide2)
            {
                if (this.gameSquares[calcIndex(i, j)].BoardValue == Board.Empty)
                    hasSpaceSide2 = true;
                else if (this.gameSquares[calcIndex(i, j)].BoardValue != color || !this.safeDiscs[i, j])
                    hasUnsafeSide2 = true;
                i++;
                j++;
            }
            if ((hasSpaceSide1 && hasSpaceSide2) ||
                (hasSpaceSide1 && hasUnsafeSide2) ||
                (hasUnsafeSide1 && hasSpaceSide2))
                return true;

            // Check the Northeast-Southwest diagonal line through the disc.
            hasSpaceSide1 = false;
            hasSpaceSide2 = false;
            hasUnsafeSide1 = false;
            hasUnsafeSide2 = false;
            // Northeast side.
            i = row - 1;
            j = col + 1;
            while (i >= 0 && j < 8 && !hasSpaceSide1)
            {
                if (this.gameSquares[calcIndex(i, j)].BoardValue == Board.Empty)
                    hasSpaceSide1 = true;
                else if (this.gameSquares[calcIndex(i, j)].BoardValue != color || !this.safeDiscs[i, j])
                    hasUnsafeSide1 = true;
                i--;
                j++;
            }
            // Southwest side.
            i = row + 1;
            j = col - 1;
            while (i < 8 && j >= 0 && !hasSpaceSide2)
            {
                if (this.gameSquares[calcIndex(i, j)].BoardValue == Board.Empty)
                    hasSpaceSide2 = true;
                else if (this.gameSquares[calcIndex(i, j)].BoardValue != color || !this.safeDiscs[i, j])
                    hasUnsafeSide2 = true;
                i++;
                j--;
            }
            if ((hasSpaceSide1 && hasSpaceSide2) ||
                (hasSpaceSide1 && hasUnsafeSide2) ||
                (hasUnsafeSide1 && hasSpaceSide2))
                return true;

            // All lines are safe so the disc cannot be outflanked.
            return false;
        }
    }
}
