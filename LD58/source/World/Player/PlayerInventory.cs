using ChaosFramework.Components;
using ChaosFramework.Graphics.Colors;
using ChaosFramework.Graphics.OpenGl.Text;
using ChaosFramework.Graphics.Text;
using ChaosFramework.Math;

namespace LD58.World.Player
{
    using Inventory;
    using LD58.World.Constants;

    public class PlayerInventory
        : StrictComponent<Player>
    {
        // TODO: be fancy and smoothly reorder lines in graphical display

        readonly ItemBag itemBag = new ItemBag();

        Text text;

#if DEBUG
        Text traits;
#endif

        protected override void Create(CreateParameters cparams)
        {
            text = new Text(parent.scene.game.textRenderer, 4096);
            text.color = Rgba.OPAQUE_WHITE;
            float tan = parent.scene.fullScreenView.tan;
            text.transform = Matrix.Scaling(0.1f)
                           * Matrix.Translation(parent.scene.fullScreenView.screenRatio * -tan, tan, 0)
                           ;
#if DEBUG
            traits = new Text(parent.scene.game.textRenderer, 4096);
            traits.color = Rgba.OPAQUE_WHITE;
            traits.transform = Matrix.Scaling(0.1f)
                             * Matrix.Translation(parent.scene.fullScreenView.screenRatio * tan, tan, 0)
                             ;
#endif

            foreach (Item item in InitialInventory.INITIAL_INVENTORY)
                itemBag.Add(item);

            UpdateText();
        }

        public void AddItem(Item item)
        {
            itemBag.Add(item);
            UpdateText();
        }

        public void Remove(Item item, bool all = false)
        {
            itemBag.Remove(item, all);
            UpdateText();
        }

        public bool Contains(Item item)
            => itemBag.Contains(item);

        void UpdateText()
        {
            System.Text.StringBuilder bldr = new System.Text.StringBuilder();
            foreach (System.Tuple<Item, int> i in itemBag)
                if (!i.Item1.traits.HasFlag(Traits.Invisible))
                    bldr.AppendLine($"{i.Item1.displayName} x{i.Item2}");

#if DEBUG
            bldr.AppendLine();
            bldr.AppendLine("Hidden:");
            foreach (System.Tuple<Item, int> i in itemBag)
                if (i.Item1.traits.HasFlag(Traits.Invisible))
                    bldr.AppendLine($"{i.Item1.displayName} x{i.Item2}");
#endif

            text.UpdateText(parent.scene.game.textFont, bldr.ToString(), LayoutInfo.TOP_LEFT);

#if DEBUG
            bldr.Clear();
            foreach (System.Tuple<Traits, int> i in itemBag.CountTraits())
                bldr.AppendLine($"{i.Item1} x{i.Item2}");

            traits.UpdateText(parent.scene.game.textFont, bldr.ToString(), LayoutInfo.TOP_RIGHT);
#endif
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

#if DEBUG
            if (text.geometry != null)
                parent.scene.game.textBuffer.Add(traits);
#endif
        }

        protected override void DoDispose()
        {
            base.DoDispose();
            text.Dispose();
#if DEBUG
            traits.Dispose();
#endif
        }
    }
}
