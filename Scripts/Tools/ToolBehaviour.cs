using System;
using UnityEngine;

public class ToolBehaviour : MonoBehaviour
{
   public ToolConfig Config;

   private void Start()
   {
      GetComponentInChildren<SpriteRenderer>().sprite = Config.Sprite;
   }
}
