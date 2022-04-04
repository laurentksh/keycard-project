namespace KeyCardIoTApp.Services;

public class TestNfcReader : INfcReader
{
    public Task<string> ReadAsync(CancellationToken token)
    {
        Console.WriteLine("Enter a NFC authentication token:");
        return Task.FromResult(Console.ReadLine()!);
    }
}
