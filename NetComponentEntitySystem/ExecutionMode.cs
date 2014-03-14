namespace NetComponentEntitySystem
{
    public enum ExecutionMode
    {
        /// <summary>
        ///     System will execute one by one in foreach
        /// </summary>
        Synchronous,

        /// <summary>
        ///     Systems will execute As parallel foreach
        /// </summary>
        Asynchronous,

        /// <summary>
        ///     Run system in its own thread
        /// </summary>
        ThreadedFast,

        /// <summary>
        ///     Run system in its own thread synchronized to xna framerate
        /// </summary>
        ThreadedSync
    }
}