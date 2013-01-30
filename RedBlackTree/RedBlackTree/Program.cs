using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    class Program
    {

        struct Pair<TKey, TValue> : IComparable<Pair<TKey, TValue>> where TKey : IComparable
        {
            public TKey First { get; set; }
            public TValue Second { get; set; }

            public Pair(TKey f, TValue s)
                : this()
            {
                First = f;
                Second = s;
            }

            public int CompareTo(Pair<TKey, TValue> other)
            {
                return First.CompareTo(other.First);
            }

            public override string ToString()
            {
                return First.ToString() + " " + Second.ToString();
            }
        }

        
        static void Main(string[] args)
        {
            Set<int> s = new Set<int>();
            Set<Pair<int, string>> ss = new Set<Pair<int, string>>();
            SortedDictionary<int, int> D = new SortedDictionary<int, int>();
            for (int i = 1000000 - 1; i >= 0; i--)
            {
                D.Add(i, 23);
            }

            Console.WriteLine("YEAP");
            foreach (var item in D)
            {
                if(item.Key % 100000 == 0)
                Console.WriteLine(item.Key);
            }

            Console.WriteLine(s.Count);
        }
    }
}
