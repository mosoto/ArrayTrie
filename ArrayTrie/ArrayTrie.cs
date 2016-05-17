using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayTrie
{

    class Node<T>
    {
        public int Level { get; set; }
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }
        public T Value { get; set; }
    }

    /// <summary>
    /// Trie based vector supporting indexing/add/remove operations.
    /// The vector is immutable, thus adding and removing values return new vectors which share nodes with the 
    /// original vector to reduce space used.
    /// </summary>
    public class ArrayTrie<T> : IArrayTrie<T>
    {
        private Node<T> Root = null;

        public T this[int index]
        {
            get { return GetItem(index); }
        }

        public int Count { get; private set; }

        public T GetItem(int index)
        {
            if (index >= Count)
            {
                throw new IndexOutOfRangeException();
            }

            return GetItemFromTrie(Root, index);
        }

        private T GetItemFromTrie(Node<T> node, int index)
        {
            T value;

            if (node.Level == 0)
            {
                value = node.Value;
            }
            else
            {
                int maxCount = (int) Math.Pow(2, node.Level);

                if (index < maxCount/2)
                {
                    value = GetItemFromTrie(node.Left, index);
                }
                else
                {
                    value = GetItemFromTrie(node.Right, index - maxCount/2);
                }
            }

            return value;
        }

        public IArrayTrie<T> Push(T value)
        {
            var newTrie = new ArrayTrie<T>();
            newTrie.Count = Count + 1;
            newTrie.Root = InsertIntoTrie(Root, value, Count);

            return newTrie;
        }

        private Node<T> InsertIntoTrie(Node<T> node, T value, int position)
        {
            Node<T> newNode = new Node<T>();

            if (node == null)
            {
                newNode.Level = 0;
                newNode.Value = value;
                return newNode;
            }

            int maxCount = (int) Math.Pow(2, node.Level);

            if (position >= maxCount)
            {
                // The trie is full, need to add a root node
                newNode.Level = node.Level + 1;
                newNode.Left = node;
                newNode.Right = InsertIntoTrie(null, value, position);
            }
            else
            {
                newNode.Level = node.Level;
                newNode.Left = node.Left;
                newNode.Right = InsertIntoTrie(node.Right, value, position - maxCount/2);
            }

            return newNode;
        }

        public IArrayTrie<T> Pop(out T value)
        {
            if (Count == 0)
                throw new InvalidOperationException("Attempted to pop an empty array");

            var newTrie = new ArrayTrie<T>();
            newTrie.Count = Count - 1;
            newTrie.Root = RemoveFromTrie(Root, Count - 1);

            value = GetItem(Count - 1);

            return newTrie;
        }

        private Node<T> RemoveFromTrie(Node<T> node, int position)
        {
            Node<T> newNode = null;

            if (node.Level != 0)
            {
                newNode = new Node<T>();
                int maxCount = (int) Math.Pow(2, node.Level);

                var rightNode = RemoveFromTrie(node.Right, position - maxCount / 2);

                if (rightNode == null)
                {
                    newNode = node.Left;
                }
                else
                {
                    newNode.Left = node.Left;
                    newNode.Level = node.Level;
                    newNode.Right = rightNode;
                }
            }

            return newNode;
        } 

        public IEnumerator<T> GetEnumerator()
        {
            return EnumerateNodes(Root).GetEnumerator();
        }
   
        private IEnumerable<T> EnumerateNodes(Node<T> node)
        {
            if (node == null)
            {
                return Enumerable.Empty<T>();
            }

            if (node.Level == 0)
            {
                return new[] {node.Value};
            }

            return EnumerateNodes(node.Left).Concat(EnumerateNodes(node.Right));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
