using System;

namespace BankVue
{
    public class StringNode : INode<string>
    {
        public StringNode(String value)
        {
            Value = value;
        }
        public string Value { get; private set; }
        public INode<string> Next { get; set; }
        public override string ToString()
        {
            return Value;
        }
    }
}