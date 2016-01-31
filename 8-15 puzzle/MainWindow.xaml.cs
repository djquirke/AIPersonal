using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _8_15_puzzle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BoardManager board_;

        public MainWindow()
        {
            InitializeComponent();

            board_ = new BoardManager();
            board_.Initialise(GameGrid, 3);
        }

        private void set8Puzzle(object sender, RoutedEventArgs e)
        {
            board_.ChangeGridSize(3);
        }

        private void set15Puzzle(object sender, RoutedEventArgs e)
        {
            board_.ChangeGridSize(4);
        }

        private async void btnShuffleClick(object sender, RoutedEventArgs e)
        {
            int shuffle_count;
            if(!Int32.TryParse(tbx_shuffle_count.Text, out shuffle_count))
            {
                MessageBox.Show("Invalid shuffle count", "Error!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }

            EnableAllButtons(false);

            await Task.Run(() => board_.ShuffleBoard(shuffle_count));
            board_.DrawBoard();
            EnableAllButtons(true);
        }

        private void EnableAllButtons(bool p)
        {
            btn_15.IsEnabled = p;
            btn_8.IsEnabled = p;
            btn_BFS.IsEnabled = p;
            btn_MD.IsEnabled = p;
            btn_PD.IsEnabled = p;
            btn_shuffle.IsEnabled = p;
        }

        private void btn_BFS_Click(object sender, RoutedEventArgs e)
        {
            if(board_.RunBFS())
            {
                MessageBox.Show("BFS was a success!", "Success", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                board_.RunSimulation();
            }
            else
            {
                MessageBox.Show("BFS failed!", "Fail!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
    }
}
