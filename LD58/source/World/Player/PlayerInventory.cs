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
