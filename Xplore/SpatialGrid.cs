﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{

    public interface ICollisionEntity
    {
        Rectangle BoundingBox { get;}
        Circle BoundingCircle { get; }
    }

    public class SpatialGrid
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _size;
        private Dictionary<string, List<ICollisionEntity>> _grid = new Dictionary<string, List<ICollisionEntity>>();
        private Rectangle _gridRectangle;


        public SpatialGrid(Rectangle rectangle, int size)
        {
            _width = rectangle.Width/size;
            _height = rectangle.Height/size;
            _size = size;
            _gridRectangle = rectangle;
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    _grid[$"{i}_{j}"] = new List<ICollisionEntity>();
                }
            }
            
        }

        public Dictionary<string, List<ICollisionEntity>> GetCollsionGrid()
        {
            return _grid;
        } 

        public void Insert(Rectangle rectangle,ICollisionEntity collisionEntity)
        {
            //check if the rectangle is in the bounds of this spatial grid - if not do not even bother adding it
            if (rectangle.X > _gridRectangle.X + _gridRectangle.Width || rectangle.Y > _gridRectangle.Y + _gridRectangle.Height)
            {
                return;
            }
            if (rectangle.X < _gridRectangle.X || rectangle.Y < _gridRectangle.Y)
            {
                return;
            }

            Vector2 topLeft = new Vector2(rectangle.X,rectangle.Y);
            Vector2 topRight = new Vector2(rectangle.X + rectangle.Width,rectangle.Y);
            Vector2 bottomLeft = new Vector2(rectangle.X,rectangle.Y+_height);
            Vector2 bottomRight = new Vector2(rectangle.X+_width,rectangle.Y+_height);

            //GridPositions
            //top left
            int xTopLeft = (int)Math.Floor((topLeft.X - (_gridRectangle.X)) / _size);
            int yTopLeft = (int)Math.Floor((topLeft.Y - (_gridRectangle.Y)) / _size);

            //top right
            var xTopRight = (int)Math.Floor((topRight.X - (_gridRectangle.X)) / _size);
            var yTopRight = (int)Math.Floor((topRight.Y - (_gridRectangle.Y)) / _size);

            
            //bottom left
            var xBottomLeft = (int)Math.Floor((bottomLeft.X - (_gridRectangle.X)) / _size);
            var yBottomLeft = (int)Math.Floor((bottomLeft.Y - (_gridRectangle.Y)) / _size);

            //bottom right
            var xBottomRight = (int)Math.Floor((bottomLeft.X - (_gridRectangle.X)) / _size);
            var yBottomRight = (int)Math.Floor((bottomLeft.Y - (_gridRectangle.Y)) / _size);

            for (int x = xTopLeft; x <= xTopRight; x++)
            {
                for (int y = yTopLeft; y <= yTopRight; y++)
                {
                    var key = $"{x}_{y}";
                    if (!_grid[key].Contains(collisionEntity))
                    {
                        _grid[key].Add(collisionEntity);
                    }
                }
            }

            for (int x = xBottomLeft; x <= xBottomRight; x++)
            {
                for (int y = yBottomLeft; y <= yBottomRight; y++)
                {
                    var key = $"{x}_{y}";
                    if (!_grid[key].Contains(collisionEntity))
                    {
                        _grid[key].Add(collisionEntity);
                    }
                }
            }

            Debug.WriteLine("Done");
        }

        public void RenderGrid(SpriteBatch spriteBatch)
        {
            
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var worldPos = Camera.GetWorldPosition(new Vector2(x*_size, y*_size));

                        DrawHelper.DrawRectangle(new Rectangle((int)worldPos.X, (int)worldPos.Y, _size, _size), ContentProvider.OutlineTexture, Color.Black, spriteBatch, false, 1, 0f, worldPos);
                        spriteBatch.DrawString(ContentProvider.SpriteFont, $"{x}_{y}",new Vector2(worldPos.X+10,worldPos.Y+10), Color.Black);
                    if (_grid[$"{x}_{y}"].Count > 0)
                    {
                        DrawHelper.DrawRectangle(new Rectangle((int)worldPos.X, (int)worldPos.Y, _size, _size), ContentProvider.OutlineTexture, Color.Black, spriteBatch, true, 1, 0f, worldPos);
                    }
                    
                    //else
                    //{
                    //    DrawHelper.DrawRectangle(new Rectangle((int)worldPos.X, (int)worldPos.Y, _size, _size), ContentProvider.OutlineTexture, Color.Black, spriteBatch, true, 1, 0f, worldPos);
                    //}

                }    
            }
        }




    }
}