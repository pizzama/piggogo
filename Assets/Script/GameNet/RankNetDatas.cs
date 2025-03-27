using System;
using System.Collections.Generic;

namespace GameNet
{
    [Serializable]
    public class RankTopPlayersNetData: GameNetBase
    {
        public RankTopPlayersData data;
    }

    [Serializable]
    public class RankMyselfNetData: GameNetBase
    {
        public  RankMyselfData data;
    }

    [Serializable]
    public class RankTopPlayersData
    {
        public string category;
        public List<RankSingleData> top_players;
        public RankMyselfData current_player;
        public int total_count;
    }

    [Serializable]
    public class RankSingleData
    {
        public string role_id;
        public int score;
    }

    [Serializable]
    public class RankMyselfData
    {
        public string category;
        public string role_id;
        public int score;
        public int rank;
    }

    [Serializable]
    public class RequestChangeRank
    {
        public string old_category;
        public string new_category;
    }
}