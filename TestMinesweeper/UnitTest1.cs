using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System;


namespace TestMinesweeper
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestButtonContent()
        {
            ContentChecker contentChecker = new ContentChecker();
            Button button = new Button();
            button.Text = "!";
            Assert.AreEqual(true, contentChecker.isFieldFlagged(button));
            button.Text = "";
            Assert.AreEqual(false, contentChecker.isFieldFlagged(button));
            Assert.AreEqual(true, contentChecker.IsUncovered(button));
        }
        [TestMethod]
        public void GeneratorTest()
        {
            for(int i=0; i<100; i++)
            {
                List<int> numbers = new List<int>();
                int number = i;
                for(int j=0; j<10; j++)
                {
                    numbers.Add(number);
                    number++;
                }
                number = Generating.RandomNumber(i, i + 10);
                Assert.IsTrue(numbers.Contains(number));
            }
        }
        List<Button> buttons;
        [TestMethod]
        public void WinTest()
        {
            Rules rules = new Rules();
            buttons = new List<Button>();
            for (int i = 0; i < 100; i++)
            {
                buttons.Add(new Button());
                buttons[i].BackColor = Color.FromArgb(192, 192, 192);
                if (i < 80)
                {
                    buttons[i].BackColor = Color.FromArgb(0, 0, 0);
                }
            }
            Assert.IsTrue(rules.isWin(buttons));
        }
        [TestMethod]
        public void ValidPlaceTest()
        {
            Rules rules = new Rules();
            for(int i=0; i<10; i++)
            {
                for(int j=0;j<10; j++)
                {
                    Assert.IsTrue(rules.IsValid(i, j));
                    Assert.IsFalse(rules.IsValid(i+10, j+10));
                }
            }
        }
        List<int> mineList;
        [TestMethod]
        public void IsMineTest()
        {
            Rules rules = new Rules();
            Mines mines = new Mines();
            mineList = new List<int>();
            mines.initMines();
            mineList = mines.getMines();
            int count = 0;
            for (int i = 0; i < 20; i++)
            {
                int tempnumber = Generating.RandomNumber(0, 99);
                while (rules.IsMine(tempnumber, mineList))
                {
                    tempnumber = Generating.RandomNumber(0, 99);
                }
                mines.AddMine(tempnumber);
            }
            for (int i = 0; i < 100; i++)
            {
                if (rules.IsMine(i, mineList))
                {
                    count++;
                }
            }
            Assert.AreEqual(20, count);
        }
        [TestMethod]
        public void CounterTest()
        {
            Mines mines = new Mines();
            Rules rules = new Rules();
            mineList = new List<int>();
            mines.initMines();
            mineList = mines.getMines();
            int count = 0;
            for (int i = 0; i < 20; i++)
            {
                int tempnumber = Generating.RandomNumber(0, 99);
                while (rules.IsMine(tempnumber, mineList))
                {
                    tempnumber = Generating.RandomNumber(0, 99);
                }
                mines.AddMine(tempnumber);
            }
            buttons = new List<Button>();
            for (int i = 0; i < 100; i++)
            {
                buttons.Add(new Button());
                buttons[i].BackColor = Color.FromArgb(192, 192, 192);
                buttons[i].Name = (i).ToString();
                if (i < 80)
                {
                    buttons[i].BackColor = Color.FromArgb(0, 0, 0);
                }
            }
            Gameplay gameplay = new Gameplay();
            List<int> numbers = new List<int>();
            for(int i = 0; i < 9; i++)
            {
                numbers.Add(i);
            }
            for(int i=0; i<100; i++)
            {
                Assert.IsTrue(numbers.Contains(gameplay.CountAdjacentMines(buttons[i], mineList)));
            }

           
        }

    }
}
