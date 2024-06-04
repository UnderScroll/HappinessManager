using UnityEngine;

public abstract class IRule : ScriptableObject
{
    public virtual string Type { get => "IRule"; }
    public abstract string BaseType { get; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Styles d'affectation de noms", Justification = "This property has protected getter")]
    public GameManager _gameManager { protected get; set; }
}
