using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Deckbuilder_RPG_Prototyper_App
{
    class Prototyper
    {
        public static List<Card> cardList;
        public static List<DeckLoader> decksToLoad;
        public static List<Deck> decks;

        public Prototyper()
        {
            cardList = JsonConvert.DeserializeObject<List<Card>>(File.ReadAllText(@"cards.json"));
            decksToLoad = JsonConvert.DeserializeObject<List<DeckLoader>>(File.ReadAllText(@"decks.json"));
            decks = new List<Deck>();

            foreach (DeckLoader dl in decksToLoad)
            {
                decks.Add(new Deck(dl, cardList));
            }
        }

        public void mainMenu()
        {
            Console.WriteLine(
                "Hello.  Here are some options." + System.Environment.NewLine + System.Environment.NewLine +
                "1. Select A Deck" + System.Environment.NewLine +
                "Q. Quit" + System.Environment.NewLine
                );
            string line = Console.ReadLine();
            switch (line)
            {
                case "Q":
                    break;
                case "1":
                    Console.Clear();
                    chooseDeck();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input.  Try again." + System.Environment.NewLine);
                    System.Threading.Thread.Sleep(500);
                    mainMenu();
                    break;
            }
        }
        public void chooseDeck()
        {

            Console.WriteLine(
                "Viewing Decks.  Enter number to see more options." + System.Environment.NewLine +
                "Enter q to go back and Q to exit the program." + System.Environment.NewLine
                );
            int deckCount = 0;
            foreach (Deck d in decks)
            {
                deckCount++;
                Console.WriteLine(deckCount + ". " + d.name);
            }
            string line = Console.ReadLine();
            int number = -1;
            Int32.TryParse(line, out number);
            if (number > 0)
            {
                if (number <= decks.Count)
                {
                    Console.Clear();
                    deckMenu(number - 1);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input.  Try again." + System.Environment.NewLine);
                    System.Threading.Thread.Sleep(500);
                    chooseDeck();
                }
            }
            else
            {
                switch (line)
                {
                    case "Q":
                        break;
                    case "q":
                        Console.Clear();
                        mainMenu();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid input.  Try again." + System.Environment.NewLine);
                        System.Threading.Thread.Sleep(500);
                        chooseDeck();
                        break;


                }
            }


        }
        public void deckMenu(int activeDeckIndex)
        {
            Console.WriteLine(
            "Viewing " + decks[activeDeckIndex].name + " deck." + System.Environment.NewLine +
            "Enter q to go back and Q to exit the program." + System.Environment.NewLine + System.Environment.NewLine +
            "1. Start playing a game using this deck." + System.Environment.NewLine +
            "2. Examine deck list."
            );

            string line = Console.ReadLine();

            switch (line)
            {
                case "Q":
                    break;
                case "q":
                    Console.Clear();
                    chooseDeck();
                    break;
                case "1":
                    Console.Clear();
                    playGame(activeDeckIndex);
                    break;
                case "2":
                    Console.Clear();
                    showDeckList(activeDeckIndex);
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input.  Try again." + System.Environment.NewLine);
                    System.Threading.Thread.Sleep(500);
                    deckMenu(activeDeckIndex);
                    break;
            }

        }
        public void showDeckList(int activeDeckIndex)
        {
            Console.WriteLine(
            "Viewing decklist for " + decks[activeDeckIndex].name + " deck." + System.Environment.NewLine +
            "Enter q to go back and Q to exit the program." + System.Environment.NewLine);

            foreach (deckCardCount c in decksToLoad[activeDeckIndex].cards)
            {
                Console.WriteLine(c.card + " x" + c.copies);
            }

            string line = Console.ReadLine();

            switch (line)
            {
                case "Q":
                    break;
                case "q":
                    Console.Clear();
                    deckMenu(activeDeckIndex);
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input.  Try again." + System.Environment.NewLine);
                    System.Threading.Thread.Sleep(500);
                    showDeckList(activeDeckIndex);
                    break;
            }
        }
        public void playGame(int activeDeckIndex)
        {
            bool endGame = false;
            List<Card> deckInPlay = new List<Card>(decks[activeDeckIndex].cards);
            List<Card> hand = new List<Card>();
            List<Card> discardPile = new List<Card>();
            List<string> turnLog = new List<string>();
            deckInPlay.Shuffle();
            turnLog.Add("Started play with " + decks[activeDeckIndex].name + " deck.");
            turnLog.Add("Shuffled deck.");
            while (!endGame)
            {
                Console.Clear();
                foreach (string s in turnLog)
                {
                    Console.WriteLine(s);
                }
                Console.WriteLine("--------------------------------------");
                Console.WriteLine( "Enter q to go back and Q to exit the program." + System.Environment.NewLine +
                     "To examine or interact with a card in your hand, enter its corresponding number." + System.Environment.NewLine +
                     "R. Reset game and shuffle deck." + System.Environment.NewLine +
                     "d. View Discard Pile [" + discardPile.Count + " cards]." + System.Environment.NewLine +
                     "c. Draw a Card [" + deckInPlay.Count + " remaining]." + System.Environment.NewLine +
                     "D. Reveal and View Deck." + System.Environment.NewLine + System.Environment.NewLine +
                     "Your hand:"  + System.Environment.NewLine);
                if (hand.Count > 0)
                {
                    int numCardsInHand = 0;
                    foreach (Card c in hand)
                    {
                        numCardsInHand++;
                        Console.WriteLine(numCardsInHand + ". " + c.name);
                    }
                } else
                {
                    Console.WriteLine("Your hand is currently empty");
                }
                string line = Console.ReadLine();
                int number = -1;
                Int32.TryParse(line, out number);
                if (number > 0)
                {
                    if (number <= hand.Count)
                    {
                        Console.Clear();
                        interactWithCardInHand(discardPile, hand, turnLog, number - 1);
                    }
                    else
                    {
                        Console.Clear();
                        turnLog.Add("Invalid input.  Try again.");
                    }
                }
                else
                {
                    switch (line)
                    {
                        case "Q":
                            return;
                        case "q":
                            Console.Clear();
                            Console.WriteLine("Are you sure you want to return to the deck menu?");
                            Console.WriteLine("This will abort the game in progress." + System.Environment.NewLine);
                            Console.WriteLine("Enter Y to return to deck menu.  Enter anything else to resume game in progress.");
                            line = Console.ReadLine();
                            if (line == "Y")
                            {
                                endGame = true;
                            } else
                            {
                                break;
                            }
                            break;
                        case "c":
                            hand.Add(deckInPlay[0]);
                            turnLog.Add("You drew " + deckInPlay[0].name);
                            deckInPlay.RemoveAt(0);
                            break;
                        case "d":
                            viewDiscardPile(discardPile, hand, turnLog);
                            break;
                        case "D":
                            viewDeck(deckInPlay, discardPile, hand, turnLog);
                            break;
                        case "R":
                            Console.Clear();
                            Console.WriteLine("Are you sure you want to reset the game?" + System.Environment.NewLine);
                            Console.WriteLine("Enter Y to restart the game.  Enter anything else to resume game in progress.");
                            line = Console.ReadLine();
                            if (line == "Y")
                            {
                                playGame(activeDeckIndex);

                            }
                            else
                            {
                                break;
                            }
                            break;
                        default:
                            Console.Clear();
                            turnLog.Add("Invalid input.  Try again.");
                            break;


                    }
                }
            }
            Console.Clear();
            deckMenu(activeDeckIndex);
        }
        public void viewDeck(List<Card> deck, List<Card> discardPile, List<Card> hand, List<String> turnLog)
        {
            {
                bool returnToGame = false;
                while (!returnToGame)
                {
                    Console.Clear();
                    turnLog.Add("You peeked at your deck.");
                    Console.WriteLine("Enter q to go back and Q to exit the program." + System.Environment.NewLine +
                         "To examine or interact with a card, enter its corresponding number." + System.Environment.NewLine +
                         "r. Reshuffle and Return to Game." + System.Environment.NewLine + System.Environment.NewLine);
                    if (deck.Count > 0)
                    {
                        int numCardsInDeck = 0;
                        foreach (Card c in deck)
                        {
                            numCardsInDeck++;
                            Console.WriteLine(numCardsInDeck + ". " + c.name);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Oh no, you don't actually have a deck.  What the hell are you going to do now?");
                    }
                    string line = Console.ReadLine();
                    int number = -1;
                    Int32.TryParse(line, out number);
                    if (number > 0)
                    {
                        if (number <= deck.Count)
                        {
                            Console.Clear();
                            interactWithCardInDeck(deck, discardPile, hand, turnLog, number - 1);
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Invalid input.  Try again.");
                            System.Threading.Thread.Sleep(1000);
                            

                        }
                    }
                    else
                    {
                        switch (line)
                        {
                            case "Q":
                                return;
                            case "q":
                                return;
                            case "r":
                                turnLog.Add("You reshuffled your deck.");
                                deck.Shuffle();
                                return;

                            default:
                                Console.Clear();
                                break;


                        }
                    }
                }
                Console.Clear();
            }
        }
        public void viewDiscardPile(List<Card> discardPile, List<Card> hand, List<String> turnLog)
        {
            {
                bool returnToGame = false;
                while (!returnToGame)
                {
                    Console.Clear();

                    Console.WriteLine("Enter q to go back and Q to exit the program." + System.Environment.NewLine +
                         "To examine or interact with a card, enter its corresponding number." + System.Environment.NewLine);
                    if (discardPile.Count > 0)
                    {
                        int numCardsInPile = 0;
                        foreach (Card c in discardPile)
                        {
                            numCardsInPile++;
                            Console.WriteLine(numCardsInPile + ". " + c.name);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Your discard pile is empty, yet you chose to examine it anyway.  What did you seek to find here?");
                    }
                    string line = Console.ReadLine();
                    int number = -1;
                    Int32.TryParse(line, out number);
                    if (number > 0)
                    {
                        if (number <= discardPile.Count)
                        {
                            Console.Clear();
                            interactWithDiscardedCard(discardPile, hand, turnLog, number - 1);
                        }
                        else
                        {
                            Console.Clear();
                            System.Threading.Thread.Sleep(1000);
                            Console.WriteLine("Invalid input.  Try again.");

                        }
                    }
                    else
                    {
                        switch (line)
                        {
                            case "Q":
                                return;
                            case "q":
                                return;

                            default:
                                Console.Clear();
                                break;


                        }
                    }
                }
                Console.Clear();
            }
        }
        public void interactWithCardInHand(List<Card> discardPile, List<Card> hand, List<String> turnLog, int handIndex)
        {
            viewCardInfo(hand[handIndex]);
            Console.WriteLine(System.Environment.NewLine + System.Environment.NewLine +
             "q. Return to the game." + System.Environment.NewLine +
             "c. Discard this card." + System.Environment.NewLine +
             "D. Remove this card from the game.");
            string line = Console.ReadLine();
            switch (line)
            {
                case "q":
                    break;
                case "c":
                    Console.Clear();
                    discardPile.Add(hand[handIndex]);
                    turnLog.Add("You discarded " + hand[handIndex].name + ".");
                    hand.RemoveAt(handIndex);
                    break;
                case "D":
                    Console.Clear();
                    turnLog.Add("You removed " + hand[handIndex].name + " from the game.");
                    hand.RemoveAt(handIndex);
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input.  Try again." + System.Environment.NewLine);
                    System.Threading.Thread.Sleep(500);
                    interactWithCardInHand(discardPile, hand, turnLog, handIndex);
                    break;
            }
        }
        public void interactWithDiscardedCard(List<Card> discardPile, List<Card> hand, List<String> turnLog, int discardIndex)
        {
            viewCardInfo(discardPile[discardIndex]);
            Console.WriteLine(System.Environment.NewLine + System.Environment.NewLine +
             "q. Return to the discard pile." + System.Environment.NewLine +
             "c. Move this card from the discard pile to your hand." + System.Environment.NewLine +
             "D. Remove this card from the game.");
            string line = Console.ReadLine();
            switch (line)
            {
                case "q":
                    break;
                case "c":
                    Console.Clear();
                    hand.Add(discardPile[discardIndex]);
                    turnLog.Add("You returned " + discardPile[discardIndex].name + " to your hand from the discard pile.");
                    discardPile.RemoveAt(discardIndex);
                    break;
                case "D":
                    Console.Clear();
                    turnLog.Add("You removed " + discardPile[discardIndex].name + " from the game.");
                    discardPile.RemoveAt(discardIndex);
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input.  Try again." + System.Environment.NewLine);
                    System.Threading.Thread.Sleep(500);
                    interactWithDiscardedCard(discardPile, hand, turnLog, discardIndex);
                    break;
            }
        }
        public void interactWithCardInDeck(List<Card> deck, List<Card> discardPile, List<Card> hand, List<String> turnLog, int deckIndex)
        {
            viewCardInfo(deck[deckIndex]);
            Console.WriteLine(System.Environment.NewLine + System.Environment.NewLine +
             "q. Return to the deck view." + System.Environment.NewLine +
             "c. Move this card from the deck to your hand." + System.Environment.NewLine +
             "d. Move this card from the deck to the discard pile." + System.Environment.NewLine +
             "D. Remove this card from the game.");
            string line = Console.ReadLine();
            switch (line)
            {
                case "q":
                    break;
                case "c":
                    Console.Clear();
                    hand.Add(deck[deckIndex]);
                    turnLog.Add("You moved " + deck[deckIndex].name + " to your hand from your deck.");
                    deck.RemoveAt(deckIndex);
                    break;
                case "d":
                    Console.Clear();
                    discardPile.Add(deck[deckIndex]);
                    turnLog.Add("You moved " + deck[deckIndex].name + " to your deck from the discard pile.");
                    deck.RemoveAt(deckIndex);
                    break;
                case "D":
                    Console.Clear();
                    turnLog.Add("You removed " + deck[deckIndex].name + " from the game.");
                    discardPile.RemoveAt(deckIndex);
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input.  Try again." + System.Environment.NewLine);
                    System.Threading.Thread.Sleep(500);
                    interactWithCardInDeck(deck, discardPile, hand, turnLog, deckIndex);
                    break;
            }
        }

        public void viewCardInfo(Card card)
        {
            Console.WriteLine(card.name + System.Environment.NewLine + System.Environment.NewLine +
             "Cost: " + card.manaCost + System.Environment.NewLine +
             "Type: " + card.type + System.Environment.NewLine +
             card.effectText);
        }

    }
}