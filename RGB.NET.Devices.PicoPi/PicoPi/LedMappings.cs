using RGB.NET.Core;

namespace RGB.NET.Devices.PicoPi
{
    public static class LedMappings
    {
        #region Properties & Fields

        public static LedMapping<int> StripeMapping = new();

        #endregion

        #region Constructors

        static LedMappings()
        {
            for (int i = 0; i < 255; i++)
                StripeMapping.Add(LedId.LedStripe1 + i, i);
        }

        #endregion
    }
}
