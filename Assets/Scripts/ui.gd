class_name ui
extends Control

var _bar_health: TextureProgressBar
var _txt_ammo: Label
var _txt_score: Label


func _ready():
	_bar_health = get_node("barHealth")
	_txt_ammo = get_node("txtAmmo")
	_txt_score = get_node("txtScore")
	if _bar_health == null || _txt_ammo == null || _txt_score == null:
		push_error("null")
		get_tree().quit(1)

func bar_health_update(cur_hp: int, max_hp: int) -> void:
	_bar_health.max_value = max_hp
	_bar_health.value = cur_hp

func txt_ammo_update(ammo: int) -> void:
	_txt_ammo.text = str("Ammo: ", ammo)

func txt_score_update(score: int) -> void:
	_txt_score.text = str("Score: ", score)
