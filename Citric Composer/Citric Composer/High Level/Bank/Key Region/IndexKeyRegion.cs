using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Index key region. Contains a bunch of regions.
    /// </summary>
    public class IndexKeyRegion : IKeyRegion, IDictionary<sbyte, VelocityRegion> {

        /// <summary>
        /// Regions.
        /// </summary>
        private SortedDictionary<sbyte, VelocityRegion> regions;

        public IndexKeyRegion() {
            regions = new SortedDictionary<sbyte, VelocityRegion>();
        }

        /// <summary>
        /// Get the type of a key region.
        /// </summary>
        /// <returns>The key region type.</returns>
        public KeyRegionType GetKeyRegionType() {
            return KeyRegionType.Index;
        }

        public VelocityRegion this[sbyte key] { get => regions[key]; set => regions[key] = value; }

        public ICollection<sbyte> Keys => regions.Keys;

        public ICollection<VelocityRegion> Values => regions.Values;

        public int Count => regions.Count;

        public bool IsReadOnly => false;

        public void Add(sbyte key, VelocityRegion value) {
            regions.Add(key, value);
        }

        public void Add(KeyValuePair<sbyte, VelocityRegion> item) {
            regions.Add(item.Key, item.Value);
        }

        public void Clear() {
            regions.Clear();
        }

        public bool Contains(KeyValuePair<sbyte, VelocityRegion> item) {
            return regions.Contains(item);
        }

        public bool ContainsKey(sbyte key) {
            return regions.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<sbyte, VelocityRegion>[] array, int arrayIndex) {
            regions.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<sbyte, VelocityRegion>> GetEnumerator() {
            return regions.GetEnumerator();
        }

        public bool Remove(sbyte key) {
            return regions.Remove(key);
        }

        public bool Remove(KeyValuePair<sbyte, VelocityRegion> item) {
            return regions.Remove(item.Key);
        }

        public bool TryGetValue(sbyte key, out VelocityRegion value) {
            return regions.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return regions.GetEnumerator();
        }

    }

}
