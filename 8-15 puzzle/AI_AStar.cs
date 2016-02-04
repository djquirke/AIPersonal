using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Board = System.Collections.Generic.List<System.Collections.Generic.List<int>>;

namespace _8_15_puzzle
{
    class AStarNode
    {
        public AStarNode parent_;
        public Board board_;
        public Position blank_pos_;
        public Move move_;
        public float f, g, h;

        public AStarNode()
        {
            parent_ = null;
            board_ = new Board();
            blank_pos_ = new Position();
            move_ = new Move();
        }

        public AStarNode(Board board, AStarNode parent, Position blank, Move move)
        {
            parent_ = parent;
            board_ = board;
            blank_pos_ = blank;
            move_ = move;
        }
    }

    class AI_AStar
    {
        int size_;
        HelperFunctions hf_;

        public bool Run(Board board, Position blank_pos, Board goal_board, ref List<Move> moves, int grid_size, ref int boards_searched)
        {
            hf_ = new HelperFunctions();

            if (hf_.CompareBoards(board, goal_board))
                return true;

            size_ = grid_size;

            AStarNode root = new AStarNode(board, null, blank_pos, null);
            AStarNode goal = new AStarNode(goal_board, null, null, null);

            List<AStarNode> open_list = new List<AStarNode>();
            Dictionary<AStarNode, AStarNode> closed_list = new Dictionary<AStarNode, AStarNode>();
            
            open_list.Add(root);

            bool goal_found = false;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            while(open_list.Count > 0 && !goal_found) // && sw.ElapsedMilliseconds < 10000)
            {
                boards_searched++;

                AStarNode next_node = GetNextNode(open_list); //inefficient

                if (hf_.CompareBoards(next_node.board_, goal.board_))
                {
                    sw.Stop();
                    goal_found = true;
                    goal = next_node;
                    continue;
                }

                open_list.Remove(next_node);
                closed_list.Add(next_node, next_node);

                List<AStarNode> successors = GetChildren(next_node);

                foreach(AStarNode successor in successors)
                {
                    if (hf_.CompareBoards(successor, closed_list)) continue;

                    successor.g = next_node.g + 1;

                    if (hf_.CompareBoardsWithG(successor, open_list)) continue; //terribly inefficient

                    successor.h = ManhattanDistance(successor.board_);
                    successor.f = successor.g + successor.h;
                    open_list.Add(successor);
                }
            }
            
            if(goal_found)
            {
                TraverseTree(ref moves, goal);
                return true;
            }

            return false;
        }

        private float ManhattanDistance(Board other)
        {
            int manhattanDistanceSum = 0;

            for(int i = 0; i < size_; i++)
            {
                for(int j = 0; j < size_; j++)
                {
                    int value = other[i][j];
                    if(value != 0) //don't run MD for 0
                    {
                        int target_x = (value - 1) / size_;
                        int target_y = (value - 1) % size_;

                        int dx = i - target_x;
                        int dy = j - target_y;

                        manhattanDistanceSum += Math.Abs(dx) + Math.Abs(dy);
                    }
                }
            }

            return manhattanDistanceSum;
        }

        private AStarNode GetNextNode(List<AStarNode> list)
        {
            float lowest = float.PositiveInfinity;
            AStarNode next = new AStarNode();

            foreach(AStarNode node in list)
            {
                if(node.f < lowest)
                {
                    lowest = node.f;
                    next = node;
                }
            }

            return next;
        }

        private List<AStarNode> GetChildren(AStarNode node_to_expand)
        {
            List<AStarNode> children = new List<AStarNode>();

            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                Position next_pos = hf_.GetCoordsFromDirection(node_to_expand.blank_pos_, dir, size_);
                Move move = new Move(next_pos, node_to_expand.blank_pos_);

                if (next_pos.x == node_to_expand.blank_pos_.x && next_pos.y == node_to_expand.blank_pos_.y) continue;

                Board nextBoard = hf_.SimulateMove(node_to_expand.board_, move);

                AStarNode node = new AStarNode(nextBoard, node_to_expand, next_pos, move);
                children.Add(node);
            }

            return children;
        }

        private void TraverseTree(ref List<Move> moves, AStarNode node)
        {
            moves.Add(node.move_);

            if (node.parent_ == null) return;

            TraverseTree(ref moves, node.parent_);
        }
    }
}
