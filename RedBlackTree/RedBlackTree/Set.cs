//Copyrighted by Roman.Labish, e-mail: stifffffler@gmail.com
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    
    class Set<T> : IEnumerable<T>, ICollection<T> where T : IComparable<T>
    {
        private enum Colors : byte
        {
            Red,
            Black
        }
        private class RedBlackNode
        {
            public T Key { get; set; }
            public RedBlackNode Left { get; set; }
            public RedBlackNode Right { get; set; }
            public RedBlackNode Parent { get; set; }
            public Colors Color { get; set; }


            public RedBlackNode(T key = default(T), RedBlackNode left = null, RedBlackNode right = null, RedBlackNode parent = null, Colors color = Colors.Black)
            {
                Key = key;
                Left = left;
                Right = right;
                Parent = parent;
                Color = color;
            }
        }

        private RedBlackNode Root;
        private RedBlackNode Nil;
        
        public Set()
        {
            Nil = new RedBlackNode();
            Root = Nil;
        }

        private RedBlackNode TreeMinimum(RedBlackNode x)
        {
            while (x.Left != Nil)
                x = x.Left;
            return x;
        }
        private RedBlackNode TreeMaximum(RedBlackNode x)
        {
            while (x.Right != Nil)
                x = x.Right;
            return x;
        }
        private RedBlackNode TreeSuccessor(RedBlackNode x)
        {
            if (x.Right != Nil)
                return TreeMinimum(x.Right);

            RedBlackNode y = x.Parent;

            while (y != Nil && x == y.Right)
            {
                x = y;
                y = y.Parent;
            }

            return y;

        }
        private RedBlackNode IterativeTreeSearch(RedBlackNode x, T key)
        {
            while (x != Nil && x.Key.CompareTo(key) != 0)
            {
                if (key.CompareTo(x.Key) < 0)
                    x = x.Left;
                else
                    x = x.Right;
            }
            return x;
        }

        #region Test

        public int Find(T key)
        {
            int cnt = 0;
            RedBlackNode x = Root;
            while (x != Nil && x.Key.CompareTo(key) != 0)
            {
                ++cnt;
                if (key.CompareTo(x.Key) < 0)
                    x = x.Left;
                else
                    x = x.Right;
            }
            return cnt;
        }
        
        #endregion
        

        private void LeftRotate(RedBlackNode x)
        {
            RedBlackNode y = x.Right;
            x.Right = y.Left;

            if (y.Left != Nil)
                y.Left.Parent = x;

            y.Parent = x.Parent;

            if (x.Parent == Nil)
                Root = y;
            else if (x == x.Parent.Left)
                x.Parent.Left = y;
            else
                x.Parent.Right = y;

            y.Left = x;
            x.Parent = y;
        }
        private void RightRotate(RedBlackNode x)
        {
            RedBlackNode y = x.Left;
            x.Left = y.Right;

            if (y.Right != Nil)
                y.Right.Parent = x;

            y.Parent = x.Parent;

            if (x.Parent == Nil)
                Root = y;
            else if (x == x.Parent.Left)
                x.Parent.Left = y;
            else
                x.Parent.Right = y;

            y.Right = x;
            x.Parent = y;
        }

        private void RBInsert(RedBlackNode z)
        {
            ++Count;
            RedBlackNode y = Nil;
            RedBlackNode x = Root;

            while (x != Nil)
            {
                y = x;
                if (z.Key.CompareTo(x.Key) == 0)
                {
                    --Count;
                    return;
                }
                if (z.Key.CompareTo(x.Key) < 0)
                    x = x.Left;
                else
                    x = x.Right;
            }

            z.Parent = y;

            if (y == Nil)
                Root = z;
            else if (z.Key.CompareTo(y.Key) < 0)
                y.Left = z;
            else
                y.Right = z;

            z.Left = Nil;
            z.Right = Nil;
            z.Color = Colors.Red;

            RBInsertFixup(z);
        }
        private void RBInsertFixup(RedBlackNode x)
        {
            RedBlackNode y = Nil;
            while (x != Root && x.Parent.Color == Colors.Red)
            {
                if (x.Parent == x.Parent.Parent.Left)
                {
                    y = x.Parent.Parent.Right;
                    if (y.Color == Colors.Red)
                    {
                        x.Parent.Color = Colors.Black;
                        y.Color = Colors.Black;
                        x.Parent.Parent.Color = Colors.Red;
                        x = x.Parent.Parent;
                    }
                    else
                    {
                        if (x == x.Parent.Right)
                        {
                            x = x.Parent;
                            LeftRotate(x);
                        }

                        x.Parent.Color = Colors.Black;
                        x.Parent.Parent.Color = Colors.Red;
                        RightRotate(x.Parent.Parent);
                    }
                }
                else
                {
                    y = x.Parent.Parent.Left;
                    if (y.Color == Colors.Red)
                    {
                        x.Parent.Color = Colors.Black;
                        y.Color = Colors.Black;
                        x.Parent.Parent.Color = Colors.Red;
                        x = x.Parent.Parent;
                    }
                    else
                    {
                        if (x == x.Parent.Left)
                        {
                            x = x.Parent;
                            RightRotate(x);
                        }

                        x.Parent.Color = Colors.Black;
                        x.Parent.Parent.Color = Colors.Red;
                        LeftRotate(x.Parent.Parent);
                    }
                }
                
            }
            Root.Color = Colors.Black;
        }
        
        private void RBDelete(RedBlackNode z)
        {
            --Count;
            RedBlackNode y = Nil, x = Nil;
            if (z.Left == Nil || z.Right == Nil)
                y = z;
            else
                y = TreeSuccessor(z);

            if (y.Left != Nil)
                x = y.Left;
            else
                x = y.Right;

            //if(x != Nil)
                x.Parent = y.Parent;

            if (y.Parent == Nil)
                Root = x;
            else if (y == y.Parent.Left)
                y.Parent.Left = x;
            else
                y.Parent.Right = x;

            if (y != z)
                z.Key = y.Key;

            if (y.Color == Colors.Black)
                RBDeleteFixup(x);
        }
        private void RBDeleteFixup(RedBlackNode x)
        {    
            while (x != Root && x.Color == Colors.Black)
            {
                RedBlackNode w = Nil;
                if (x == x.Parent.Left)
                {
                    w = x.Parent.Right;
                    if (w.Color == Colors.Red)
                    {
                        w.Color = Colors.Black;
                        x.Parent.Color = Colors.Red;
                        LeftRotate(x.Parent);
                        w = x.Parent.Right;
                    }
                    if (w.Left.Color == Colors.Black && w.Right.Color == Colors.Black)
                    {
                        w.Color = Colors.Red;
                        x = x.Parent;
                    }
                    else 
                    {
                        if (w.Right.Color == Colors.Black)
                        {
                            w.Left.Color = Colors.Black;
                            w.Color = Colors.Red;
                            RightRotate(w);
                            w = x.Parent.Right;
                        }
                        w.Color = x.Parent.Color;
                        x.Parent.Color = Colors.Black;
                        w.Right.Color = Colors.Black;
                        LeftRotate(x.Parent);
                        x = Root;
                    }
                }
                else
                {
                    w = x.Parent.Left;
                    if (w.Color == Colors.Red)
                    {
                        w.Color = Colors.Black;
                        x.Parent.Color = Colors.Red;
                        RightRotate(x.Parent);
                        w = x.Parent.Left;
                    }
                    if (w.Right.Color == Colors.Black && w.Left.Color == Colors.Black)
                    {
                        w.Color = Colors.Red;
                        x = x.Parent;
                    }
                    else
                    {
                        if (w.Left.Color == Colors.Black)
                        {
                            w.Right.Color = Colors.Black;
                            w.Color = Colors.Red;
                            LeftRotate(w);
                            w = x.Parent.Left;
                        }
                        w.Color = x.Parent.Color;
                        x.Parent.Color = Colors.Black;
                        w.Left.Color = Colors.Black;
                        RightRotate(x.Parent);
                        x = Root;
                    }
                }
            }
            x.Color = Colors.Black;
        }

        public T Min
        {
            get { return TreeMinimum(Root).Key; }
        }
        public T Max
        {
            get { return TreeMaximum(Root).Key; }
        }
        public int Count { get; private set; }

        #region IEnumerable

        private Queue<T> list;
        private void Traverse(RedBlackNode x)
        {
            if (x.Left != Nil)
                Traverse(x.Left);
            if (x != Nil)
                list.Enqueue(x.Key);
            if (x.Right != Nil)
                Traverse(x.Right);

        }

        public IEnumerator<T> GetEnumerator()
        {
            list = new Queue<T>();
            Traverse(Root);
            while (list.Count > 0)
            {
                yield return list.Dequeue();
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        } 

        #endregion

        #region ICollection
        
        public void Add(T item)
        {
            //if(!Contains(item))
                RBInsert(new RedBlackNode(key: item));
        }

        public void Clear()
        {
            Root.Left = Nil;
            Root.Right = Nil;
            Root = Nil;
            Count = 0;
        }

        public bool Contains(T item)
        {
            RedBlackNode isInSet = IterativeTreeSearch(Root, item);

            if (isInSet != Nil)
                return true;

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            RedBlackNode forDelete = IterativeTreeSearch(Root, item);
            if (forDelete != Nil)
            {
                RBDelete(forDelete);
                return true;
            }
            return false;
        }
        
        #endregion
    }
}
