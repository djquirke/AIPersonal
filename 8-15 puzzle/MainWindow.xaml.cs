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
        public MainWindow()
        {
            InitializeComponent();
            TextBlock tb = new TextBlock();
            tb.Text = "1";
            GameGrid.Children.Add(tb);
        }

        private void set8Puzzle(object sender, RoutedEventArgs e)
        {
            DeleteGrid();
            SetupGrid(3);
        }

        private void set15Puzzle(object sender, RoutedEventArgs e)
        {
            DeleteGrid();
            SetupGrid(4);
        }

        private void DeleteGrid()
        {
            GameGrid.ColumnDefinitions.RemoveRange(0, GameGrid.ColumnDefinitions.Count);
            GameGrid.RowDefinitions.RemoveRange(0, GameGrid.RowDefinitions.Count);
        }

        private void SetupGrid(int size)
        {
            for (int i = 0; i < size; i++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition());
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }
    }
}
