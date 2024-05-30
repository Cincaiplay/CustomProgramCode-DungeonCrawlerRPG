using CustomProgramCode;
using SplashKitSDK;
using System;
using System.Collections.Generic;

namespace CustomProgramCode
{
    public class Player
    {
        private Sprite _sprite;
        private bool _doingAnim = false;
        private const double SPEED = 0.5F;
        private string _facingDirection = "right"; // Variable to keep track of the last facing direction
        private bool _isAttacking = false;
        private SplashKitSDK.Timer _attackTimer;
        private bool _useAlternateAttack = false; // Flag to do alternate attack animations
        private int _health;
        private List<HealthMessage> _damageMessages;
        private const double ATTACK_COOLDOWN_TIME = 500; // Attack cooldown time in milliseconds (0.5 second)
        private SplashKitSDK.Timer _attackCooldownTimer; // Timer for managing attack cooldown
        private const int AttackRange = 100;  // Default Attack range
        private bool _isDead = false;
        private int damage = 20;

        public Sprite Sprite => _sprite;
        public float X => _sprite.X;
        public float Y => _sprite.Y;
        public bool IsDead => _isDead;

        // Constructor: Initialize player with bitmap, animations, health, and timers
        public Player()
        {
            // Load the player bitmap and set cell details
            Bitmap playerBitmap = SplashKit.LoadBitmap("player", "player.png");
            playerBitmap.SetCellDetails(playerBitmap.Width / 6, playerBitmap.Height / 12, 6, 12, 72); // Correct cell width and height based on the image

            // Load the animation script
            AnimationScript playerAnimations = SplashKit.LoadAnimationScript("Animations", "animations.txt");
            _sprite = SplashKit.CreateSprite(playerBitmap, playerAnimations);

            // Set the initial animation state
            _sprite.StartAnimation("idleright");

            // Initialize the attack timer
            _attackTimer = new SplashKitSDK.Timer("AttackTimer");

            // Initialize and start the attack cooldown timer
            _attackCooldownTimer = new SplashKitSDK.Timer("PlayerAttackCooldown");
            _attackCooldownTimer.Start();

            // Initialize health
            _health = 100; // Set initial health to 100

            _damageMessages = new List<HealthMessage>();
        }

        // SetSpawnPoint: Set the player's spawn position
        public void SetSpawnPoint(float x, float y)
        {
            _sprite.X = x;
            _sprite.Y = y;
        }
        
        // Update: Update the player's state, including animations and damage messages
        public void Update()
        {
            _sprite.UpdateAnimation();

            // Check if the attack animation has ended and reset to idle animation
            if (_isAttacking && _attackTimer.Ticks >= ATTACK_COOLDOWN_TIME)
            {
                _isAttacking = false;
                _attackTimer.Stop();
                _attackTimer.Reset();
                if (_facingDirection == "left")
                {
                    _sprite.StartAnimation("idleleft");
                }
                else
                {
                    _sprite.StartAnimation("idleright");
                }
            }

            // Draw damage messages
            foreach (var message in _damageMessages)
            {
                message.Render();
            }

            // Remove expired damage messages
            _damageMessages.RemoveAll(msg => msg.IsExpired());
            foreach (var msg in _damageMessages)
            {
                msg.Update();
            }
        }

        // Draw: Draw the player, health bar, and any damage messages or attack effects
        public void Draw()
        {
            SplashKit.DrawSprite(_sprite);
            // Draw the health bar above the player
            SplashKit.FillRectangle(Color.Red, X, Y - 10, _sprite.Width * ((float)_health / 100), 5);
            SplashKit.DrawRectangle(Color.Black, X, Y - 10, _sprite.Width * (100 / 100), 5);

            // Draw damage messages
            foreach (var message in _damageMessages)
            {
                message.Render();
            }

            if (_isAttacking)
            {
                SplashKit.DrawCircle(Color.Red, X + _sprite.Width / 2, Y + _sprite.Height / 2, AttackRange);
            }
        }

