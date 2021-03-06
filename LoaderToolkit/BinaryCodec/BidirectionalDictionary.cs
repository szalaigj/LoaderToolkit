﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryCodec
{
    /// <summary>
    /// This inplementation is based on:
    /// http://stackoverflow.com/questions/10966331/two-way-bidirectional-dictionary-in-c/10966684#10966684
    /// and it is extended.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class BidirectionalDictionary<T1,T2>
    {
        private Dictionary<T1, T2> _forward;
        private Dictionary<T2, T1> _reverse;

        public BidirectionalDictionary()
        {
            this._forward = new Dictionary<T1, T2>();
            this._reverse = new Dictionary<T2, T1>();
            this.Forward = new Indexer<T1, T2>(_forward);
            this.Reverse = new Indexer<T2, T1>(_reverse);
        }

        public BidirectionalDictionary(IEqualityComparer<T2> equalityComparer)
        {
            this._forward = new Dictionary<T1, T2>();
            this._reverse = new Dictionary<T2, T1>(equalityComparer);
            this.Forward = new Indexer<T1, T2>(_forward);
            this.Reverse = new Indexer<T2, T1>(_reverse);
        }

        public class Indexer<T3, T4>
        {
            private Dictionary<T3, T4> _dictionary;
            public Indexer(Dictionary<T3, T4> dictionary)
            {
                _dictionary = dictionary;
            }
            public T4 this[T3 index]
            {
                get { return _dictionary[index]; }
                set { _dictionary[index] = value; }
            }
            public Dictionary<T3, T4>.ValueCollection Values
            {
                get { return _dictionary.Values; }
            }
            public bool TryGetValue(T3 key, out T4 value)
            {
                return _dictionary.TryGetValue(key, out value);
            }
        }

        public void Add(T1 t1, T2 t2)
        {
            _forward.Add(t1, t2);
            _reverse.Add(t2, t1);
        }

        public Indexer<T1, T2> Forward { get; private set; }
        public Indexer<T2, T1> Reverse { get; private set; }
    }
}
