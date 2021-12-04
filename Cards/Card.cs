using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards
{
    public class Card
    {
        public CardLevel level;
        public CardType type;

        public Card() : this(CardLevel.VII, CardType.M) { } // Supply constructor with M VII card when no argument is present

        public Card(CardLevel level, CardType type)
        {
            this.level = level;
            this.type = type;
        }

        public override string ToString()
        {
            return type + " - " + level;
        }
    }
}