        // HandleInput: Handle player input for movement and actions
        public void HandleInput(List<Wall> walls, List<Enemy> enemies)
        {
            float newX = _sprite.X;
            float newY = _sprite.Y;
            bool moved = false;

            // Handle attack animation
            if (!_isAttacking && SplashKit.KeyTyped(KeyCode.SpaceKey) && _attackCooldownTimer.Ticks >= ATTACK_COOLDOWN_TIME) // Trigger attack on key press
            {
                Console.WriteLine("Attack key pressed");
                HandleAttack(enemies);
                _attackCooldownTimer.Reset(); // Reset the timer after an attack
                _attackCooldownTimer.Start(); // Restart the timer for the cooldown period
            }
            else if (!_isAttacking) // Handle movement only if not attacking
            {
                HandleMovement(ref newX, ref newY, ref moved, walls);
            }

            // If not moving, set the idle animation based on the last direction
            if (!moved && !_isAttacking)
            {
                SetIdleAnimation();
            }

            if (moved)
            {
                _sprite.MoveTo(newX, newY);
            }
        }

        // HandleAttack: Handle the attack logic and animations
        private void HandleAttack(List<Enemy> enemies)
        {
            AttackEnemy(enemies);
            _attackTimer.Start();

            // Check vertical directions first
            if (SplashKit.KeyDown(KeyCode.UpKey))
            {
                Console.WriteLine("Attack up");
                StartAttackAnimation("attackup", "attackup2");
            }
            else if (SplashKit.KeyDown(KeyCode.DownKey))
            {
                Console.WriteLine("Attack down");
                StartAttackAnimation("attackdown", "attackdown2");
            }
            // If no vertical direction is pressed, check horizontal directions
            else if (SplashKit.KeyDown(KeyCode.LeftKey))
            {
                Console.WriteLine("Attack left");
                StartAttackAnimation("attackleft", "attackleft2");
            }
            else if (SplashKit.KeyDown(KeyCode.RightKey))
            {
                Console.WriteLine("Attack right");
                StartAttackAnimation("attackright", "attackright2");
            }
            // Default to facing direction attack if no direction key is pressed
            else
            {
                if (_facingDirection == "left")
                {
                    Console.WriteLine("Default attack left");
                    StartAttackAnimation("attackleft", "attackleft2");
                }
                else if (_facingDirection == "right")
                {
                    Console.WriteLine("Default attack right");
                    StartAttackAnimation("attackright", "attackright2");
                }
                else if (_facingDirection == "up")
                {
                    Console.WriteLine("Default attack up");
                    StartAttackAnimation("attackup", "attackup2");
                }
                else if (_facingDirection == "down")
                {
                    Console.WriteLine("Default attack down");
                    StartAttackAnimation("attackdown", "attackdown2");
                }
            }

            _useAlternateAttack = !_useAlternateAttack; // Toggle the alternate attack flag
        }

        // StartAttackAnimation: Start the appropriate attack animation based on direction and type
        private void StartAttackAnimation(string primaryAnimation, string alternateAnimation)
        {
            if (_useAlternateAttack)
            {
                _sprite.StartAnimation(alternateAnimation);
                Console.WriteLine($"Starting alternate attack animation: {alternateAnimation}");
            }
            else
            {
                _sprite.StartAnimation(primaryAnimation);
                Console.WriteLine($"Starting primary attack animation: {primaryAnimation}");
            }
        }

