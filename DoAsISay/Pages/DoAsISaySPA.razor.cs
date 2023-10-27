using DoAsISay.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Toolbelt.Blazor.SpeechSynthesis;

namespace DoAsISay.Pages
{
    public partial class DoAsISaySPA
    {
        [Inject] public IJSRuntime JsRuntime { get; set; } = default!;
        [Inject] SpeechSynthesis SpeechSynthesis { get; set; } = default!;

        IEnumerable<SpeechSynthesisVoice> Voices  = default!;
        SpeechSynthesisVoice? selectedVoice;
        private bool ShowSettings { get; set; } = true;

        string? InputVolume { get; set; } = "1.0";// 0.0 ~ 1.0 (Default 1.0)
        string? InputLang { get; set; } = "en-US"; // BCP 47 language tag
        string? InputVoice { get; set; } = "Microsoft David - English (United States)";
        string? InputPitch { get; set; } = "1.0";// 0.0 ~ 2.0 (Default 1.0)
        string? InputRate { get; set; } = "1.0"; // 0.1 ~ 10.0 (Default 1.0)

        string? InputRows { get; set; } = "4"; // 3 ~ 10 (Default 4)
        string? InputCols { get; set; } = "4"; // 3 ~ 10 (Default 4)
        string? NumInstrs { get; set; } = "4";   // default to 4 instruction
        string? NumReps { get; set; } = "4";   // default to 4 instruction
        string? NumCorrect { get; set; } = "0";   // default to 0
        string? CellType { get; set; } = "Blank"; // default to blank cells


        private IJSObjectReference jsModule  = default!;
        //private DotNetObjectReference<DoAsISaySPA>? dotNetHelper;
        private int numReps = 0;
        private int numCorrect = 0;
        private bool allowClick = false;

        private VoiceInstructions instructions = new VoiceInstructions();
        private List<string> instr =  default!;

        public int windowHeight = 0, windowWidth = 0;

        public string bonusSound = "/sounds/bonus.wav";
        public string losingSound = "/sounds/losing.wav";
        public string treasureSound = "/sounds/treasure.wav";
        public string gameBoard = "";
        public int clickedCell = 0;
        public string oldValue = "";

        private int EndRow = 0;
        private int EndCol = 0;

        private bool showReDraw = true;
        private bool showStart = false;
        private bool showNext = false;
        private bool showStop = false;
        private bool showRepeat = false;
        private bool showBoard = false;
        private bool speechInProcess = false;

        private string? PauseBetweenUtterance { get; set; } = "2500";
        private string? PauseBetweenDistraction { get; set; } = "3000";


        private GameBoard gb = new GameBoard();

        public DoAsISaySPA()
        {
            InputLang = "en-US";  // BCP 47 language tag
            InputVoice = "Microsoft David - English (United States)";  //inputVoice = { };
            InputVolume = "1.0";  // 0.0 ~ 1.0 (Default 1.0)
            InputPitch = "1.0";   // 0.0 ~ 2.0 (Default 1.0)
            InputRate = "1.0";    // 0.1 ~ 10.0 (Default 1.0)
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                this.jsModule = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/DoAsISaySPA.razor.js");
                this.Voices = await this.SpeechSynthesis.GetVoicesAsync();
            }
            await GetWindowSize();
            this.StateHasChanged();
        }

