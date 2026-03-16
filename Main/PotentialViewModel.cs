using MottSchottkyAnalizer.Core.User;
using MottSchottkyAnalizer.Core.ViewModel;
using MottSchottkyAnalizer.DI.Registration;

namespace MottSchottkyAnalizer.Application.Main;

[ViewModel<PotentialViewModel>]
public class PotentialViewModel : ViewModelBase
{
    public IRelayCommand Recalc { get; }
    public IRelayCommand Reverse { get; }

    public double Start
    {
        get => field;
        set
        {
            Set(ref field, value);
            CalcStep(StepCount);
        }
    }

    public double End
    {
        get => field;
        set
        {
            Set(ref field, value);
            CalcStep(StepCount);
        }
    }

    public double Step
    {
        get => field;
        set => Set(ref field, value);
    }

    public int StepCount { get; set; }

    public PotentialViewModel()
    {
        Recalc = new RelayCommand(() => CalcStep(StepCount));
        Reverse = new RelayCommand(ReverseStep);
    }

    public IEnumerable<double> GetPotentialSequence()
    {
        int currentCount = (int)(Math.Abs(End - Start) * Step);
        if (currentCount < StepCount)
            throw new UserException($"Диапазон потенциалов ({currentCount}) меньше числа шагов ({StepCount})");

        for (double i = Start; i <= End; i += Step)
            yield return i;
    }

    public void CalcStep(int stepsCount)
    {
        if (stepsCount == 0 || (Start == 0 && End == 0))
            return;

        StepCount = stepsCount;
        Step = Math.Abs(End - Start) / stepsCount;
    }

    public void ReverseStep()
    {
        double end = Start;
        Start = End;
        End = end;
        Step = -Step;
    }
}
