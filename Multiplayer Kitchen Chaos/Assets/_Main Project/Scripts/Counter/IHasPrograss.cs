using System;

public interface IHasPrograss 
{
    public event EventHandler<OnCuttingPrograssChangedEventArgs> OnFryingPrograssChanged;
    public class OnCuttingPrograssChangedEventArgs : EventArgs
    {
        public float PrograssAmountNormalized;
    }
}
