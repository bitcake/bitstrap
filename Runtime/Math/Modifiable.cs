using System.Collections.Generic;
using UnityEngine;

namespace BitStrap
{
    public interface IModifiable
    {
        void UpdateModifiedValues();
    }

    /// <summary>
    /// Specialized version of Modifiable for int.
    /// </summary>
    [System.Serializable]
    public class ModifiableInt : Modifiable<int>
    {
        public ModifiableInt( int value, System.Func<int, int, int> aggregateFunction ) : base( value, aggregateFunction )
        { }
    }

    /// <summary>
    /// Specialized version of Modifiable for float.
    /// </summary>
    [System.Serializable]
    public class ModifiableFloat : Modifiable<float>
    {
        public ModifiableFloat( float value, System.Func<float, float, float> aggregateFunction ) : base( value, aggregateFunction )
        { }
    }

    /// <summary>
    /// Wraps a type that can be modified given an aggregate function.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class Modifiable<T> : IModifiable
    {
        [SerializeField]
        private T originalValue;

        [SerializeField]
        private T modifiedValue;

        private Dictionary<object, T> modifiers = new Dictionary<object, T>();
        private System.Func<T, T, T> aggregateFunction;

        /// <summary>
        /// Original value.
        /// Use this to write new values to be modified.
        /// </summary>
        public T OriginalValue
        {
            get { return originalValue; }
            set { originalValue = value; UpdateModifiedValues(); }
        }

        /// <summary>
        /// The cached modified value. Calculated by passing the original value
        /// through all the aggregate functions.
        /// </summary>
        public T ModifiedValue
        {
            get { return modifiedValue; }
        }

        /// <summary>
        /// All modifiers connected to this modifiable
        /// </summary>
        public Dictionary<object, T> Modifiers
        {
            get { return modifiers; }
        }

        public Modifiable( T value, System.Func<T, T, T> aggregateFunction )
        {
            this.originalValue = value;
            this.aggregateFunction = aggregateFunction;
            this.modifiedValue = value;
        }

        public static implicit operator T( Modifiable<T> modifiable )
        {
            return modifiable.modifiedValue;
        }

        /// <summary>
        /// Sets a modifier within a context key.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="modifier"></param>
        public void SetModifier( object context, T modifier )
        {
            modifiers[context] = modifier;
            UpdateModifiedValues();
        }

        /// <summary>
        /// Removes a modifier given its context key.
        /// </summary>
        /// <param name="context"></param>
        public void RemoveModifier( object context )
        {
            modifiers.Remove( context );
            UpdateModifiedValues();
        }

        /// <summary>
        /// Remove all modifiers.
        /// </summary>
        public void ClearModifiers()
        {
            modifiers.Clear();
            modifiedValue = originalValue;
        }

        public override string ToString()
        {
            return modifiedValue.ToString();
        }

        public void UpdateModifiedValues()
        {
            modifiedValue = originalValue;
            T lastValue = originalValue;

            foreach( var pair in modifiers.Iter() )
            {
                T value = pair.Value;
                modifiedValue = aggregateFunction( lastValue, value );
                lastValue = modifiedValue;
            }
        }
    }
}
