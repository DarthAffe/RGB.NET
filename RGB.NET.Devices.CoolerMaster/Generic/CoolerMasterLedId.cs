using System.Diagnostics;
using RGB.NET.Core;

namespace RGB.NET.Devices.CoolerMaster
{
    /// <summary>
    /// Represents a Id of a <see cref="Led"/> on a <see cref="CoolerMasterRGBDevice"/>.
    /// </summary>
    [DebuggerDisplay("{" + nameof(LedId) + "}")]
    public class CoolerMasterLedId : ILedId
    {
        #region Properties & Fields

        internal readonly CoolerMasterLedIds LedId;

        internal readonly int Row;
        internal readonly int Column;

        /// <inheritdoc />
        public IRGBDevice Device { get; }

        /// <inheritdoc />
        public bool IsValid => LedId != CoolerMasterLedIds.Invalid;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoolerMasterLedId"/> class.
        /// </summary>
        /// <param name="device">The <see cref="IRGBDevice"/> the <see cref="ILedId"/> belongs to.</param>
        /// <param name="ledId">The <see cref="CoolerMasterLedId"/> of the represented <see cref="Led"/>.</param>
        public CoolerMasterLedId(IRGBDevice device, CoolerMasterLedIds ledId)
        {
            this.Device = device;
            this.LedId = ledId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoolerMasterLedId"/> class.
        /// </summary>
        /// <param name="device">The <see cref="IRGBDevice"/> the <see cref="ILedId"/> belongs to.</param>
        /// <param name="ledId">The <see cref="CoolerMasterLedId"/> of the represented <see cref="Led"/>.</param>
        /// <param name="row">The row in the mapping table of the device.</param>
        /// <param name="column">The column in the mapping table of the device.</param>
        public CoolerMasterLedId(IRGBDevice device, CoolerMasterLedIds ledId, int row, int column)
        {
            this.Device = device;
            this.LedId = ledId;
            this.Row = row;
            this.Column = column;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the Id of this <see cref="CoolerMasterLedId"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that contains the Id of this <see cref="CoolerMasterLedId"/>. For example "Enter".</returns>
        public override string ToString()
        {
            return LedId.ToString();
        }

        /// <summary>
        /// Tests whether the specified object is a <see cref="CoolerMasterLedId" /> and is equivalent to this <see cref="CoolerMasterLedId" />.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is a <see cref="CoolerMasterLedId" /> equivalent to this <see cref="CoolerMasterLedId" />; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            CoolerMasterLedId compareLedId = obj as CoolerMasterLedId;
            if (ReferenceEquals(compareLedId, null))
                return false;

            if (ReferenceEquals(this, compareLedId))
                return true;

            if (GetType() != compareLedId.GetType())
                return false;

            return compareLedId.LedId == LedId;
        }

        /// <summary>
        /// Returns a hash code for this <see cref="CoolerMasterLedId" />.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this <see cref="CoolerMasterLedId" />.</returns>
        public override int GetHashCode()
        {
            return LedId.GetHashCode();
        }

        #endregion

        #region Operators

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="CoolerMasterLedId" /> are equal.
        /// </summary>
        /// <param name="ledId1">The first <see cref="CoolerMasterLedId" /> to compare.</param>
        /// <param name="ledId2">The second <see cref="CoolerMasterLedId" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="ledId1" /> and <paramref name="ledId2" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(CoolerMasterLedId ledId1, CoolerMasterLedId ledId2)
        {
            return ReferenceEquals(ledId1, null) ? ReferenceEquals(ledId2, null) : ledId1.Equals(ledId2);
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="CoolerMasterLedId" /> are equal.
        /// </summary>
        /// <param name="ledId1">The first <see cref="CoolerMasterLedId" /> to compare.</param>
        /// <param name="ledId2">The second <see cref="CoolerMasterLedId" /> to compare.</param>
        /// <returns><c>true</c> if <paramref name="ledId1" /> and <paramref name="ledId2" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(CoolerMasterLedId ledId1, CoolerMasterLedId ledId2)
        {
            return !(ledId1 == ledId2);
        }

        #endregion
    }
}
