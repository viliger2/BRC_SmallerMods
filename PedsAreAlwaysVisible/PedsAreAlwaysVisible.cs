using BepInEx;
using BepInEx.Configuration;

namespace BRC_DisableNewStarCutscene
{
    [BepInPlugin("com.Viliger.PedsAreAlwaysVisible", "PedsAreAlwaysVisible", "0.9.0")]
    public class PedsAreAlwaysVisible : BaseUnityPlugin
    {
        public static ConfigEntry<int>? DespawnOnStars;
        //public static ConfigEntry<float>? DespawnRenderRange;

        public void Awake()
        {
            DespawnOnStars = Config.Bind("Configuration", "Despawn On Stars", 10, "On which heat level pedestrians despawn. By default they never despawn. Game's value is 2. You can set it to -1 from them to be always visible, including during scripted events.");
            //DespawnRenderRange = Config.Bind("Configuration", "Despawn Render Range", 1000f, "From which range pedestrians start to despawn. Game's value is 63 and it is halved (31.5) for low settings.");

            On.Reptile.StreetLifeCluster.Awake += StreetLifeCluster_Awake;
        }

        private void StreetLifeCluster_Awake(On.Reptile.StreetLifeCluster.orig_Awake orig, Reptile.StreetLifeCluster self)
        {
            //float oldDeactivateValue = self.deactivateDistance;

            self.hideFromWantedStar = DespawnOnStars?.Value ?? self.hideFromWantedStar;
            //self.deactivateDistance = DespawnRenderRange?.Value ?? self.deactivateDistance;
            //self.distanceOptimize = self.deactivateDistance == oldDeactivateValue;
            orig(self);
        }
    }
}
