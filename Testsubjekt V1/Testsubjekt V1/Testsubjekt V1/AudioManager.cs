using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace TestsubjektV1
{
    class AudioManager
    {
        private SoundEffect playerShootSound;
        private SoundEffect npcShootSound;
        private SoundEffect npcNeutralHitSound;
        private SoundEffect npcStrongHitSound;
        private SoundEffect npcWeakHitSound;
        private SoundEffect npcCritHitSound;
        private SoundEffect playerHitSound;

        private SoundEffect backgroundSound0;
        private SoundEffect backgroundSound1;
        private SoundEffect backgroundSound2;
        private SoundEffect backgroundSound3;
        private SoundEffect[] backgroundSounds;
        private SoundEffectInstance currentBackground;

        private SoundEffect clickSound;

        public AudioManager(ContentManager Content)
        {
            return;
            backgroundSounds = new SoundEffect[] {backgroundSound0, backgroundSound1, backgroundSound2, backgroundSound3};
        }

        public void playBackground(byte theme)
        {
            return;
            currentBackground.Stop();
            currentBackground = backgroundSounds[theme].CreateInstance();
            currentBackground.IsLooped = true;
            currentBackground.Play();
        }

        public void playNPCHit(float dmgModifier)
        {
            return;
            if (dmgModifier > 1)
                npcStrongHitSound.CreateInstance().Play();
            else if (dmgModifier == 1)
                npcNeutralHitSound.CreateInstance().Play();
            else
                npcWeakHitSound.CreateInstance().Play();
        }

        public void playCrit()
        {
            return;
            npcCritHitSound.CreateInstance().Play();
        }

        public void playShoot(bool fromPlayer)
        {
            return;
            if (fromPlayer)
                playerShootSound.CreateInstance().Play();
            else npcShootSound.CreateInstance().Play();
        }

        public void playClick()
        {
            return;
            clickSound.CreateInstance().Play();
        }
    }
}
