using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Model
{
    public class SortedLinkedList<T> : ICollection<T> where T : IComparable<T>
    {
        private Node<T> _head;

        public int Count
        {
            get => _getCount();
        }

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            int? result = _head?.Value.CompareTo(item);

            if (result.HasValue)
            {
                Node<T> previous = null;
                Node<T> current = _head;

                bool foundSpot = false;
                while (!foundSpot && current != null)
                {
                    int compareResult = current.Value.CompareTo(item);

                    if (compareResult <= 0)
                    {
                        var nodeToBeAdded = new Node<T>(item);
                        if (previous != null)
                        {
                            previous.Next = nodeToBeAdded;
                        }
                        else
                        {
                            _head = nodeToBeAdded;
                        }

                        nodeToBeAdded.Next = current;
                        foundSpot = true;
                    }
                    else
                    {
                        if (!current.HasNext)
                        {
                            current.Next = new Node<T>(item);
                            foundSpot = true;
                        }
                    }

                    previous = current;
                    current = current.Next;
                }
            }
            else
            {
                _head = new Node<T>(item);
            }
        }

        public void Clear()
        {
            _head = null;
        }

        public bool Contains(T item)
        {
            bool objectFound = false;
            Node<T> current = _head;

            while (!objectFound && current != null)
            {
                if (current.Value.Equals(item))
                {
                    objectFound = true;
                }
                else
                {
                    current = current.Next;
                }
            }

            return objectFound;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            T[] buffer = new T[this.Count];
            Node<T> current = _head;

            for (int i = 0; i < buffer.Length; i++)
            {
                array[arrayIndex + i] = current.Value;
                if (current.HasNext)
                {
                    current = current.Next;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new SortedLinkedListEnumerator<T>(_head);
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SortedLinkedListEnumerator<T>(_head);
        }

        private int _getCount()
        {
            int counter = 0;
            Node<T> current = _head;
            while (current != null)
            {
                current = current.Next;
                ++counter;
            }

            return counter;
        }
    }
    internal class Node<T>
    {
        public Node<T> Next { get; set; }

        public T Value { get; set; }

        public bool HasNext => (Next != null);

        public Node(T value)
        {
            Value = value;
        }
    }

    internal class SortedLinkedListEnumerator<T> : IEnumerator<T>
    {
        Node<T> _head;
        Node<T> _current;

        internal SortedLinkedListEnumerator(Node<T> head)
        {
            _head = head;
        }

        public T Current => _current.Value;

        object IEnumerator.Current => _current;

        public void Dispose()
        {
            _head = null;
            _current = null;
        }

        public bool MoveNext()
        {
            bool result = false;

            if (_current == null)
            {
                _current = _head;
                result = true;
            }
            else
            {
                if (_current.HasNext)
                {
                    _current = _current.Next;
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public void Reset()
        {
            _current = null;//_head;
        }
    }
}
