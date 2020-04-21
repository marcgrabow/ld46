using System.Text;
using UnityEditor;
using UnityEngine;

// treatment combines injury and tool to something that can happen
[CreateAssetMenu(menuName = "ld46/Treatment")]
public class Treatment  :ScriptableObject
{
    public string Title;
    public float Duration = 1f;
    public ToolTypeEnum ToolType;
    
    public override string ToString()
    {
        var txt = new StringBuilder();
        txt.AppendFormat("<b>{0}</b> ({1:0.00}s with {2})", Title, Duration, ToolType);
        return txt + "";
    }
}