        // HandleMovement: Handle player movement and collisions with walls
        private void HandleMovement(ref float newX, ref float newY, ref bool moved, List<Wall> walls)
        {
            bool movingUp = SplashKit.KeyDown(KeyCode.UpKey);
            bool movingDown = SplashKit.KeyDown(KeyCode.DownKey);
            bool movingLeft = SplashKit.KeyDown(KeyCode.LeftKey);
            bool movingRight = SplashKit.KeyDown(KeyCode.RightKey);

            bool canMoveUp = true;
            bool canMoveDown = true;
            bool canMoveLeft = true;
            bool canMoveRight = true;

            foreach (Wall wall in walls)
            {
                string collisionDirection = HasCollidedWith(wall.Sprite);
                if (collisionDirection == "Top" || collisionDirection == "Top-Left" || collisionDirection == "Top-Right")
                {
                    canMoveUp = false;
                }
                if (collisionDirection == "Bottom" || collisionDirection == "Bottom-Left" || collisionDirection == "Bottom-Right")
                {
                    canMoveDown = false;
                }
                if (collisionDirection == "Left" || collisionDirection == "Top-Left" || collisionDirection == "Bottom-Left")
                {
                    canMoveLeft = false;
                }
                if (collisionDirection == "Right" || collisionDirection == "Top-Right" || collisionDirection == "Bottom-Right")
                {
                    canMoveRight = false;
                }
            }

            // Add boundary checks
            if (movingUp && canMoveUp && newY - SPEED >= 0)
            {
                newY -= (float)SPEED;
                moved = true;
                if (!_doingAnim || (_facingDirection == "left" && _sprite.AnimationName() != "walkleft") || (_facingDirection == "right" && _sprite.AnimationName() != "walkright"))
                {
                    if (_facingDirection == "left")
                    {
                        _sprite.StartAnimation("walkleft");
                    }
                    else
                    {
                        _sprite.StartAnimation("walkright");
                    }
                    _doingAnim = true;
                }
            }
            if (movingDown && canMoveDown && newY + _sprite.Height + SPEED <= SplashKit.ScreenHeight())
            {
                newY += (float)SPEED;
                moved = true;
                if (!_doingAnim || (_facingDirection == "left" && _sprite.AnimationName() != "walkleft") || (_facingDirection == "right" && _sprite.AnimationName() != "walkright"))
                {
                    if (_facingDirection == "left")
                    {
                        _sprite.StartAnimation("walkleft");
                    }
                    else
                    {
                        _sprite.StartAnimation("walkright");
                    }
                    _doingAnim = true;
                }
            }
            if (movingLeft && canMoveLeft && newX - SPEED >= 0)
            {
                newX -= (float)SPEED;
                moved = true;
                if (!_doingAnim || _sprite.AnimationName() != "walkleft")
                {
                    _sprite.StartAnimation("walkleft");
                    _facingDirection = "left"; // Update the facing direction
                    _doingAnim = true;
                }
            }
            if (movingRight && canMoveRight && newX + _sprite.Width + SPEED <= SplashKit.ScreenWidth())
            {
                newX += (float)SPEED;
                moved = true;
                if (!_doingAnim || _sprite.AnimationName() != "walkright")
                {
                    _sprite.StartAnimation("walkright");
                    _facingDirection = "right"; // Update the facing direction
                    _doingAnim = true;
                }
            }
        }

        // SetIdleAnimation: Set the idle animation based on the last direction faced
        private void SetIdleAnimation()
        {
            if (_doingAnim)
            {
                if (_facingDirection == "left")
                {
                    _sprite.StartAnimation("idleleft");
                }
                else
                {
                    _sprite.StartAnimation("idleright");
                }
                _doingAnim = false;
            }
        }

        // HasCollidedWith: Check for collisions with another sprite and determine direction of collision
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

        // TakeDamage: Reduce player's health and handle death if health reaches zero
        public void TakeDamage(int amount)
        {
            // Handle player's death
            if (_health <= 0)
            {
                _health = 0;
                _isDead = true;
            }
            else
            {
                Console.WriteLine("Player gets hit");
                _health -= amount;
                _damageMessages.Add(new HealthMessage(X + _sprite.Width / 2, Y + _sprite.Height / 2, $"-{amount}", Color.Red));
            }
        }

        // AttackEnemy: Attack enemies within range and deal damage
        public void AttackEnemy(List<Enemy> enemies)
        {
            _isAttacking = true;

            foreach (var enemy in enemies)
            {
                if (IsInRange(enemy))
                {
                    enemy.TakeDamage(damage);
                }
            }
        }

        // IsInRange: Check if an enemy is within attack range
        private bool IsInRange(Enemy enemy)
        {
            double distance = SplashKit.PointPointDistance(new Point2D() { X = X + _sprite.Width / 2, Y = Y + _sprite.Height / 2 }, new Point2D() { X = enemy.X + (enemy.Sprite.Width / 2), Y = enemy.Y + (enemy.Sprite.Height / 2) });
            return distance <= AttackRange;
        }
    }
}
