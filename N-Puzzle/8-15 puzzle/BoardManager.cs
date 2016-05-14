using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Board = System.Collections.Generic.List<System.Collections.Generic.List<int>>;

namespace _8_15_puzzle
{
    class BoardManager
    {
        Board tile_values_;
        Board goal_board_;
        List<Move> moves_;
        Grid game_grid_;
        HelperFunctions hf_;
        int size_;
        Position blank_pos_;

        public void Initialise(Grid grid, int size)
        {
            tile_values_ = new Board();
            goal_board_ = new Board();
            game_grid_ = grid;
            moves_ = new List<Move>();
            hf_ = new HelperFunctions();
            blank_pos_ = new Position();

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
                        tb.Text = tile_values_.board[j][i].ToString();
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
            tile_values_ = new Board();
            goal_board_ = new Board();
            int counter = 0;
            for (int i = 0; i < size; i++)
            {
                List<int> temp_list = new List<int>();
                List<int> temp_list2 = new List<int>();

                for (int j = 0; j < size; j++)
                {
                    int num = ++counter;

                    if (counter == size * size)
                    {
                        counter = 0;
                        blank_pos_ = new Position(size - 1, size - 1);
                    }

                    temp_list.Add(counter);
                    temp_list2.Add(counter);
                }
                tile_values_.board.Add(temp_list);
                goal_board_.board.Add(temp_list2);
            }
        }

        private void DeleteGrid()
        {
            game_grid_.Children.RemoveRange(0, game_grid_.ColumnDefinitions.Count * game_grid_.RowDefinitions.Count);
            game_grid_.ColumnDefinitions.RemoveRange(0, game_grid_.ColumnDefinitions.Count);
            game_grid_.RowDefinitions.RemoveRange(0, game_grid_.RowDefinitions.Count);
            tile_values_ = new Board();
        }

        private void SetupGrid(int size)
        {
            for (int i = 0; i < size; i++)
            {
                game_grid_.ColumnDefinitions.Add(new ColumnDefinition());
                game_grid_.RowDefinitions.Add(new RowDefinition());
            }
        }

        internal void InitialiseHardest8()
        {
            tile_values_.board[0][0] = 6;
            tile_values_.board[0][1] = 4;
            tile_values_.board[0][2] = 7;

            tile_values_.board[1][0] = 8;
            tile_values_.board[1][1] = 5;
            tile_values_.board[1][2] = 0;

            tile_values_.board[2][0] = 3;
            tile_values_.board[2][1] = 2;
            tile_values_.board[2][2] = 1;

            hf_.SetBlankPos(tile_values_, ref blank_pos_, size_);

            DrawBoard();
        }

        public void ShuffleBoard(int count)
        {
            hf_.SetBlankPos(tile_values_, ref blank_pos_, size_);

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

        public List<Position> GetValidMoves()
        {
            List<Position> valid_moves = new List<Position>();

            foreach(var dir in Enum.GetValues(typeof(Direction)))
            {
                Position nextMove = hf_.GetCoordsFromDirection(blank_pos_, (Direction)dir, size_);//(blank_pos_[0], blank_pos_[1], (Direction)dir);

                if(nextMove.x == blank_pos_.x && nextMove.y == blank_pos_.y) continue; //At same tile

                Position valid_move = new Position(nextMove);
                valid_moves.Add(valid_move);
            }
            return valid_moves;
        }

        private void MakeMove(Position move)
        {
            Position temp_blank = new Position(blank_pos_);
            int move_val = tile_values_.board[move.x][move.y];
            blank_pos_ = move;

            tile_values_.board[temp_blank.x][temp_blank.y] = move_val;
            tile_values_.board[move.x][move.y] = 0;
        }

        public bool RunBFS(ref long timer, ref int move_count, ref int boards_searched, ref int open_list_size, ref long mem_used)
        {
            bool ret = false;
            AI_BFS bfs = new AI_BFS();
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            moves_.Clear();
            //sw.Start();
            bool success = bfs.Run(tile_values_, blank_pos_, goal_board_, ref moves_, size_, ref boards_searched, ref open_list_size, ref timer, ref mem_used);
            //sw.Stop();
            //timer = sw.ElapsedMilliseconds;
            move_count = moves_.Count - 1;
            if(success)
            {
                ret = true;
            }
            return ret;
        }

        public bool RunAStar(ref long timer, ref int move_count, ref int boards_searched, ref int open_list_size, ref long mem_used)
        {
            bool ret = false;
            AI_AStar astar = new AI_AStar();
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            moves_.Clear();
            //sw.Start();
            bool success = astar.Run(tile_values_, blank_pos_, goal_board_, ref moves_, size_, ref boards_searched, ref open_list_size, ref timer, ref mem_used);
            //sw.Stop();
            //timer = sw.ElapsedMilliseconds;
            move_count = moves_.Count - 1;
            if (success)
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
                    MakeMove(moves_[moves_.Count - (i + 1)]);
                    DrawBoard();
                    System.Threading.Thread.Sleep(speed);
                }
            }
        }

