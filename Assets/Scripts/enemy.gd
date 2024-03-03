class_name enemy
extends CharacterBody3D

@export_category("Stats")
@export var _health: int = 5
@export var _move_speed: float = 2

@export_category("attacking")
@export var _damage: int = 1
@export var _attack_rate: float = 1
@export var _attack_dist: float = 2

@export_category("Reward")
@export var _score_to_give: int = 1

# Components
var _player: player
var _timer: Timer
	
# Aux
var _player_dir: Vector3 = Vector3.ZERO
#var _vel: Vector3 = Vector3.ZERO

func _ready():
	_player = get_node("/root/Main/Player")
	_timer = get_node("Timer")
	if (_player == null || _timer == null):
		push_error("null")
		get_tree().quit(1)
	else:
		# Setup the Timer
		_timer.set_wait_time(_attack_rate)
		_timer.start()

func _physics_process(_delta):
	# Calculate the direction to the Player
	_player_dir = (_player.position - position).normalized();
	_player_dir.y = 0;
		
	# Move the enemy towards the player
	if (position.distance_to(_player.position) > _attack_dist):
		velocity = _player_dir * _move_speed
		move_and_slide()

func _on_timer_timeout():
	if (position.distance_to(_player.position) <= _attack_dist):
		attack()

func attack() -> void:
	_player.take_damage(_damage)

func take_damage(damage: int) -> void:
	_health -= damage;
	if (_health <= 0):
		die()

func die() -> void:
	_player.score_add(_score_to_give)
	queue_free()



