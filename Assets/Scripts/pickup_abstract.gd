class_name pickup_abstract
extends Area3D

enum PickupType {
	Health,
	Ammo
}

@export_category("Stats")
@export var _type: PickupType = PickupType.Health
@export var _amount: int
	
# bobling
var _start_pos_y: float
	
@export_category("Movement")
@export var _bob_height: float = 1
@export var _bob_speed: float = 1
	
var _bobbing_up: bool = true
var _position: Vector3 = Vector3.ZERO

func _ready():
	_start_pos_y = position.y

func _process(delta):
	# Move up and down
	# translation.y += (bobSpeed if bobbingUp == true else -bobSpeed) * delta
	_position = position;
	# _position.y += (_bobbing_up ? _bob_speed : -_bob_speed) * delta
	_position.y += (_bob_speed if _bobbing_up == true else -_bob_speed) * delta
	position = _position;
	# if it at the top
	if (_bobbing_up && _position.y > _start_pos_y + _bob_height):
		_bobbing_up = false
		# if it at bottom
	elif(!_bobbing_up && _position.y < _start_pos_y):
			_bobbing_up = true

func _on_body_entered(body):
	if (body.name == "Player"):
		pickup(body as player);
		queue_free()

func pickup(body: player):
	if(_type == PickupType.Health):
		body.health_add(_amount)
	else:
		body.ammo_add(_amount)

