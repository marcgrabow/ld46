using UnityEngine;

namespace Assets
{
    public class Msg
    {
        public Msg(string txt, Vector3 pos)
        {
            Pos = pos;
            Text = txt;
            // Debug.Log(txt);
        }

        public Vector3 Pos { get; }
        public string Text { get; }
    }
}