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
        int x, y, size = 50, difficult = 60;
        Random random = new Random();
        Cell[,] cells = new Cell[9, 9];
        bool win = false;


        public Form1()
        {

            InitializeComponent();
            generateMap();
        }
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startGame();
        }

        private void checkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool error = false;
            win = true;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    try
                    {
                        Convert.ToInt32(cells[i, j].Text);
                        if (cells[i, j].Value != Convert.ToInt32(cells[i, j].Text))
                        {
                            MessageBox.Show($"Error in: row {i + 1} col {j + 1}");
                            win = false;
                            error = true;
                            break;
                        }
                    }
                    catch
                    {
                        win = false;
                        error = true;
                        MessageBox.Show($"Empty cell in row {i + 1} col {j + 1}");
                        break;
                    }

                }
                if (error)
                    break;
            }

            if (win)
            {
                MessageBox.Show("Win!");
            }
        }        

        public void startGame()
        {
            clearMap();
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
                    
                    cells[i, j].BackColor = SystemColors.Window;

                    cells[i, j].BackColor = ((i / 3) + (j / 3)) % 2 == 0 ? SystemColors.Control : Color.LightGray;

                    cells[i, j].Location = new Point(i * size, j * size);
                    cells[i, j].X = i;
                    cells[i, j].Y = j;
                    cells[i, j].KeyPress += setNumber;
                    //Testing
                    //cells[i, j].Text = Convert.ToString(cells[i, j].Value);
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
                    setCellsNumbers(i, j);
                }
            }
        }

        public void setCellsNumbers(int i, int j)
        {
            cells[i, j].Value = (i * 3 + i / 3 + j) % 9 + 1;
        }

        public void showRandomNumber()
        {            
            for (int i = 0; i < difficult; i++)
            {
                x = random.Next(9);
                y = random.Next(9);
                cells[x, y].Text = Convert.ToString(cells[x, y].Value);
                cells[x, y].Locked = true;                
            }
        }       

        public void clearMap()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cells[i, j].Text = "";
                    cells[i, j].Locked = false;
                }
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

        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            difficult = 60;
            easyToolStripMenuItem.Checked = true;
            normalToolStripMenuItem.Checked = false;
            hardToolStripMenuItem.Checked = false;
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            difficult = 40;
            easyToolStripMenuItem.Checked = false;
            normalToolStripMenuItem.Checked = true;
            hardToolStripMenuItem.Checked = false;
        }

        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            difficult = 20;
            easyToolStripMenuItem.Checked = false;
            normalToolStripMenuItem.Checked = false;
            hardToolStripMenuItem.Checked = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"is a logic-based, combinatorial number-placement puzzle. In classic Sudoku, the objective is to fill a 9 × 9 grid with digits so that each column, each row, and each of the nine 3 × 3 subgrids that compose the grid (also called ""boxes"", ""blocks"", or ""regions"") contain all of the digits from 1 to 9. The puzzle setter provides a partially completed grid, which for a well-posed puzzle has a single solution.", "Help");
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
