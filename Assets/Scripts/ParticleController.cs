using UnityEngine;
namespace Assets.Scripts
{
    class ParticleController : MonoBehaviour
    {
        public ParticleSystem blinkSystem;
        public ParticleSystem blastSystem;
        public void startBlinkAnimation()
        {
            if (!blinkSystem.isPlaying)
            {
                blinkSystem.Play();
            }
        }
        public void startBlastAnimation()
        {
            if (!blastSystem.isPlaying)
            {
                blastSystem.Play();
            }
        }
    }
}
