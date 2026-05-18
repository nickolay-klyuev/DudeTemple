extends Node3D

func TriggerParticlesRandom():
	var childs = get_children()
	var rand_index: int = randi_range(0, childs.size() - 1)
	var particle = get_child(rand_index)
	particle.restart()
