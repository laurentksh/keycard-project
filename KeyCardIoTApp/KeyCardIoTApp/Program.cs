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
            args.GetValue(1) as string ??
            "http://localhost:9000");

        if ((args.GetValue(0) as string)?.ToLower() == "prd")
            nfcReader = new TestNfcReader();
        else
            nfcReader = new PhysicalPn532NfcReader();

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
                Console.WriteLine("Could not reach server: " + ex.ToString());
                continue;
            }

            Console.WriteLine($"Successfully registered punch for user {vm.UserId} ({vm.Id})");
        }
    }
}