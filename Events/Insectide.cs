using System.Linq;
using System.Collections.Generic;
using System.Collections;
using RimWorld;
using Verse;
using UnityEngine;

namespace InsectideMod
{
    public class CompProperties_AcidSpit : CompProperties
    {
        public CompProperties_AcidSpit()
        {
            this.compClass = typeof(CompAcidSpit);
        }
    }

    public class CompAcidSpit : ThingComp
    {
        private int cooldownTicks = 0;

        public override void CompTick()
        {
            base.CompTick();

            if (cooldownTicks > 0)
                cooldownTicks--;

            if (cooldownTicks <= 0 && this.parent is Pawn pawn && pawn.Spawned)
            {
                Pawn target = FindTarget(pawn);
                if (target != null)
                {
                    LaunchAcid(pawn, target);
                    cooldownTicks = 600;
                }
            }
        }

        private Pawn FindTarget(Pawn pawn)
        {
            return pawn.Map.mapPawns.AllPawnsSpawned
                .Where(p => p.HostileTo(pawn) && p.Position.InHorDistOf(pawn.Position, 10f))
                // .OrderBy(_ => Rand.Value)
                .OrderBy(p => p.Position.DistanceTo(pawn.Position))
                .FirstOrDefault();
            // var enemies = pawn.Map.mapPawns.AllPawnsSpawned
            //     .Where(p => p.HostileTo(pawn) && p.Position.InHorDistOf(pawn.Position, 10f))
            //     .ToList();
            // return enemies.Count > 0 ? enemies.RandomElement() : null;
        }

        private void LaunchAcid(Pawn caster, Pawn target)
        {
            if (target == null) return;

            DamageInfo dinfo = new DamageInfo(DamageDefOf.AcidBurn, 10, 0, -1, caster);
            target.TakeDamage(dinfo);

            Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.ToxicBuildup, target);
            hediff.Severity = 0.1f;
            target.health.AddHediff(hediff);

            Messages.Message($"{caster.LabelCap} плюнул кислотой в {target.LabelCap}!", target, MessageTypeDefOf.NegativeEvent);
        }

    }
}
