using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prodigy_SD_04
{
    public partial class Form1 : Form
    {
        TextBox[,] textBoxes = new TextBox[9, 9];
        int[,] initialBoard = new int[9, 9]
        {
            {8, 0, 6, 0, 0, 9, 3, 0, 1},
            {9, 0, 4, 0, 3, 0, 2, 0, 5},
            {0, 7, 0, 0, 0, 5, 0, 9, 0},
            {2, 0, 7, 0, 0, 0, 0, 0, 0},
            {0, 8, 0, 0, 0, 0, 0, 2, 0},
            {0, 0, 0, 0, 0, 0, 5, 0, 7},
            {0, 6, 0, 7, 0, 0, 0, 1, 0},
            {7, 0, 1, 0, 9, 0, 6, 0, 8},
            {4, 0, 9, 6, 0, 0, 7, 0, 3}
        };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeSudokuGrid();
        }

        private void InitializeSudokuGrid()
        {
            try
            {
                int textBoxSize = 30;
                int margin = 5;
                int gridSize = 9 * textBoxSize + 8 * margin; // Total grid size

                // Having the Sudoku Grid in the Center
                int topOffset = (this.ClientSize.Height - gridSize) / 2;
                int leftOffset = (this.ClientSize.Width - gridSize) / 2;

                for (int r = 0; r < 9; r++)
                {
                    for (int c = 0; c < 9; c++)
                    {
                        TextBox textBox = new TextBox //managing the design
                        {
                            Width = textBoxSize,
                            Height = textBoxSize,
                            Top = topOffset + r * (textBoxSize + margin),
                            Left = leftOffset + c * (textBoxSize + margin),
                            TextAlign = HorizontalAlignment.Center,
                            MaxLength = 1,
                            Font = new Font("Arial", 16),
                            BorderStyle = BorderStyle.FixedSingle
                        };

                        if (initialBoard[r, c] != 0)
                        {
                            textBox.Text = initialBoard[r, c].ToString();
                            textBox.ReadOnly = true; // Make initial numbers read-only
                            textBox.BackColor = Color.LightGray; // Set background color for initial numbers
                        }

                        textBoxes[r, c] = textBox;
                        this.Controls.Add(textBox);
                    }
                }

                // Adding lines to distinguish the 3x3 sub-grids
                for (int i = 0; i <= 9; i++)
                {
                    if (i % 3 == 0)
                    {
                        Panel hLine = new Panel //panel design
                        {
                            Height = 2,
                            Width = gridSize + 1,
                            Top = topOffset + i * (textBoxSize + margin) - margin / 2,
                            Left = leftOffset,
                            BackColor = Color.Black
                        };
                        this.Controls.Add(hLine);

                        Panel vLine = new Panel
                        {
                            Width = 2,
                            Height = gridSize + 1,
                            Top = topOffset,
                            Left = leftOffset + i * (textBoxSize + margin) - margin / 2,
                            BackColor = Color.Black
                        };
                        this.Controls.Add(vLine);
                    }
                }

                Button solveButton = new Button // Solve Button in the center 
                {
                    Text = "Solve",
                    Width = 100, // Set a fixed width for the button
                    Top = topOffset + gridSize + margin * 2,
                    Left = (this.ClientSize.Width - 100) / 2 // Center the button horizontally
                };
                solveButton.Click += SolveButton_Click;
                this.Controls.Add(solveButton);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing Sudoku grid: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SolveButton_Click(object sender, EventArgs e)
        {
            try//having the error handling to catch method or exceptions errors 
            {
                int[,] board = new int[9, 9];
                for (int r = 0; r < 9; r++)
                {
                    for (int c = 0; c < 9; c++)
                    {
                        if (int.TryParse(textBoxes[r, c].Text, out int value) && value >= 1 && value <= 9)
                        {
                            board[r, c] = value;
                        }
                        else
                        {
                            board[r, c] = 0;
                        }
                    }
                }

                if (SolveSudoku(board, 0, 0))
                {
                    for (int r = 0; r < 9; r++)
                    {
                        for (int c = 0; c < 9; c++)
                        {
                            textBoxes[r, c].Text = board[r, c].ToString();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No solution found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error solving Sudoku: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CanPlace(int[,] board, int row, int col, int num)
        {
            for (int x = 0; x < 9; x++)
            {
                if (board[row, x] == num || board[x, col] == num ||
                    board[row / 3 * 3 + x / 3, col / 3 * 3 + x % 3] == num)
                {
                    return false;
                }
            }
            return true;
        }

        private bool SolveSudoku(int[,] board, int row, int col)
        {
            if (row == 9) return true;
            if (col == 9) return SolveSudoku(board, row + 1, 0);
            if (board[row, col] != 0) return SolveSudoku(board, row, col + 1);

            for (int num = 1; num <= 9; num++)
            {
                if (CanPlace(board, row, col, num))
                {
                    board[row, col] = num;
                    if (SolveSudoku(board, row, col + 1))
                    {
                        return true;
                    }
                    board[row, col] = 0;
                }
            }

            return false;
        }
    }
}
