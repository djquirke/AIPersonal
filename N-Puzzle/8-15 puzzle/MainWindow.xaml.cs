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
            rdio_8.IsChecked = true;
        }

        private void setHardest8(object sender, RoutedEventArgs e)
        {
            board_.ChangeGridSize(3);
            cbxi_bfs.IsEnabled = true;
            rdio_8.IsChecked = true;

            board_.InitialiseHardest8();
        }

        private async void btnShuffleClick(object sender, RoutedEventArgs e)
        {
            int shuffle_count;
            if(!Int32.TryParse(tbx_shuffle_count.Text, out shuffle_count))
            {
                MessageBox.Show("Invalid shuffle count", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            EnableAllButtons(false);

            await Task.Run(() => board_.ShuffleBoard(shuffle_count));
            board_.DrawBoard();

            EnableAllButtons(true);
        }

        private void EnableAllButtons(bool p)
        {
            rdio_8.IsEnabled = p;
            rdio_15.IsEnabled = p;
            cbx_alg.IsEnabled = p;
            cbx_heur.IsEnabled = p;
            cbxi_bfs.IsEnabled = p;
            btn_shuffle.IsEnabled = p;
            btn_hard8.IsEnabled = p;
            btn_10move.IsEnabled = p;
            btn_22move.IsEnabled = p;
            btn_34moves.IsEnabled = p;
            btn_42moves.IsEnabled = p;
            btn_58moves.IsEnabled = p;
            btn_Run.IsEnabled = p;

            if (rdio_15.IsChecked.Value) cbxi_bfs.IsEnabled = false;
        }

        private void rdio_8_Checked(object sender, RoutedEventArgs e)
        {
            board_.ChangeGridSize(3);
            cbxi_bfs.IsEnabled = true;
        }

        private void rdio_15_Checked(object sender, RoutedEventArgs e)
        {
            board_.ChangeGridSize(4);
            cbxi_bfs.IsEnabled = false;
            if(cbx_alg.Text.Equals("Breadth First Search"))
            {
                cbx_alg.Text = "";
                cbx_heur.IsEnabled = true;
            }
        }

        private void cbx_alg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            ComboBoxItem cbi = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            if(cbi.Content.Equals("Breadth First Search"))
            {
                cbx_heur.IsEnabled = false;
            }
            else if(cbi.Content.Equals("A*"))
            {
                cbx_heur.IsEnabled = true;
            }
        }

        private void btn_Run_Click(object sender, RoutedEventArgs e)
        {
            if(cbx_alg.Text.Equals("A*") && cbx_heur.SelectedItem == null)
            {
                MessageBox.Show("Please select a heuristic!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(cbx_alg.Text.Equals(""))
            {
                MessageBox.Show("Please select an algorithm!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if(cbx_alg.Text.Equals("Breadth First Search"))
            {
                runBFS();
            }
            else if(cbx_alg.Text.Equals("A*") && cbx_heur.Text.Equals("Manhattan Distance"))
            {
                runAStar();
            }
            else if(cbx_alg.Text.Equals("A*") && cbx_heur.Text.Equals("Pattern Databases"))
            {
                //PatternDatabase.Instance.Build();
                //runAStar(true);
                MessageBox.Show("Pattern database not implemented, please see commented pseudocode in PatternDatabase.cs", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private async void runBFS()
        {
            EnableAllButtons(false);

            long rt = 0;
            int move_count = 0;
            int boards_searched = 0;
            int open_list_size = 0;
            long mem_used = 0;

            bool success = await Task.Run(() => board_.RunBFS(ref rt, ref move_count, ref boards_searched, ref open_list_size, ref mem_used));

            EnableAllButtons(true);

            mem_used /= 1024;
            mem_used /= 1024;

            lbl_rt.Content = rt.ToString() + "ms";
            lbl_moves.Content = move_count.ToString();
            lbl_boards_searched.Content = boards_searched.ToString();
            lbl_open_list.Content = open_list_size.ToString();
            lbl_mem_used.Content = mem_used.ToString();

            if (success)
            {
                MessageBox.Show("BFS was a success!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Simulate();
            }
            else
            {
                MessageBox.Show("BFS failed!", "Fail!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void runAStar()
        {
            EnableAllButtons(false);

            long rt = 0;
            int move_count = 0;
            int boards_searched = 0;
            int open_list_size = 0;
            long mem_used = 0;

            bool success = await Task.Run(() => board_.RunAStar(ref rt, ref move_count, ref boards_searched, ref open_list_size, ref mem_used));

            EnableAllButtons(true);

            lbl_rt.Content = rt.ToString() + "ms";
            lbl_moves.Content = move_count.ToString();
            lbl_boards_searched.Content = boards_searched.ToString();
            lbl_open_list.Content = open_list_size.ToString();

            mem_used /= 1024;
            mem_used /= 1024;

            lbl_mem_used.Content = mem_used;

            if (success)
            {
                MessageBox.Show("AStar was a success!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Simulate();
            }
            else
            {
                MessageBox.Show("AStar failed!", "Fail!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Simulate()
        {
            EnableAllButtons(false);
            await Task.Run(() => board_.RunSimulation(250));
            EnableAllButtons(true);
        }

        private void set22Moves(object sender, RoutedEventArgs e)
        {
            board_.ChangeGridSize(3);
            cbxi_bfs.IsEnabled = true;
            rdio_8.IsChecked = true;

            board_.Initialise22Moves();
        }

        private void set10moves(object sender, RoutedEventArgs e)
        {
            board_.ChangeGridSize(3);
            cbxi_bfs.IsEnabled = true;
            rdio_8.IsChecked = true;

            board_.Initialise10Moves();
        }

        private void set34moves(object sender, RoutedEventArgs e)
        {
            board_.ChangeGridSize(4);
            cbxi_bfs.IsEnabled = false;
            rdio_15.IsChecked = true;

            board_.Initialise34Moves();
        }

        private void set42moves(object sender, RoutedEventArgs e)
        {
            board_.ChangeGridSize(4);
            cbxi_bfs.IsEnabled = false;
            rdio_15.IsChecked = true;

            board_.Initialise42Moves();
        }

        private void set58moves(object sender, RoutedEventArgs e)
        {
            board_.ChangeGridSize(4);
            cbxi_bfs.IsEnabled = false;
            rdio_15.IsChecked = true;

            board_.Initialise58Moves();
        }
    }
}
