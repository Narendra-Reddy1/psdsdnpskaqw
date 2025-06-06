using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace CardGame
{
    /// <summary>
    /// It is responsible for matching the cards.
    /// Depending on the MIN_CARDS_TO_MATCH constant it handles the card matching logic.
    /// It is scalable to N-card matching. 
    /// But changing the MIN_CARDS_TO_MATCH constant may required to update the level data SOs.
    /// </summary>
    public class MatchHandler : MonoBehaviour
    {
        private List<BaseCard> _flippedCards = new List<BaseCard>();

        private void OnEnable()
        {
            GlobalEventHandler.AddListener(EventID.OnCardRevealed, Callback_On_Card_Flipped);
        }
        private void OnDisable()
        {
            GlobalEventHandler.RemoveListener(EventID.OnCardRevealed, Callback_On_Card_Flipped);
        }


        private void Callback_On_Card_Flipped(object args)
        {
            BaseCard flippedCard = args as BaseCard;
            if (_flippedCards.Count < Konstants.MIN_CARDS_TO_MATCH - 1)
            {
                _flippedCards.Add(flippedCard);
            }
            else
            {
                var copyList = new List<BaseCard>(_flippedCards)
                {
                    flippedCard
                };
                _flippedCards.Clear();
                if (copyList.Any(x => x.IconId != flippedCard.IconId))
                {
                    foreach (BaseCard card in copyList)
                        card.OnMatchFail();
                    Debug.Log($"Match Failed for {copyList.Count}");
                    GlobalEventHandler.TriggerEvent(EventID.RequestToPlaySFXWithId, AudioID.MatchFailSFX);
                    GlobalEventHandler.TriggerEvent(EventID.OnCardMatchFailed, copyList);
                }
                else
                {
                    foreach (BaseCard card in copyList)
                        card.OnMatchSuccess();
                    Debug.Log($"Match Found for {copyList.Count}");
                    GlobalEventHandler.TriggerEvent(EventID.RequestToPlaySFXWithId, AudioID.MatchSuccessSFX);
                    GlobalEventHandler.TriggerEvent(EventID.OnCardMatchSuccess, copyList);
                }
            }
        }
    }

}
