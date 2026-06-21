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
        , SysCol.IEnumerable<ItemBag.ItemCount>
    {
        // TODO: be fancy and smoothly reorder lines in graphical display

        const float CHAR_SIZE = 0.05f;
        const float MARGIN_X = 0.025f;
        const float MARGIN_Y = 0.0125f;

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
            foreach (ItemBag.ItemCount i in itemBag)
                if (!i.item.traits.HasFlag(Traits.Invisible))
                    bldr.AppendLine($"{i.item.displayName} x{i.count}");

#if DEBUG
            SysCol.IEnumerable<ItemBag.ItemCount> invisible = itemBag.Filter(Traits.Invisible);
            if (invisible.Any(L.PredicateTrue))
            {
                if (bldr.Length > 0)
                    bldr.AppendLine();

                bldr.AppendLine("Hidden:");
                foreach (ItemBag.ItemCount i in invisible)
                    bldr.AppendLine($"{i.item.displayName} x{i.count}");
            }
#endif

            text.UpdateText(parent.scene.game.textFont, bldr.ToString(), LayoutInfo.TOP_LEFT);

#if DEBUG
            bldr.Clear();
            foreach (ItemBag.TraitCount i in itemBag.CountTraits())
                bldr.AppendLine($"{i.traits} x{i.count}");

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

        SysCol.IEnumerator<ItemBag.ItemCount> SysCol.IEnumerable<ItemBag.ItemCount>.GetEnumerator()
            => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public SysCol.IEnumerator<ItemBag.ItemCount> GetEnumerator()
            => itemBag.GetEnumerator();

        public ItemBag CopyBag()
        {
            ItemBag bag = new ItemBag();
            bag.Transfer(itemBag);
            return bag;
        }
    }
}
