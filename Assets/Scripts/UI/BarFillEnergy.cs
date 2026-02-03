using UnityEngine;

public class BarFillEnergy : BarFill
{
    protected override void TrackPlayer(Player player)
    {
        _Player = player;
        _Player.OnEnergyChanged += Player_OnEnergyChanged;
        _Player.OnEnergyDepleted += Player_OnEnergyDepleted;
        UpdateBar(_Player.Energy.Current, _Player.Energy.Max, 0);
    }

    private void Player_OnEnergyChanged(object sender, Character.DyanmicStatChangeEvent e)
    {
        UpdateBar(e.Current, e.Max, e.OverTime);
    }

    private void Player_OnEnergyDepleted(object sender, Character.OnDynamicStatDepletionEventArgs e)
    {
        // TODO show death animation, respawn at the business or whatever makes sense
    }
}
