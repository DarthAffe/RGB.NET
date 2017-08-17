using System.Diagnostics;
using RGB.NET.Core;

namespace RGB.NET.Devices.Novation
{
    /// <summary>
    /// Represents a Id of a <see cref="Led"/> on a <see cref="NovationRGBDevice"/>.
    /// </summary>
    [DebuggerDisplay("{" + nameof(LedId) + "}")]
    public class NovationLedId : ILedId
    {
        #region Properties & Fields

        internal readonly NovationLedIds LedId;

        /// <inheritdoc />
        public IRGBDevice Device { get; }

        /// <inheritdoc />
        public bool IsValid => LedId != NovationLedIds.Invalid;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NovationLedId"/> class.
        /// </summary>
        /// <param name="device">The <see cref="IRGBDevice"/> the <see cref="ILedId"/> belongs to.</param>
        /// <param name="ledId">The <see cref="NovationLedId"/> of the represented <see cref="Led"/>.</param>
        public NovationLedId(IRGBDevice device, NovationLedIds ledId)
        {
            this.Device = device;
            this.LedId = ledId;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the Id of this <see cref="NovationLedId"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that contains the Id of this <see cref="NovationLedId"/>. For example "Enter".</returns>
        public override string ToString()
        {
            return LedId.ToString();
        }

        /// <summary>
        /// Tests whether the specified object is a <see cref="NovationLedId" /> and is equivalent to this <see cref="NovationLedId" />.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="NovationLedId" /> equivalent to this <see cref="NovationLedId" />; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            NovationLedId compareLedId = obj as NovationLedId;
            if (ReferenceEquals(compareLedId, null))
                return false;

            if (ReferenceEquals(this, compareLedId))
                return true;

            if (GetType() != compareLedId.GetType())
                return false;

            return compareLedId.LedId == LedId;
        }

        /// <summary>
        /// Returns a hash code for this <see cref="NovationLedId" />.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this <see cref="NovationLedId" />.</returns>
        public override int GetHashCode()
        {
            return LedId.GetHashCode();
        }

        #endregion

        #region Operators

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="NovationLedId" /> are equal.
        /// </summary>
        /// <param name="ledId1">The first <see cref="NovationLedId" /> to compare.</param>
        /// <param name="ledId2">The second <see cref="NovationLedId" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="ledId1" /> and <paramref name="ledId2" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(NovationLedId ledId1, NovationLedId ledId2)
        {
            return ReferenceEquals(ledId1, null) ? ReferenceEquals(ledId2, null) : ledId1.Equals(ledId2);
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="NovationLedId" /> are equal.
        /// </summary>
        /// <param name="ledId1">The first <see cref="NovationLedId" /> to compare.</param>
        /// <param name="ledId2">The second <see cref="NovationLedId" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="ledId1" /> and <paramref name="ledId2" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(NovationLedId ledId1, NovationLedId ledId2)
        {
            return !(ledId1 == ledId2);
        }

        #endregion
    }
}
