using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_15_puzzle
{
    class BFSNode
    {
        public Board board;
        public Position blank_pos;
        public BFSNode parent;
        public Move move;

        public BFSNode()
        {
            board = new Board();
            blank_pos = new Position();
            parent = null;
            move = new Move();
        }

        public BFSNode(BFSNode node)
        {
            board = node.board;
            parent = node.parent;
            blank_pos = node.blank_pos;
            move = node.move;
        }
        
        public BFSNode(Board nextBoard, Position blank_pos, BFSNode parent = null, Move move = null)
        {
            board = nextBoard;
            this.blank_pos = blank_pos;
            this.parent = parent;
            this.move = move;
        }
    }

    class AI_BFS
    {
        int size_;
        HelperFunctions hf_;

        public bool Run(Board board, Position blank_pos, Board goal_board, ref List<Move> moves, int grid_size, ref int boards_searched)
        {
            hf_ = new HelperFunctions();

            if (hf_.CompareBoards(board, goal_board))
                return true; //start board is goal

            size_ = grid_size;

            BFSNode root = new BFSNode(board, blank_pos);
            BFSNode goal = new BFSNode();
            
            HashSet<Board> searched_boards = new HashSet<Board>(new BoardEqualityComparer());
            Queue<BFSNode> search = new Queue<BFSNode>();
            search.Enqueue(root);

            bool goal_reached = false;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            while(search.Count > 0 && !goal_reached && sw.ElapsedMilliseconds < 10000)
            {
                BFSNode node_to_expand = new BFSNode(search.Dequeue());

                List<BFSNode> children = GetChildren(node_to_expand);

                foreach(BFSNode child in children)
                {
                    if (hf_.CompareBoards(child.board, searched_boards)) continue;
                    else if (child.board.Equals(goal_board)) { sw.Stop(); goal_reached = true; goal = child; }
                    else { search.Enqueue(child); searched_boards.Add(child.board); }
                }
            }

            boards_searched = searched_boards.Count;

            if (goal_reached)  TraverseTree(ref moves, goal);

            searched_boards.Clear();
            search.Clear();
            GC.Collect();

            return goal_reached;
        }

        private List<BFSNode> GetChildren(BFSNode node_to_expand)
        {
            List<BFSNode> children = new List<BFSNode>();

            foreach(Direction dir in Enum.GetValues(typeof(Direction)))
            {
                Position next_pos = hf_.GetCoordsFromDirection(node_to_expand.blank_pos, dir, size_);
                Move move = new Move(next_pos, node_to_expand.blank_pos);

                if (next_pos.x == node_to_expand.blank_pos.x && next_pos.y == node_to_expand.blank_pos.y) continue;

                Board nextBoard = hf_.SimulateMove(node_to_expand.board, move);

                BFSNode node = new BFSNode(nextBoard, next_pos, node_to_expand, move);
                children.Add(node);
            }

            return children;
        }

        private void TraverseTree(ref List<Move> moves, BFSNode node)
        {
            moves.Add(node.move);

            if (node.parent == null) return;

            TraverseTree(ref moves, node.parent);
        }
    }
}
