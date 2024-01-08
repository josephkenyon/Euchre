using webapi.Domain.Bidding;
using webapi.Domain.GameDetails;
using webapi.Domain.Statics;
using static webapi.Domain.Statics.Enums;

namespace webapi.Domain.Game
{
    public class Game : IGame
    {
        public IGameDetails GetGameDetails() => new GameDetails.GameDetails(Name);

        public string Name { get; set; }

        public Phase Phase { get; set; }
        public Suit? TrumpSuit { get; set; }

        public int? IgnoredPlayerIndex { get; set; }

        public string? TrumpCardString { get; set; }
        public string? KittyString { get; set; }

        public int PlayerTurnIndex { get; set; }
        public int StartingPlayerTurnIndex { get; set; }
        public int TookBidTeamIndex { get; set; }

        public string TeamOneScoresString { get; set; }
        public string TeamTwoScoresString { get; set; }
        public string RoundBidResults { get; set; }
        public string TeamOneCardsTakenIds { get; set; }
        public string TeamTwoCardsTakenIds { get; set; }

        public Game()
        {
            Name = "Unknown";
            Phase = Phase.Initializing;
            TeamOneScoresString = "";
            TeamTwoScoresString = "";
            TeamOneCardsTakenIds = "";
            TeamTwoCardsTakenIds = "";
            RoundBidResults = "";
        }

        public Game(string gameName)
        {
            Name = gameName;
            Phase = Phase.Initializing;
            TeamOneScoresString = "";
            TeamTwoScoresString = "";
            TeamOneCardsTakenIds = "";
            TeamTwoCardsTakenIds = "";
            RoundBidResults = "";
            StartingPlayerTurnIndex = 0;
        }

        public int GetDealerIndex()
        {
            return Utils.DecrementIndex(StartingPlayerTurnIndex, 1);
        }

        public int GetPlayerTurnIndex()
        {
            return PlayerTurnIndex;
        }

        public void SetPlayerTurnIndex(int index)
        {
            PlayerTurnIndex = index;
        }

        public void IncrementPlayerTurnIndex()
        {
            PlayerTurnIndex++;
            if (PlayerTurnIndex > 3)
            {
                PlayerTurnIndex = 0;
            }

            if (PlayerTurnIndex == IgnoredPlayerIndex)
            {
                IncrementPlayerTurnIndex();
            }
        }

        public int GetTookBidTeamIndex()
        {
            return TookBidTeamIndex;
        }

        public void SetTookBidTeamIndex(int index)
        {
            TookBidTeamIndex = index;
        }

        public Phase GetPhase()
        {
            return Phase;
        }

        public Suit? GetTrumpSuit()
        {
            return TrumpSuit;
        }

        public Card GetTrumpCard()
        {
            return Utils.GetCardFromString(TrumpCardString!);
        }

        public void DealRoundStart(List<Card> kitty, Card trumpCard)
        {
            KittyString = Utils.StringifyCards(kitty);
            TrumpCardString = Utils.StringifyCard(trumpCard);
            TrumpSuit = trumpCard.Suit;
        }

        public List<Card> GetAndThenReplenishKitty(string replenishIds)
        {
            var cards = Utils.GetCardsFromString(KittyString);


            var idStringList = replenishIds.Split(";").ToList();
            var cardList = idStringList.Select(id => Utils.GetCardFromId(int.Parse(id)));

            KittyString = Utils.StringifyCards(cardList.ToList());

            return cards;
        }

        public void SetTrumpSuit(Suit suit)
        {
            TrumpSuit = suit;
        }

        public void IncrementPhase()
        {
            var phaseInt = (int) Phase;

            if (Phase == Phase.RoundEnd)
            {
                Phase = Phase.Initializing;
            }
            else
            {
                Phase = (Phase)Enum.ToObject(typeof(Phase), phaseInt + 1);
            }
        }

        public void SkipPhase()
        {
            var phaseInt = (int)Phase;

            Phase = (Phase)Enum.ToObject(typeof(Phase), phaseInt + 2);
        }

        public void LonerCalled(int ignorePlayerIndex)
        {
            IgnoredPlayerIndex = ignorePlayerIndex;
        }

        public int? GetIgnoredPlayerIndex()
        {
            return IgnoredPlayerIndex;
        }

        public void FlipDealerCard()
        {
            TrumpCardString = null;
            TrumpSuit = null;
        }

