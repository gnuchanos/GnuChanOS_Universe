extends CharacterBody3D

var _Name = ''
var Ilk = true
var IDD = 0

@onready var playback = null

func _ready() -> void:
	if Ilk==true:
		name=_Name
		Ilk=false

	#animation
	var anim_tree = get_node_or_null("Body/CubeChan_default/anim/AnimationTree")
	if anim_tree:
		playback = anim_tree.get("parameters/playback")

@rpc("any_peer")
func GlobalPosition(_Data):
	if _Data['Name'] == name:
		global_position = _Data['Pos']
		if _Data.has("Rot"):
			global_rotation = _Data['Rot']
