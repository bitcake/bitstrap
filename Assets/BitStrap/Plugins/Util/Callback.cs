namespace BitStrap
{
    /// <summary>
    /// System.Action helper class.
    /// It allows you to bypass the C# delegate idiom "if( callback != null ) callback();".
    ///
    /// Use it like "Callback.Trigger( delegate );"
    /// </summary>
    public static class Callback
    {
        /// <summary>
        /// Trigger a callback bypassing the standard C# delegate call idiom.
        /// </summary>
        /// <param name="callback"></param>
        public static void Trigger( System.Action callback )
        {
            if( callback != null )
            {
                callback();
            }
        }

        /// <summary>
        /// Trigger a callback bypassing the standard C# delegate call idiom.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        /// <param name="param"></param>
        public static void Trigger<T>( System.Action<T> callback, T param )
        {
            if( callback != null )
            {
                callback( param );
            }
        }

        /// <summary>
        /// Trigger a callback bypassing the standard C# delegate call idiom.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="callback"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        public static void Trigger<T1, T2>( System.Action<T1, T2> callback, T1 param1, T2 param2 )
        {
            if( callback != null )
            {
                callback( param1, param2 );
            }
        }
    }
}
