using Godot;

namespace Godot3DFirstPersonShooter.Assets.Scrips;

public partial class Ui : Control
{
    private TextureProgressBar _barHealth;
    private Label _txtAmmo;
    private Label _txtScore;

    public override void _Ready()
    {
        _barHealth = GetNode<TextureProgressBar>("barHealth");
        _txtAmmo = GetNode<Label>("txtAmmo");
        _txtScore = GetNode<Label>("txtScore");
        if (_barHealth == null || _txtAmmo == null || _txtScore == null)
        {
            GD.PushError("null");
            GetTree().Quit();
        }
    }

    public void BarHealthUpdate(int curHp, int maxHp)
    {
        _barHealth.MaxValue = maxHp;
        _barHealth.Value = curHp;
        
    }

    public void TxtAmmoUpdate(int ammo)
    {
        _txtAmmo.Text = "Ammo: " + ammo;
    }

    public void TxtScoreUpdate(int score)
    {
        _txtScore.Text = "Score: " + score;
    }
}