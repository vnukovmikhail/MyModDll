using System.Linq;
using System.Collections.Generic;
using System.Collections;
using RimWorld;
using Verse;
using UnityEngine;

namespace MyMod
{
    public class MyMod_Main : Mod
    {
        MyModSettings settings;

        public MyMod_Main(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<MyModSettings>();
        }

        public override string SettingsCategory() => "My Custom Mod";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            listingStandard.CheckboxLabeled("Включить доп. инфо", ref settings.showExtraInfo);

            listingStandard.Label($"Множитель силы: {settings.powerMultiplier:F2}");
            settings.powerMultiplier = listingStandard.Slider(settings.powerMultiplier, 0.1f, 5.0f);

            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }
    }

}
