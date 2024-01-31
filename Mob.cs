using Godot;

public partial class Mob : RigidBody2D
{
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
        // esse randi ai eu achei dificil de compreender
        animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
	}

	// quando o mob sair da tela, isso sera executado.
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
}
