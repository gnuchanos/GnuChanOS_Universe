extends CharacterBody3D

var _Name = ''
var Ilk = true
var IDD = 0

func _ready() -> void:
	if Ilk==true:
		name=_Name
		Ilk=false

@rpc("any_peer")
func GlobalPosition(_Data):
	if _Data['Name']==name:
		global_position=_Data['Pos']
