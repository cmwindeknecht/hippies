using UnityEngine;

public class BarFillMagic : BarFill
{
    protected override void TrackPlayer(Player player)
    {
        _Player = player;
        _Player.OnMagicChanged += Player_OnMagicChanged;
        _Player.OnMagicDepleted += Player_OnMagicDepleted;
        UpdateBar(_Player.Magic.Current, _Player.Magic.Max, 0);
    }

    private void Player_OnMagicChanged(object sender, Character.DyanmicStatChangeEvent e)
    {
        UpdateBar(e.Current, e.Max, e.OverTime);
    }

    private void Player_OnMagicDepleted(object sender, Character.OnDynamicStatDepletionEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}
