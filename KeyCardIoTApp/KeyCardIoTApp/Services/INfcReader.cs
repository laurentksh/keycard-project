namespace KeyCardIoTApp.Services;

public interface INfcReader
{
    Task<string> ReadAsync(CancellationToken token);
}
