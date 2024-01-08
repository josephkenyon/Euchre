﻿using webapi.Domain.Bidding;
using webapi.Domain.Tricks;

namespace webapi.Domain.PlayerState
{
    public class HeroPlayerState : PlayerState
    {
        public bool ShowPlayButton { get; set; }
        public bool ShowCollectButton { get; set; }
        public bool ShowDiscard { get; set; }
        public bool ShowBiddingBox { get; set; }
        public bool ShowTrumpSelection { get; set; }
        public bool ShowTrumpIndicator { get; set; }
        public bool ShowTricksTaken { get; set; }
        public bool ShowGoingUnder { get; set; }
        public bool ShowPickItUp { get; set; }
        public bool ShowPassed { get; set; }
        public bool ShowPassButton { get; set; }
        public bool ShowSwapPosition { get; set; }
        public bool CanSelectMultipleCards { get; set; }
        public int TeamOneTricksTaken { get; set; }
        public int TeamTwoTricksTaken { get; set; }
        public Card? TrumpCard { get; set; }
        public List<string> TeamOneScoreList { get; set; }
        public List<string> TeamTwoScoreList { get; set; }
        public List<RoundBidResult> RoundBidResults { get; set; }
        public PlayerState AllyState { get; set; }
        public PlayerState LeftOpponentState { get; set; }
        public PlayerState RightOpponentState { get; set; }
        public TrickState TrickState { get; set; }

        public HeroPlayerState() : base()
        {
            TeamOneScoreList = new List<string>();
            TeamTwoScoreList = new List<string>();
            RoundBidResults = new List<RoundBidResult>();
            AllyState = new PlayerState();
            LeftOpponentState = new PlayerState();
            RightOpponentState = new PlayerState();
            TrickState = new TrickState();
            TrumpCard = null;
        }
    }
}
