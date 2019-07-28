using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deckbuilder_RPG_Prototyper_App
{
    class deckCardCount
    {
        public string card = null;
        public int copies = 0;

        public deckCardCount()
        {

        }
    }


    class Deck
    {
        public string name = null;
        public List<Card> cards = null;
        public Deck(DeckLoader D, List<Card> allCards)
        {
            name = D.name;
            cards = new List<Card>();
            foreach (deckCardCount c in D.cards)
            {
                if (allCards.Exists(o => o.name == c.card))
                {
                    for (int i = c.copies; i > 0; i--)
                    {
                        cards.Add(allCards.Find(o => o.name == c.card));
                    }
                }
                else
                {
                    Console.WriteLine("Error! Card " + c.card + " was defined in deck " + D.name + " but no card with that name was found!");
                }
            }
        }
    }

    class DeckLoader
    {
        public string name = null;
        public deckCardCount[] cards = null;
    }


}
