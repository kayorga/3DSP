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
        private byte clickDelay;

        public AudioManager(ContentManager Content)
        {
            playerShootSound = Content.Load<SoundEffect>("Audio/shoot1");
            npcShootSound = Content.Load<SoundEffect>("Audio/shoot1");
            npcNeutralHitSound = Content.Load<SoundEffect>("Audio/hit1");
            npcStrongHitSound = Content.Load<SoundEffect>("Audio/hit1");
            npcWeakHitSound = Content.Load<SoundEffect>("Audio/hit1");
            npcCritHitSound = Content.Load<SoundEffect>("Audio/hit1");
            playerHitSound = Content.Load<SoundEffect>("Audio/hit1");

            clickSound = Content.Load<SoundEffect>("Audio/click");

            backgroundSound0 = Content.Load<SoundEffect>("Audio/loop1");
            backgroundSound1 = Content.Load<SoundEffect>("Audio/loop2");
            backgroundSound2 = Content.Load<SoundEffect>("Audio/loop3");
            backgroundSound3 = Content.Load<SoundEffect>("Audio/loop4");

            backgroundSounds = new SoundEffect[] {backgroundSound0, backgroundSound1, backgroundSound2, backgroundSound3};

            clickDelay = 0;
        }

        public void update()
        {
            if (clickDelay > 0)
                clickDelay--;
        }

        public void playBackground(byte theme)
        {
            if (currentBackground != null) currentBackground.Stop();
            currentBackground = backgroundSounds[theme].CreateInstance();
            currentBackground.IsLooped = true;
            currentBackground.Play();
        }

        public void playNPCHit(float dmgModifier)
        {
            if (dmgModifier > 1)
                npcStrongHitSound.CreateInstance().Play();
            else if (dmgModifier == 1)
                npcNeutralHitSound.CreateInstance().Play();
            else
                npcWeakHitSound.CreateInstance().Play();
        }

        public void playCrit()
        {
            npcCritHitSound.CreateInstance().Play();
        }

        public void playShoot(bool fromPlayer)
        {
            if (fromPlayer)
                playerShootSound.CreateInstance().Play();
            else npcShootSound.CreateInstance().Play();
        }

        public void playClick()
        {
            if (clickDelay == 0)
            {
                clickSound.CreateInstance().Play();
                clickDelay = 10;
            }
        }
    }
}
