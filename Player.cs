using Godot;

public partial class Player : Area2D
{
    // criando um signal de hit
    [Signal]
    public delegate void HitEventHandler();

	[Export]
	public int Speed { get; set; } = 400;

	public Vector2 ScreenSize;

    // Ao carregar o node, ja definimos o tamanho da tela maximo.
    public override void _Ready()
    {
        ScreenSize = GetViewportRect().Size;
        Hide();
    }

    // cada frame
    public override void _Process(double delta)
    {
        var velocity = Vector2.Zero;

        if (Input.IsActionPressed("move_right")) velocity.X += 1;
        if (Input.IsActionPressed("move_left")) velocity.X -= 1;
        if (Input.IsActionPressed("move_down")) velocity.Y += 1;
        if (Input.IsActionPressed("move_up")) velocity.Y -= 1;

        // Estamos selecionando o um filho desse node.
        var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        if (velocity.Length() > 0)
        {
            // aqui estamos normalizando a velocidade para nao "correr" mais rapido na diagonal.
            velocity = velocity.Normalized() * Speed;
            animatedSprite2D.Play();
        }
        else animatedSprite2D.Stop();

        // Quando multiplicamos a velocidade, tanto X e Y e multiplicado.
        Position += velocity * (float)delta;

        // Aqui criamos uma limitacao na posicao, criando um novo vetor
        // que vai restringir uma posicao maxima utilizando o Mathf.Clamp
        Position = new Vector2(
            x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
            y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
        );

        if(velocity.X != 0)
        {
            animatedSprite2D.Animation = "walk";
            animatedSprite2D.FlipV = false;
            animatedSprite2D.FlipH = velocity.X < 0;
        } else if(velocity.Y != 0) {
            animatedSprite2D.Animation = "up";
            animatedSprite2D.FlipH = false;
            animatedSprite2D.FlipV = velocity.Y > 0;
        }
    }

    private void OnBodyEntered(Node2D node)
    {
        Hide();
        EmitSignal(SignalName.Hit);
        // Desativando a colisao de forma segura ja que o SetDeferred faz espera um momento certo para executar
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }

    public void Start(Vector2 position)
    {
        Position = position;
        Show();

        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
    }
}
