using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
	public partial class Form1 : Form
	{
        private List<int> mines;
        private List<Button> buttons;
        private Boolean InitGame;

        public Form1()
		{
			InitializeComponent();
            InitGame = false;
            InitBoard();
            
		}
        private void InitBoard()
        {
            buttons = new List<Button>();
            mines = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                Button button1 = new Button();
                buttons.Add(button1);
                buttons[i].Name = (i + 1).ToString();
                button1.Size = new System.Drawing.Size(30, 30);
                //button1.Text = (i + 1).ToString();
                button1.UseVisualStyleBackColor = true;
                button1.Cursor = System.Windows.Forms.Cursors.Hand;
                button1.Margin = new System.Windows.Forms.Padding(0);
                flowLayoutPanel1.Controls.Add(button1);
                button1.Click += button1_Click;
            }
            flowLayoutPanel1.Visible = true;
        }
        private void ClearBoard()
        {
            for (int i = 0; i < 100; i++)
            {
                buttons[i].Text = "";
                buttons[i].Enabled = true;
                buttons[i].ForeColor = System.Drawing.Color.Black;
            }
            mines = new List<int>();
        }
        private void SetButtonsDisabled()
        {
            for (int i = 0; i < 100; i++)
            {
                buttons[i].Enabled = false;
            }
        }
        private void DeleteAllButtons()
        {
            flowLayoutPanel1.Visible = false;
            for (int i = 0; i < 100; i++)
            {
                flowLayoutPanel1.Controls.Remove(buttons[i]);
            }
        }


        private int RandomNumber(int a, int b)
        {
            Random rand = new Random();
            return rand.Next(a, b);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            restartToolStripMenuItem.Click += new System.EventHandler(restart);
        }
        private void restart(object sender, EventArgs e)
        {
            InitGame = false;
            ClearBoard();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clickButton(sender);
        }
        private void clickButton(object sender)
        {
            Button button = (Button)sender;
            if (mines.Contains(Int32.Parse(button.Name)))
            {
                button.ForeColor = System.Drawing.Color.Red;
                ShowMines();
                SetButtonsDisabled();
            }
            else
            {
                if (!InitGame)
                {
                    for (int i = 0; i < 30; i++)
                    {
                        int tempnumber = RandomNumber(1, 100);
                        Console.WriteLine("tak");
                        while (IsMine(tempnumber) || tempnumber == Int32.Parse(button.Name))
                        {
                            tempnumber = RandomNumber(1, 100);
                        }
                        mines.Add(tempnumber);
                    }
                    InitGame = true;
                }
                int AdjacentMinesNumber = CountAdjacentMines(button);
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
                        button.Text = " ";
                        button.Enabled = false;
                        int tempnumber = RandomNumber(1, 8);
                        int buttonNumber = Int32.Parse(button.Name);
                        int[] ButtonNumbersTable = {buttonNumber-11, buttonNumber - 10, buttonNumber - 9, buttonNumber -1,
                        buttonNumber + 1, buttonNumber+9, buttonNumber+10, buttonNumber+11};
                        while (ButtonNumbersTable[tempnumber] < 0 || ButtonNumbersTable[tempnumber] > 100 || IsMine(tempnumber))
                        {
                            tempnumber = RandomNumber(1, 8);
                        }
                        clickButton(buttons[ButtonNumbersTable[tempnumber]]);
                        break;
                }
                if(AdjacentMinesNumber!=0)
                button.Text = AdjacentMinesNumber.ToString();
            }
        }
        private void ShowMines()
        {
            for (int i = 0; i < 100; i++)
            {
                buttons[i].Text = "";
                buttons[i].UseVisualStyleBackColor = true;
            }
            foreach (var mine in mines)
            {
                buttons[mine-1].Text = "X";
            }
        }
        private int CountAdjacentMines(Button button)
        {
            int placeNumber = Int32.Parse(button.Name);
            int AdjacentMines = 0;
            List<int> PlacesToCheck = new List<int>();
            int Number = -12;
            for (int i = 0; i < 8; i++)
            {
                Number++;
                if (Number == 0)
                {
                    Number = 1;
                }
                if((placeNumber - Number)<101 && (placeNumber - Number) > 0)
                {
                    PlacesToCheck.Add(placeNumber - Number);
                }

                if (Number == -9)
                {
                    Number = -2;
                }
                if(Number == 1)
                {
                    Number = 8;
                }
            }
            foreach (int place in PlacesToCheck)
            {
                if (IsMine(place))
                {
                    AdjacentMines++;
                }
            }
            return AdjacentMines;
        }
        private Boolean IsMine(int placeNumber)
        {
            if (mines.Contains(placeNumber))
            {
                return true;
            }
            else return false;
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
