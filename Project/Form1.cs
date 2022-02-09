using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class Form1 : Form
    {
        int x, y, size = 50;
        Random random = new Random();
        Cell[,] cells = new Cell[9, 9];
        bool win = false;
        

        public Form1()
        {
            InitializeComponent();
        }      

        private void button1_Click(object sender, EventArgs e)
        {
            startGame();                     
        }
        private void button2_Click(object sender, EventArgs e)
        {
            win = true;
            for (int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                 {                   
                    try
                    {
                        if (cells[i, j].Value != Convert.ToInt32(cells[i, j].Text))
                        {
                            cells[i, j].BackColor = Color.Red;
                            MessageBox.Show($"Error in: row {i+1} col {j+1}");
                            win = false;                           
                        }
                        else
                        {
                            cells[i, j].BackColor = SystemColors.Window;
                        }
                    }
                    catch
                    {                       
                        win = false;
                    }
                                      
                }
            }

            if (win)
            {
                MessageBox.Show("Win!");
            }
        }

        public void startGame()
        {
            generateMap();
            shuffle();
            showRandomNumber();
            display();
        }

        public void display()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    
                    cells[i, j].Font = new Font(SystemFonts.DefaultFont.FontFamily, 20);
                    cells[i, j].Size = new Size(size, size);

                    if (!cells[i, j].Locked)
                    {
                        cells[i, j].ForeColor = SystemColors.ControlDarkDark;                        
                    }                   

                    cells[i, j].BackColor = SystemColors.Window;
                    cells[i, j].Location = new Point(i * size, j * size);
                    cells[i, j].X = i;
                    cells[i, j].Y = j;
                    cells[i, j].KeyPress += setNumber;
                    //Testing
                    cells[i, j].Text = Convert.ToString(cells[i, j].Value);
                    panel1.Controls.Add(cells[i, j]);                   
                }
            }
        }

        private void setNumber(object sender, KeyPressEventArgs e)
        {
            Cell cell = sender as Cell;
            int value;

            if (!cell.Locked)
            {
                if (int.TryParse(e.KeyChar.ToString(), out value))
                {
                    if (value == 0)
                        cell.Clear();
                    else
                        cell.Text = value.ToString();

                    cell.ForeColor = SystemColors.ControlDarkDark;
                }
            }
            
        }
        
        public void generateMap()
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    cells[i, j] = new Cell();
                    cells[i, j].Value = (i * 3 + i / 3 + j) % 9 + 1;                    
                }
            }
        }

        public void showRandomNumber()
        {
            for(int i = 0; i < 80; i++)
            {
                x = random.Next(9);
                y = random.Next(9);
                cells[x, y].Text = Convert.ToString(cells[x, y].Value);
                cells[x, y].Locked = true;
            }
        }       

        //Shuffle
        public void shuffle()
        {
            for(int i = 0; i < random.Next(5, 15); i++)            
                shuffleRow();

            for (int i = 0; i < random.Next(5, 15); i++)            
                shuffleCol();

            for (int i = 0; i < random.Next(5, 15); i++)
                shuffleBlocksInColumn();

            for (int i = 0; i < random.Next(5, 15); i++)
                shuffleBlocksInRow();
        }

        public void shuffleRow()
        {            
            int block = random.Next(0, 3);
            int row1 = random.Next(0, 3);
            int line1 = block * 3 + row1;
            int row2 = random.Next(0, 3);
            while (row1 == row2)
                row2 = random.Next(0, 3);
            int line2 = block * 3 + row2;
            for (int i = 0; i < 9; i++)
            {
                int cur = cells[i, line1].Value;
                cells[i, line1].Value = cells[i, line2].Value;
                cells[i, line2].Value = cur;
            }
        }

        public void shuffleCol()
        {
            int block = random.Next(0, 3);
            int row1 = random.Next(0, 3);
            int line1 = block * 3 + row1;
            int row2 = random.Next(0, 3);
            while (row1 == row2)
                row2 = random.Next(0, 3);
            int line2 = block * 3 + row2;
            for (int i = 0; i < 9; i++)
            {
                int cur = cells[line1, i].Value;
                cells[line1, i].Value = cells[line2, i].Value;
                cells[line2, i].Value = cur;
            }
        }      

        public void shuffleBlocksInColumn()
        {        
            int block1 = random.Next(0, 3)*3;
            int block2 = random.Next(0, 3)*3;
            while (block1 == block2)
                block2 = random.Next(0,3)*3;
      
            for (int i = 0; i < 9; i++)
            {
                int k = block2;
                for (int j = block1; j < block1 + 3; j++)
                {
                    int cur = cells[i, j].Value;
                    cells[i, j].Value = cells[i, k].Value;
                    cells[i, k].Value = cur;
                    k++;
                }
            }
        }

        public void shuffleBlocksInRow()
        {    
            int block1 = random.Next(0, 3)*3;
            int block2 = random.Next(0, 3)*3;
            while (block1 == block2)
                block2 = random.Next(0, 3)*3;
            
            for (int i = 0; i < 9; i++)
            {
                int k = block2;
                for (int j = block1; j < block1 + 3; j++)
                {
                    int cur = cells[j, i].Value;
                    cells[j, i].Value = cells[k, i].Value;
                    cells[k, i].Value = cur;
                    k++;
                }
            }
        }


    }
}
