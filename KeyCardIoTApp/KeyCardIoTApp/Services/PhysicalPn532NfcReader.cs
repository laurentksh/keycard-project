using System.Device.Spi;
using System.Text;
using Iot.Device.Pn532;
using Iot.Device.Pn532.ListPassive;
using Iot.Device.Rfid;

namespace KeyCardIoTApp.Services;

/*
 * TODO: Use the correct bus and pin for SPI communication.
 * 
 */
public class PhysicalPn532NfcReader : INfcReader
{
    //private readonly string _device = "/dev/ttyS0"; //or /dev/serial0

    public PhysicalPn532NfcReader()
    {
        /*if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            _device = "COM7";*/
    }

    public async Task<string> ReadAsync(CancellationToken cancellationToken)
    {
        var pn532 = new Pn532(SpiDevice.Create(new SpiConnectionSettings(0)
        {
            Mode = SpiMode.Mode0,
            DataFlow = DataFlow.LsbFirst
        }), 4);

        byte[] dataBuffer = null!;
        while (!cancellationToken.IsCancellationRequested)
        {
            dataBuffer = pn532.AutoPoll(5, 300, new PollingType[]
            {
                PollingType.Passive106kbpsISO144443_4A,
                PollingType.Passive106kbpsISO144443_4B
            })!;

            if (dataBuffer is not null)
            {
                if (dataBuffer.Length >= 3)
                    break;
            }

            await Task.Delay(200, cancellationToken);
        }

        string? token = null;
        
        if (pn532.TryDecode106kbpsTypeA(dataBuffer.AsSpan()[3..]) is Data106kbpsTypeA typeA && typeA.Ats != null)
            token = Encoding.UTF8.GetString(typeA.Ats);
        if (pn532.TryDecodeData106kbpsTypeB(dataBuffer.AsSpan()[3..]) is Data106kbpsTypeB typeB)
            token = Encoding.UTF8.GetString(typeB.ApplicationData);

        return token ?? throw new Exception("Could not read tag.");
    }
}
