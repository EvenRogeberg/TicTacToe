using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Members

        /// <summary>
        /// array of cells
        /// </summary>
     //   private MarkType[] mResults;
        private MarkType[,] grid;

        /// <summary>
        /// true if its player 1's turn
        /// </summary>
        private bool mPlayer1Turn;

        /// <summary>
        /// true if the game has ended
        /// </summary>
        private bool mGameEnded;

        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            NewGame();
        }
        #endregion
        /// <summary>
        /// starts a new game and clears all values back to the start
        /// </summary>
        private void NewGame()
        {
        
            // create array of 9
            Make2DArray();

            // player 1 starts
            mPlayer1Turn = true;

            mGameEnded = false;
        }

        /// <summary>
        /// Handels a button clicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //starts a new game on click after it finished
            if (mGameEnded)
            {
                NewGame();
                return;
            }

            // cast sender to a button
            var button = (Button)sender;

            // find the button in the array
            var row = Grid.GetRow(button);
            var column = Grid.GetColumn(button);
            var index = column + (row * 3);

            if (grid[row, column] != MarkType.Free)
            {
                return;
            }

            // set call value based on player 1 || 2
            grid[row, column] = mPlayer1Turn ? MarkType.Cross : MarkType.Nought;
            button.Content = mPlayer1Turn ? "X" : "O";

            //Change noughts(player 2) to red
            if (!mPlayer1Turn)
            {
                button.Foreground = Brushes.Red;
            }

            //switch player
            mPlayer1Turn = !mPlayer1Turn;

            //check for three line
            CheckForWinner();

            CheckNoWinners();

        }

        public MarkType[,] Make2DArray()
        {
            grid = new MarkType[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    grid[i, j] = MarkType.Free;
                }
            }

            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                // change background
                button.Content = string.Empty;
                button.Background = Brushes.White;
                button.Foreground = Brushes.Blue;
            });

            return grid;
        }

        private bool CheckHorizontal(int row)
        {
            if (grid[row, 0] != MarkType.Free && (grid[row, 0] & grid[row, 1] & grid[row, 2]) == grid[row, 0])
            {

                if (row == 0)
                {
                    Button0_0.Background = Button0_1.Background = Button0_2.Background = Brushes.Green;
                }
                else if (row == 1)
                {
                    Button1_0.Background = Button1_1.Background = Button1_2.Background = Brushes.Green;
                } else //row == 2
                {
                    Button2_0.Background = Button2_1.Background = Button2_2.Background = Brushes.Green;
                }

                return true;

            } else
            {
                return false;
            }
        }

        private bool CheckVertical(int vertical)
        {
            if (grid[0, vertical] != MarkType.Free && (grid[0, vertical] & grid[1, vertical] & grid[2, vertical]) == grid[0, vertical])
            {

                if (vertical == 0)
                {
                    Button0_0.Background = Button1_0.Background = Button2_0.Background = Brushes.Green;
                }
                else if (vertical == 1)
                {
                    Button0_1.Background = Button1_1.Background = Button2_1.Background = Brushes.Green;
                }
                else //vertical == 2
                {
                    Button0_2.Background = Button1_2.Background = Button2_2.Background = Brushes.Green;
                }

                return true;

            }
            else
            {
                return false;
            }
        }

        private bool CheckDiagonal()
        {
            if (grid[1, 1] != MarkType.Free && ((grid[0, 0] & grid[1, 1] & grid[2, 2]) == grid[1, 1] ||
                                               (grid[0, 2] & grid[1, 1] & grid[2, 0]) == grid[1, 1]))
            {

                if ((grid[0, 2] & grid[1, 1] & grid[2, 0]) == grid[1, 1])
                {
                    Button0_2.Background = Button1_1.Background = Button2_0.Background = Brushes.Green;
                }
                if ((grid[0, 0] & grid[1, 1] & grid[2, 2]) == grid[1, 1])
                {
                    Button0_0.Background = Button1_1.Background = Button2_2.Background = Brushes.Green;
                }

                return true;

            }
            else
            {
                return false;
            }
        }

        private void CheckForWinner()
        {

            for (int i = 0; i < 3; i++)
            {
                if (CheckHorizontal(i) || CheckVertical(i) || CheckDiagonal())
                {
                    mGameEnded = true;
                }
            }
        }

        private void CheckNoWinners()
        {
            var freeCell = false;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (grid[i, j] == MarkType.Free)
                    {
                        freeCell = true;
                    }
                }
            }
                       
            if (!freeCell)
            {
                // Game ended
                mGameEnded = true;

                // Turn all cells orange
                Container.Children.Cast<Button>().ToList().ForEach(button =>
                {
                    button.Background = Brushes.Orange;
                });
            }
        }
    }
}
