using UnityEngine;

public class PlayerCombat : Entity_CombatComponent
{
    [Header("CounterAttack details")] 
    [SerializeField] private float counterRecovery;
    public bool CounterAttackPerformed()
    {
        bool hasPerformedCounter = false;
        
        foreach (var target in GetDetectedColliders())
        {
            ICountarable counterable = target.GetComponent<ICountarable>();

            if (counterable == null) continue;
            
            if(counterable.CanBeCountered)
            {
                counterable.HandleCounter();
                hasPerformedCounter = true;
            }
        }
        
        return hasPerformedCounter;
    }
    
    public float GetCounterRecoveryDuration() => counterRecovery;
}
