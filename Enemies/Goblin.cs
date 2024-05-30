using CustomProgramCode;
using SplashKitSDK;
using System;
using System.Collections.Generic;

namespace CustomProgramCode
{
    public class Goblin : Enemy
    {
        private const float FOLLOW_DISTANCE = 50.0f; // Distance within which the goblin will stop following the player
        private const float attackRange = 60.0f; // Distance within which the goblin will attack the player
        private string _facingDirection = "right"; // Variable to keep track of the last facing direction
        private bool _isAttacking = false;
        private const double ATTACK_COOLDOWN_TIME = 5000; // Attack speed in milliseconds (5 seconds)
        private SplashKitSDK.Timer _attackCooldownTimer; // Timer for managing attack cooldown

        public Goblin(float x, float y)
            : base("Goblin", "GoblinAnimations", x, y, 50) // Pass initial health to base constructor
        {
            // Set the initial animation state
            _sprite.StartAnimation("idleright");
            _attackCooldownTimer = new SplashKitSDK.Timer("GoblinAttackCooldown");
            _attackCooldownTimer.Start(); // Start the attack cooldown timer

            AttackSpeed = 1000;
            AttackRange = 80;
        }

        public override void HandleMovement(Player player, List<Wall> walls)
        {
            float newX = _sprite.X;
            float newY = _sprite.Y;
            bool movingHorizontally = false;

            float distanceToPlayer = SplashKit.PointPointDistance(_sprite.Position, player.Sprite.Position);

            if (distanceToPlayer > FOLLOW_DISTANCE)
            {
                _isAttacking = false;
                if (Math.Abs(player.X - _sprite.X) > 2)
                {
                    if (player.X < _sprite.X)
                    {
                        newX -= (float)SPEED;
                        if (!_doingAnim || _sprite.AnimationName() != "walkleft")
                        {
                            _sprite.StartAnimation("walkleft");
                            _doingAnim = true;
                        }
                        _facingDirection = "left";
                    }
                    else if (player.X > _sprite.X)
                    {
                        newX += (float)SPEED;
                        if (!_doingAnim || _sprite.AnimationName() != "walkright")
                        {
                            _sprite.StartAnimation("walkright");
                            _doingAnim = true;
                        }
                        _facingDirection = "right";
                    }
                    movingHorizontally = true;
                }

                if (Math.Abs(player.Y - _sprite.Y) > 2)
                {
                    if (player.Y < _sprite.Y)
                    {
                        newY -= (float)SPEED;
                        if (!movingHorizontally && (!_doingAnim || (_sprite.AnimationName() != "walkleft" && _sprite.AnimationName() != "walkright")))
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
                    else if (player.Y > _sprite.Y)
                    {
                        newY += (float)SPEED;
                        if (!movingHorizontally && (!_doingAnim || (_sprite.AnimationName() != "walkleft" && _sprite.AnimationName() != "walkright")))
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
                }
            }
            else
            {
                _doingAnim = false;
                if (!_isAttacking && distanceToPlayer <= attackRange && _attackCooldownTimer.Ticks >= ATTACK_COOLDOWN_TIME)
                {
                    HandleAttack(player);
                }
                else
                {
                    if (_facingDirection == "left")
                    {
                        _sprite.StartAnimation("idleleft");
                    }
                    else
                    {
                        _sprite.StartAnimation("idleright");
                    }
                }
            }

            // Handle collisions with walls
            foreach (var wall in walls)
            {
                string collisionDirection = HasCollidedWith(wall.Sprite);
                if (collisionDirection.Contains("Top"))
                {
                    newY = _sprite.Y + (float)SPEED; // Move down slightly to avoid getting stuck
                }
                if (collisionDirection.Contains("Bottom"))
                {
                    newY = _sprite.Y - (float)SPEED; // Move up slightly to avoid getting stuck
                }
                if (collisionDirection.Contains("Left"))
                {
                    newX = _sprite.X + (float)SPEED; // Move right slightly to avoid getting stuck
                }
                if (collisionDirection.Contains("Right"))
                {
                    newX = _sprite.X - (float)SPEED; // Move left slightly to avoid getting stuck
                }
            }

            _sprite.MoveTo(newX, newY);
        }

        public override void HandleAttack(Player player)
        {
            _isAttacking = true;

            if (Math.Abs(player.X - _sprite.X) > Math.Abs(player.Y - _sprite.Y))
            {
                if (player.X < _sprite.X)
                {
                    _sprite.StartAnimation("attackleft");
                }
                else
                {
                    _sprite.StartAnimation("attackright");
                }
            }
            else
            {
                if (player.Y < _sprite.Y)
                {
                    _sprite.StartAnimation("attackup");
                }
                else
                {
                    _sprite.StartAnimation("attackdown");
                }
            }

            _attackCooldownTimer.Reset(); // Reset the timer after an attack
            _attackCooldownTimer.Start(); // Restart the timer for the cooldown period
        }

        public override void Draw()
        {
            base.Draw();
            // Draw the health bar above the goblin
            SplashKit.FillRectangle(Color.Red, X, Y - 10, _sprite.Width * ((float)Health / 50), 5); // Assuming max health is 50
            SplashKit.DrawRectangle(Color.Black, X, Y - 10, _sprite.Width, 5); // Health bar background
        }
    }
}
