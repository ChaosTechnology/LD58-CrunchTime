using ChaosFramework.Collections;
using System;
using System.Collections;
using SysCol = System.Collections.Generic;

namespace LD58.World.Inventory
{
    public class ItemBag
        : SysCol.IEnumerable<ItemBag.Entry>
    {
        public class Entry
        {
            public readonly Item item;
            public readonly int count;

            public Entry(Item item, int count)
            {
                this.item = item;
                this.count = count;
            }
        }

        public class Traitor
        {
            public readonly Traits item;
            public readonly int count;

            public Traitor(Traits item, int count)
            {
                this.item = item;
                this.count = count;
            }
        }

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

            public static implicit operator Entry(Node n)
                => new Entry(n.item, n.count);
        }

        AdvancedLinkedList<Node> items = new AdvancedLinkedList<Node>();

        public int numItemKinds => items.length;

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

        public void Remove(Item item, bool all = false)
        {
            foreach (Node n in items)
                if (n.item.Equals(item))
                {
                    if (all || n.Discard())
                        items.RemoveCurrent();

                    return;
                }

            if (!all)
                throw new InvalidOperationException("Can't remove items that we don't have.");
        }

        public bool Contains(Item item)
        {
            foreach (Node n in items)
                if (n.item.Equals(item))
                    return true;

            return false;
        }

        public bool Contains(Traits trait, int count = 1)
        {
            foreach (Node n in items)
                if (n.item.traits.HasFlag(trait))
                    if ((count -= n.count) <= 0)
                        return true;

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        SysCol.IEnumerator<Entry> SysCol.IEnumerable<Entry>.GetEnumerator()
            => GetEnumerator();

        public SysCol.IEnumerator<Entry> GetEnumerator()
        {
            foreach (Node n in items)
                yield return new Entry(n.item, n.count);
        }

        public SysCol.IEnumerable<Traitor> CountTraits()
        {
            int[] traits = new int[64];
            foreach (Entry item in this)
                for (int i = 0; i < 64; i++)
                    if (((ulong)item.item.traits & (1ul << i)) != 0)
                        traits[i] += item.count;

            for (int i = 0; i < 64; i++)
                if (traits[i] > 0)
                    yield return new Traitor((Traits)(1ul << i), traits[i]);
        }

        public void Transfer(ItemBag other)
            => items = new AdvancedLinkedList<Node>(other.items);

        public ItemBag Filter(Traits filter)
        {
            ItemBag bag = new ItemBag();
            foreach (Node node in items)
                if (node.item.traits.HasFlag(filter))
                    for (int i = 0; i < node.count; ++i)
                        bag.Add(node.item);

            return bag;
        }

        public ItemBag Filter(System.Func<Entry, bool> filter)
        {
            ItemBag bag = new ItemBag();
            foreach (Node node in items)
                if (filter(node))
                    for (int i = 0; i < node.count; ++i)
                        bag.Add(node.item);

            return bag;
        }
    }
}
