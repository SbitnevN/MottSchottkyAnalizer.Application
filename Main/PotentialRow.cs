using ElinsData.Data;
using MottSchottkyAnalizer.Core.ViewModel;
using MottSchottkyAnalizer.DI.Registration;

namespace MottSchottkyAnalizer.Application.Main;

[ViewModel<PotentialRow>]
public class PotentialRow : ViewModelBase, IStepPotential
{
    private readonly IStepPotential _potential;

    public event Action? PotentialChanged;

    public int Step
    {
        get => _potential.Step;
        set
        {
            if (_potential.Potential == value)
                return;

            _potential.Step = value;
            OnPropertyChanged(nameof(Step));
        }
    }


    public double Potential
    {
        get => _potential.Potential;
        set
        {
            if (_potential.Potential == value)
                return;

            _potential.Potential = value;
            OnPropertyChanged(nameof(Potential));
            PotentialChanged?.Invoke();
        }
    }

    public PotentialRow(IStepPotential potential, Action onPotentialChanged)
    {
        _potential = potential;

        PotentialChanged += onPotentialChanged;
    }
}
