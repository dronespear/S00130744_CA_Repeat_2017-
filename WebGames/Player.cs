﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace WebGames
{
    public class Player : AnimatedSprite
    {
        public enum DIRECTION { LEFT, RIGHT, UP, DOWN, STANDING };
        DIRECTION _direction = DIRECTION.STANDING;

        public DIRECTION Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        int _score;
        float _speed;
        Texture2D[] _textures;
        SoundEffect[] _directionSounds;
        SoundEffectInstance _soundPlayer;
        WeaponAttachment _axeattactment;//An attachment for our player, can be subtituted for a list of attachments

        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }

        public Player(Texture2D[] tx, SoundEffect[] sounds,
            Vector2 pos, int frameCount,
            int startScore, float speed, WeaponAttachment axeattachment)
            : base(tx[0], pos, frameCount)
        {
            _score = startScore;
            _speed = speed;
            _textures = tx;
            _directionSounds = sounds;
            _axeattactment = axeattachment;
            _axeattactment.ParentBody = this;
        }

        public void update(GameTime gameTime)
        {
            base.UpdateAnimation(gameTime);
            // TODO: Add your update logic here
            _direction = DIRECTION.STANDING;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _direction = DIRECTION.LEFT;
                base.Move(new Vector2(-1, 0) * _speed);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                _direction = DIRECTION.UP;
                base.Move(new Vector2(0, -1) * _speed);
            }
            if
            (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                _direction = DIRECTION.DOWN;
                base.Move(new Vector2(0, 1) * _speed);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _direction = DIRECTION.RIGHT;
                base.Move(new Vector2(1, 0) * _speed);
            }
            SpriteImage = _textures[(int)_direction];

            _axeattactment.Update();

            if (_soundPlayer == null || _soundPlayer.State == SoundState.Stopped)
            {
                if (_direction != DIRECTION.STANDING)
                {
                    _soundPlayer = _directionSounds[(int)_direction].CreateInstance();
                    _soundPlayer.Play();
                }
            }


        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont borrowedFont)
        {
            _axeattactment.Draw(spriteBatch);
            base.Draw(spriteBatch, borrowedFont);
        }
    }


}