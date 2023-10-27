namespace DoAsISay.Data
{
    public class VoiceInstructions
    {
        // I Rows, by J columns, cell address i*J + j, where i= 0 to (I-1)  and j=0 to (J-1)
        //
        //  if i=0 then Top border,  if i=I then Bottom border
        //  if j=0 then Left border, if j=J then Right border  
        //
        private enum Direction : int
        {
            Left = 0,
            Right = 1,
            Up = 2,
            Down = 3,
            LeftUp = 4,
            LeftDown = 5,
            RightUp = 6,
            RightDown = 7
        }
        static string[] numCardinal = new string[10]
        { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"};
        string[] numOrdinal = new string[10]
        { "first", "second", "third", "fourth", "fifth", "sixth", "seventh", "eighth", "ninth", "tenth"};

        string beginMove = "Start on the cell in the {0} row from the top, and the {1} column from the Left.";


        //string moveLeft = "Move left {0} column";
        //string moveRight = "Move right {0} column";
        //string moveUp   = "Move up {0} row";
        //string moveDown = "Move down {0} row";
        string moveLeft = "Move left {0} cell";
        string moveRight = "Move right {0} cell";
        string moveUp = "Move up {0} cell";
        string moveDown = "Move down {0} cell";

        //Move diagonally, either Left Up; Left Down; Right Up; or Right Down. (Dependent on grid size)
        string moveLeftUp = "Move diagonally left Up {0} cell";
        string moveLeftDown = "Move diagonally  left dowm {0} cell";
        string moveRightUp = "Move diagonally  right up {0} cell";
        string moveRightDown = "Move diagonally  right down {0} cell";

        string addPlural = "s";

        public List<string> Instructions = new List<string>();
        private Random random = new(DateTime.Now.Millisecond);

        public int StartRow { get; set; }
        public int StartCol { get; set; }

        public int EndRow { get; set; }

        public int EndCol { get; set; }

        public int NumRows { get; set; } = 4;
        public int NumCols { get; set; } = 4;
        public List<string> GetInstructions(int numReps, int numInstr, int numRows, int numCols)
        {
            int spRow, spCol, nDir, nStep;
            NumRows = numRows;
            NumCols = numCols;

            Instructions.Clear();
            // Pick a random starting cell in the matrix
            this.StartCol = random.Next(0, numCols - 1);
            this.StartRow = random.Next(0, numRows - 1);

            spRow = this.StartRow;
            spCol = this.StartCol;

            string str;
            str = string.Format(beginMove, numOrdinal[spRow], numOrdinal[spCol]);
            Instructions.Add(str);

            for (int i = 0; i < numInstr; i++)
            {
                //pick a random direction
                nDir = GetDirection(spRow, spCol);
                //pick a random step size within matrix
                nStep = GetStep(spRow, spCol, nDir);

                switch ((Direction)nDir)
                {
                    case Direction.Left:
                        spCol = spCol - nStep;
                        str = string.Format(moveLeft, nStep);
                        break;
                    case Direction.Right:
                        spCol = spCol + nStep;
                        str = string.Format(moveRight, nStep);
                        break;
                    case Direction.Up:
                        spRow = spRow - nStep;
                        str = string.Format(moveUp, nStep);
                        break;
                    case Direction.Down:
                        spRow = spRow + nStep;
                        str = string.Format(moveDown, nStep);
                        break;
                    case Direction.LeftUp:
                        spRow = spRow - nStep;
                        spCol = spCol - nStep;
                        str = string.Format(moveLeftUp, nStep);
                        break;
                    case Direction.LeftDown:
                        spRow = spRow + nStep;
                        spCol = spCol - nStep;
                        str = string.Format(moveLeftDown, nStep);
                        break;
                    case Direction.RightUp:
                        spRow = spRow - nStep;
                        spCol = spCol + nStep;
                        str = string.Format(moveRightUp, nStep);
                        break;
                    case Direction.RightDown:
                        spRow = spRow + nStep;
                        spCol = spCol + nStep;
                        str = string.Format(moveRightDown, nStep);
                        break;

                }
                if (nStep > 1) str = str + addPlural;
                Instructions.Add(str);
            }
            this.EndRow = spRow;
            this.EndCol = spCol;
            str = "Click on the specified cell please";
            Instructions.Add(str);

            return Instructions;
        }

        private int GetDirection(int pRow, int pCol)
        {
            int nDir;
            List<int> dirs = new();

            // If not on left edge, can move left
            if (pCol > 0) dirs.Add((int)Direction.Left);

            // If not on right edge, can move right
            if (pCol < this.NumCols - 1) dirs.Add((int)Direction.Right);

            // If no on top edge, can move up
            if (pRow > 0) dirs.Add((int)Direction.Up);

            // If not on bottom edge, can move down
            if (pRow < this.NumRows - 1) dirs.Add((int)Direction.Down);

            // If not on Left Edge and Not on top edge, can move diagonally Leftup
            if (pCol > 0 & pRow > 0) dirs.Add((int)Direction.LeftUp);

            // If not on Left Edge and Not on bottom edge, can move diagonally Leftdown
            if (pCol > 0 & pRow < this.NumRows - 1) dirs.Add((int)Direction.LeftDown);

            // If not on right edge and not on top row, can diagonally rightUp
            if (pCol < this.NumCols - 1 & pRow > 0) dirs.Add((int)Direction.RightUp);

            // If not on right edge and not on bottom row, can diagonally rightDown
            if (pCol < this.NumCols - 1 & pRow < this.NumRows - 1) dirs.Add((int)Direction.RightDown);

            nDir = dirs[random.Next(dirs.Count)];

            return nDir;
        }
        private int GetStep(int pRow, int pCol, int pDir)
        {
            int nStep = 0;
            int nStepD = 0;

            if (pDir == (int)Direction.Up) nStep = random.Next(1, pRow + 1);

            if (pDir == (int)Direction.Down) nStep = random.Next(1, this.NumRows - pRow - 1);

            if (pDir == (int)Direction.Left) nStep = random.Next(1, pCol + 1);

            if (pDir == (int)Direction.Right) nStep = random.Next(1, this.NumCols - pCol - 1);

            if (pDir == (int)Direction.LeftUp)
            {
                nStepD = Math.Min(pCol + 1, pRow + 1);
                nStep = random.Next(1, nStepD);
            }
            if (pDir == (int)Direction.LeftDown)
            {
                nStepD = Math.Min(pCol + 1, this.NumRows - pRow - 1);
                nStep = random.Next(1, nStepD);
            }
            if (pDir == (int)Direction.RightUp)
            {
                nStepD = Math.Min(this.NumCols - pCol - 1, pRow + 1);
                nStep = random.Next(1, nStepD);
            }
            if (pDir == (int)Direction.RightDown)
            {
                nStepD = Math.Min(this.NumCols - pCol - 1, this.NumRows - pRow - 1);
                nStep = random.Next(1, nStepD);
            }

            return nStep;
        }

    }
}
