using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi.Models
{
    public class GameSquare
    {

        // ID 0 - 63    64 Squares on a board
        [Range(0, 63, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int ID { get; set; }

        // Row 0-7   8 rows
        [Range(0, 7, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int BoardRow { get; set; }

        // Col 0-7  8 Columns
        [Range(0, 7, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int BoardCol { get; set;}

        // Green = -1, Blank = 0, White = 1
        [Range(-1, 1, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int BoardValue { get; set;}

    }
}
