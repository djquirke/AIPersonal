using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Board = System.Collections.Generic.List<System.Collections.Generic.List<int>>;

namespace _8_15_puzzle
{
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
            int hash = 1234567;
            foreach(List<int> list2 in list)
            {
                foreach(int i in list2)
                {
                    hash = hash * 37 + i.GetHashCode();
                }
            }
            return hash;
        }

    }

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

    class AI_BFS
    {
        int size_;
        public int searched_board_count;

        public bool Run(Board board, Position blank_pos, Board goal_board, ref List<Move> moves, int grid_size)
        {
            if (CompareBoards(board, goal_board))
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
                    if (CompareBoards(child.board, searched_boards)) continue;
                    else if (CompareBoards(child.board, goal_board)) { sw.Stop(); goal_reached = true; goal = child; }
                    else { search.Enqueue(child); searched_boards.Add(child.board); }
                }
            }

            if (!goal_reached)
                return false;

            searched_board_count = searched_boards.Count;
            TraverseTree(ref moves, goal);

            return true;
        }

        private bool CompareBoards(Board list, HashSet<Board> searched_boards)
        {
            return searched_boards.Contains(list);
        }

        private void TraverseTree(ref List<Move> moves, BFSNode node)
        {
            moves.Add(node.move);

            if (node.parent == null) return;

            TraverseTree(ref moves, node.parent);
        }

        private bool CompareBoards(Board board, Board goal_board)
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
                Move move = new Move(next_pos, node_to_expand.blank_pos);

                if(move.pos1.y == -1)
                {
                    AdjustOOB(ref next_pos);
                }

                if (next_pos.x == node_to_expand.blank_pos.x && next_pos.y == node_to_expand.blank_pos.y) continue;

                Board nextBoard = SimulateMove(node_to_expand.board, move);//, node_to_expand.blank_pos, next_pos);

                BFSNode node = new BFSNode(nextBoard, next_pos, node_to_expand, move);
                children.Add(node);
            }

            return children;
        }

        private Board SimulateMove(Board board, Move move)
        {
            Board temp_board = new Board();
            foreach (List<int> list in board)
            {
                temp_board.Add(new List<int>(list));
            }

            int val1 = temp_board[move.pos1.x][move.pos1.y];
            int val2 = temp_board[move.pos2.x][move.pos2.y];

            temp_board[move.pos1.x][move.pos1.y] = val2;
            temp_board[move.pos2.x][move.pos2.y] = val1;
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
