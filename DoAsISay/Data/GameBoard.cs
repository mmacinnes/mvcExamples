namespace DoAsISay.Data
{
    public class GameBoard
    {
        private readonly string[] htmlColors = { "blue", "cyan", "fuschia", "gray", "indigo" };

        private readonly string[] chars = {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m",
                                           "n","o","p","q","r","s","t","u","v","w","x","y","z",
                                           "A","B","C","D","E","F","G","H","I","J","K","L","M",
                                           "N","O","P","Q","R","S","T","U","V","W","X","Y","Z" };
        // Unicode Graphic Characters
        private readonly string[] shapes = {"\u25A0","\u25A1","\u25A2","\u25A3","\u25A4",
                                            "\u25A5","\u25A6","\u25A7","\u25A8","\u25A9",
                                            "\u25AA","\u25AB","\u25AC","\u25AD","\u25AE",
                                            "\u25AF","\u25B0","\u25B1","\u25B2","\u25B3",
                                            "\u25B4","\u25B5","\u25B6","\u25B7","\u25B8",
                                            "\u25B9","\u25BA","\u25BB","\u25BC","\u25BD",
                                            "\u25BE","\u25BF","\u25C0","\u25C1","\u25C2",
                                            "\u25C3","\u25C4","\u25C5","\u25C6","\u25C7",
                                            "\u25C8","\u25C9","\u25CA","\u25CB","\u25CC",
                                            "\u25CD","\u25CE","\u25CF","\u25D0","\u25D1",
                                            "\u25D2","\u25D3","\u25D4","\u25D5","\u25D6",
                                            "\u25D7","\u25D8","\u25D9","\u25DA","\u25DB",
                                            "\u25DC","\u25DD","\u25DE","\u25DF","\u25E0",
                                            "\u25E1","\u25E2","\u25E3","\u25E4","\u25E5",
                                            "\u25E6","\u25E7","\u25E8","\u25E9","\u25EA",
                                            "\u25EB","\u25EC","\u25ED","\u25EE","\u25EF",
                                            "\u25F0","\u25F1","\u25F2","\u25F3","\u25F4",
                                            "\u25F5","\u25F6","\u25F7","\u25F8","\u25F9",
                                            "\u25FA","\u25FB","\u25FC","\u25FD","\u25FE",
                                            "\u25FF"};

        private List<GameSquare> gameSquares = new();
        public int NumRows { get; set; }
        public int NumCols { get; set; }
        public int WindowHeight { get; set; }
        public int WindowWidth { get; set; }

        public int BdWidth { get; set; }
        public int BdHeight { get; set; }
        public int CellSize { get; set; }
        public string? CellFill { get; set; }
        public int FontSize { get; set; }

        public List<GameSquare> GameSquares
        {
            get => gameSquares;
            set => gameSquares.AddRange(value);
        }
        public string GetCell(int cellText)
        {
            return cellText.ToString();
        }

        public GameBoard()
        {
            // uses default vales  4, 4, 400,800
            NumRows = 4;
            NumCols = 4;
            WindowHeight = 800;
            WindowWidth = 1200;
            CellFill = "Blank";

            InitGameBoard();
        }
        public GameBoard(int numRows, int numColumns, int windowHeight, int windowWidth, string? cellFill)
        {
            NumRows = numRows;
            NumCols = numColumns;
            WindowHeight = windowHeight;
            WindowWidth = windowWidth;
            CellFill = cellFill;
            InitGameBoard();
        }
        public void InitGameBoard()
        {
            //The student sees a grid of cells on the screen.
            //The size of the grid is N rows x M columns.
            //A Therapist UID establishes the size of the grid through a parameter.
            //  The minimum number of Rows = 3; maximum number = 10;
            //  minimum number of Columns = 3; maximum number = 10;
            //  i.e.the grid can be square or rectangular.
            //
            //The grid can be blank, or
            //it can be populated with ‘distracting’ information (random numbers, letters, shapes, etc).
            //The therapist can specify that the cells are filled with certain ‘noise’ by
            //setting a parameter(“Distraction Type”) and can use another parameter to select the type of distractors to be used.  

            double cellSize, cellSizeW, cellSizeH;

            //            cellSizeW = (double)(WindowWidth - 400) / NumCols;
            //            cellSizeH = (double)(WindowHeight - 200) / NumRows;
            cellSizeW = (double)(WindowWidth / NumCols) * 0.8;
            cellSizeH = (double)(WindowHeight / NumRows) * 0.8;

            cellSize = Math.Min(cellSizeW, cellSizeH);

            BdWidth = ((int)(cellSize * (double)NumCols));
            BdHeight = ((int)(cellSize * (double)NumRows));
            FontSize = (int)(cellSize * 0.5);

            CellSize = (int)cellSize;

            //Initialize list of game squares
            this.gameSquares = new();

            // Clear the board and map.
            int i, j;
            for (i = 0; i < NumRows; i++)
                for (j = 0; j < NumCols; j++)
                {
                    //Create square and add to list
                    GameSquare gs = new GameSquare();
                    gs.ID = (i * NumCols) + j;
                    gs.BoardRow = i;
                    gs.BoardCol = j;
                    gs.ColorFore = "black";
                    gs.ColorBack = "cornsilk";

                    if (CellFill == "Blank")
                    {
                        gs.ColorFore = "cornsilk";
                        gs.BoardValue = "?";  // set a default value, may change if distractors are applied
                    }
                    if (CellFill == "Numbers")
                    {
                        gs.BoardValue = Convert.ToString(gs.ID % 10);  // set a default value, may change if distractors are applied
                    }
                    if (CellFill == "Letters")
                    {
                        gs.BoardValue = chars[(gs.ID % 52)]; // 72 letters a-zA-Z
                    }
                    if (CellFill == "Shapes")
                    {
                        gs.BoardValue = shapes[(gs.ID % 96)]; // 72 unicode graphic characters
                    }
                    //else
                    //{
                    //    gs.BoardValue = gs.ID.ToString();  // set a default value, may change if distractors are applied

                    //}
                    this.gameSquares.Add(gs);
                }
        }
        public GameSquare GetGameSquare(int i, int j)
        {
            GameSquare gs = gameSquares.Find(g => g.ID == (i * NumCols) + j) ?? gameSquares[0];
            return gs;
        }
        public GameSquare GetGameSquare(string cellID)
        {
            GameSquare gs = gameSquares.Find(gs => gs.ID == Convert.ToInt32(cellID))  ?? gameSquares[0];
            return gs;
        }
        public string ShuffleSquares()
        {
            string oldValue = this.gameSquares[0].BoardValue;
            for (int i = 0; i <= this.gameSquares.Count() - 1; i++)
            {
                if (i <= this.gameSquares.Count() - 2)
                {
                    this.gameSquares[i].BoardValue = this.gameSquares[i + 1].BoardValue;
                }
                else
                {
                    this.gameSquares[i].BoardValue = oldValue;
                }
            }
            return oldValue;
        }

        public string ShuffleColors()
        {
            string newcolor = "blue";

            for (int i = 0; i < this.gameSquares.Count(); i++)
            {
                if (this.gameSquares[i].ColorBack == "cornsilk")
                {
                    newcolor = htmlColors[i % 5];
                    this.gameSquares[i].ColorBack = newcolor;
                }
                else
                {
                    this.gameSquares[i].ColorBack = "cornsilk";
                }
            }
            return newcolor;
        }
    }
}
