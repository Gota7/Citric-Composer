using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Index instrument. Contains a bunch of regions.
    /// </summary>
    public class IndexInstrument : IInstrument, IDictionary<sbyte, IKeyRegion> {

        /// <summary>
        /// Regions.
        /// </summary>
        private SortedDictionary<sbyte, IKeyRegion> regions;

        public IndexInstrument() {
            regions = new SortedDictionary<sbyte, IKeyRegion>();
        }

        /// <summary>
        /// Get the type of an instrument.
        /// </summary>
        /// <returns>The instrument type.</returns>
        public InstrumentType GetInstrumentType() {
            return InstrumentType.Index;
        }

        public IKeyRegion this[sbyte key] { get => regions[key]; set => regions[key] = value; }

        public ICollection<sbyte> Keys => regions.Keys;

        public ICollection<IKeyRegion> Values => regions.Values;

        public int Count => regions.Count;

        public bool IsReadOnly => false;

        public void Add(sbyte key, IKeyRegion value) {
            regions.Add(key, value);
        }

        public void Add(KeyValuePair<sbyte, IKeyRegion> item) {
            regions.Add(item.Key, item.Value);
        }

        public void Clear() {
            regions.Clear();
        }

        public bool Contains(KeyValuePair<sbyte, IKeyRegion> item) {
            return regions.Contains(item);
        }

        public bool ContainsKey(sbyte key) {
            return regions.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<sbyte, IKeyRegion>[] array, int arrayIndex) {
            regions.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<sbyte, IKeyRegion>> GetEnumerator() {
            return regions.GetEnumerator();
        }

        public bool Remove(sbyte key) {
            return regions.Remove(key);
        }

        public bool Remove(KeyValuePair<sbyte, IKeyRegion> item) {
            return regions.Remove(item.Key);
        }

        public bool TryGetValue(sbyte key, out IKeyRegion value) {
            return regions.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return regions.GetEnumerator();
        }

    }

}
