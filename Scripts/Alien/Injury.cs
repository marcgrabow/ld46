    using System.Text;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ld46/Injury")]
    public class Injury  :ScriptableObject
    {
        public string Title;
        public Sprite Sprite;
        public float BloodLossPerSecond = 0.1f;
        public Treatment[] Treatments;

        public override string ToString()
        {
            var txt = new StringBuilder();
            txt.AppendFormat("Injury: <b>{0}</b>", Title);
            txt.AppendLine();
            txt.AppendFormat("Blood loss per second: {0:0.00}", BloodLossPerSecond);
            txt.AppendLine();
            txt.AppendLine("<i>Possible treatments:</i>");
            foreach (var treatm in Treatments)
            {
                txt.AppendLine("- "+treatm.ToString());
            }
            txt.AppendLine();
            return txt + "";
        }
    }
