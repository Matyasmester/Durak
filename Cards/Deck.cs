using System;
using System.Collections.Generic;
using System.Linq;

namespace Cards
{
    public static class Deck
    {
        private const short max_cap = 32;
        public static List<Card> cards = new List<Card>(max_cap);

        public static CardType tromf;

        private static Random rand = new Random();

        static Deck()
        {
            tromf = (CardType)rand.Next(3);

            Fill();
        }

        public static void Add(Card card)
        {
            cards.Add(card);
        }

        private static void Shuffle()
        {
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                Card value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }

        /*public int GetRandom(int max)
        {
            long miliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            return (int)(miliseconds % (max + 1));                                   a downcast miatt szar.
        }*/

        private static void Fill()
        {
            foreach (CardType type in (CardType[]) Enum.GetValues(typeof(CardType)))
            {
                foreach (CardLevel level in (CardLevel[]) Enum.GetValues(typeof(CardLevel)))
                {
                    Add(new Card(level, type));
                }
            }
        }

        public static void StartingHandout(List<Player> players)
        {
            Shuffle();
            foreach (Player player in players)
            {
                for (int i = 0; i < 5; i++)
                {
                    Card first = cards.First();
                    player.hand.Add(first);
                    cards.Remove(first);
                }
            }
        }
    }
}
