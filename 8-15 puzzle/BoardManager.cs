using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace _8_15_puzzle
{

    class Position
    {
        public int x, y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Position(Position pos)
        {
            this.x = pos.x;
            this.y = pos.y;
        }

        public Position()
        {
            x = 0;
            y = 0;
        }
    }

    enum Direction
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }

    class BoardManager
    {
        List<List<int>> tile_values_;
        List<List<List<int>>> moves_;
        Grid game_grid_;
        int size_;
        int[] blank_pos_;
        

        public void Initialise(Grid grid, int size)
        {
            tile_values_ = new List<List<int>>();
            game_grid_ = grid;
            moves_ = new List<List<List<int>>>();

            ChangeGridSize(size);
        }

        public void ChangeGridSize(int size)
        {
            size_ = size;
            DeleteGrid();
            SetupGrid(size);
            InitialiseValues(size);
            DrawBoard();
        }

        public void DrawBoard()
        {
            game_grid_.Dispatcher.Invoke((Action)(() =>
            {
                game_grid_.Children.RemoveRange(0, game_grid_.ColumnDefinitions.Count * game_grid_.RowDefinitions.Count);

                TextBlock tb = new TextBlock();
                for (int i = 0; i < game_grid_.ColumnDefinitions.Count; i++)
                {
                    for (int j = 0; j < game_grid_.RowDefinitions.Count; j++)
                    {
                        tb = new TextBlock();
                        tb.Text = tile_values_[j][i].ToString();
                        tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                        tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                        if (tb.Text.Equals("0")) tb.Text = " ";

                        game_grid_.Children.Add(tb);
                        Grid.SetColumn(tb, i);
                        Grid.SetRow(tb, j);
                    }
                }

            }));
        }

        private void InitialiseValues(int size)
        {
            tile_values_ = new List<List<int>>();
            int counter = 0;
            for (int i = 0; i < size; i++)
            {
                List<int> temp_list = new List<int>();

                for (int j = 0; j < size; j++)
                {
                    int num = ++counter;

                    if (counter == size * size)
                    {
                        counter = 0;
                        blank_pos_ = new int[] { size - 1, size - 1 };
                    }

                    temp_list.Add(counter);
                }
                tile_values_.Add(temp_list);
            }
        }

        private void DeleteGrid()
        {
            game_grid_.Children.RemoveRange(0, game_grid_.ColumnDefinitions.Count * game_grid_.RowDefinitions.Count);
            game_grid_.ColumnDefinitions.RemoveRange(0, game_grid_.ColumnDefinitions.Count);
            game_grid_.RowDefinitions.RemoveRange(0, game_grid_.RowDefinitions.Count);
            tile_values_ = new List<List<int>>();
        }

        private void SetupGrid(int size)
        {
            for (int i = 0; i < size; i++)
            {
                game_grid_.ColumnDefinitions.Add(new ColumnDefinition());
                game_grid_.RowDefinitions.Add(new RowDefinition());
            }
        }

        public void ShuffleBoard(int count)
        {
            SetBlankPos();

            for(int i = 0; i < count; i++)
            {
                List<Position> valid_moves = GetValidMoves();

                //Make sure there is a move that can be made
                if(valid_moves.Count == 0)
                {
                    MessageBox.Show("Error shuffling", "Error!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }

                //Make a random move
                Random r = new Random();
                int x = r.Next(0, valid_moves.Count);
                MakeMove(valid_moves[x]);
            }
        }

        private void SetBlankPos()
        {
            for(int i = 0; i < size_; i++)
            {
                for(int j = 0; j < size_; j++)
                {
                    if(tile_values_[i][j] == 0)
                    {
                        blank_pos_ = new int[] { i, j };
                        return;
                    }
                }
            }
        }

        public List<Position> GetValidMoves()
        {
            List<Position> valid_moves = new List<Position>();

            foreach(var dir in Enum.GetValues(typeof(Direction)))
            {
                int[] nextMove = GetCoordsFromDirection(blank_pos_[0], blank_pos_[1], (Direction)dir);

                if(nextMove[0] == blank_pos_[0] && nextMove[1] == blank_pos_[1]) continue; //At same tile

                Position valid_move = new Position(nextMove[0], nextMove[1]);
                valid_moves.Add(valid_move);
            }
            return valid_moves;
        }

        private int[] GetCoordsFromDirection(int cur_x, int cur_y, Direction dir)
        {
            int next_x = 0, next_y = 0;

            switch (dir)
            {
                case Direction.NORTH:
                    next_x = cur_x;
                    next_y = cur_y - 1;
                    break;
                case Direction.EAST:
                    next_x = cur_x + 1;
                    next_y = cur_y;
                    break;
                case Direction.SOUTH:
                    next_x = cur_x;
                    next_y = cur_y + 1;
                    break;
                case Direction.WEST:
                    next_x = cur_x - 1;
                    next_y = cur_y;
                    break;
            }

            return AdjustOOB(next_x, next_y);
        }

        private int[] AdjustOOB(int next_x, int next_y)
        {
            if(next_x >= size_) next_x = size_ - 1;
            if(next_x < 0) next_x = 0;
            if(next_y < 0) next_y = 0;
            if(next_y >= size_) next_y = size_ - 1;

            return new int[] { next_x, next_y };
        }

        private void MakeMove(Position move)
        {
            int[] temp_blank = new int[] { blank_pos_[0], blank_pos_[1] };
            int move_val = tile_values_[move.x][move.y];
            blank_pos_[0] = move.x;
            blank_pos_[1] = move.y;

            tile_values_[temp_blank[0]][temp_blank[1]] = move_val;
            tile_values_[move.x][move.y] = 0;
        }

        public bool RunBFS(ref long timer, ref int move_count)
        {
            bool ret = false;
            AI_BFS bfs = new AI_BFS();
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            moves_.Clear();
            sw.Start();
            bool success = bfs.Run(tile_values_, new Position(blank_pos_[0], blank_pos_[1]), ref moves_, size_);
            sw.Stop();
            timer = sw.ElapsedMilliseconds;
            move_count = moves_.Count - 1;
            if(success)//bfs.Run(tile_values_, new Position(blank_pos_[0], blank_pos_[1]), ref moves_, size_));//(tile_values_, new Position(blank_pos_[0], blank_pos_[1]), ref moves_, size_))
            {
                ret = true;
            }
            return ret;
        }

        public void RunSimulation(int speed)
        {
            if(moves_.Count == 0)
            {
                return;
            }
            else
            {
                for(int i = 0; i < moves_.Count; i++)
                {
                    tile_values_ = moves_[moves_.Count - (i + 1)];
                    DrawBoard();
                    System.Threading.Thread.Sleep(speed);
                }
            }
        }

        internal void InitialiseHardest8()
        {
            tile_values_[0][0] = 6;
            tile_values_[0][1] = 4;
            tile_values_[0][2] = 7;

            tile_values_[1][0] = 8;
            tile_values_[1][1] = 5;
            tile_values_[1][2] = 0;

            tile_values_[2][0] = 3;
            tile_values_[2][1] = 2;
            tile_values_[2][2] = 1;

            SetBlankPos();

            DrawBoard();
        }
    }
}
