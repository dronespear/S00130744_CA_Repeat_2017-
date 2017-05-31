using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;


namespace GameHub
{
    public class MyHub : Hub
    {
        //static List<string> listOfAddedPlayers;
        //static string player1;
        //static string player2;

        //public override Task OnConnected()
        //{
        //    playerCount++;
        //    if (playerCount == 1)
        //    {
        //        player1 = Context.ConnectionId;
        //    }
        //    if (playerCount == 2)
        //    {
        //        player2 = Context.ConnectionId;
        //        Clients.All.sendCount(playerCount);
        //    }


        //    return base.OnConnected();
        //}



        public void Hello()
        {
            Clients.All.hello();
        }
        public void WelcomeMessage(int level)
        {
            if (level == 0)
            {
                var msgText = @"Welcome to Level 1!!
<--Collect all items to win, 
but watch out for the Rabbits-->";
                Clients.Caller.welcomeMessage(msgText);
            }
            if (level == 1)
            {
                var msgText2 = "Welcome to Level 2!!";
                Clients.Caller.welcomeMessage(msgText2);
            }
            if (level == 2)
            {
                var msgText3 = "Welcome to the Final Level!!";
                Clients.Caller.welcomeMessage(msgText3);
            }
            //Clients.All.welcomeMessage(msgText);

            //This used so that text variable was only broadcast to each caller as they connected.
            //replace it with the All command above once the multiplayer synced start is developed.
            //Clients.Caller.welcomeMessage(msgText);
        }
        public void PlayerOne()
        {
            var profileText = @"  Player Name: John Smith

     Games          High Score  XP      Date
     Pong              481      40     14/09/16
     SpaceShooter      250      30     20/10/16
     Racing            300      20     13/10/15
     AxeAttack         ---      --     --/--/--";

            //Todo: Replace this and PlayerTwo with connection contextID once player number and user are resolved.
            //Also replace hard coded results with live data from achievements interface.
            Clients.Caller.sendPlayerProfile(profileText);
        }

        public void PlayerTwo()
        {
            var profileText = @"  Player Name: Jane Doe

    Games          High Score  XP      Date
    Pong              800      70     24/09/16
    SpaceShooter      350      20     10/08/16
    Racing             50       4     14/12/15  
    AxeAttack         ---      --     --/--/--";

            //Todo: Replace this and PlayerTwo with connection contextID once player number and user are resolved.
            //Also replace hard coded results with live data from achievements interface.

            Clients.Caller.sendPlayerProfile(profileText);
        }

        public void PlayerJoin(String currentPlayer)
        {
            //On the invoke command in the lobby this passes the currentUser's context connectionId to other users in the lobby.
            var currentUser = Context.ConnectionId;
            
            Clients.Others.sendOtherId(currentUser);
            Clients.Others.lobbyMessage(currentPlayer + " has joined the game!!");

            //Below group method didnt work due to new instances being created on each hub request.
            //Groups.Add(currentUser, groupName);
            //int playerCount = 0;
            //var currentUser = Context.ConnectionId;
            //while (listOfAddedPlayers.Count < 2)
            //{
            //    var matchingUsers = listOfAddedPlayers.Any(str => str.Contains(currentUser));
            //    if(matchingUsers == false)
            //    {
            //        Groups.Add(Context.ConnectionId, groupName);
            //        listOfAddedPlayers.Add(Context.ConnectionId);
            //        playerCount = listOfAddedPlayers.Count;
            //        Clients.Caller.welcomeMessage(playerCount.ToString());
            //    }

            //}

            //    if (playerCount == 2)
            //    {
            //        Clients.Group(groupName).sendOtherId(playerCount);
            //    }

            //    //Clients.All.sendCount(playerCount);

        }

   }
}