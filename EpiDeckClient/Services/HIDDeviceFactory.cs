using EpiDeckClient.Configuration;
using EpiDeckClient.Services.Interfaces;
using HidLibrary;
using System.Linq;

namespace EpiDeckClient.Services
{
    public class HidDeviceFactory : IHIDDeviceFactory
    {
        private readonly DeviceSettingsConfiguration _settings;
        public HidDeviceFactory(DeviceSettingsConfiguration settings)
        {
            _settings = settings;
        }

        public HidDevice CreateDevice()
        {
            // Use _settings.VendorId and _settings.ProductId to create the HidDevice instance
            var devices = HidDevices.Enumerate(_settings.VendorId, _settings.ProductId);
            var device = devices.FirstOrDefault();  // Or any other logic if multiple devices match.
            return device;
        }
    }
}