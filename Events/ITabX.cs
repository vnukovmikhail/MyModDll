using System.Linq;
using System.Collections.Generic;
using System.Collections;
using RimWorld;
using Verse;
using UnityEngine;

namespace MyMod
{
    public class ITab_X : ITab
    {
        public ITab_X()
        {
            this.size = new Vector2(400f, 0f);
            this.labelKey = "MyCustomTabLabel";
        }

        public override bool IsVisible
        {
            get
            {
                if (SelThing is Pawn pawn)
                {
                    if (pawn.RaceProps.Humanlike)
                    {
                        Log.Message($"[MyMod] Выбрана пешка (человек): {pawn.LabelCap}");
                    }
                    else if (pawn.RaceProps.Animal)
                    {
                        Log.Message($"[MyMod] Выбрано животное: {pawn.LabelCap}");
                    }
                    else
                    {
                        Log.Message($"[MyMod] Другая пешка: {pawn.LabelCap}");
                    }
                }
                else
                {
                    Log.Message($"[MyMod] SelThing не является Pawn, тип: {SelThing?.GetType()}");
                }

                return true;
            }
        }



        protected override void FillTab()
        {
            Log.Message($"[MyMod] FillTab opened for SelThing type: {SelThing?.GetType()}");
            Pawn? pawn = SelThing as Pawn;
            if (pawn == null) return;

            var settings = LoadedModManager.GetMod<MyMod_Main>().GetSettings<MyModSettings>();

            Listing_Standard listing = new Listing_Standard();
            listing.Begin(new Rect(0f, 0f, size.x, 9999f).ContractedBy(10f));

            string pawnName = pawn.Name != null ? pawn.Name.ToStringFull : pawn.LabelCap;
            listing.Label("MyMod_PawnName".Translate(pawnName));
            listing.Gap();

            if (settings.showExtraInfo)
            {
                float healthPct = pawn.health.summaryHealth.SummaryHealthPercent;
                Widgets.FillableBar(listing.GetRect(20f), healthPct);
                listing.Label("MyMod_HealthBody".Translate() + $": {healthPct:P0}");
            }

            listing.Gap(20f);

            if (listing.ButtonText("MyMod_ClickMe".Translate()))
            {
                Messages.Message("MyMod_ButtonPushed".Translate(), MessageTypeDefOf.SilentInput, false);
            }

            // // THIS BLOCK IS TEMPLATE FOR AUTOSIZE FOR TEXT
            // Vector2 textSize = Text.CalcSize("MyMod_PawnName".Translate(pawn.Name.ToStringFull));
            // this.size.x = Mathf.Max(400f, textSize.x + 50f);

            listing.GapLine(12f);

            listing.Label($"Настройка множителя: {settings.powerMultiplier:F2}");
            settings.powerMultiplier = listing.Slider(settings.powerMultiplier, 0f, 5f);

            if (listing.ButtonText("MyMod_Boom".Translate()))
            {
                IntVec3 pos = pawn.Position;
                Map map = pawn.Map;

                GenExplosion.DoExplosion(
                    center: pos,
                    map: map,
                    radius: settings.powerMultiplier,
                    damType: DamageDefOf.Flame,
                    instigator: pawn,
                    damAmount: 50,
                    armorPenetration: 0.5f,
                    explosionSound: SoundDefOf.Artillery_ShellLoaded,
                    weapon: ThingDef.Named("Gun_AssaultRifle"),
                    projectile: null,
                    intendedTarget: null,
                    postExplosionSpawnThingDef: ThingDefOf.Filth_Blood,
                    postExplosionSpawnChance: 0.3f,
                    postExplosionSpawnThingCount: 2,
                    applyDamageToExplosionCellsNeighbors: true,
                    chanceToStartFire: 0.2f,
                    damageFalloff: true
                );
            }


            listing.End();
            this.size = new Vector2(this.size.x, listing.CurHeight + 20f);
        }
    }
}
