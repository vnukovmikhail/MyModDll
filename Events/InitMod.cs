using System.Linq;
using System.Collections.Generic;
using System.Collections;
using RimWorld;
using Verse;
using UnityEngine;


public class MyModSettings : ModSettings
{
    public bool showExtraInfo = true;
    public float powerMultiplier = 1.0f;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref showExtraInfo, "showExtraInfo", true);
        Scribe_Values.Look(ref powerMultiplier, "powerMultiplier", 1.0f);
        base.ExposeData();
    }
}
