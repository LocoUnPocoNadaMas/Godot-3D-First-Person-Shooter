using Godot;

namespace  Godot3DFirstPersonShooter.Assets.Scrips;

enum PickupType
{
    Health,
    Ammo
}
public partial class PickupAbstract : Area3D
{
    [ExportCategory("Stats")]
    [Export] private PickupType _type;
    [Export] private int _amount;
    
    // bobling
    private float _startPosY;
    [ExportCategory("Movement")]
    [Export] private float _bobHeight = 1f;
    [Export] private float _bobSpeed = 1f;
    private bool _bobbingUp = true;

    public override void _Ready()
    {
        _startPosY = Position.Y;
    }

    private void _OnBodyEntered(Node3D body)
    {
        if (body.Name == "Player")
        {
            Pickup(body as Player);
            QueueFree();
        }
    }

    private void Pickup(Player body)
    {
        if(_type == PickupType.Health)
            body.HealthAdd(_amount);
        else
        {
            body.AmmoAdd(_amount);
        }
    }
}
