using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Board = System.Collections.Generic.List<System.Collections.Generic.List<int>>;

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

    class Move
    {
        public Position pos1;
        public Position pos2;

        public Move(Position pos1, Position pos2)
        {
            this.pos1 = pos1;
            this.pos2 = pos2;
        }

        public Move()
        {
            pos1 = new Position();
            pos2 = new Position();
        }

    }

    class BoardEqualityComparer : IEqualityComparer<Board>
    {
        public bool Equals(Board l1, Board l2)
        {
            for (int i = 0; i < l1.Count; i++)
            {
                if (!l1[i].SequenceEqual(l2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(Board list)
        {
            int hash = 19;
            foreach (List<int> list2 in list)
            {
                foreach (int i in list2)
                {
                    hash = hash * 31 + i.GetHashCode();
                }
            }
            return hash;
        }

    }

    class HelperFunctions
    {
        public void SetBlankPos(Board board, ref Position blank_pos, int grid_size)
        {
            for (int i = 0; i < grid_size; i++)
            {
                for (int j = 0; j < grid_size; j++)
                {
                    if (board[i][j] == 0)
                    {
                        blank_pos.x = i;
                        blank_pos.y = j;
                        return;
                    }
                }
            }
        }

        public Position GetCoordsFromDirection(Position position, Direction dir, int grid_size)
        {
            Position next_pos = new Position(0, 0);

            switch (dir)
            {
                case Direction.NORTH:
                    next_pos.x = position.x;
                    next_pos.y = position.y - 1;
                    break;
                case Direction.EAST:
                    next_pos.x = position.x + 1;
                    next_pos.y = position.y;
                    break;
                case Direction.SOUTH:
                    next_pos.x = position.x;
                    next_pos.y = position.y + 1;
                    break;
                case Direction.WEST:
                    next_pos.x = position.x - 1;
                    next_pos.y = position.y;
                    break;
            }

            AdjustOOB(ref next_pos, grid_size);

            return next_pos;
        }

        private void AdjustOOB(ref Position next_pos, int grid_size)
        {
            if (next_pos.x >= grid_size) next_pos.x = grid_size - 1;
            if (next_pos.x < 0) next_pos.x = 0;
            if (next_pos.y < 0) next_pos.y = 0;
            if (next_pos.y >= grid_size) next_pos.y = grid_size - 1;
        }

        public bool CompareBoards(Board list, HashSet<Board> searched_boards)
        {
            return searched_boards.Contains(list);
        }

        public bool CompareBoards(Board board, Board goal_board)
        {
            for (int i = 0; i < board.Count; i++)
            {
                for (int j = 0; j < board.Count; j++)
                {
                    if (board[i][j] != goal_board[i][j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Board SimulateMove(Board board, Move move)
        {
            Board temp_board = new Board();
            foreach (List<int> list in board)
            {
                temp_board.Add(new List<int>(list));
            }

            int val1 = temp_board[move.pos1.x][move.pos1.y];
            //int val2 = temp_board[move.pos2.x][move.pos2.y];

            temp_board[move.pos1.x][move.pos1.y] = temp_board[move.pos2.x][move.pos2.y]; ;
            temp_board[move.pos2.x][move.pos2.y] = val1;
            return temp_board;
        }

        internal bool CompareBoardsWithG(AStarNode list, List<AStarNode> open_list)
        {
            foreach(AStarNode node in open_list)
            {
                if(CompareBoards(list.board_, node.board_))
                {
                    if(node.g < list.g)
                        return true;
                    return false;
                }
            }

            return false;
        }

        internal bool CompareBoards(AStarNode node, Dictionary<AStarNode, AStarNode> closed_list)
        {
            if (closed_list.ContainsKey(node))
            {
                if (closed_list[node].g < node.g)
                    return true;
                return false;
            }
            return false;
        }
    } 
}