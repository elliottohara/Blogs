using System;

namespace BankVue
{
    //this is TOTALLY cheating, but that's what we software guys are supposed to do
    public class CheatingLinkedList<T> : ILinkedList<T>
    {
        public INode<T> Nodes { get; private set; }
        private INode<T> _lastNode;
        private INode<T> _reverse { get; set; }
        public void Add(INode<T> node)
        {
            if (Nodes == null)
            {
                Nodes = node;
                _lastNode = node;
                _reverse = node;
                return;
            }
            
            _lastNode.Next = node;
            _lastNode = node;
            _reverse.Next = _lastNode;
            _reverse = _lastNode;

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
            Nodes = _reverse;
        }
        
    }
}