        private void MakeMove(Move move)
        {
            if (move == null)
                return;
            int val1 = tile_values_.board[move.pos1.x][move.pos1.y];
            int val2 = tile_values_.board[move.pos2.x][move.pos2.y];

            tile_values_.board[move.pos1.x][move.pos1.y] = val2;
            tile_values_.board[move.pos2.x][move.pos2.y] = val1;
        }

        internal void Initialise22Moves()
        {
            tile_values_.board[0][0] = 7;
            tile_values_.board[0][1] = 6;
            tile_values_.board[0][2] = 0;

            tile_values_.board[1][0] = 5;
            tile_values_.board[1][1] = 4;
            tile_values_.board[1][2] = 3;

            tile_values_.board[2][0] = 2;
            tile_values_.board[2][1] = 8;
            tile_values_.board[2][2] = 1;

            hf_.SetBlankPos(tile_values_, ref blank_pos_, size_);

            DrawBoard();
        }

        internal void Initialise10Moves()
        {
            tile_values_.board[0][0] = 1;
            tile_values_.board[0][1] = 5;
            tile_values_.board[0][2] = 2;

            tile_values_.board[1][0] = 8;
            tile_values_.board[1][1] = 3;
            tile_values_.board[1][2] = 6;

            tile_values_.board[2][0] = 4;
            tile_values_.board[2][1] = 7;
            tile_values_.board[2][2] = 0;

            hf_.SetBlankPos(tile_values_, ref blank_pos_, size_);

            DrawBoard();
        }

        internal void Initialise58Moves()
        {
            tile_values_.board[0][0] = 14;
            tile_values_.board[0][1] = 13;
            tile_values_.board[0][2] = 5;
            tile_values_.board[0][3] = 2;

            tile_values_.board[1][0] = 15;
            tile_values_.board[1][1] = 10;
            tile_values_.board[1][2] = 11;
            tile_values_.board[1][3] = 1;

            tile_values_.board[2][0] = 8;
            tile_values_.board[2][1] = 7;
            tile_values_.board[2][2] = 12;
            tile_values_.board[2][3] = 9;

            tile_values_.board[3][0] = 3;
            tile_values_.board[3][1] = 0;
            tile_values_.board[3][2] = 4;
            tile_values_.board[3][3] = 6;

            hf_.SetBlankPos(tile_values_, ref blank_pos_, size_);

            DrawBoard();
        }

        internal void Initialise42Moves()
        {
            tile_values_.board[0][0] = 9;
            tile_values_.board[0][1] = 5;
            tile_values_.board[0][2] = 7;
            tile_values_.board[0][3] = 12;

            tile_values_.board[1][0] = 6;
            tile_values_.board[1][1] = 13;
            tile_values_.board[1][2] = 1;
            tile_values_.board[1][3] = 2;

            tile_values_.board[2][0] = 14;
            tile_values_.board[2][1] = 11;
            tile_values_.board[2][2] = 8;
            tile_values_.board[2][3] = 3;

            tile_values_.board[3][0] = 10;
            tile_values_.board[3][1] = 0;
            tile_values_.board[3][2] = 4;
            tile_values_.board[3][3] = 15;

            hf_.SetBlankPos(tile_values_, ref blank_pos_, size_);

            DrawBoard();
        }

        internal void Initialise34Moves()
        {
            tile_values_.board[0][0] = 2;
            tile_values_.board[0][1] = 4;
            tile_values_.board[0][2] = 8;
            tile_values_.board[0][3] = 7;

            tile_values_.board[1][0] = 1;
            tile_values_.board[1][1] = 9;
            tile_values_.board[1][2] = 10;
            tile_values_.board[1][3] = 0;

            tile_values_.board[2][0] = 14;
            tile_values_.board[2][1] = 3;
            tile_values_.board[2][2] = 5;
            tile_values_.board[2][3] = 6;

            tile_values_.board[3][0] = 13;
            tile_values_.board[3][1] = 11;
            tile_values_.board[3][2] = 12;
            tile_values_.board[3][3] = 15;

            hf_.SetBlankPos(tile_values_, ref blank_pos_, size_);

            DrawBoard();
        }
    }
}
