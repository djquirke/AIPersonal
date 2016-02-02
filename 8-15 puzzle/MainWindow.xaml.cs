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
            lbl_rt.Content = "";
            lbl_moves.Content = "";

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
            rdio_8.IsEnabled = p;
            rdio_15.IsEnabled = p;
            cbx_alg.IsEnabled = p;
            cbxi_bfs.IsEnabled = p;
            btn_shuffle.IsEnabled = p;
            btn_hard8.IsEnabled = p;
            btn_Run.IsEnabled = p;
        }

        private async void runBFS()
        {
            EnableAllButtons(false);

            long rt = 0;
            int move_count = 0;

            bool success = await Task.Run(() => board_.RunBFS(ref rt, ref move_count));

            EnableAllButtons(true);

            lbl_rt.Content = rt.ToString() + "ms";
            lbl_moves.Content = move_count.ToString();

            if(success)
            {
                MessageBox.Show("BFS was a success!", "Success", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                Simulate();
            }
            else
            {
                MessageBox.Show("BFS failed!", "Fail!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private async void Simulate()
        {
            EnableAllButtons(false);
            await Task.Run(() => board_.RunSimulation(250));
            EnableAllButtons(true);
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
            if(cbx_alg.Text.Equals("A*") && cbx_heur.SelectedItem == null) return;

            if(cbx_alg.Text.Equals("Breadth First Search"))
            {
                runBFS();
            }
            else if(cbx_alg.Text.Equals("A*") && cbx_heur.Text.Equals("Manhattan Distance"))
            {
                MessageBox.Show("A* with MD");
            }
            else if(cbx_alg.Text.Equals("A*") && cbx_heur.Text.Equals("Pattern Databases"))
            {
                MessageBox.Show("A* with PD");
            }

        }
    }
}
