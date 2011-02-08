using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace BankVue
{
    [TestFixture]
    public class reversing_Linked_list_tests
    {
        [Test]
        public void simple_linked_list_will_reverse_values()
        {
            when_using_a_SimpleLinkedList();
            
            after_the_list_is_reversed();
            
            the_values_are_reversed();

        }
        [Test]
        public void a_ReversableLinkedList()
        {
            when_using_a_reversable_linked_list();

            after_the_list_is_reversed();

            the_values_are_reversed();   
        }
        public void when_using_a_reversable_linked_list()
        {
            _list = new CheatingLinkedList<string>();
        }
        private void the_values_are_reversed()
        {
            var node = _list.Nodes;
            var index = 0;
            while(node!=null)
            {
                Assert.AreEqual(_expectedValues[index++],node.Value);
                node = node.Next;
            }
        }

        private void after_the_list_is_reversed()
        {
            var startedAt = DateTime.Now;
            _list.Reverse();
            Console.WriteLine("Sorted {0} Nodes in {1} milliseconds",_expectedValues.Count,  DateTime.Now.Subtract(startedAt).TotalMilliseconds);
      
        }

        public void when_using_a_SimpleLinkedList()
        {
            _list = new SimpleLinkedList<String>();
            PopulateNodes();
        }

        private void PopulateNodes()
        {
            _originalValues.ForEach(s => _list.Add(new StringNode(s)));
        }

        private ILinkedList<String> _list;
        private List<String> _originalValues;
        private List<string> _expectedValues;

        [SetUp]
        public void Arrange()
        {
            //_originalValues = new List<String>{"BankVue", "Should", "Hire", "Elliott"};
            // lets check some performance issues
            _originalValues = new List<string>();
            for (var i = 0; i < 1000000; i++ )
            {
                _originalValues.Add(Guid.NewGuid().ToString());
            }
            _list = new CheatingLinkedList<String>();
            _expectedValues = new List<String>(_originalValues);
            _expectedValues.Reverse();

        }
    }
}