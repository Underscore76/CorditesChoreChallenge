using System;
namespace ChoreChallenge
{
	public abstract class CumulativeAchievement : IAchievement
	{
        protected int PreviousValue;
        protected int CurrentValue;
        protected int MaxValue;

        protected CumulativeAchievement(string description, int score)
            : base(description, score)
        {
        }

        public override void OnSaveLoaded()
        {
            PreviousValue = 0;
            CurrentValue = 0;
            base.OnSaveLoaded();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (HasSeen || PreviousValue == CurrentValue) return;
            if (CurrentValue == MaxValue)
            {
                HasSeen = true;
                PreviousValue = MaxValue;
                return;
            }

            while (PreviousValue < CurrentValue)
            {
                PreviousValue++;
                DisplayInfo($"{Description}: {PreviousValue}/{MaxValue}");
            }
        }
    }
}

