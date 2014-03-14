#region

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace NetComponentEntitySystem.Utilities
{
    /// <summary>
    ///     Helper class. Just a collection of types
    /// </summary>
    /// <typeparam name="T">Base type</typeparam>
    public class TypeGroup<T> : IEnumerable<Type>
    {
        private readonly HashSet<Type> _types;

        public TypeGroup()
        {
            _types = new HashSet<Type>();
        }

        #region IEnumerable<Type> Members

        public IEnumerator<Type> GetEnumerator()
        {
            return _types.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _types.GetEnumerator();
        }

        #endregion

        /// <summary>
        ///     Сhecks whether a Type in the group
        /// </summary>
        /// <typeparam name="T1">Specified type</typeparam>
        /// <returns></returns>
        public bool Contains<T1>()
            where T1 : T
        {
            return _types.Contains(typeof (T1));
        }

        /// <summary>
        ///     Includes passed types to group
        /// </summary>
        /// <typeparam name="T1">Type to include, must be derived from T</typeparam>
        /// <returns></returns>
        public TypeGroup<T> Add<T1>()
            where T1 : T
        {
            if (!_types.Add(typeof (T1)))
                throw new DuplicateTypeException();
            return this;
        }

        /// <summary>
        ///     Includes passed types to group
        /// </summary>
        public TypeGroup<T> Add<T1, T2>()
            where T1 : T
            where T2 : T
        {
            Add<T1>();
            Add<T2>();
            return this;
        }

        /// <summary>
        ///     Includes passed types to group
        /// </summary>
        public TypeGroup<T> Add<T1, T2, T3>()
            where T1 : T
            where T2 : T
            where T3 : T
        {
            Add<T1>();
            Add<T2>();
            Add<T3>();
            return this;
        }

        /// <summary>
        ///     Includes passed types to group
        /// </summary>
        public TypeGroup<T> Add<T1, T2, T3, T4>()
            where T1 : T
            where T2 : T
            where T3 : T
            where T4 : T
        {
            Add<T1>();
            Add<T2>();
            Add<T3>();
            Add<T4>();
            return this;
        }

        /// <summary>
        ///     Includes passed types to group
        /// </summary>
        public TypeGroup<T> Add<T1, T2, T3, T4, T5>()
            where T1 : T
            where T2 : T
            where T3 : T
            where T4 : T
            where T5 : T
        {
            Add<T1>();
            Add<T2>();
            Add<T3>();
            Add<T4>();
            Add<T5>();
            return this;
        }

        /// <summary>
        ///     Excludes passed type from group
        /// </summary>
        /// <typeparam name="T1">Type to exclude, must be derived from T</typeparam>
        /// <returns></returns>
        public TypeGroup<T> Remove<T1>()
            where T1 : T
        {
            _types.Remove(typeof (T1));
            return this;
        }

        /// <summary>
        ///     Excludes passed types from group
        /// </summary>
        public TypeGroup<T> Remove<T1, T2>()
            where T1 : T
            where T2 : T
        {
            Remove<T1>();
            Remove<T2>();
            return this;
        }

        /// <summary>
        ///     Excludes passed types from group
        /// </summary>
        public TypeGroup<T> Remove<T1, T2, T3>()
            where T1 : T
            where T2 : T
            where T3 : T
        {
            Remove<T1>();
            Remove<T2>();
            Remove<T3>();
            return this;
        }

        /// <summary>
        ///     Excludes passed types from group
        /// </summary>
        public TypeGroup<T> Remove<T1, T2, T3, T4>()
            where T1 : T
            where T2 : T
            where T3 : T
            where T4 : T
        {
            Remove<T1>();
            Remove<T2>();
            Remove<T3>();
            Remove<T4>();
            return this;
        }

        /// <summary>
        ///     Excludes passed types from group
        /// </summary>
        public TypeGroup<T> Remove<T1, T2, T3, T4, T5>()
            where T1 : T
            where T2 : T
            where T3 : T
            where T4 : T
            where T5 : T
        {
            Remove<T1>();
            Remove<T2>();
            Remove<T3>();
            Remove<T4>();
            Remove<T5>();
            return this;
        }
    }
}