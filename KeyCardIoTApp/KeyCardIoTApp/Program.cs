using Microsoft.AspNetCore.SignalR.Client;

namespace KeyCardIoTApp;

public static class Program
{
    public static int Main(string[] args)
    {
        HubConnectionBuilder builder = new HubConnectionBuilder();
        builder.WithAutomaticReconnect();
        builder.WithUrl("");
        return 0;
    }
}