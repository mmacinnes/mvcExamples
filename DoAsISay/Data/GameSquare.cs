using System.ComponentModel.DataAnnotations;

namespace DoAsISay.Data
{
    public class GameSquare
    {
        // ID 0 - 100    up to 100 Squares on a board
        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int ID { get; set; } = 0;

        // Row 0-10   up to 10 rows
        [Range(0, 10, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int BoardRow { get; set; } = 0;

        // Col 0-10  up to 10 Columns
        [Range(0, 10, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int BoardCol { get; set; } = 0;

        // Green = -1, Blank = 0, White = 1
        //[Range(-1, 1, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public string BoardValue { get; set; } = string.Empty;

        public string ColorBack { get; set; } = string.Empty;

        public string ColorFore { get; set; } = string.Empty;

    }
}
