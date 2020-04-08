using UnityEngine;

namespace BitStrap
{
    /// <summary>
    /// A ParticleSystem wrapper that does not generate garbage.
    /// </summary>
    [System.Serializable]
    public class ParticleController
    {
        /// <summary>
        /// The main ParticleSystem reference.
        /// </summary>
        [SerializeField]
        private ParticleSystem rootParticleSystem;

        private ParticleSystem[] particles;

        /// <summary>
        /// Access the main ParticleSystem reference.
        /// </summary>
        public ParticleSystem RootParticleSystem
        {
            get { return rootParticleSystem; }
            set { rootParticleSystem = value; }
        }

        /// <summary>
        /// Just like ParticleSystem.Emit() however it does not generate garbage.
        /// </summary>
        /// <param name="n"></param>
        public void Emit( int n )
        {
            if( !CheckAndSetup() )
                return;

            foreach( var particleSystem in particles )
                particleSystem.Emit( n );
        }

        /// <summary>
        /// Just like ParticleSystem.Play() however it does not generate garbage.
        /// </summary>
        public void Play()
        {
            if( !CheckAndSetup() )
                return;

            foreach( var particleSystem in particles )
                particleSystem.Play( false );
        }

        /// <summary>
        /// Just like ParticleSystem.Simulate() however it does not generate garbage.
        /// </summary>
        /// <param name="t"></param>
        public void Simulate( float t )
        {
            if( !CheckAndSetup() )
                return;

            foreach( var particleSystem in particles )
                particleSystem.Simulate( t, false );
        }

        /// <summary>
        /// Just like ParticleSystem.Simulate() however it does not generate garbage.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="restart"></param>
        public void Simulate( float t, bool restart )
        {
            if( !CheckAndSetup() )
                return;

            foreach( var particleSystem in particles )
                particleSystem.Simulate( t, false, restart );
        }

        /// <summary>
        /// Just like ParticleSystem.Stop() however it does not generate garbage.
        /// </summary>
        public void Stop()
        {
            if( !CheckAndSetup() )
                return;

            foreach( var particleSystem in particles )
                particleSystem.Stop( false );
        }

        private bool CheckAndSetup()
        {
            if( particles == null && rootParticleSystem != null )
                particles = rootParticleSystem.GetComponentsInChildren<ParticleSystem>( true );

            return particles != null;
        }
    }
}
