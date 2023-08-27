using System;

public interface IHasPrograss 
{
    public event EventHandler<OnCuttingPrograssChangedEventArgs> OnCuttingPrograssChanged;
    public class OnCuttingPrograssChangedEventArgs : EventArgs
    {
        public float PrograssAmountNormalized;
    }
}
