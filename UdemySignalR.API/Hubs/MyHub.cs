using Microsoft.AspNetCore.SignalR;

namespace UdemySignalR.API.Hubs
{
    public class MyHub : Hub
    {
        //Clientlerin çağıracağu metotlar tanımlanır
        private static List<string> Names { get; set; } = new List<string>();
        private static int ClientCount { get; set; } = 0;
        public static int TeamCount { get; set; } = 7;
        public async Task SendName(string name)
        {
            if (Names.Count >= TeamCount)
            {
                await Clients.Caller.SendAsync("Error", $"Takım En Fazla {TeamCount} kişi olabilir");
            }
            else
            {

                Names.Add(name);
                await Clients.All.SendAsync("ReceiveName", name);
            }
        }
        public async Task GetNames()
        {
            await Clients.All.SendAsync("ReceiveNames", Names);
        }
        //Virtual Methods
        public async override Task OnConnectedAsync()
        {

            ClientCount++;
            //geriye kullanıcı sayısını döner
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);

            await base.OnConnectedAsync();
        }
        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            ClientCount--;
            //geriye kullanıcı sayısını döner
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);

            await base.OnDisconnectedAsync(exception);
        }

    }
}
