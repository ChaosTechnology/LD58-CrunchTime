using ChaosFramework.Collections;
using System;
using System.Collections;
using SysCol = System.Collections.Generic;

namespace LD58.World.Inventory
{
    class ItemBag
        : SysCol.IEnumerable<Tuple<Item, int>>
    {
        class Node
        {
            public readonly Item item;
            public int count;

            public Node(Item item)
            {
                this.item = item;
                count = 1;
            }

            /// <summary> Add a single instance of this item. </summary>
            public void Pickup()
                => count++;

            /// <summary> Discard a single instance of this item. </summary>
            /// <returns>
            ///     true, if the node can be removed from the collection;
            ///     false otherwise
            /// </returns>
            public bool Discard()
                => --count <= 0;
        }

        AdvancedLinkedList<Node> items = new AdvancedLinkedList<Node>();

        public void Add(Item item)
        {
            foreach (Node n in items)
                if (n.item.Equals(item))
                {
                    items.RemoveCurrent();
                    items.Add(n);
                    n.Pickup();
                    return;
                }

            items.Add(new Node(item));
        }

        public void Remove(Item item)
        {
            foreach (Node n in items)
                if (n.item.Equals(item))
                {
                    if (n.Discard())
                        items.RemoveCurrent();

                    return;
                }

            throw new InvalidOperationException("Can't remove items that we don't have.");
        }

        public bool Contains(Item item)
        {
            foreach (Node n in items)
                if (n.item.Equals(item))
                    return true;

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        SysCol.IEnumerator<Tuple<Item, int>> SysCol.IEnumerable<Tuple<Item, int>>.GetEnumerator()
            => GetEnumerator();

        public SysCol.IEnumerator<Tuple<Item, int>> GetEnumerator()
        {
            foreach (Node n in items)
                yield return new Tuple<Item, int>(n.item, n.count);
        }

        public SysCol.IEnumerable<Tuple<Traits, int>> CountTraits()
        {
            int[] traits = new int[64];
            foreach (Tuple<Item, int> item in this)
                for (int i = 0; i < 64; i++)
                    if (((ulong)item.Item1.traits & (1ul << i)) != 0)
                        traits[i] += item.Item2;

            for (int i = 0; i < 64; i++)
                if (traits[i] > 0)
                    yield return new Tuple<Traits, int>((Traits)(1ul << i), traits[i]);
        }
    }
}