        private async Task OnButtonClickDrawBoard()
        {
            await GetWindowSize();
            this.gb = new GameBoard(Convert.ToInt32(this.InputRows), Convert.ToInt32(this.InputCols), windowHeight, windowWidth, CellType);
            showBoard = true;
            showStart = true;
        }
        private async Task OnButtonClickPlay()
        {
            speechInProcess = true;
            showNext = false;
            showRepeat = false;
            showStop = true;
            showStart = false;
            showReDraw = false;

            allowClick = false;
            numCorrect = 0;
            numReps = Convert.ToInt32(this.NumReps);

            this.instr = instructions.GetInstructions(1, Convert.ToInt32(this.NumInstrs), Convert.ToInt32(this.InputRows), Convert.ToInt32(this.InputCols));
            await Task.Delay(1500);
            if (instr.Count > 0)
            {
                await PlayInstructions(instr);
            }
            numReps = numReps - 1;
            showRepeat = true;
        }
        private async Task OnButtonClickNext()
        {
            if (allowClick == false)
            {
                speechInProcess = true;
                showNext = false;
                showRepeat = false;
                showStop = true;
                showStart = false;
                showReDraw = false;

                if (numReps > 0)
                {
                    this.instr = instructions.GetInstructions(1, Convert.ToInt32(this.NumInstrs), Convert.ToInt32(this.InputRows), Convert.ToInt32(this.InputCols));
                    await Task.Delay(1500);
                    if (instr.Count > 0)
                    {
                        await PlayInstructions(instr);
                    }
                }
                numReps = numReps - 1;
                if (numReps > 0)
                {
                    showNext = true;
                }
            }
        }


        private async Task OnButtonClickRePlay()
        {
            speechInProcess = true;

            if (instr.Count > 0)
            {
                await PlayInstructions(instr);
            }
        }

        private async Task OnButtonClickStop()
        {
            speechInProcess = false;

            await this.SpeechSynthesis.CancelAsync();
            showNext = false;
            showRepeat = false;
            showStop = false;
            showStart = true;
            showReDraw = true;
        }

        private async Task PlayInstructions(List<string> instr)
        {
            await Task.Delay(1000); // Wait 1 seconds without blocking

            EndRow = this.instructions.EndRow;
            EndCol = this.instructions.EndCol;

            if (this.InputVoice == null) return;
            selectedVoice = this.Voices.Where(x => x.Name == this.InputVoice).FirstOrDefault();
            if (selectedVoice == null) return;

            var utterancet = new SpeechSynthesisUtterance
            {
                Text = instr[0],
                Voice = selectedVoice,
                Lang = selectedVoice.Lang,
                Pitch = Convert.ToDouble(this.InputPitch),
                Rate = Convert.ToDouble(this.InputRate),
                Volume = Convert.ToDouble(this.InputVolume)
            };

            int delay1 = Convert.ToInt32(PauseBetweenUtterance);

            for (int i = 0; i < instr.Count; i++)
            {
                utterancet.Text = instr[i];
                await this.SpeechSynthesis.SpeakAsync(utterancet); // <-- Speak!
                await Task.Delay(delay1); // Wait 1 seconds without blocking
            }
            allowClick = true;
        }
        private async Task PlayDistraction()
        {
            int delay2 = Convert.ToInt32(PauseBetweenDistraction);
            while (speechInProcess == true)
            {
                oldValue = this.gb.ShuffleSquares();
                //oldValue = this.gb.ShuffleColors();
                this.StateHasChanged();
                await Task.Delay(delay2); // Wait 2 seconds without blocking
            }
        }

        private async Task GameSquareClick(string cellID)
        {
            if (allowClick == true)
            {
                this.clickedCell = Convert.ToInt32(cellID);
                GameSquare? gs = this.gb.GetGameSquare(cellID);
                if (gs.BoardRow == EndRow & gs.BoardCol == EndCol)
                {
                    numCorrect = numCorrect + 1;
                    gs.ColorBack = "green";
                    await JsRuntime.InvokeAsync<string>("PlaySound", "treasure");
                }
                else
                {
                    gs.ColorBack = "red";
                    await JsRuntime.InvokeAsync<string>("PlaySound", "losing");
                }

                allowClick = false;
                showRepeat = false;
                if (numReps > 0) showNext = true;
                await Task.Delay(2000);
                gs.ColorBack = "cornsilk";
            }
        }

        protected async Task GetWindowSize()
        {
            var dimension = await jsModule.InvokeAsync<WindowDimensions>("getWindowSize");
            windowHeight = dimension.Height;
            windowWidth = dimension.Width;
        }

    }
}
