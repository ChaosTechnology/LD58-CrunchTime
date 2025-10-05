using ChaosFramework.Collections;
using ChaosFramework.Components;
using ChaosFramework.Graphics.OpenGl.Text;
using ChaosUtil.Primitives;
using SysCol = System.Collections.Generic;

namespace LD58.World.Player
{
    using Inventory;
    using System;
    using System.Collections;

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
                => --count >= 0;
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

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        SysCol.IEnumerator<Tuple<Item, int>> SysCol.IEnumerable<Tuple<Item, int>>.GetEnumerator()
            => GetEnumerator();

        public SysCol.IEnumerator<Tuple<Item, int>> GetEnumerator()
        {
            foreach (Node n in items)
                yield return new Tuple<Item, int>(n.item, n.count);
        }
    }

    public class PlayerInventory
        : StrictComponent<Player>
    {
        // TODO: be fancy and smoothly reorder lines in graphical display

        readonly SysCol.Dictionary<Item, Wrapper<int>> inventory
            = new SysCol.Dictionary<Item, Wrapper<int>>();

        Text text;

        protected override void Create(CreateParameters cparams)
        {
            text = new Text(parent.scene.game.textRenderer, 4096);
        }

        public void AddItem(Item item)
        {
            Wrapper<int> count;
            if (!inventory.TryGetValue(item, out count))
                inventory[item] = new Wrapper<int>(1);
            else
                count.value++;

            UpdateText();
        }

        public void Remove(Item item)
        {
            Wrapper<int> count;
            if (!inventory.TryGetValue(item, out count))
                throw new System.Exception();
            else if (--count.value <= 0)
                inventory.Remove(item);

            UpdateText();
        }

        void UpdateText()
        {
            System.Text.StringBuilder bldr = new System.Text.StringBuilder();
        }

        public override void SetDrawCalls()
        {
            base.SetDrawCalls();
        }
    }
}
