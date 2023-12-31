using System;
using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair;

[StructLayout(LayoutKind.Sequential)]
public readonly struct CorsairLedId : IComparable<CorsairLedId>, IEquatable<CorsairLedId>
{
    #region Properties & Fields

    public readonly uint Id;

    public CorsairLedGroup Group => (CorsairLedGroup)(Id >> 16);
    public uint Index => Id & 0x0000FFFF;

    #endregion

    #region Constructors

    public CorsairLedId(uint id)
    {
        this.Id = id;
    }

    public CorsairLedId(CorsairLedGroup group, CorsairLedIdKeyboard id)
        : this(group, (int)id)
    { }

    public CorsairLedId(CorsairLedGroup group, int index)
    {
        Id = (((uint)group) << 16) | (uint)index;
    }

    #endregion

    #region Methods

    public int CompareTo(CorsairLedId other) => Id.CompareTo(other.Id);

    public bool Equals(CorsairLedId other) => Id == other.Id;

    public override bool Equals(object? obj) => obj is CorsairLedId other && Equals(other);

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(CorsairLedId left, CorsairLedId right) => left.Id == right.Id;
    public static bool operator !=(CorsairLedId left, CorsairLedId right) => !(left == right);
    public static bool operator <(CorsairLedId left, CorsairLedId right) => left.Id < right.Id;
    public static bool operator <=(CorsairLedId left, CorsairLedId right) => left.Id <= right.Id;
    public static bool operator >(CorsairLedId left, CorsairLedId right) => left.Id > right.Id;
    public static bool operator >=(CorsairLedId left, CorsairLedId right) => left.Id >= right.Id;

    #endregion
}