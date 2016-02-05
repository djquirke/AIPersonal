using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_15_puzzle
{
    class Board
    {
        public List<List<int>> board;

        public Board()
        {
            board = new List<List<int>>();
        }

        public override bool Equals(object obj)
        {
            Board temp = obj as Board;
            for (int i = 0; i < board.Count; i++)
            {
                if (!board[i].SequenceEqual(temp.board[i])) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 19;

            foreach(List<int> list in board)
            {
                foreach(int i in list)
                {
                    hash = hash * 31 + i.GetHashCode();
                }
            }

            return hash;
        }
    }
}
