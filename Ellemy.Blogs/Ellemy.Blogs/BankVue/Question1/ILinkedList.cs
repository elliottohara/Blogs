namespace BankVue
{
    public interface ILinkedList<T>
    {
        INode<T> Nodes { get; }
        void Add(INode<T> node);
        void Reverse();
    }
}