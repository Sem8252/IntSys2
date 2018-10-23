using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntSys2
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = Convert.ToInt32(Console.ReadLine());
            State start = new State(size);
            //State finish = new State(size);
            //finish.SoldiersOnRight = finish.SoldiersOnLeft;
            //finish.SoldiersOnLeft = 0;
            List<Path> path = new List<Path>();
            //switch (Console.Read())
            //{
            //    case '1':
            //        path = bypassWide(start, 20);
            //        break;
            //    case '2':
            //        path = bypassDeep(start, 5);
            //        break;
            //}
            path = bypassWide(start, 20);
            var way = GetWay(path, start);
            foreach(var cur in way)
            {
                Console.WriteLine(cur);
            }
        }

        static List<Path> bypassWide(State start, int max)
        {
            List<Path> path = new List<Path>();
            List<State> open = new List<State>();
            List<State> closed = new List<State>();
            open.Add(start);
            int count = 0;
            while(open.Count>0)
            {
                count++;
                if (open[0].SoldiersOnRight == start.SoldiersOnLeft||open[0].Deep==max) break;
                var child = open[0].CreateChild();
                foreach (var current in child)
                {
                    if (!(open.Contains(current) || closed.Contains(current)))
                        open.Add(current);
                    path.Add(new Path(open[0], current));
                    if (current.SoldiersOnRight == start.SoldiersOnLeft)
                        Console.WriteLine("Ать!");
                }
                closed.Add(open[0]);
                open.RemoveAt(0);
            }
            Console.WriteLine(count);
            if (open[0].Deep == max) Console.WriteLine("ОпЪ");
            return path;
        }

        static List<Path> bypassDeep(State start, int max)
        {
            List<Path> path = new List<Path>();
            List<State> open = new List<State>();
            List<State> closed = new List<State>();
            open.Add(start);
            int count = 0;
            while (open.Count > 0)
            {
                count++;
                if (open[0].SoldiersOnRight == start.SoldiersOnLeft || open[0].Deep == max)
                    break;
                var buff = open[0];
                var child = open[0].CreateChild();
                foreach (var current in child)
                {
                    if (!(open.Contains(current) || closed.Contains(current)))
                        open.Insert(0,current);
                    path.Add(new Path(open[0], current));
                    if (current.SoldiersOnRight == start.SoldiersOnLeft)
                        Console.WriteLine("Ать!");
                }
                closed.Add(buff);
                open.Remove(buff);
            }
            Console.WriteLine(count);
            return path;
        }

        static List<string> GetWay(List<Path> path, State start)
        {
            List<string> result = new List<string>();

            var currentUnit = path.Find(
                delegate (Path pt)
                {
                    return pt.Out.SoldiersOnRight == start.SoldiersOnLeft;
                }
                );
            result.Add(string.Format("{0},{1},{2},{3},{4} ",currentUnit.Out.SoldiersOnLeft, currentUnit.Out.ChildOnLeft, 
                currentUnit.Out.SoldiersOnRight, currentUnit.Out.ChildOnRight, currentUnit.Out.Boat));
            result.Add(string.Format("{0},{1},{2},{3},{4} ", currentUnit.In.SoldiersOnLeft, currentUnit.In.ChildOnLeft,
                currentUnit.In.SoldiersOnRight, currentUnit.In.ChildOnRight, currentUnit.In.Boat));
            while (currentUnit.In != start)
            {
                currentUnit = path.Find(delegate (Path pt)
                {
                    return pt.Out == currentUnit.In;
                }
                );
                result.Add(string.Format("{0},{1},{2},{3},{4} ", currentUnit.In.SoldiersOnLeft, currentUnit.In.ChildOnLeft,
                currentUnit.In.SoldiersOnRight, currentUnit.In.ChildOnRight, currentUnit.In.Boat));
            }
            result.Reverse();
            return result;
        }
    }

    class State
    {
        public int SoldiersOnLeft;
        public int SoldiersOnRight;
        public int ChildOnLeft;
        public int ChildOnRight;
        public bool Boat;
        public int Deep;
        public State(int SoldiersOnLeft, int SoldiersOnRight, int ChildOnLeft, int ChildOnRight, bool Boat, int deep)
        {
            this.SoldiersOnLeft = SoldiersOnLeft;
            this.SoldiersOnRight = SoldiersOnRight;
            this.ChildOnLeft = ChildOnLeft;
            this.ChildOnRight = ChildOnRight;
            this.Boat = Boat;
            this.Deep = deep;
        }
        public State(int Soldiers)
        {
            this.SoldiersOnLeft = Soldiers;
            this.SoldiersOnRight = 0;
            this.ChildOnLeft = 2;
            this.ChildOnRight = 0;
            this.Boat = false;
            this.Deep = 0;
        }
        public List<State> CreateChild()
        {
            List<State> Child = new List<State>();
            if (this.ChildOnLeft > 0 && this.Boat == false)
                Child.Add(new State(this.SoldiersOnLeft, this.SoldiersOnRight,  
                this.ChildOnLeft - 1, this.ChildOnRight + 1, true, this.Deep+1));
            if (this.ChildOnLeft == 2 && this.Boat == false)
                Child.Add(new State(this.SoldiersOnLeft, this.SoldiersOnRight,
                this.ChildOnLeft - 2, this.ChildOnRight + 2, true, this.Deep + 1));
            if (this.ChildOnRight > 0 && this.Boat == true)
                Child.Add(new State(this.SoldiersOnLeft, this.SoldiersOnRight,
                this.ChildOnLeft + 1, this.ChildOnRight - 1, false, this.Deep + 1));
            if (this.ChildOnRight == 2 && this.Boat == true)
                Child.Add(new State(this.SoldiersOnLeft, this.SoldiersOnRight,
                this.ChildOnLeft + 2, this.ChildOnRight - 2, false, this.Deep + 1));
            if (this.SoldiersOnLeft > 0 && this.Boat == false)
                Child.Add(new State(this.SoldiersOnLeft - 1, this.SoldiersOnRight + 1,
                this.ChildOnLeft, this.ChildOnRight, true, this.Deep + 1));
            if (this.SoldiersOnRight > 0 && this.Boat == true)
                Child.Add(new State(this.SoldiersOnLeft + 1, this.SoldiersOnRight - 1,
                this.ChildOnLeft, this.ChildOnRight, false, this.Deep + 1));
            return Child;
        }
    }

    class Path
    {
        public State In;
        public State Out;
        public Path(State From, State To)
        {
            In = From;
            Out = To;
        }
    }
}
