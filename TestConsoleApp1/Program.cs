using Newtonsoft.Json.Linq;

var server = new MySignalR.Server();
server.MessageReceived += Server_MessageReceived;

void Server_MessageReceived(JObject? obj)
{
    Console.WriteLine("Message received: " + obj);
}

server.Start();
var client = new MySignalR.Client();
while (true)
{
    var hmm = Console.ReadLine();
    client.Send(new { naber="a", mudur="müdür"});
}