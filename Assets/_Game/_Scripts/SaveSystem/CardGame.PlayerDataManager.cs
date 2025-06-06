using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    /// The project dependent code will be here.
    public partial class PlayerDataManager : MonoBehaviour
    {
        public void SaveLevelData(int score, byte streak, IEnumerable<BaseCard> cards)
        {
            if (GlobalVariables.isLevelComplete) return;
            LevelDataModel leveldata;
            if (_playerData.levelDataModel == null)
            {
                leveldata = new LevelDataModel();
                _playerData.levelDataModel = leveldata;
            }
            else
            {
                leveldata = _playerData.levelDataModel as LevelDataModel;
            }
            leveldata.score = score;
            leveldata.highestStreak = streak;
            leveldata.cardsData.Clear();
            foreach (BaseCard card in cards)
            {
                leveldata.cardsData.Add(new CardData()
                {
                    cardState = card.CurrentState,
                    iconId = card.IconId,
                    uniqueId = card.UniqueId
                });
            }
            SaveData();
        }
        public LevelDataModel GetLevelData()
        {
            return _playerData.levelDataModel == null ? null : _playerData.levelDataModel as LevelDataModel;
        }
        public bool HasLevelData()
        {
            return _playerData.levelDataModel != null;
        }
        public void ClearLevelData()
        {
            _playerData.levelDataModel = null;
            SaveData();
        }
    }
    [Serializable]
    public class LevelDataModel : ILevelDataModel
    {
        public int score;
        public byte highestStreak;
        public List<CardData> cardsData;
        public LevelDataModel()
        {
            score = 0;
            cardsData = new List<CardData>();
        }


        //store the list of the gridcell.
        //each element wil have
        ///gridcell state--->flipped? matched? unflipped?
        ///iconId
        ///uniqueId
        /// 
        ///score achieved
    }
    [Serializable]
    public class CardData
    {
        public CardState cardState;
        public int iconId;
        public int uniqueId;
    }
    public enum CardState
    {
        Revealed,
        Matched,
        Hidden,
    }
}
