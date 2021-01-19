using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
	public partial class Form1 : Form
	{ 
        private List<Button> buttons;
        private Label label;
        private Gameplay gameplay;
        //private Mines mines;
        
       // private void setWin()
       // {
         //   if(rules.isWin(buttons))
           // {
             //   timer.Stop();
               // label.ForeColor = System.Drawing.Color.ForestGreen;
          //      stopwatch.UpdateLabel(label, (stopwatch.getSeconds() - 1).ToString() + " s. WIN");
            ///    gameplay.SetButtonsDisabled(buttons);
            //}
       // }

        public Form1()
		{
			InitializeComponent();
            InitBoard();
		}

        public void InitBoard(){
            buttons = new List<Button>();
            timer.Start();
            label = new Label();
            label.AutoSize = true;
            label.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            label.Location = new System.Drawing.Point(0, 0);
            label.Name = "label1";
            label.Size = new System.Drawing.Size(52, 21);
            label.TabIndex = 0;
            flowLayoutPanel2.Controls.Add(label);

            for (int i = 0; i < 100; i++)
            {
                Button button1 = new Button();
                buttons.Add(button1);
                buttons[i].Name = (i).ToString();
                button1.Size = new System.Drawing.Size(30, 30);
                button1.BackColor = Color.FromArgb(192, 192, 192);
                button1.Text = " ";
                button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                button1.Cursor = System.Windows.Forms.Cursors.Hand;
                button1.Margin = new System.Windows.Forms.Padding(0);
                flowLayoutPanel1.Controls.Add(button1);
                button1.Click += field_LeftClick;
                button1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.field_RightClick);
            }
            flowLayoutPanel1.Visible = true;
            gameplay = new Gameplay();
            gameplay.InitGameplay(buttons, label, timer);
        }

        
        private void restart(object sender, EventArgs e)
        {
            gameplay.restart();
        }
        private void field_LeftClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            gameplay.UncoverField(button);
            
        }
        private void field_RightClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            gameplay.CreateFlag(button);
        }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            menuToolStripMenuItem.Click += new System.EventHandler(restart);
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            gameplay.timer_Tick();
        }
        

    }
    public partial class Rules : Form{
        private List<Button> buttons;
        public bool isWin(List<Button> buttons) {
            int count = 0;
            for (int i = 0; i < 100; i++)
            {
                if(buttons[i].BackColor != Color.FromArgb(192, 192, 192))
                {
                    count++;
                }
            }
            if(count == 80)
            {
                return true;
            }
            return false;
        }
        public Boolean IsMine(int placeNumber, List<int> mines)
        {
            if (mines.Contains(placeNumber))
            {
                return true;
            }
            else return false;
        }
        public Boolean IsValid(int row, int column)
        {
            if (row < 0 || row > 9) return false;
            return column < 10 && column >-1;
        }

    }
    public partial class Gameplay : Rules{

        private int time = 0;
        private List<Button> buttons;
        private Label label;
        private Mines mines;
        private ContentChecker contentChecker;
        private Boolean InitGame;
        private Stopwatch stopwatch;
        private System.Windows.Forms.Timer timer;

        public void InitGameplay(List<Button> buttons, Label label, System.Windows.Forms.Timer timer){
            mines = new Mines();
            mines.initMines();
            stopwatch = new Stopwatch();
            this.buttons = buttons;
            this.label = label;
            this.timer = timer;
            this.contentChecker = contentChecker;
        }
        public void UncoverField(Button button)
        {
            if (contentChecker.isFieldFlagged(button))
            {
                return;
            }
            button.BackColor = Color.FromArgb(225, 225, 225);
            buttons[Int32.Parse(button.Name)].BackColor = Color.FromArgb(225, 225, 225);
            buttons[Int32.Parse(button.Name)].Text = " ";

            if (mines.getMines().Contains(Int32.Parse(button.Name))){
                setDefeat();
            }
            else
            {
                if (!InitGame)
                {
                    AddMinesToBoard(button,mines, mines.getMines());
                    InitGame = true;
                }
                
                int AdjacentMinesNumber = CountAdjacentMines(button, mines.getMines());
                String name = button.Name;
                if ((AdjacentMinesNumber != 0))
                {
                    button.Text = AdjacentMinesNumber.ToString();
                }
                
                switch (AdjacentMinesNumber)
                {
                    case 1:
                        button.ForeColor = System.Drawing.Color.Blue;
                        break;
                    case 2:
                        button.ForeColor = System.Drawing.Color.Green;
                        break;
                    case 3:
                        button.ForeColor = System.Drawing.Color.Red;
                        break;
                    case 4:
                        button.ForeColor = System.Drawing.Color.Purple;
                        break;
                    case 5:
                        button.ForeColor = System.Drawing.Color.Black;
                        break;
                    case 6:
                        button.ForeColor = System.Drawing.Color.Maroon;
                        break;
                    case 7:
                        button.ForeColor = System.Drawing.Color.Gray;
                        break;
                    case 8:
                        button.ForeColor = System.Drawing.Color.Turquoise;
                        break;
                    default:
                            int row = Int32.Parse(button.Name) / 10;
            int col = Int32.Parse(button.Name) % 10;
                            for (int i = -1; i < 2; i++)
                            {
                            for (int j = -1; j < 2; j++)
                                {
                                    if (j == 0 && i == 0) continue;
                                    if (isCorrect(button, i, j))
                                    {
                                    int number = 10 * (row - i) + col - j;
                                    if (buttons[number].BackColor == Color.FromArgb(225, 225, 225)) continue;
                                        
                                        UncoverField(buttons[number]);
                                    }
                              
                                }
                            }
                        break;
                }
                
                if ((AdjacentMinesNumber != 0))
                {
                    button.Text = AdjacentMinesNumber.ToString();
                }

            }
            setWin();
        }
        public void CreateFlag(object sender)
        {
            contentChecker = new ContentChecker();
            Button button = (Button)sender;
            if(contentChecker.IsUncovered(button))
            {
                return;
            }
            switch (MouseButtons)
            {
                case MouseButtons.Right: 
                    if(button.Text == "!")
                    {
                        button.ForeColor = System.Drawing.Color.Black;
                        button.Text = " ";
                    }
                    else
                    {
                        button.ForeColor = System.Drawing.Color.OrangeRed;
                        button.Text = "!";
                    }
                    break;
            }
        }
        public void restart(){
            label.ForeColor = System.Drawing.Color.Black;
            InitGame = false;
            for (int i = 0; i < 100; i++)
            {
                buttons[i].Text = " ";
                buttons[i].Enabled = true;
                buttons[i].ForeColor = System.Drawing.Color.Black;
                buttons[i].BackColor = Color.FromArgb(192, 192, 192);
            }
            mines.initMines();
            stopwatch.reset();
            stopwatch.UpdateLabel(label, " ");
            timer.Start();
        }
        public void setDefeat() {
            mines.ShowMines(buttons);
            SetButtonsDisabled();
            timer.Stop();
            label.ForeColor = System.Drawing.Color.OrangeRed;
        }
        public void setWin()
        {
            if(isWin(buttons))
            {
                timer.Stop();
                label.ForeColor = System.Drawing.Color.ForestGreen;
                stopwatch.UpdateLabel(label, (stopwatch.getSeconds() - 1).ToString() + " s. WIN");
                SetButtonsDisabled();
            }
        }
        public void timer_Tick()
        {
            if(InitGame){
                stopwatch.addSecond();
                stopwatch.UpdateLabel(label);
            }
            
        }
        public bool isCorrect(Button button, int i, int j){
            int row = Int32.Parse(button.Name) / 10;
            int col = Int32.Parse(button.Name) % 10;
            if (IsValid(row - i, col - j) && ! IsMine(10 * (row - i) + col - j, mines.getMines())){
                return true;
            }
            return false;
        }
        public int CountAdjacentMines(Button button, List<int> mines) 
        {
            int row = Int32.Parse(button.Name) / 10;
            int col = Int32.Parse(button.Name) % 10;
            int AdjacentMines = 0;
            for(int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                    if (IsValid(row-i, col - j) && IsMine(10 * (row-i) + col - j, mines))
                    {
                        AdjacentMines++;
                    }
            }
            return AdjacentMines;
        }
        public void AddMinesToBoard(Button button, Mines mines, List<int> minesList){
            for (int i = 0; i < 20; i++)
                    {
                        int tempnumber = Generating.RandomNumber(0, 99);
                        while (IsMine(tempnumber, minesList) || tempnumber == Int32.Parse(button.Name))
                        {
                            tempnumber = Generating.RandomNumber(0, 99);
                        }
                        mines.AddMine(tempnumber);
                    }
                    //ShowMines();     
        }
        public void SetButtonsDisabled()
        {
            for (int i = 0; i < 100; i++)
            {
                buttons[i].Enabled = false;
            }
        }
        

    }
    public partial class Stopwatch : Form{
        private int time = 0;
        public void reset(){
            time = 0;
        }
        public int getSeconds(){
            return time;
        }
        public void addSecond(){
        time++;
        }
        public void UpdateLabel(Label label){
            label.Text = getSeconds().ToString() + " s.";
        }
        public void UpdateLabel(Label label, String text){
            label.Text = text;
        }
    } 
    public abstract class Generating{
        public static int RandomNumber(int a, int b)
        {
            Random rand = new Random();
            return rand.Next(a, b);
        }
    } 
    public class Mines{
        private List<int> mines;
        public void initMines(){ 
            mines =  new List<int>();
        }
        public void setMines(List<int> mines){
            this.mines = mines;
        }
        public List<int> getMines(){
            return mines;
        }
        public void AddMine(int tempnumber){
            this.mines.Add(tempnumber);
        }
        public void ShowMines(List<Button> buttons)
        {
            foreach (var mine in getMines())
            {
                buttons[mine].Text = "X";
            }
        }
    }
    public class ContentChecker{
        public bool isFieldFlagged(Button field){
            if (field.Text == "!")
            {
                return true;
            }
            return false;
        }
        public bool IsUncovered(Button button){
            if(button.Text != " " && button.Text != "!")
            {
                return true;
            }
            return false;
        }
    }


    
}

   
