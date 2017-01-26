using System.Diagnostics;
using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair
{
    /// <summary>
    /// Represents a Id of a <see cref="Led"/> on a <see cref="CorsairRGBDevice"/>.
    /// </summary>
    [DebuggerDisplay("{" + nameof(LedId) + "}")]
    public class CorsairLedId : ILedId
    {
        #region Properties & Fields

        internal readonly CorsairLedIds LedId;

        /// <inheritdoc />
        public IRGBDevice Device { get; }

        /// <inheritdoc />
        public bool IsValid => LedId != CorsairLedIds.Invalid;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairLedId"/> class.
        /// </summary>
        /// <param name="device">The <see cref="IRGBDevice"/> the <see cref="ILedId"/> belongs to.</param>
        /// <param name="ledId">The <see cref="CorsairLedId"/> of the represented <see cref="Led"/>.</param>
        public CorsairLedId(IRGBDevice device, CorsairLedIds ledId)
        {
            this.Device = device;
            this.LedId = ledId;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the Id of this <see cref="CorsairLedId"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that contains the Id of this <see cref="CorsairLedId"/>. For example "Enter".</returns>
        public override string ToString()
        {
            return LedId.ToString();
        }

        /// <summary>
        /// Tests whether the specified object is a <see cref="CorsairLedId" /> and is equivalent to this <see cref="CorsairLedId" />.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="CorsairLedId" /> equivalent to this <see cref="CorsairLedId" />; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            CorsairLedId compareLedId = obj as CorsairLedId;
            if (ReferenceEquals(compareLedId, null))
                return false;

            if (ReferenceEquals(this, compareLedId))
                return true;

            if (GetType() != compareLedId.GetType())
                return false;

            return compareLedId.LedId == LedId;
        }

        /// <summary>
        /// Returns a hash code for this <see cref="CorsairLedId" />.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this <see cref="CorsairLedId" />.</returns>
        public override int GetHashCode()
        {
            return LedId.GetHashCode();
        }

        #endregion

        #region Operators

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="CorsairLedId" /> are equal.
        /// </summary>
        /// <param name="ledId1">The first <see cref="CorsairLedId" /> to compare.</param>
        /// <param name="ledId2">The second <see cref="CorsairLedId" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="ledId1" /> and <paramref name="ledId2" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(CorsairLedId ledId1, CorsairLedId ledId2)
        {
            return ReferenceEquals(ledId1, null) ? ReferenceEquals(ledId2, null) : ledId1.Equals(ledId2);
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="CorsairLedId" /> are equal.
        /// </summary>
        /// <param name="ledId1">The first <see cref="CorsairLedId" /> to compare.</param>
        /// <param name="ledId2">The second <see cref="CorsairLedId" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="ledId1" /> and <paramref name="ledId2" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(CorsairLedId ledId1, CorsairLedId ledId2)
        {
            return !(ledId1 == ledId2);
        }

        #endregion
    }
}
