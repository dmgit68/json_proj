using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WindowsFormsApplication1
{


    public class BlockingQueue<T>
    {
        internal class Node
        {
            internal T _item;
            internal Node _next;

            public Node() { }
            public Node(T item, Node next) { _item = item; _next = next; }
        }

        private Object _lockObj;
        private Node _tail;
        private Node _head;

        public BlockingQueue()
        {
            _lockObj = new Object();
            _head = _tail = new Node(default(T), null);
        }

        public void Enqueue(T item)
        {
            Node newNode = new Node(item, null);

            lock (_lockObj)
            {
                _tail._next = newNode;
                _tail = newNode;

                Monitor.Pulse(_lockObj);
            }
        }

        public T Dequeue()
        {
            T retItem;

            lock (_lockObj)
            {
                while (_head._next == null)
                    Monitor.Wait(_lockObj);

                retItem = _head._next._item;
                _head = _head._next;

                return retItem;
            }
        }
    }
}
