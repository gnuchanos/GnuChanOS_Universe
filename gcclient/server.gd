extends Node2D

var Enet = ENetMultiplayerPeer.new()

var Name_ = ''
var ID_ = 0

@rpc("any_peer")
func Login(PlayerName,_ID):
	pass

@rpc("any_peer")
func BackMessage(PlayerName,_ID,No):
	if PlayerName == Name_ and _ID == ID_:
		if No == 1:
			$CanvasLayer/ColorRect/Label.text = 'Oyuncu Zaten Kayıtlı Amına Koduğum!'
		elif No == 0:
			var Player = preload('res://player.tscn').instantiate()
			Player._Name = PlayerName
			Global.Name = PlayerName
			Player.name = PlayerName
			$Players.add_child(Player)
			$CanvasLayer/ColorRect.visible = false
			rpc("PlayerCreate", PlayerName)  # Burada rpc çağrısı

func _ready() -> void:
	Enet.create_client("127.0.0.1", 2025)
	multiplayer.multiplayer_peer = Enet

	multiplayer.peer_disconnected.connect(_on_server_disconnected)
	multiplayer.connection_failed.connect(_on_connection_failed)

func _on_server_disconnected(id):
	print("Sunucu bağlantısı kesildi, istemci kapanıyor...")
	get_tree().quit()

func _on_connection_failed():
	print("Sunucuya bağlanılamadı, istemci kapanıyor...")
	get_tree().quit()

@rpc("any_peer")
func PlayerCreate(_Name):
	var Player = preload('res://player.tscn').instantiate()
	Player._Name = _Name
	Player.name = _Name
	$Players.add_child(Player)

func _on_button_pressed() -> void:
	if multiplayer.multiplayer_peer == null:
		print("Multiplayer peer hazır değil, bağlantı kurulmadı.")
		$CanvasLayer/ColorRect/debug.text = "Multiplayer peer hazır değil, bağlantı kurulmadı."
		return
	
	if multiplayer.multiplayer_peer.get_connection_status() != MultiplayerPeer.CONNECTION_CONNECTED:
		print("Henüz sunucuya bağlanmadınız!")
		$CanvasLayer/ColorRect/debug.text = "Henüz sunucuya bağlanmadınız!"
		return

	randomize()
	ID_ = randi_range(1, 10000)
	Name_ = $CanvasLayer/ColorRect/LineEdit.text
	rpc("Login", Name_, ID_)
