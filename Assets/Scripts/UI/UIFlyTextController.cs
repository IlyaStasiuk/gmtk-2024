using Common;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random=UnityEngine.Random;

public class UIFlyTextController : MonoBehaviour
{
   [SerializeField] private UIFlyText goFlyText = null;

   [SerializeField] private List<UIFlyText> flyTextsPoolDefault = new List<UIFlyText>();

   // [SerializeField] private BattleCharacter battleCharacterPlayer = null;
   // [SerializeField] private BattleCharacter battleCharacterAI     = null;

   private List<UIFlyText> currentFlying = new List<UIFlyText>();
   private Stack<UIFlyText> flyTextsPoolStack = null;

   //Scale settings
   private const float TWEEN_SCALE_TIME_IN  = 0.1f;
   private const float TWEEN_SCALE_TIME_OUT = 0.3f;
   private static readonly Vector3 SCALE_TEXT_MIN      = Vector3.one * 0.01f;
   private static readonly Vector3 SCALE_TEXT_MAX      = Vector3.one * 1.5f;
   private static readonly Vector3 SCALE_TEXT          = Vector3.one;
   //Move settings
   private const float TIME_TWEEN_TEXT = 0.6f;
   private const float SPAWN_OFFSET_Y = 80;


   private void Awake()
   {
      init();
   }

   public void init()
   {
      // battleCharacterPlayer.onHealthChanged += (oldVal, newVal) => spawnFlyText(toScreenCoords(battleCharacterPlayer.transform.position), Mathf.Abs(newVal - oldVal), newVal - oldVal > 0);
      // battleCharacterAI    .onHealthChanged += (oldVal, newVal) => spawnFlyText(toScreenCoords(battleCharacterAI    .transform.position), Mathf.Abs(newVal - oldVal), newVal - oldVal > 0);

      flyTextsPoolStack = new Stack<UIFlyText>(flyTextsPoolDefault);
   }

   public void spawnFlyText(Vector3 startPos, float damageAmount, bool isHeal, bool isCrit = false)
   {
      if (flyTextsPoolStack.Count == 0)
      {
         UIFlyText newFlyText = Instantiate(goFlyText, new Vector3(999999, 999999), Quaternion.identity);
         newFlyText.transform.parent = transform;
         flyTextsPoolStack.Push(newFlyText);
      }

      UIFlyText flyText = flyTextsPoolStack.Pop();
      currentFlying.Add(flyText);

      flyText.init(damageAmount, isHeal, isCrit);
      flyText.gameObject.SetActive(true);
      Transform trans = flyText.transform;
      trans.position = startPos.plusY(SPAWN_OFFSET_Y);
      trans.localScale = SCALE_TEXT_MIN;
      trans.SetAsLastSibling();


      Sequence sequenceScale = DOTween.Sequence();
      sequenceScale.Append(trans.DOScale(SCALE_TEXT_MAX, TWEEN_SCALE_TIME_IN));
      sequenceScale.Append(trans.DOScale(SCALE_TEXT, TWEEN_SCALE_TIME_OUT));
      sequenceScale.Play();


      Vector3 randomOffset = new Vector3( Random.Range( -30, 30 ), Random.Range( 50, 70 ) );
      var moveCoroutine = trans.DOLocalMove(trans.localPosition + randomOffset, TIME_TWEEN_TEXT);
      moveCoroutine.Play();


      Sequence sequenceFade = DOTween.Sequence();
      flyText.canvasGroup.alpha = 1f;
      sequenceFade.AppendInterval(TIME_TWEEN_TEXT * 0.8f);
      sequenceFade.Append(flyText.canvasGroup.DOFade(0, TIME_TWEEN_TEXT * 0.2f));
      sequenceFade.onComplete += onFlyEnded;
      sequenceFade.Play();


      void onFlyEnded()
      {
         flyText.gameObject.SetActive(false);
         flyTextsPoolStack.Push(flyText);
         currentFlying.Remove(flyText);
      }
   }

   private Vector3 toScreenCoords(Vector3 coords)
   {
      return Camera.main.WorldToScreenPoint(coords);
   }

   
}
