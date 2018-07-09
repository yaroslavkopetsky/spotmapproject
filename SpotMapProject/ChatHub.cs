using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
namespace SpotMapProject
{
    public class ChatHub : Hub
    {

        // to the clients every 3 seconds.
       
            public void Send(string name, string message)
            {
                // Call the broadcastMessage method to update clients.
                Clients.All.broadcastMessage(name, message);
            }
        
    }
}