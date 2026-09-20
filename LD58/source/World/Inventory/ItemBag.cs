using ChaosFramework.Collections;
using System;
using System.Collections;
using SysCol = System.Collections.Generic;

namespace LD58.World.Inventory
{
    public class ItemBag
        : SysCol.IEnumerable<ItemBag.ItemCount>
    {
        [System.Diagnostics.DebuggerDisplay("{" + nameof(item) + "." + nameof(Item.displayName) + "}, {" + nameof(count) + "}")]
        public class ItemCount
        {
            public readonly Item item;
            public readonly int count;

            public ItemCount(Item item, int count)
            {
                this.item = item;
                this.count = count;
            }
        }

        [System.Diagnostics.DebuggerDisplay("{" + nameof(traits) + "}, {" + nameof(count) + "}")]
        public class TraitCount
        {
            public readonly Traits traits;
            public readonly int count;

            public TraitCount(Traits traits, int count)
            {
                this.traits = traits;
                this.count = count;
            }
        }

        [System.Diagnostics.DebuggerDisplay("{" + nameof(item) + "." + nameof(Item.displayName) + "}, {" + nameof(count) + "}")]
        class Node
        {
            public readonly Item item;
            public int count;

            public Node(Item item, int count)
            {
                this.item = item;
                this.count = count;
            }

            /// <summary> Add a single instance of this item. </summary>
            public void Pickup(int howMany)
                => count += howMany;

            /// <summary> Discard a single instance of this item. </summary>
            /// <param name="howMany">How many items to remove. If negative remove all.</param>
            /// <returns>
            ///     true, if the node can be removed from the collection;
            ///     false otherwise
            /// </returns>
            public bool Discard(int howMany)
            {
                if (howMany < 0)
                {
                    count = 0;
                    return true;
                }
                else
                {
                    count -= howMany;
                    if (count < 0)
                        throw new InvalidOperationException("Can't remove more than we have.");

                    return count == 0;
                }
            }

            public static implicit operator ItemCount(Node n)
                => new ItemCount(n.item, n.count);
        }

        AdvancedLinkedList<Node> items = new AdvancedLinkedList<Node>();

        public int numItemKinds => items.length;

        public void Add(Item item, int count = 1)
        {
            if (count < 0)
                throw new InvalidOperationException("Can't add negative amount of item. Use remove instead.");

            foreach (Node n in items)
                if (n.item.Equals(item))
                {
                    items.RemoveCurrent();
                    items.Add(n);
                    n.Pickup(count);
                    return;
                }

            items.Add(new Node(item, count));
        }

        public void Remove(Item item, int count = 1)
        {
            foreach (Node n in items)
                if (n.item.Equals(item))
                {
                    if (n.Discard(count))
                        items.RemoveCurrent();

                    return;
                }

            if (count > 0)
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

        SysCol.IEnumerator<ItemCount> SysCol.IEnumerable<ItemCount>.GetEnumerator()
            => GetEnumerator();

        public SysCol.IEnumerator<ItemCount> GetEnumerator()
        {
            foreach (Node n in items)
                yield return new ItemCount(n.item, n.count);
        }

        public SysCol.IEnumerable<TraitCount> CountTraits()
        {
            int[] traits = new int[64];
            foreach (ItemCount item in this)
                for (int i = 0; i < 64; i++)
                    if (((ulong)item.item.traits & (1ul << i)) != 0)
                        traits[i] += item.count;

            for (int i = 0; i < 64; i++)
                if (traits[i] > 0)
                    yield return new TraitCount((Traits)(1ul << i), traits[i]);
        }

        public void Transfer(ItemBag other)
            => items = new AdvancedLinkedList<Node>(other.items);

        public ItemBag Filter(Traits filter)
        {
            ItemBag bag = new ItemBag();
            foreach (Node node in items)
                if (node.item.traits.HasFlag(filter))
                    bag.Add(node.item, node.count);

            return bag;
        }

        public ItemBag Filter(params Traits[] filters)
        {
            ItemBag bag = new ItemBag();
            foreach (Node node in items)
                foreach (Traits filter in filters)
                    if (node.item.traits.HasFlag(filter))
                    {
                        bag.Add(node.item, node.count);
                        break;
                    }

            return bag;
        }

        public ItemBag Filter(System.Func<ItemCount, bool> filter)
        {
            ItemBag bag = new ItemBag();
            foreach (Node node in items)
                if (filter(node))
                    bag.Add(node.item, node.count);

            return bag;
        }
    }
}
