using ChaosFramework.Components;
using ChaosFramework.Graphics.Colors;
using ChaosFramework.Graphics.OpenGl.Text;
using ChaosFramework.Graphics.Text;
using ChaosFramework.Math;
using ChaosUtil.Primitives;
using SysCol = System.Collections.Generic;

namespace LD58.World.Player
{
    using Inventory;

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
            text.color = Rgba.OPAQUE_WHITE;
            float tan = parent.scene.fullScreenView.tan;
            text.transform = Matrix.Scaling(0.1f)
                           * Matrix.Translation(parent.scene.fullScreenView.screenRatio * -tan, tan, 0)
                           ;
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
            foreach (SysCol.KeyValuePair<Item, Wrapper<int>> i in inventory)
                bldr.AppendLine($"{i.Key.displayName} x{i.Value.value}");

            text.UpdateText(parent.scene.game.textFont, bldr.ToString(), LayoutInfo.TOP_LEFT);
        }

        public override void SetDrawCalls()
        {
            base.SetDrawCalls();
            scene.drawLayers[(int)DrawLayers.Text].Add(AddText);
        }

        void AddText()
        {
            if (text.geometry != null)
                parent.scene.game.textBuffer.Add(text);
        }
    }
}
