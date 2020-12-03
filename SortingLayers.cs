using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SortingLayers
{
    public static string DetermineParticleLayer(bool isOutside, PlayerStats ps)
    {
        string layerName;
        
        if(isOutside)
        {
            if(ps.GetPlayerId() == 1)
            {
                layerName = "Player1Front";
            }
            else
            {
                layerName = "Player2Front";
            }
        }
        else
        {
            if(ps.GetPlayerId() == 1)
            {
                layerName = "Player1Back";
            }
            else
            {
                layerName = "Player2Back";
            }
        }

        return layerName;
    }
}
