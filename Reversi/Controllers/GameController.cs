using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reversi.Models;

namespace Reversi.Controllers
{
    public class GameController : Controller
    {
        static Board board = new();
        static int playercolor = -1;
        public IActionResult Index()
        {
            board.SetForNewGame();
            ViewBag.PlayerColor = playercolor;
            return View(board);
        }
        public IActionResult ClickSquare(int id)
        {
            if (board.IsValidMove(playercolor, board.GameSquares[id].BoardRow, board.GameSquares[id].BoardCol))
            { 
                board.MakeMove(playercolor, board.GameSquares[id].BoardRow, board.GameSquares[id].BoardCol);
                playercolor = -1 * playercolor;
            }

            // If player has no valid moves, change player
            if (!board.HasAnyValidMove(playercolor)) playercolor = -1 * playercolor;

            ViewBag.PlayerColor = playercolor;
            return View("Index", board);
        }
    }
}
