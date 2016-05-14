using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_15_puzzle
{
    class PatternNode
    {
        //what tiles it represents (coordinates)
        List<List<bool>> tile_space;
        //values of said tiles
        List<List<int>> tile_values;
        //where the blank tile is
        Position blank_tile;
        //heuristic
        float h;
        //parent
        public PatternNode parent = null;

        public PatternNode(List<List<bool>> tile_space, List<List<int>> tile_values, Position blank, float h)
        {
            this.tile_space = tile_space;
            this.tile_values = tile_values;
            blank_tile = blank;
            this.h = h;
        }

        public override bool Equals(object obj)
        {
            PatternNode temp = obj as PatternNode;
            for (int i = 0; i < tile_values.Count; i++)
            {
                if (!tile_values[i].SequenceEqual(temp.tile_values[i])) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 19;

            foreach (List<int> list in tile_values)
            {
                foreach (int i in list)
                {
                    hash = hash * 31 + i.GetHashCode();
                }
            }

            return hash;
        }

    }

    class PatternDatabase
    {
        private static PatternDatabase instance;
        private List<List<bool>> pattern1, pattern2, pattern3;
        private Dictionary<List<List<int>>, PatternNode> database;

        private PatternDatabase() { }

        public static PatternDatabase Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new PatternDatabase();
                }
                return instance;
            }
        }

        private void Initialise()
        {
            database = new Dictionary<List<List<int>>, PatternNode>();
            //initialise 3 pattern chunks
            Pattern1();
            Pattern2();
            Pattern3();
        }

        private void Pattern3()
        {
            //00,00,00,00
            //00,00,00,00
            //00,10,11,12
            //00,14,15,00

            pattern3 = new List<List<bool>>();
            List<bool> temp = new List<bool>();
            List<List<int>> values = new List<List<int>>();
            List<int> temp_values = new List<int>();

            temp.Add(false);//0,0
            temp_values.Add(0);
            temp.Add(false);//0,1
            temp_values.Add(0);
            temp.Add(false);//0,2
            temp_values.Add(0);
            temp.Add(false);//0,3
            temp_values.Add(0);
            pattern3.Add(temp);
            values.Add(temp_values);

            List<bool> temp2 = new List<bool>();
            List<int> temp_values2 = new List<int>();
            temp2.Add(false);//1,0
            temp_values2.Add(0);
            temp2.Add(false);//1,1
            temp_values2.Add(0);
            temp2.Add(false);//1,2
            temp_values2.Add(0);
            temp2.Add(false);//1,3
            temp_values2.Add(0);
            pattern1.Add(temp2);
            values.Add(temp_values2);

            List<bool> temp3 = new List<bool>();
            List<int> temp_values3 = new List<int>();
            temp3.Add(false);//2,0
            temp_values3.Add(0);
            temp3.Add(true);//2,1
            temp_values3.Add(10);
            temp3.Add(true);//2,2
            temp_values3.Add(11);
            temp3.Add(true);//2,3
            temp_values3.Add(12);
            pattern1.Add(temp3);
            values.Add(temp_values3);

            List<bool> temp4 = new List<bool>();
            List<int> temp_values4 = new List<int>();
            temp4.Add(false);//3,0
            temp_values4.Add(0);
            temp4.Add(true);//3,1
            temp_values4.Add(14);
            temp4.Add(true);//3,2
            temp_values4.Add(15);
            temp4.Add(false);//3,3
            temp_values4.Add(0);
            pattern1.Add(temp4);
            values.Add(temp_values4);

            PatternNode pn = new PatternNode(pattern3, values, new Position(3, 3), 0.0f);
            database.Add(values, pn);
        }

        private void Pattern2()
        {
            //00,00,03,04
            //00,06,07,08
            //00,00,00,00
            //00,00,00,00

            pattern2 = new List<List<bool>>();
            List<bool> temp = new List<bool>();
            List<List<int>> values = new List<List<int>>();
            List<int> temp_values = new List<int>();

            temp.Add(false);//0,0
            temp_values.Add(0);
            temp.Add(false);//0,1
            temp_values.Add(0);
            temp.Add(true);//0,2
            temp_values.Add(3);
            temp.Add(true);//0,3
            temp_values.Add(4);
            pattern2.Add(temp);
            values.Add(temp_values);

            List<bool> temp2 = new List<bool>();
            List<int> temp_values2 = new List<int>();
            temp2.Add(false);//1,0
            temp_values2.Add(0);
            temp2.Add(true);//1,1
            temp_values2.Add(6);
            temp2.Add(true);//1,2
            temp_values2.Add(7);
            temp2.Add(true);//1,3
            temp_values2.Add(8);
            pattern1.Add(temp2);
            values.Add(temp_values2);

            List<bool> temp3 = new List<bool>();
            List<int> temp_values3 = new List<int>();
            temp3.Add(false);//2,0
            temp_values3.Add(0);
            temp3.Add(false);//2,1
            temp_values3.Add(0);
            temp3.Add(false);//2,2
            temp_values3.Add(0);
            temp3.Add(false);//2,3
            temp_values3.Add(0);
            pattern1.Add(temp3);
            values.Add(temp_values3);

            List<bool> temp4 = new List<bool>();
            List<int> temp_values4 = new List<int>();
            temp4.Add(false);//3,0
            temp_values4.Add(0);
            temp4.Add(false);//3,1
            temp_values4.Add(0);
            temp4.Add(false);//3,2
            temp_values4.Add(0);
            temp4.Add(false);//3,3
            temp_values4.Add(0);
            pattern1.Add(temp4);
            values.Add(temp_values4);

            PatternNode pn = new PatternNode(pattern2, values, new Position(3, 3), 0.0f);
            database.Add(values, pn);
        }

        private void Pattern1()
        {
            //01,02,00,00
            //05,00,00,00
            //09,00,00,00
            //12,00,00,00

            pattern1 = new List<List<bool>>();
            List<bool> temp = new List<bool>();
            List<List<int>> values = new List<List<int>>();
            List<int> temp_values = new List<int>();

            temp.Add(true);//0,0
            temp_values.Add(1);
            temp.Add(true);//0,1
            temp_values.Add(2);
            temp.Add(false);//0,2
            temp_values.Add(0);
            temp.Add(false);//0,3
            temp_values.Add(0);
            pattern1.Add(temp);
            values.Add(temp_values);

            List<bool> temp2 = new List<bool>();
            List<int> temp_values2 = new List<int>();
            temp2.Add(true);//1,0
            temp_values2.Add(5);
            temp2.Add(false);//1,1
            temp_values2.Add(0);
            temp2.Add(false);//1,2
            temp_values2.Add(0);
            temp2.Add(false);//1,3
            temp_values2.Add(0);
            pattern1.Add(temp2);
            values.Add(temp_values2);

            List<bool> temp3 = new List<bool>();
            List<int> temp_values3 = new List<int>();
            temp3.Add(true);//2,0
            temp_values3.Add(9);
            temp3.Add(false);//2,1
            temp_values3.Add(0);
            temp3.Add(false);//2,2
            temp_values3.Add(0);
            temp3.Add(false);//2,3
            temp_values3.Add(0);
            pattern1.Add(temp3);
            values.Add(temp_values3);

            List<bool> temp4 = new List<bool>();
            List<int> temp_values4 = new List<int>();
            temp4.Add(true);//3,0
            temp_values4.Add(13);
            temp4.Add(false);//3,1
            temp_values4.Add(0);
            temp4.Add(false);//3,2
            temp_values4.Add(0);
            temp4.Add(false);//3,3
            temp_values4.Add(0);
            pattern1.Add(temp4);
            values.Add(temp_values4);

            PatternNode pn = new PatternNode(pattern1, values, new Position(3, 3), 0.0f);
            database.Add(values, pn);
        }

        public void Build()
        {
            Initialise();

            //generate goal state
            Board board_state = new Board();
            Position blank_pos;
            int counter = 0;
            for(int i = 0; i < 4; i++)
            {
                List<int> temp = new List<int>();
                for(int j = 0; j < 4; j++)
                {
                    counter++;
                    if (counter == 16)
                    {
                        counter = 0;
                        blank_pos = new Position(3, 3);
                    }
                    temp.Add(counter);
                }
                board_state.board.Add(temp);
            }

            //run BFS in reverse from goal state to generate pattern database

            //Create Hashset for searched boards
            //Create FIFO Queue for successor nodes

            //While successor nodes available
                //Get next node to expand, NEXT
                //Generate its children

                //For each child
                    //if it has been searched already, discard
                    //else
                        //add child board to queue
                        //Find out which pattern was affected by change from NEXT to child
                        //create a new PatternNode for that pattern only
                        //set new PatternNode parent to previous board that affected this pattern

        }
        



    }
}
