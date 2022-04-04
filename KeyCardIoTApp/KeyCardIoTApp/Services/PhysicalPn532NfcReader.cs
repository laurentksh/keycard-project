using System.Text;
using Iot.Device.Pn532;
using Iot.Device.Pn532.ListPassive;

namespace KeyCardIoTApp.Services;

public class PhysicalPn532NfcReader : INfcReader
{
    private readonly string _device = "/dev/ttyS0";

    public PhysicalPn532NfcReader()
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            _device = "COM7";
    }

    public Task<string> ReadAsync(CancellationToken token)
    {
        var pn532 = new Pn532(_device);

        byte[] dataBuffer = null!;
        while (!token.IsCancellationRequested)
        {
            dataBuffer = pn532.AutoPoll(5, 300, new PollingType[]
            {
                PollingType.Passive106kbpsISO144443_4B
            })!;

            if (dataBuffer is not null)
            {
                if (dataBuffer.Length >= 3)
                    break;
            }
        }

        return Task.FromResult(Encoding.UTF8.GetString(pn532.TryDecodeData106kbpsTypeB(dataBuffer.AsSpan()[3..])!.ApplicationData));
    }
}
