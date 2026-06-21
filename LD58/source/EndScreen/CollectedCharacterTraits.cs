using System.Linq;
using System.Reflection;
using System.Text;
using ChaosFramework.Components;
using ChaosFramework.Graphics;
using ChaosFramework.Graphics.Text;
using static ChaosFramework.Collections.Linq;
using SysCol = System.Collections.Generic;

namespace LD58.EndScreen
{
    using World.Inventory;
    using World.Player;
    internal class CollectedCharacterTraits : Component<EndScreen>
    {
        TextBox text;

        protected override void Create(CreateParameters cparams)
        {
            CParams<PlayerInventory> args = CreateParameters.RequireAs<PlayerInventory>(cparams);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Well, I've made it through my workday, and today I was...");
            sb.AppendLine();

            SysCol.Dictionary<Traits, int> lookup = args.v1.Filter(PredicateTrue).CountTraits().ToDictionary(x => x.traits, x => x.count);

            foreach (FieldInfo traitInfo in typeof(Traits).GetFields())
            {
                string displayName = traitInfo.GetCustomAttributes<CharacterTraitAttribute>().FirstOrDefault()?.displayName;
                if (displayName != null)
                {
                    Traits trait = (Traits)traitInfo.GetValue(null);

                    sb.Append(displayName);
                    sb.Append(": ");
                    sb.Append(lookup.TryGetValue(trait, out int count) ? count : 0);
                    sb.AppendLine();
                }
            }

            text = AddComponent<TextBox>();
            text.Update(sb.ToString(), new LayoutInfo(Align.TopLeft), 0, 0, 0.05f, 50);
        }
    }
}
