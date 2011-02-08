using System;

namespace BankVue
{
    public class SimpleLinkedList<T> : ILinkedList<T>
    {
        public INode<T> Nodes { get; private set; }
        private INode<T> _lastNode;

        public void Add(INode<T> node)
        {
            if (Nodes == null)
            {
                Nodes = node;
                _lastNode = node;
                return;
            }

            _lastNode.Next = node;
            _lastNode = node;
        }

        public void Print()
        {
            var n = Nodes;
            while (n != null)
            {
                Console.WriteLine(n.Value);
                n = n.Next;
            }
        }

        public void Reverse()
        {
            //This is the simplest way I can think of to solve this problem
            if (Nodes == null || Nodes.Next == null)
                return;
            
            var current = Nodes;
            var next = Nodes.Next;
            INode<T> previous = null;
            
            while (current != null)
            {
                current.Next = previous;

                if (next == null)
                    break;

                previous = current;
                current = next;
                next = next.Next;
            }
            Nodes = current;
        }
    }
}
