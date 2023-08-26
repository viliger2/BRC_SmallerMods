using BepInEx;
using MonoMod.Cil;

namespace BRC_DisableNewStarCutscene
{
    [BepInPlugin("com.Viliger.DisableAnnoyingCutscenes", "DisableAnnoyingCutscenes", "0.7.0")]
    public class DisableAnnoyingCutscenes : BaseUnityPlugin
    {
        private static BepInEx.Logging.ManualLogSource logger;

        public void Awake()
        {
            logger = Logger;
            IL.Reptile.WantedManager.UpdateStarProgress += WantedManager_UpdateStarProgress;
            //IL.Reptile.GraffitiGame.SetStateVisual += GraffitiGame_SetStateVisual;
        }

        private void GraffitiGame_SetStateVisual(ILContext il)
        {
            logger.LogMessage("trying to get inside graffiti game");
            ILCursor c = new ILCursor(il);
            if(c.TryGotoNext(MoveType.Before,
                x => x.MatchSwitch(out _)))
            {
                ((ILLabel[])c.Next.Operand)[1].Target = ((ILLabel[])c.Next.Operand)[3].Target;
                ((ILLabel[])c.Next.Operand)[2].Target = ((ILLabel[])c.Next.Operand)[3].Target;
                foreach (ILLabel value in (ILLabel[])c.Next.Operand)
                {
                    Logger.LogMessage(value.Target.ToString());
                }
                logger.LogMessage(il.ToString());
            }
            //ILCursor c = new ILCursor(il);
            //if(c.TryGotoNext(MoveType.Before,
            //    x => x.MatchLdarg(out _),
            //    x => x.MatchLdfld<Reptile.GraffitiGame>("characterPuppet"),
            //    x => x.MatchLdfld<Reptile.CharacterVisual>("anim"),
            //    x => x.MatchLdcR4(out _)
            //    ))
            //{
            //    c.Index++;
            //    c.Emit(OpCodes.Switch, )
            //    c.RemoveRange(136);
            //    logger.LogMessage(il.ToString());
            //    logger.LogMessage("inside graffiti game");
            //}
        }

        private void WantedManager_UpdateStarProgress(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdarg(out _),
                x => x.MatchLdloc(out _),
                x => x.MatchCallOrCallvirt<Reptile.WantedManager>("DoNewStarSequence")
                ))
            {
                c.RemoveRange(3);
            }
        }
    }
}
