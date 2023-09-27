using System;

namespace EpiDeckClient.Configuration
{
    public class DeviceSettingsConfiguration
    {
        public string VendorIdString { get; set; }
        public string ProductIdString { get; set; }

        public int VendorId => Convert.ToInt32(VendorIdString, 16);
        public int ProductId => Convert.ToInt32(ProductIdString, 16);
    }
}