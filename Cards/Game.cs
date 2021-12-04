using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Cards.Deck;

namespace Cards
{
    public static class Game
    {
        public static void BeginGame()
        {
            Player player1 = new Player("Orbán Viktor");
            Player player2 = new Player("Gyurcsány Ferenc");
            Player player3 = new Player("Németh Szilárd");

            List<Player> players = new List<Player> { player1, player2, player3 };

            int playerIndex = 1;
            int turns = 1;

            StartingHandout(players);

            Console.WriteLine("Tromf: " + tromf);
            Console.WriteLine("--------> Starting hands");

            foreach (Player player in players)
            {
                Console.WriteLine("----> " + player.Name);

                foreach(Card card in player.hand)
                {
                    Console.WriteLine("--> " + card.ToString());
                }

            }

            players[0].GiveCardsTo(players[1]);

            // Gameloop
            while (true)
            {
                if (players.Count == 0)
                {
                    Console.WriteLine("Game has concluded in " + turns + " turns.");
                    break;
                }

                Player current = players[playerIndex];

                Player next = (playerIndex++ == players.Count) ? players[0] : players[playerIndex++];

                current.HitGivenCards();

                if(!current.hasPickedUp) current.GiveCardsTo(next);

                if (current.isOut)
                {
                    players.Remove(current);
                }

                playerIndex = (playerIndex++ == players.Count) ? 0 : playerIndex++;

                current.hasPickedUp = false;
                turns++;
                Console.Read();
            }

            Console.Read();
        }
    }
}
