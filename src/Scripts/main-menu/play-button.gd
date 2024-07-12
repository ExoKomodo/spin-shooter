extends Button

const GAME_SCENE_PATH = "res://Scenes/game.tscn"

func _on_pressed():
	var current_scene_tree = get_tree()
	current_scene_tree.change_scene_to_file(GAME_SCENE_PATH)
