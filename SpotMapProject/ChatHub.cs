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

        public void send(string message)
        {
            Clients.All.addMessage(message);
        }

    }
}