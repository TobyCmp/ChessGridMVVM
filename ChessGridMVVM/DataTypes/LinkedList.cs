using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGridMVVM.DataTypes
{
    public class DoublyLinkedList<T>
    {
        public int count { get; private set; }
        private Node<T> head { get; set; }
        private Node<T> tail { get; set; }


        public DoublyLinkedList(T data)
        {
            head = new Node<T>(data);
            tail = head;
            count = 1;
        }


        public void AddFirst(T data)
        {
            var newNode = new Node<T>(data);
            head.Next = newNode;
            newNode.Previous = head;
            head = newNode;
            count++;
        }

        public void AddLast(T data)
        {
            var newNode = new Node<T>(data);
            tail.Previous = newNode;
            newNode.Next = tail;
            tail = newNode;
            count++;
        }

        public Node<T> RemoveLast()
        {
            var temp = tail;
            tail = tail.Next;
            return temp;
        }

        public Node<T> RemoveFirst()
        {
            var temp = head;
            head = head.Previous;
            return temp;
        }

    }
}
