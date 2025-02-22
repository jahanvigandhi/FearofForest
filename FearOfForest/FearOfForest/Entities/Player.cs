// Player.cs
using FearOfForest.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FearOfForest.Entities
{
    public enum PlayerState
    {
        Run,
        Jump,
        Attack,
        Dead
    }


    public class Player
    {
        public event Action OnPlayerDeath;

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public AnimatedSprite AnimatedTexture { get; private set; }
        public bool IsOnGround { get; private set; }

        public bool IsPlayerDead => CurrentState == PlayerState.Dead;

        private float jumpStrength = 350f;
        private float gravity = 12f;

        public Rectangle BoundingBox
        {
            get
            {
                var actualPlayerWidth = AnimatedTexture.FrameWidth / 2;
                var actualPlayerHeight = AnimatedTexture.FrameHeight / 2;

                return new Rectangle(
                    (int)Position.X + actualPlayerWidth / 2,
                    (int)Position.Y + actualPlayerHeight / 2,
                    actualPlayerWidth,
                    (int)actualPlayerHeight
                );
            }
        }

        public PlayerState CurrentState { get; private set; }

        // AnimatedSprites for different states
        private AnimatedSprite _runAnimation;
        private AnimatedSprite _jumpAnimation;
        private AnimatedSprite _attackAnimation;
        private AnimatedSprite _deadAnimation;

        // Health properties
        public int MaxHealth { get; private set; } = 3; // Maximum health
        public int CurrentHealth { get; private set; }

        // Invincibility frame properties
        private bool isInvincible = false;
        private float invincibilityDuration = 1.0f; // Duration in seconds
        private float invincibilityTimer = 0f;

        // Flashing effect properties
        private bool isVisible = true;
        private float flashInterval = 0.1f; // Interval for flashing
        private float flashTimer = 0f;

        public Player(Texture2D runTexture, Vector2 startPosition, int runFrameCount = 8, float runTimePerFrame = 0.1f,
                      Texture2D jumpTexture = null, int jumpFrameCount = 15, float jumpTimePerFrame = 0.1f,
                      Texture2D attackTexture = null, int attackFrameCount = 8, float attackTimePerFrame = 0.1f,
                      Texture2D deadTexture = null, int deadFrameCount = 8, float deadTimePerFrame = 0.1f)
        {
            // Initialize animations
            _runAnimation = new AnimatedSprite(runTexture, runFrameCount, runTimePerFrame, isLooping: true);
            _jumpAnimation = new AnimatedSprite(jumpTexture, jumpFrameCount, jumpTimePerFrame, isLooping: false);
            _attackAnimation = new AnimatedSprite(attackTexture, attackFrameCount, attackTimePerFrame, isLooping: false);
            _deadAnimation = new AnimatedSprite(deadTexture, deadFrameCount, deadTimePerFrame, isLooping: false);

            // Set initial animation
            AnimatedTexture = _runAnimation;
            CurrentState = PlayerState.Run;

            Position = startPosition;
            Velocity = Vector2.Zero;
            IsOnGround = false;

            // Initialize health
            CurrentHealth = MaxHealth;
        }

        /// <summary>
        /// Reduces the player's health by the specified amount.
        /// Triggers invincibility frames to prevent immediate subsequent damage.
        /// </summary>
        /// <param name="damage">Amount of damage to take.</param>
        public void TakeDamage(int damage)
        {
            if (isInvincible || CurrentState == PlayerState.Dead)
                return;

            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Die();
            }
            else
            {
                // Trigger invincibility frames
                isInvincible = true;
                invincibilityTimer = 0f;
                flashTimer = 0f;
                isVisible = false; // Start with invisible for flashing
            }
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, List<Platform> platforms)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Fixed X position for endless runner
            Position = new Vector2(100, Position.Y);

            // Handle input for jumping and attacking based on current state
            if (CurrentState != PlayerState.Dead)
            {
                // Handle Attack
                if ((keyboardState.IsKeyDown(Keys.A) || 
                    keyboardState.IsKeyDown(Keys.LeftShift) || 
                    keyboardState.IsKeyDown(Keys.RightShift) || 
                    keyboardState.IsKeyDown(Keys.RightControl) || 
                    keyboardState.IsKeyDown(Keys.LeftControl))
                    && CurrentState != PlayerState.Attack)
                {
                    StartAttack();
                }


                // Handle Jumping
                if ((keyboardState.IsKeyDown(Keys.Space) || keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
                    && IsOnGround
                    && CurrentState != PlayerState.Jump
                    && CurrentState != PlayerState.Attack)
                {
                    StartJump();
                }
            }

            // Apply gravity
            Velocity = new Vector2(Velocity.X, Velocity.Y + gravity);

            // Update vertical position
            Position += new Vector2(0, Velocity.Y * deltaTime);

            // Collision detection with platforms
            IsOnGround = false;
            foreach (var platform in platforms)
            {
                // Allow the player to stand on the platform like 1/3 of the platform height can be ignored
                var safeBounds = new Rectangle(platform.BoundingBox.X,
                                               platform.BoundingBox.Y + platform.BoundingBox.Height / 3,
                                               platform.BoundingBox.Width,
                                               platform.BoundingBox.Height / 3 * 2);

                if (BoundingBox.Intersects(safeBounds) && Velocity.Y >= 0)
                {
                    Position = new Vector2(Position.X, platform.Position.Y - AnimatedTexture.FrameHeight);
                    Velocity = new Vector2(Velocity.X, 0);
                    IsOnGround = true;

                    // If landed after a jump, switch back to Run state
                    if (CurrentState == PlayerState.Jump)
                    {
                        SwitchState(PlayerState.Run);
                    }
                }
            }

            // Prevent player from falling below the screen
            if (Position.Y > 400)
            {
                Position = new Vector2(Position.X, 400);
                Velocity = new Vector2(Velocity.X, 0);
                IsOnGround = true;

                if (CurrentState == PlayerState.Jump)
                {
                    SwitchState(PlayerState.Run);
                }
            }

            // Handle invincibility frames
            if (isInvincible)
            {
                invincibilityTimer += deltaTime;
                flashTimer += deltaTime;

                // Toggle visibility for flashing effect
                if (flashTimer >= flashInterval)
                {
                    flashTimer -= flashInterval;
                    isVisible = !isVisible;
                }

                if (invincibilityTimer >= invincibilityDuration)
                {
                    isInvincible = false;
                    isVisible = true; // Ensure visibility is reset
                }
            }

            // Update the current animation
            AnimatedTexture.Update(gameTime);

            // Check if non-looping animations have finished
            if (CurrentState == PlayerState.Jump && _jumpAnimation.IsAnimationFinished && IsOnGround)
            {
                SwitchState(PlayerState.Run);
            }

            if (CurrentState == PlayerState.Attack && _attackAnimation.IsAnimationFinished)
            {
                SwitchState(PlayerState.Run);
            }

            if (CurrentState == PlayerState.Dead && _deadAnimation.IsAnimationFinished)
            {
                // You can handle game over logic here
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the player only if visible (for flashing effect)
            if (isVisible)
            {
                AnimatedTexture.Draw(spriteBatch, Position, Color.White);
            }
        }

        /// <summary>
        /// Switches the player's state and updates the current animation accordingly.
        /// </summary>
        /// <param name="newState">The new state to switch to.</param>
        private void SwitchState(PlayerState newState)
        {
            if (CurrentState == newState)
                return;

            CurrentState = newState;

            switch (newState)
            {
                case PlayerState.Run:
                    AnimatedTexture = _runAnimation;
                    break;
                case PlayerState.Jump:
                    AnimatedTexture = _jumpAnimation;
                    _jumpAnimation.Reset();
                    break;
                case PlayerState.Attack:
                    AnimatedTexture = _attackAnimation;
                    _attackAnimation.Reset();
                    break;
                case PlayerState.Dead:
                    AnimatedTexture = _deadAnimation;
                    _deadAnimation.Reset();
                    break;
                default:
                    AnimatedTexture = _runAnimation;
                    break;
            }
        }

        /// <summary>
        /// Initiates the Jump state.
        /// </summary>
        private void StartJump()
        {
            SwitchState(PlayerState.Jump);
            Velocity = new Vector2(Velocity.X, -jumpStrength);
            IsOnGround = false;
        }

        /// <summary>
        /// Initiates the Attack state.
        /// </summary>
        private void StartAttack()
        {
            SwitchState(PlayerState.Attack);
        }

        /// <summary>
        /// Initiates the Dead state.
        /// </summary>
        public void Die()
        {
            if (CurrentState == PlayerState.Dead)
                return;

            SwitchState(PlayerState.Dead);
            Velocity = Vector2.Zero;

            OnPlayerDeath?.Invoke();
        }
    }
}
