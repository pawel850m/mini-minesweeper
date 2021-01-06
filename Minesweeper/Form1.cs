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
       private int time = 0;
        
        private List<Button> buttons;
        private Boolean InitGame;
        private Label label;
        private Rules rules;
        private Gameplay gameplay;
        private Mines mines = new Mines();
        public Form1()
		{
			InitializeComponent();
            InitGame = false;
            InitBoard();
            rules = new Rules();
           gameplay = new Gameplay();
		}
        private void startTimer()
        {
            time = 0;
            timer.Start();
        }
        private void InitBoard()
        {
            startTimer();
            label = new Label();
            label.Text = "10:00 WIN";
            label.AutoSize = true;
            label.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            label.Location = new System.Drawing.Point(0, 0);
            label.Name = "label1";
            label.Size = new System.Drawing.Size(52, 21);
            label.TabIndex = 0;
            flowLayoutPanel2.Controls.Add(label);


            buttons = new List<Button>();
            mines.initMines();
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
                //button1.Text = (i + 1).ToString();
                button1.Cursor = System.Windows.Forms.Cursors.Hand;
                button1.Margin = new System.Windows.Forms.Padding(0);
                flowLayoutPanel1.Controls.Add(button1);
                button1.Click += field_LeftClick;
                button1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.field_RightClick);
            }
            flowLayoutPanel1.Visible = true;
        }

        private void setWin()
        {
            if(rules.isWin(buttons))
            {
                timer.Stop();
                label.ForeColor = System.Drawing.Color.ForestGreen;
                label.Text = (time - 1).ToString() + " s. WIN";
                gameplay.SetButtonsDisabled(buttons);
            }
        }
        private void ClearBoard()
        {
            for (int i = 0; i < 100; i++)
            {
                buttons[i].Text = " ";
                buttons[i].Enabled = true;
                buttons[i].ForeColor = System.Drawing.Color.Black;
                buttons[i].BackColor = Color.FromArgb(192, 192, 192);
            }
            mines.initMines();
        }
        //private void DeleteAllButtons()
        //{
         //   flowLayoutPanel1.Visible = false;
           // for (int i = 0; i < 100; i++)
            //{
              //  flowLayoutPanel1.Controls.Remove(buttons[i]);
            //}
        //}
     

        private void Form1_Load(object sender, EventArgs e)
        {
            menuToolStripMenuItem.Click += new System.EventHandler(restart);
        }
        private void restart(object sender, EventArgs e)
        {
            label.ForeColor = System.Drawing.Color.Black;
            InitGame = false;
            flag = true;
            ClearBoard();
            label.Text = "restarting...";
            startTimer();
        }
                Boolean flag = true;
        private void field_LeftClick(object sender, EventArgs e)
        {
            clickButton(sender);
        }
        private void field_RightClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if(button.Text != " " && button.Text != "!")
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
        private void clickButton(object sender)
        {
            Button button = (Button)sender;
            if (button.Text == "!")
            {
                return;
            }
            button.BackColor = Color.FromArgb(225, 225, 225);
            buttons[Int32.Parse(button.Name)].BackColor = Color.FromArgb(225, 225, 225);
            buttons[Int32.Parse(button.Name)].Text = " ";
            if (mines.getMines().Contains(Int32.Parse(button.Name)))
            {
                button.ForeColor = System.Drawing.Color.Red;
                ShowMines();
                gameplay.SetButtonsDisabled(buttons);
                timer.Stop();
                label.ForeColor = System.Drawing.Color.OrangeRed;
            }
            else
            {
                if (!InitGame)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        int tempnumber = Generating.RandomNumber(0, 99);
                        while (rules.IsMine(tempnumber, mines.getMines()) || tempnumber == Int32.Parse(button.Name))
                        {
                            tempnumber = Generating.RandomNumber(0, 99);
                        }
                        mines.AddMine(tempnumber);
                    }
                    //ShowMines();
                    InitGame = true;
                }
                
                int AdjacentMinesNumber = gameplay.CountAdjacentMines(button, mines.getMines());
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
                            Console.WriteLine(i);
                            for (int j = -1; j < 2; j++)
                                {
                                    if (j == 0 && i == 0) continue;
                                Console.WriteLine(i);
                                Console.WriteLine(j);
                                    if (rules.IsValid(row - i, col - j) && ! rules.IsMine(10 * (row - i) + col - j, mines.getMines()))
                                    {
                                    int number = 10 * (row - i) + col - j;
                                    if (buttons[number].BackColor == Color.FromArgb(225, 225, 225)) continue;
                                        
                                        clickButton(buttons[number]);
                                    }
                              
                                }
                            }
                            flag = false;

                        
             
                        
                        //button.Text = " ";
                        //button.ForeColor = System.Drawing.Color.Black;
                        //int tempnumber = RandomNumber(0, 7);
                        //int buttonNumber = Int32.Parse(button.Name);
                        //int[] ButtonNumbersTable = {buttonNumber-11, buttonNumber - 10, buttonNumber - 9, buttonNumber -1,
                        //buttonNumber + 1, buttonNumber+9, buttonNumber+10, buttonNumber+11};
                        //while (ButtonNumbersTable[tempnumber] < 0 || ButtonNumbersTable[tempnumber] > 99 || IsMine(tempnumber))
                        //{
                        //    tempnumber = RandomNumber(0, 7);
                        //}
                        //clickButton(buttons[ButtonNumbersTable[tempnumber]]);
                        break;
                }
                if ((AdjacentMinesNumber != 0))
                {
                    button.Text = AdjacentMinesNumber.ToString();
                }

            }
            //flag = true;
            setWin();
        }
        private void ShowMines()
        {
            for (int i = 0; i < 100; i++)
            {
                //buttons[i].Text = "";
                //buttons[i].UseVisualStyleBackColor = true;
            }
            foreach (var mine in mines.getMines())
            {
                buttons[mine].Text = "X";
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            time++;
            label.Text = time.ToString() + " s.";
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
        public void SetButtonsDisabled(List<Button> buttons)
        {
            for (int i = 0; i < 100; i++)
            {
                buttons[i].Enabled = false;
            }
        }
    }
    public partial class Stopwatch{
        
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
    
    }
}