        public void StartNewRound()
        {
            if (Phase == Phase.Initializing)
            {
                TeamOneScoresString = "";
                TeamTwoScoresString = "";
                TeamOneCardsTakenIds = "";
                TeamTwoCardsTakenIds = "";
                RoundBidResults = "";
                StartingPlayerTurnIndex = 0;
            }

            Phase = Phase.Pick_It_Up;

            StartingPlayerTurnIndex++;
            if (StartingPlayerTurnIndex > 3)
            {
                StartingPlayerTurnIndex = 0;
            }

            PlayerTurnIndex = StartingPlayerTurnIndex;
            TeamOneCardsTakenIds = "";
            TeamTwoCardsTakenIds = "";

            IgnoredPlayerIndex = null;
            TrumpCardString = null;
            KittyString = null;
        }

        public int IncrementAndGetStartingPlayerTurnIndex()
        {
            StartingPlayerTurnIndex++;

            if (StartingPlayerTurnIndex > 3)
            {
                StartingPlayerTurnIndex = 0;
            }

            return StartingPlayerTurnIndex;
        }

        public void AddScore(int teamIndex, int scoreIncrementValue)
        {
            var scoreArray = (teamIndex == 0 ? TeamOneScoresString : TeamTwoScoresString).Split(";").ToList();
            scoreArray.RemoveAll(string.IsNullOrEmpty);
            scoreArray.Add(scoreIncrementValue.ToString());

            if (teamIndex == 0)
            {
                TeamOneScoresString = string.Join(";", scoreArray);
            }
            else
            {
                TeamTwoScoresString = string.Join(";", scoreArray);
            }
        }

        public List<string> GetScoreLog(string teamName, int teamIndex)
        {

            var newList = new List<string>
            {
                teamName
            };

            newList.AddRange(teamIndex == 0 ? TeamOneScoresString.Split(";") : TeamTwoScoresString.Split(";"));

            return newList;
        }

        public int GetTotalScore(int teamIndex)
        {
            int scoreTotal = 0;

            foreach (var score in teamIndex == 0 ? TeamOneScoresString.Split(";") : TeamTwoScoresString.Split(";"))
            {
                var parsedScore = 0;
                try
                {
                    parsedScore = int.Parse(score);
                }
                catch (Exception) { }

                scoreTotal += parsedScore;
            }

            return scoreTotal;
        }

        public void AddRoundBidResult(Suit trumpSuit, int teamIndex)
        {
            var resultsList = RoundBidResults.Split(";").ToList();
            resultsList.RemoveAll(string.IsNullOrEmpty);

            var result = $"{trumpSuit}:{teamIndex}";

            resultsList.Add(result);

            RoundBidResults = string.Join(";", resultsList);
        }

        public List<RoundBidResult> GetRoundBidResults()
        {
            var resultsList = RoundBidResults.Split(";").ToList();
            resultsList.RemoveAll(string.IsNullOrEmpty);

            var newList = new List<RoundBidResult>();

            foreach (var result in resultsList)
            {
                var resultArray = result.Split(":").ToList();
                _ = Enum.TryParse(resultArray[0], out Suit suit);

                newList.Add(new RoundBidResult(suit, int.Parse(resultArray[1])));
            }

            return newList;
        }

        public void AddCardIds(int teamIndex, List<int> cardIds)
        {
            var idsList = (teamIndex == 0 ? TeamOneCardsTakenIds : TeamTwoCardsTakenIds).Split(";").ToList();
            idsList.RemoveAll(string.IsNullOrEmpty);
            idsList.AddRange(cardIds.Select(id => id.ToString()));

            if (teamIndex == 0)
            {
                TeamOneCardsTakenIds = string.Join(";", idsList);
            }
            else
            {
                TeamTwoCardsTakenIds = string.Join(";", idsList);
            }
        }

        public int GetTricksTaken(int teamIndex)
        {
            var newList = new List<int>();

            var ids = (teamIndex == 0 ? TeamOneCardsTakenIds.Split(";") : TeamTwoCardsTakenIds.Split(";")).ToList();
            ids.RemoveAll(string.IsNullOrEmpty);
            newList.AddRange(ids.Select(int.Parse));

            return newList.Count / (IgnoredPlayerIndex == null ? 4 : 3);
        }

        public bool TeamWasAlone(int teamIndex)
        {
            return IgnoredPlayerIndex != null && Utils.GetTeamIndex((int) IgnoredPlayerIndex) == teamIndex;
        }
    }
}
