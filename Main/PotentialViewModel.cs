using MottSchottkyAnalizer.Core.User;
using MottSchottkyAnalizer.Core.ViewModel;
using MottSchottkyAnalizer.DI.Registration;

namespace MottSchottkyAnalizer.Application.Main;

[ViewModel<PotentialViewModel>]
public class PotentialViewModel : ViewModelBase
{
    public event Action<double[]>? OnApply;

    public IRelayCommand Recalc { get; }
    public IRelayCommand Reverse { get; }
    public IRelayCommand Apply { get; }

    public decimal Start
    {
        get => field;
        set
        {
            Set(ref field, value);
            CalcStep(StepCount);
        }
    }

    public decimal End
    {
        get => field;
        set
        {
            Set(ref field, value);
            CalcStep(StepCount);
        }
    }

    public decimal Step
    {
        get => field;
        set
        {
            Set(ref field, value);
        }
    }

    public int StepCount
    {
        get => field;
        set
        {
            if (Set(ref field, value))
                CalcStep(value);
        }
    }

    public PotentialViewModel()
    {
        Recalc = new RelayCommand(() => CalcStep(StepCount));
        Reverse = new RelayCommand(ReverseStep);
        Apply = new RelayCommand(() => OnApply?.Invoke(GetPotentialSequence()));
    }

    private double[] GetPotentialSequence()
    {
        if (Step == 0)
            return Array.Empty<double>();

        int length = (int)((End - Start) / Step) + 1;
        if (length < StepCount)
            throw new UserException($"Диапазон потенциалов ({length}) меньше числа шагов ({StepCount})");

        double[] potentials = new double[length];

        for (int i = 0; i < length; i++)
        {
            decimal value = Start + i * Step;
            potentials[i] = System.Math.Round((double)value, 6);
        }

        return potentials;
    }

    public void CalcStep(int stepCount)
    {
        if (stepCount <= 1)
        {
            Step = 0;
            return;
        }

        Step = (End - Start) / (stepCount - 1);
    }

    public void ReverseStep()
    {
        decimal temp = Start;
        Start = End;
        End = temp;
        CalcStep(StepCount);
    }
}