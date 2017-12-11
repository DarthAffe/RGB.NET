using RGB.NET.Core;

namespace RGB.NET.Devices.Razer
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Id of a <see cref="T:RGB.NET.Core.Led" /> on a <see cref="T:RGB.NET.Devices.Razer.RazerRGBDevice" />.
    /// </summary>
    public class RazerLedId : ILedId
    {
        #region Properties & Fields

        internal int Index { get; }

        /// <inheritdoc />
        public IRGBDevice Device { get; }

        /// <inheritdoc />
        public bool IsValid => true;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RazerLedId"/> class.
        /// </summary>
        /// <param name="device">The <see cref="IRGBDevice"/> the <see cref="ILedId"/> belongs to.</param>
        /// <param name="index">The index representing the led-location in the grid.</param>
        public RazerLedId(IRGBDevice device, int index)
        {
            this.Device = device;
            this.Index = index;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the Id of this <see cref="RazerLedId"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that contains the Id of this <see cref="RazerLedId"/>. For example "Enter".</returns>
        public override string ToString() => Index.ToString();

        /// <summary>
        /// Tests whether the specified object is a <see cref="RazerLedId" /> and is equivalent to this <see cref="RazerLedId" />.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="RazerLedId" /> equivalent to this <see cref="RazerLedId" />; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!(obj is RazerLedId other)) return false;

            return (Index == other.Index) && Equals(Device, other.Device);
        }

        /// <summary>
        /// Returns a hash code for this <see cref="RazerLedId" />.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this <see cref="RazerLedId" />.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Index * 397) ^ (Device != null ? Device.GetHashCode() : 0);
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="RazerLedId" /> are equal.
        /// </summary>
        /// <param name="ledId1">The first <see cref="RazerLedId" /> to compare.</param>
        /// <param name="ledId2">The second <see cref="RazerLedId" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="ledId1" /> and <paramref name="ledId2" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(RazerLedId ledId1, RazerLedId ledId2) => ReferenceEquals(ledId1, null) ? ReferenceEquals(ledId2, null) : ledId1.Equals(ledId2);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="RazerLedId" /> are equal.
        /// </summary>
        /// <param name="ledId1">The first <see cref="RazerLedId" /> to compare.</param>
        /// <param name="ledId2">The second <see cref="RazerLedId" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="ledId1" /> and <paramref name="ledId2" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(RazerLedId ledId1, RazerLedId ledId2) => !(ledId1 == ledId2);

        #endregion
    }
}
