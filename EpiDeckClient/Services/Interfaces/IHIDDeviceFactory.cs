using HidLibrary;

namespace EpiDeckClient.Services.Interfaces
{
    public interface IHIDDeviceFactory
    {
        HidDevice CreateDevice();
    }
}