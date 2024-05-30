using CustomProgramCode;
using SplashKitSDK;
using System;
using System.Collections.Generic;

namespace CustomProgramCode
{
    public abstract class Enemy
    {
        protected Bitmap _enemyBitmap;
        protected Sprite _sprite;
        protected bool _doingAnim = false;
        protected const double SPEED = 0.3F; // Default speed for enemies
        protected List<HealthMessage> _messages = new List<HealthMessage>();
        public int AttackSpeed { get; protected set; } = 1000; // Default attack speed in ms
        public int AttackRange { get; protected set; } = 50; // Default attack range
        public int Health { get; protected set; } = 50; // Default health value
        public int Damage { get; protected set; } = 10; // Default damage value
        private int _attackCooldown = 0; // Cooldown between attacks
        private bool _isAttacking = false;
        private int _attackEffectDuration = 10; // Duration of the attack effect in frames

        public Sprite Sprite => _sprite;
        public float X => _sprite.X;
        public float Y => _sprite.Y;

        // Constructor: Initialize enemy with bitmap, animation, position, and health
        protected Enemy(string bitmapName, string animationScriptName, float x, float y, int initialHealth)
        {
            // Load the enemy bitmap and set cell details
            _enemyBitmap = SplashKit.LoadBitmap(bitmapName, $"{bitmapName}.png");
            _enemyBitmap.SetCellDetails(_enemyBitmap.Width / 7, _enemyBitmap.Height / 8, 7, 8, 56);

            // Load the animation script
            AnimationScript enemyAnimations = SplashKit.LoadAnimationScript(animationScriptName, $"{animationScriptName}.txt");
            _sprite = SplashKit.CreateSprite(_enemyBitmap, enemyAnimations);

            // Set the initial position
            _sprite.X = x;
            _sprite.Y = y;

            // Initialize health
            Health = initialHealth;
            Console.WriteLine("Enemy created with health: " + Health);
        }

        // Update: Update the enemy's state, including animations, attack cooldown, and messages
        public virtual void Update(Player player)
        {
            _sprite.UpdateAnimation();

            // Handle attack cooldown
            if (_attackCooldown > 0)
            {
                _attackCooldown--;
            }

            if (_isAttacking)
            {
                _attackEffectDuration--;
                if (_attackEffectDuration <= 0)
                {
                    _isAttacking = false;
                    _attackEffectDuration = 10;
                }
            }

            // Check if player is in range to attack
            if (IsInRange(player))
            {
                Attack(player);
            }

            // Update messages
            _messages.RemoveAll(msg => msg.IsExpired());
            foreach (var msg in _messages)
            {
                msg.Update();
            }
        }

        // IsInRange: Check if the player is within attack range
        public bool IsInRange(Player player)
        {
            double distance = SplashKit.PointPointDistance(new Point2D() { X = X + (_sprite.Width / 2), Y = Y + (_sprite.Height / 2) }, new Point2D() { X = player.X + (player.Sprite.Width / 2), Y = player.Y + (player.Sprite.Height / 2) });
            return distance <= AttackRange;
        }

        // Draw the enemy and any health messages or attack effects
        public virtual void Draw()
        {
            SplashKit.DrawSprite(_sprite);
            RenderMessages();
            RenderAttackEffect();
        }

        // Abstract method to handle enemy movement
        public abstract void HandleMovement(Player player, List<Wall> walls);

        // Abstract method to handle enemy attack behavior
        public abstract void HandleAttack(Player player);

        // Execute an attack on the player if cooldown allows
        public void Attack(Player player)
        {
            if (_attackCooldown == 0)
            {
                Console.WriteLine("Enemy Attacks Player");
                player.TakeDamage(Damage);
                _isAttacking = true;
                _attackCooldown = AttackSpeed;
            }
        }

        // Reduce enemy health and handle death if health reaches zero
        public void TakeDamage(int damage)
        {
            Health -= damage;
            _messages.Add(new HealthMessage(X, Y, $"-{damage}", Color.Red));

            if (Health <= 0)
            {
                Die();
            }
        }

        // Handle enemy death (e.g., set health to zero, potentially drop items)
        protected virtual void Die()
        {
            Health = 0; // Mark the enemy as dead
            // TryDropItem();
        }

        // Render all health messages on the screen
        public void RenderMessages()
        {
            foreach (var msg in _messages)
            {
                msg.Render();
            }
        }

        // Render the visual effect of an attack
        public void RenderAttackEffect()
        {
            SplashKit.DrawCircle(Color.Orange, X + (_sprite.Width / 2), Y + (_sprite.Height / 2), AttackRange);
        }

        // Check for collisions with another sprite and determine direction of collision
        public string HasCollidedWith(Sprite objects)
        {
            bool up = false;
            bool down = false;
            bool left = false;
            bool right = false;

            if (SplashKit.SpriteCollision(_sprite, objects))
            {
                if (objects.X < _sprite.X)
                {
                    left = true;
                }
                if (objects.X + objects.Width > _sprite.X + _sprite.Width)
                {
                    right = true;
                }
                if (objects.Y < _sprite.Y)
                {
                    up = true;
                }
                if (objects.Y + objects.Height > _sprite.Y + _sprite.Height)
                {
                    down = true;
                }

                if (up && left) return "Top-Left";
                if (up && right) return "Top-Right";
                if (down && left) return "Bottom-Left";
                if (down && right) return "Bottom-Right";
                if (up) return "Top";
                if (down) return "Bottom";
                if (left) return "Left";
                if (right) return "Right";

                return "Overlap";
            }

            return "No Collision";
        }
    }
}
