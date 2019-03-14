using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Range key region. Contains a list of velocity regions.
    /// </summary>
    public class RangeKeyRegion : IKeyRegion, IList<VelocityRegion> {

        /// <summary>
        /// List of ranges.
        /// </summary>
        private List<VelocityRegion> ranges;

        /// <summary>
        /// Start velocity.
        /// </summary>
        public int StartVelocity;

        /// <summary>
        /// End velocity.
        /// </summary>
        public int EndVelocity;

        /// <summary>
        /// Number of velocities.
        /// </summary>
        public int VelocityCount => (EndVelocity - StartVelocity + 1);

        public RangeKeyRegion() {
            ranges = new List<VelocityRegion>();
        }

        /// <summary>
        /// Get the type of a key region.
        /// </summary>
        /// <returns>The key region type.</returns>
        public KeyRegionType GetKeyRegionType() {
            return KeyRegionType.Range;
        }

        public VelocityRegion this[int index] { get => ranges[index]; set => ranges[index] = value; }

        public int Count => ranges.Count;

        public bool IsReadOnly => false;

        public void Add(VelocityRegion item) {
            ranges.Add(item);
        }

        public void Clear() {
            ranges.Clear();
        }

        public bool Contains(VelocityRegion item) {
            return ranges.Contains(item);
        }

        public void CopyTo(VelocityRegion[] array, int arrayIndex) {
            ranges.CopyTo(array, arrayIndex);
        }

        public IEnumerator<VelocityRegion> GetEnumerator() {
            return ranges.GetEnumerator();
        }

        public int IndexOf(VelocityRegion item) {
            return ranges.IndexOf(item);
        }

        public void Insert(int index, VelocityRegion item) {
            ranges.Insert(index, item);
        }

        public bool Remove(VelocityRegion item) {
            return ranges.Remove(item);
        }

        public void RemoveAt(int index) {
            ranges.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ranges.GetEnumerator();
        }
    }

}
