using System.Linq;
using System.Collections.Generic;
using System.Collections;
using RimWorld;
using Verse;

namespace MyMod.Events
{
    public class IncidentWorker_TestIncident : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            // Logic: Spawn 100 Silver in a random spot
            Thing silver = ThingMaker.MakeThing(ThingDefOf.Silver);
            silver.stackCount = 100;
            GenSpawn.Spawn(silver, CellFinder.RandomCell(map), map);
            Find.LetterStack.ReceiveLetter("Gift", "You received 100 silver.", LetterDefOf.PositiveEvent);
            // PawnsFinder.AllMaps_FreeColonists.RandomElement();

            Log.Message(string.Format("Pawns count equals {0}", map.mapPawns.ColonistCount));
            Pawn singlePawn = map.mapPawns.FreeColonistsSpawned.First();
            Log.Message(string.Format("Pawn -> {0}", singlePawn.Name.ToStringFull));

            foreach (var pawn in map.mapPawns.FreeColonists)
            {
                Log.Message(string.Format("Your pawn -> {0}", pawn.Name.ToStringFull));
            }

            return true;
        }
    }
}