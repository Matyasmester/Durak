using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Cards.Deck;
using static Cards.GameState;

namespace Cards
{
    public class Player
    {
        public HashSet<Card> hand = new HashSet<Card>();

        public string Name;

        public bool isOut = false;
        public bool hasPickedUp = false;

        private Dictionary<Card, int> cardScores = new Dictionary<Card, int>();

        HashSet<Card> toGive = new HashSet<Card>();
        HashSet<Card> hitWith = new HashSet<Card>();

        private const int TromfBaseScore = 7;
        private const int TromfAceBaseScore = 100;

        public Player(string Name)
        {
            this.Name = Name;
        }

        private Card GetLowestScoreCard()
        {
            return cardScores.First(x => x.Value == cardScores.Values.Min()).Key;
        }

        private void CheckIfOut()
        {
            isOut = hand.Count == 0;
        }

        private void GetCardScores()
        {
            int score = 0;

            foreach (Card card in hand)
            {
                if (cardScores.ContainsKey(card)) continue;

                score += (int)card.level;
                if (tromf == card.type) score += TromfBaseScore;

                // Most valuable card, keep it till the end
                if (tromf == card.type && card.level == CardLevel.Ace) score += TromfAceBaseScore;

                cardScores.Add(card, score);

                score = 0;
            }
        }
        
        private void PickUpRemainingCards()
        {
            Console.WriteLine(this.Name + " is picking up cards: ");

            foreach (Card card in givenCards)
            {
                Console.Write(card.type + " - " + card.level + "   ");
                hand.Add(card);
            }
            givenCards.Clear();

            hasPickedUp = true;
        }

        public void GiveCardsTo(Player player)
        {
            if (givenCards.Any()) throw new WrongMoveException("Attempted to send cards while there is still some given to him.");

            GetCardScores();

            int playerHandSize = player.hand.Count;
            int sendlimit = 1;

            if (playerHandSize >= 5) sendlimit = 5;
            if (playerHandSize < 5 && playerHandSize >= 3) sendlimit = 3;

            // Recognize and consider giving pairs
            foreach (Card first in hand)
            {
                // Skip if enemy doesnt have enough cards
                if (sendlimit == 1) break;

                foreach (Card second in hand)
                {
                    if(first.level == second.level && !toGive.Contains(first) && !toGive.Contains(second) && first.type != second.type)
                    {
                        cardScores.TryGetValue(first, out int firstScore);
                        cardScores.TryGetValue(second, out int secondScore);

                        int totalScore = firstScore + secondScore;

                        if(tromf != first.type && tromf != second.type)
                        {
                            toGive.Add(first);
                            toGive.Add(second);

                            cardScores.Remove(first);
                            cardScores.Remove(second);

                            if (sendlimit == 3) break;
                        }

                        if(tromf == first.type || tromf == second.type && totalScore <= 33)
                        {
                            toGive.Add(first);
                            toGive.Add(second);

                            cardScores.Remove(first);
                            cardScores.Remove(second);

                            if (sendlimit == 3) break;
                        }
                    }
                }
            }

            // Reinforcement is the card with lowest score
            Card reinforcement = GetLowestScoreCard();
            toGive.Add(reinforcement);

            Console.WriteLine("-> " + this.Name + " is giving " + toGive.Count + " cards to " + player.Name);

            foreach(Card card in toGive)
            {
                Console.WriteLine("> " + card.type + "   " + card.level);
            }

            int sent = toGive.Count;
            if (sent % 2 == 0) throw new WrongMoveException($"Attempted to send {sent} cards.");

            foreach (Card card in toGive)
            {
                givenCards.Add(card);
            }

            hand.Remove(reinforcement);
            hand.RemoveWhere(x => toGive.Contains(x));

            if(hand.Count > 5) DrawCards(5 - (hand.Count - toGive.Count));

            toGive.Clear();

            CheckIfOut();
        }

        public void HitGivenCards()
        {
            if (!givenCards.Any()) return;

            GetCardScores();

            foreach (Card given in givenCards)
            {
                foreach (Card inHand in hand)
                {
                    cardScores.TryGetValue(inHand, out int score);

                    // Matched types and also greater level, meaning we can hit it easy
                    if(given.type == inHand.type || inHand.type == tromf && given.level < inHand.level)
                    {
                        Console.WriteLine("> " + this.Name + " hitting " + given.ToString() + " with " + inHand.ToString());
                        givenCards.Remove(given);
                        hitWith.Add(inHand);
                    }

                    if(given.type != tromf && inHand.type == tromf && score < TromfAceBaseScore)
                    {
                        Console.WriteLine("> " + this.Name + " hitting " + given.ToString() + " with " + inHand.ToString());
                        givenCards.Remove(given);
                        hitWith.Add(inHand);
                    }
                }
            }

            hand.RemoveWhere(x => hitWith.Contains(x));

            if (givenCards.Any()) PickUpRemainingCards();
            CheckIfOut();
        }

        public void DrawCards(int count)
        {
            if (cards.Count == 0) return;

            Console.WriteLine("->" + this.Name + " is drawing cards: ");

            for (int i = 0; i < count; i++)
            {
                Card first = cards.First();
                this.hand.Add(first);
                cards.Remove(first);

                Console.WriteLine(">" + first.type + " - " + first.level);
            }
        }
    }
}
