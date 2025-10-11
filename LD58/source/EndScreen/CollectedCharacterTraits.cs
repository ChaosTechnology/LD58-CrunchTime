using ChaosFramework.Components;
using ChaosFramework.Graphics.Text;
using ChaosFramework.Graphics;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LD58.EndScreen
{
    using World.Inventory;
    internal class CollectedCharacterTraits : Component<EndScreen>
    {
        TextBox text;

        protected override void Create(CreateParameters cparams)
        {
            CParams<ItemBag> args = CreateParameters.RequireAs<ItemBag>(cparams);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Well, I've made it through my workday, and today I was...");
            sb.AppendLine();

            System.Collections.Generic.Dictionary<Traits, int> lookup = args.v1.CountTraits().ToDictionary(x => x.Item1, x => x.Item2);
            foreach (FieldInfo traitInfo in typeof(Traits).GetFields())
            {
                string displayName = traitInfo.GetCustomAttributes<CharacterTraitAttribute>().FirstOrDefault()?.displayName;
                if (displayName != null)
                {
                    Traits trait = (Traits)traitInfo.GetValue(null);

                    int count;
                    sb.Append(displayName);
                    sb.Append(": ");
                    sb.Append(lookup.TryGetValue(trait, out count) ? count : 0);
                    sb.AppendLine();
                }
            }

            text = AddComponent<TextBox>();
            text.Update(sb.ToString(), new LayoutInfo(Align.TopLeft), 0, 0, 0.05f, 50);
        }
    }
}
