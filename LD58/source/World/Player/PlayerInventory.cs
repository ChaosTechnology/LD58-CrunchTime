using ChaosFramework.Components;
using ChaosFramework.Graphics.Colors;
using ChaosFramework.Graphics.OpenGl.Text;
using ChaosFramework.Graphics.Text;
using ChaosFramework.Math;
using System;
using System.Collections;
using System.Linq;
using L = ChaosFramework.Collections.Linq;
using SysCol = System.Collections.Generic;

namespace LD58.World.Player
{
    using Inventory;

    public class PlayerInventory
        : StrictComponent<Player>
        , SysCol.IEnumerable<System.Tuple<Item, int>>
    {
        // TODO: be fancy and smoothly reorder lines in graphical display

        const float CHAR_SIZE = 0.05f;
        const float MARGIN_X = 0.025f;
        const float MARGIN_Y = 0.0125f;

        static bool Hidden(Tuple<Item, int> item)
            => item.Item1.traits.HasFlag(Traits.Invisible);

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
            text.transform = Matrix.Scaling(CHAR_SIZE)
                           * Matrix.Translation(parent.scene.fullScreenView.screenRatio * -tan + MARGIN_X, tan - MARGIN_Y, 0)
                           ;
#if DEBUG
            traits = new Text(parent.scene.game.textRenderer, 4096);
            traits.color = Rgba.OPAQUE_WHITE;
            traits.transform = Matrix.Scaling(CHAR_SIZE)
                             * Matrix.Translation(parent.scene.fullScreenView.screenRatio * tan - MARGIN_X, tan - MARGIN_Y, 0)
                             ;

            foreach (Item item in new[] {
                Constants.KnownItems.PANTS,
                Constants.KnownItems.BACON,
                Constants.KnownItems.BEER,
                Constants.KnownItems.EGG,
                Constants.KnownItems.CANDY_UNDIES,
                Constants.KnownItems.BLACK_SUBSTANCE,
                Constants.KnownItems.ESSENCE_OF_DARKNESS,
                })
                itemBag.Add(new Item($"Hidden Debug {item.displayName}", item.traits | Traits.Invisible));
#endif

            UpdateText();
        }

        public void CarryOver(PlayerInventory source)
        {
            itemBag.Transfer(source.itemBag);
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

        public bool Contains(Traits trait, int count)
            => itemBag.Contains(trait, count);

        void UpdateText()
        {
            System.Text.StringBuilder bldr = new System.Text.StringBuilder();
            foreach (System.Tuple<Item, int> i in itemBag)
                if (!i.Item1.traits.HasFlag(Traits.Invisible))
                    bldr.AppendLine($"{i.Item1.displayName} x{i.Item2}");

#if DEBUG
            SysCol.IEnumerable<Tuple<Item, int>> invisible = itemBag.Where(Hidden);
            if (invisible.Any(L.PredicateTrue))
            {
                if (bldr.Length > 0)
                    bldr.AppendLine();

                bldr.AppendLine("Hidden:");
                foreach (Tuple<Item, int> i in invisible)
                    bldr.AppendLine($"{i.Item1.displayName} x{i.Item2}");
            }
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

        SysCol.IEnumerator<Tuple<Item, int>> SysCol.IEnumerable<Tuple<Item, int>>.GetEnumerator()
            => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public SysCol.IEnumerator<Tuple<Item, int>> GetEnumerator()
            => itemBag.GetEnumerator();
    }
}
