using System.Diagnostics;
using RGB.NET.Core;

namespace RGB.NET.Devices.Aura
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Id of a <see cref="T:RGB.NET.Core.Led" /> on a <see cref="T:RGB.NET.Devices.Aura.AuraRGBDevice" />.
    /// </summary>
    [DebuggerDisplay("{" + nameof(LedId) + "}")]
    public class AuraLedId : ILedId
    {
        #region Properties & Fields

        internal readonly AuraLedIds LedId;

        internal readonly int Index;

        /// <inheritdoc />
        public IRGBDevice Device { get; }

        /// <inheritdoc />
        public bool IsValid => LedId != AuraLedIds.Invalid;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuraLedId"/> class.
        /// </summary>
        /// <param name="device">The <see cref="IRGBDevice"/> the <see cref="ILedId"/> belongs to.</param>
        /// <param name="ledId">The <see cref="AuraLedId"/> of the represented <see cref="Led"/>.</param>
        public AuraLedId(IRGBDevice device, AuraLedIds ledId)
        {
            this.Device = device;
            this.LedId = ledId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuraLedId"/> class.
        /// </summary>
        /// <param name="device">The <see cref="IRGBDevice"/> the <see cref="ILedId"/> belongs to.</param>
        /// <param name="ledId">The <see cref="AuraLedId"/> of the represented <see cref="Led"/>.</param>
        /// <param name="index">The index in the mapping array of the device.</param>
        public AuraLedId(IRGBDevice device, AuraLedIds ledId, int index)
        {
            this.Device = device;
            this.LedId = ledId;
            this.Index = index;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the Id of this <see cref="AuraLedId"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that contains the Id of this <see cref="AuraLedId"/>. For example "Enter".</returns>
        public override string ToString() => LedId.ToString();

        /// <summary>
        /// Tests whether the specified object is a <see cref="AuraLedId" /> and is equivalent to this <see cref="AuraLedId" />.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="AuraLedId" /> equivalent to this <see cref="AuraLedId" />; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            AuraLedId compareLedId = obj as AuraLedId;
            if (ReferenceEquals(compareLedId, null))
                return false;

            if (ReferenceEquals(this, compareLedId))
                return true;

            if (GetType() != compareLedId.GetType())
                return false;

            return compareLedId.LedId == LedId;
        }

        /// <summary>
        /// Returns a hash code for this <see cref="AuraLedId" />.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this <see cref="AuraLedId" />.</returns>
        public override int GetHashCode() => LedId.GetHashCode();

        #endregion

        #region Operators

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="AuraLedId" /> are equal.
        /// </summary>
        /// <param name="ledId1">The first <see cref="AuraLedId" /> to compare.</param>
        /// <param name="ledId2">The second <see cref="AuraLedId" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="ledId1" /> and <paramref name="ledId2" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(AuraLedId ledId1, AuraLedId ledId2) => ReferenceEquals(ledId1, null) ? ReferenceEquals(ledId2, null) : ledId1.Equals(ledId2);

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="AuraLedId" /> are equal.
        /// </summary>
        /// <param name="ledId1">The first <see cref="AuraLedId" /> to compare.</param>
        /// <param name="ledId2">The second <see cref="AuraLedId" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="ledId1" /> and <paramref name="ledId2" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(AuraLedId ledId1, AuraLedId ledId2) => !(ledId1 == ledId2);

        #endregion
    }
}
