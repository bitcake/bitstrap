using UnityEngine;

namespace BitStrap
{
    /// <summary>
    /// Bunch of utility extension methods to the Component class.
    /// </summary>
    public static class ComponentExtensions
    {
        /// <summary>
        /// Like the Component.GetComponentInParent however it allows to find in inactive GameObjects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static T GetComponentInParent<T>( this Component self, bool includeInactive ) where T : Component
        {
            return self.gameObject.GetComponentInParent<T>( includeInactive );
        }
    }
}
