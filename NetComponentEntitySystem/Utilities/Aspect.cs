#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace NetComponentEntitySystem.Utilities
{
    public class Aspect<T>
    {
        private readonly List<Tuple<PredicateType, IEnumerable<T>>> _linq;

        public Aspect()
        {
            _linq = new List<Tuple<PredicateType, IEnumerable<T>>>();
        }

        public Aspect(IEnumerable<T> all)
            : this()
        {
            _linq.Add(new Tuple<PredicateType, IEnumerable<T>>(PredicateType.AllOf, all));
        }

        public Aspect<T> AllOf(IEnumerable<T> group)
        {
            _linq.Add(new Tuple<PredicateType, IEnumerable<T>>(PredicateType.AllOf, group));
            return this;
        }

        public Aspect<T> AllOf(params T[] group)
        {
            _linq.Add(new Tuple<PredicateType, IEnumerable<T>>(PredicateType.AllOf, group));
            return this;
        }

        public Aspect<T> AnyOf(IEnumerable<T> group)
        {
            _linq.Add(new Tuple<PredicateType, IEnumerable<T>>(PredicateType.AnyOf, group));
            return this;
        }

        public Aspect<T> AnyOf(params T[] group)
        {
            _linq.Add(new Tuple<PredicateType, IEnumerable<T>>(PredicateType.AnyOf, group));
            return this;
        }

        public Aspect<T> OneOf(IEnumerable<T> group)
        {
            _linq.Add(new Tuple<PredicateType, IEnumerable<T>>(PredicateType.OneOf, group));
            return this;
        }

        public Aspect<T> OneOf(params T[] group)
        {
            _linq.Add(new Tuple<PredicateType, IEnumerable<T>>(PredicateType.OneOf, group));
            return this;
        }

        public Aspect<T> ExceptOf(IEnumerable<T> group)
        {
            _linq.Add(new Tuple<PredicateType, IEnumerable<T>>(PredicateType.ExceptOf, group));
            return this;
        }

        public Aspect<T> ExceptOf(params T[] group)
        {
            _linq.Add(new Tuple<PredicateType, IEnumerable<T>>(PredicateType.ExceptOf, group));
            return this;
        }

        private static bool SingeQuery(PredicateType predicateType, IEnumerable<T> group, Func<T, bool> predicate)
        {
            switch (predicateType)
            {
                case PredicateType.AllOf:
                    return group.All(predicate);
                case PredicateType.AnyOf:
                    return group.Any(predicate);
                case PredicateType.OneOf:
                    return group.Where(predicate).Count() == 1;
                case PredicateType.ExceptOf:
                    return !group.Any(predicate);
                default:
                    throw new Exception("Unknown predicate type");
            }
        }

        public bool Query(Func<T, bool> predicate)
        {
            return _linq.All(tuple => SingeQuery(tuple.Item1, tuple.Item2, predicate));
        }

        public bool Query(IEnumerable<T> collection)
        {
            return Query(collection.Contains);
        }
    }
}