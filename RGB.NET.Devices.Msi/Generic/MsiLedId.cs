using System.Diagnostics;
using RGB.NET.Core;

namespace RGB.NET.Devices.Msi
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Id of a <see cref="T:RGB.NET.Core.Led" /> on a <see cref="T:RGB.NET.Devices.Msi.MsiRGBDevice" />.
    /// </summary>
    [DebuggerDisplay("{" + nameof(LedId) + "}")]
    public class MsiLedId : ILedId
    {
        #region Properties & Fields

        internal readonly MsiLedIds LedId;

        internal readonly int Index;

        /// <inheritdoc />
        public IRGBDevice Device { get; }

        /// <inheritdoc />
        public bool IsValid => LedId != MsiLedIds.Invalid;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MsiLedId"/> class.
        /// </summary>
        /// <param name="device">The <see cref="IRGBDevice"/> the <see cref="ILedId"/> belongs to.</param>
        /// <param name="ledId">The <see cref="MsiLedId"/> of the represented <see cref="Led"/>.</param>
        public MsiLedId(IRGBDevice device, MsiLedIds ledId)
        {
            this.Device = device;
            this.LedId = ledId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsiLedId"/> class.
        /// </summary>
        /// <param name="device">The <see cref="IRGBDevice"/> the <see cref="ILedId"/> belongs to.</param>
        /// <param name="ledId">The <see cref="MsiLedId"/> of the represented <see cref="Led"/>.</param>
        /// <param name="index">The index in the mapping array of the device.</param>
        public MsiLedId(IRGBDevice device, MsiLedIds ledId, int index)
        {
            this.Device = device;
            this.LedId = ledId;
            this.Index = index;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the Id of this <see cref="MsiLedId"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that contains the Id of this <see cref="MsiLedId"/>. For example "Enter".</returns>
        public override string ToString() => LedId.ToString();

        /// <summary>
        /// Tests whether the specified object is a <see cref="MsiLedId" /> and is equivalent to this <see cref="MsiLedId" />.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="MsiLedId" /> equivalent to this <see cref="MsiLedId" />; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            MsiLedId compareLedId = obj as MsiLedId;
            if (ReferenceEquals(compareLedId, null))
                return false;

            if (ReferenceEquals(this, compareLedId))
                return true;

            if (GetType() != compareLedId.GetType())
                return false;

            return compareLedId.LedId == LedId;
        }

        /// <summary>
        /// Returns a hash code for this <see cref="MsiLedId" />.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this <see cref="MsiLedId" />.</returns>
        public override int GetHashCode() => LedId.GetHashCode();

        #endregion

        #region Operators

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="MsiLedId" /> are equal.
        /// </summary>
        /// <param name="ledId1">The first <see cref="MsiLedId" /> to compare.</param>
        /// <param name="ledId2">The second <see cref="MsiLedId" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="ledId1" /> and <paramref name="ledId2" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(MsiLedId ledId1, MsiLedId ledId2) => ReferenceEquals(ledId1, null) ? ReferenceEquals(ledId2, null) : ledId1.Equals(ledId2);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="MsiLedId" /> are equal.
        /// </summary>
        /// <param name="ledId1">The first <see cref="MsiLedId" /> to compare.</param>
        /// <param name="ledId2">The second <see cref="MsiLedId" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="ledId1" /> and <paramref name="ledId2" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(MsiLedId ledId1, MsiLedId ledId2) => !(ledId1 == ledId2);

        #endregion
    }
}
