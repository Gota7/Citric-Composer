using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitraFileLoader {

    /// <summary>
    /// Range instrument. Contains a list of key regions.
    /// </summary>
    public class RangeInstrument : IInstrument, IList<IKeyRegion> {

        /// <summary>
        /// List of ranges.
        /// </summary>
        private List<IKeyRegion> ranges;

        /// <summary>
        /// Start note.
        /// </summary>
        public int StartNote;

        /// <summary>
        /// End note.
        /// </summary>
        public int EndNote;

        /// <summary>
        /// Get the note count.
        /// </summary>
        public int NoteCount => (EndNote - StartNote + 1);

        public RangeInstrument() {
            ranges = new List<IKeyRegion>();
        }

        /// <summary>
        /// Get the type of an instrument.
        /// </summary>
        /// <returns>The instrument type.</returns>
        public InstrumentType GetInstrumentType() {
            return InstrumentType.Range;
        }

        public IKeyRegion this[int index] { get => ranges[index]; set => ranges[index] = value; }

        public int Count => ranges.Count;

        public bool IsReadOnly => false;

        public void Add(IKeyRegion item) {
            ranges.Add(item);
        }

        public void Clear() {
            ranges.Clear();
        }

        public bool Contains(IKeyRegion item) {
            return ranges.Contains(item);
        }

        public void CopyTo(IKeyRegion[] array, int arrayIndex) {
            ranges.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IKeyRegion> GetEnumerator() {
            return ranges.GetEnumerator();
        }

        public int IndexOf(IKeyRegion item) {
            return ranges.IndexOf(item);
        }

        public void Insert(int index, IKeyRegion item) {
            ranges.Insert(index, item);
        }

        public bool Remove(IKeyRegion item) {
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
