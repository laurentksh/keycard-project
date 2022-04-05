using KeyCardIoTApp.Models;
using KeyCardIoTApp.Services;
using Refit;

namespace KeyCardIoTApp;

public static class Program
{
    public static async Task Main(string[] args)
    {
        INfcReader nfcReader;
        IPunchService punchService = RestService.For<IPunchService>(
            Environment.GetEnvironmentVariable("WebServiceUri") ??
            (args.Length > 1 ? args[1] : null) ??
            "http://localhost:9000");

        if ((args.Length > 0 ? args[0] : null)?.ToLower() == "prd")
            nfcReader = new PhysicalPn532NfcReader();
        else
            nfcReader = new TestNfcReader();

        while (true)
        {
            string token;
            try
            {
                token = await nfcReader.ReadAsync(CancellationToken.None);
            } catch (Exception ex)
            {
                Console.WriteLine("Could not read NFC tag value: " + ex.ToString());
                continue;
            }

            PunchViewModel vm;
            try
            {
                vm = await punchService.RegisterPunch(token);
            }
            catch (ApiException ex)
            {
                Console.WriteLine("Api responded with error: " + ex.ToString());
                continue;
            }
            catch (HttpRequestException netEx)
            {
                Console.WriteLine("Could not reach remote server: " + netEx.ToString());
                continue;
            }

            Console.WriteLine($"Successfully registered punch for user {vm.UserId} ({vm.Id})");
        }
    }
}