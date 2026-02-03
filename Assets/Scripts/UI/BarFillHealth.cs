using UnityEngine;

public class HealthBarFill : BarFill
{
    protected override void TrackPlayer(Player player)
    {
        _Player = player;
        _Player.OnHealthChanged += Player_OnHealthChanged;
        _Player.OnDeath += Player_OnDeath;
        UpdateBar(_Player.Health.Current, _Player.Health.Max, 0);
    }

    private void Player_OnHealthChanged(object sender, Player.DyanmicStatChangeEvent e)
    {
        UpdateBar(e.Current, e.Max, e.OverTime);
    }

    private void Player_OnDeath(object sender, System.EventArgs e)
    {
        // TODO show death animation, respawn at the business or whatever makes sense
    }
}
