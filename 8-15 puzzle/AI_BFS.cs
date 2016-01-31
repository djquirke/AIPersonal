using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_15_puzzle
{

    class BFSNode
    {
        public List<List<int>> board;
        public Position blank_pos;
        public BFSNode parent;

        public BFSNode(List<List<int>> board, Position blank)
        {
            this.board = board;
            blank_pos = blank;
        }
        public BFSNode(BFSNode node)
        {
            board = node.board;
            parent = node.parent;
            blank_pos = node.blank_pos;
        }
        public BFSNode()
        {
            board = new List<List<int>>();
            blank_pos = new Position();
            parent = null;
        }
    }

    struct Move
    {
        public Position blank;
        public Position other;
    }

    class AI_BFS
    {
        int size_;
        List<List<int>> goal_board;

        public bool Run(List<List<int>> board, Position blank_pos, ref List<Position> moves, int grid_size)
        {
            goal_board = new List<List<int>>();

            size_ = grid_size;
            int counter = 0;
            for (int i = 0; i < size_; i++)
            {
                List<int> temp_list = new List<int>();

                for (int j = 0; j < size_; j++)
                {
                    int num = ++counter;

                    if (counter == size_ * size_)
                    {
                        counter = 0;
                    }

                    temp_list.Add(counter);
                }
                goal_board.Add(temp_list);
            }


            if (CompareBoards(board, goal_board))
                return true; //start board is goal

            BFSNode root = new BFSNode(board, blank_pos);
            root.parent = null;
            BFSNode goal = new BFSNode();
            
            List<BFSNode> search = new List<BFSNode>();
            search.Add(root);

            bool goal_reached = false;

            while(search.Count > 0 && !goal_reached)
            {
                BFSNode node_to_expand = new BFSNode(search[0]);
                search.RemoveAt(0);

                List<BFSNode> children = GetChildren(node_to_expand);

                foreach(BFSNode child in children)
                {
                    if(CompareBoards(child.board, goal_board))
                    {
                        goal_reached = true;
                        goal = child;
                    }
                    else 
                    {
                        search.Add(child);
                    }
                }
            }

            return goal_reached;
        }

        private bool CompareBoards(List<List<int>> board, List<List<int>> goal_board)
        { 
            for(int i = 0; i < board.Count; i++)
            {
                for(int j = 0; j < board.Count; j++)
                {
                    if(board[i][j] != goal_board[i][j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private List<BFSNode> GetChildren(BFSNode node_to_expand)
        {
            List<BFSNode> children = new List<BFSNode>();

            foreach(Direction dir in Enum.GetValues(typeof(Direction)))
            {
                Position next_pos = GetCoordsFromDirection(node_to_expand.blank_pos, dir);

                if (next_pos.x == node_to_expand.blank_pos.x && next_pos.y == node_to_expand.blank_pos.y) continue;

                List<List<int>> nextBoard = SimulateMove(node_to_expand.board, node_to_expand.blank_pos, next_pos);

                BFSNode node = new BFSNode(nextBoard, next_pos);
                node.parent = node_to_expand;
                children.Add(node);
            }

            return children;
        }

        private List<List<int>> SimulateMove(List<List<int>> board, Position blank, Position other)
        {
            List<List<int>> temp_board = new List<List<int>>();
            foreach(List<int> list in board)
            {
                temp_board.Add(new List<int>(list));
            }
            temp_board[blank.x][blank.y] = board[other.x][other.y];
            temp_board[other.x][other.y] = 0;

            return temp_board;
        }

        private Position GetCoordsFromDirection(Position position, Direction dir)
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

            AdjustOOB(ref next_pos);

            return next_pos;
        }

        private void AdjustOOB(ref Position next_pos)
        {
            if (next_pos.x >= size_) next_pos.x = size_ - 1;
            if (next_pos.x < 0) next_pos.x = 0;
            if (next_pos.y < 0) next_pos.y = 0;
            if (next_pos.y >= size_) next_pos.y = size_ - 1;
        }
    }
}
