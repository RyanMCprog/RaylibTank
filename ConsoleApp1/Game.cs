using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using MathClasses;
using static Raylib.Raylib;
using Raylib;
using rl = Raylib.Raylib;

namespace ConsoleApp1
{
    class Game
    {
        //Sceneobjects for tank
        SceneObject tankObject = new SceneObject();
        SceneObject turretObject = new SceneObject();
        SceneObject PointOne = new SceneObject();
        SceneObject PointTwo = new SceneObject();
        SceneObject PointThree = new SceneObject();
        SceneObject PointFour = new SceneObject();

        //Sceneobjects for wall
        SceneObject WallObject = new SceneObject();
        SceneObject WallPointOne = new SceneObject();
        SceneObject WallPointTwo = new SceneObject();
        SceneObject WallPointThree = new SceneObject();
        SceneObject WallPointFour = new SceneObject();

        //Sceneobjects for bullets
        SceneObject BulletObject = new SceneObject();
        SceneObject BPointOne = new SceneObject();
        SceneObject BPointTwo = new SceneObject();
        SceneObject BPointThree = new SceneObject();
        SceneObject BPointFour = new SceneObject();

        //the sprites for all objects
        SpriteObject BulletSprite = new SpriteObject();
        SpriteObject TankSprite = new SpriteObject();
        SpriteObject turretSprite = new SpriteObject();
        SpriteObject WallSprite = new SpriteObject();

        Stopwatch stopwatch = new Stopwatch();

        // aabb collision
        AABB myAABB = new AABB();
        AABB waAABB = new AABB();
        AABB buAABB = new AABB();

        MathClasses.Vector3[] Walltmp = new MathClasses.Vector3[4];

        MathClasses.Vector3[] tmp = new MathClasses.Vector3[4];

        MathClasses.Vector3[] Butmp = new MathClasses.Vector3[4];

        MathClasses.Vector3 BulletDirection = new MathClasses.Vector3();

        private float BulletCooldown = 0;
        private long currentTime = 0;
        private long lastTime = 0;
        private float timer = 0;
        private int fps = 1;
        private int frames;
        private int BrokenWall = 0;

        private float deltaTime = 0.005f;

        //When the game starts
        public void Init()
        {
 
            stopwatch.Start();
            lastTime = stopwatch.ElapsedMilliseconds;

            WallSprite.Load("topdowntanks/PNG/Obstacles/barrelGrey_side.png");

            BulletSprite.Load("topdowntanks/PNG/Bullets/bulletYellow_outline.png");


            WallSprite.SetPosition(-WallSprite.Width / 2.0f, -WallSprite.Height / 2.0f);

            //wallpoints set position
            WallPointOne.SetPosition(WallSprite.Width / 2.0f, WallSprite.Height / 2.0f);
            WallPointTwo.SetPosition(WallSprite.Width / 2.0f, -WallSprite.Height / 2.0f);
            WallPointThree.SetPosition(-WallSprite.Width / 2.0f, -WallSprite.Height / 2.0f);
            WallPointFour.SetPosition(-WallSprite.Width / 2.0f, WallSprite.Height / 2.0f);

            TankSprite.Load("topdowntanks/PNG/Tanks/tankBlue_outline.png");

            TankSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));

            TankSprite.SetPosition(-TankSprite.Width / 2.0f, TankSprite.Height / 2.0f);

            turretSprite.Load("topdowntanks/PNG/Tanks/barrelBlue.png");

            turretSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));

            turretSprite.SetPosition(0, turretSprite.Width / 2.0f);

            PointOne.SetRotate(-90 * (float)(Math.PI / 180.0f));
            PointOne.SetPosition(TankSprite.Width/2.0f, TankSprite.Height/2.0f);

            PointTwo.SetRotate(-90 * (float)(Math.PI / 180.0f));
            PointTwo.SetPosition(TankSprite.Width/2.0f, -TankSprite.Height/2.0f);

            PointThree.SetRotate(-90 * (float)(Math.PI / 180.0f));
            PointThree.SetPosition(-TankSprite.Width/2.0f, -TankSprite.Height/2.0f);

            PointFour.SetRotate(-90 * (float)(Math.PI / 180.0f));
            PointFour.SetPosition(-TankSprite.Width/2.0f, TankSprite.Height / 2.0f);

            
            tankObject.AddChild(PointOne);
            tankObject.AddChild(PointTwo);
            tankObject.AddChild(PointThree);
            tankObject.AddChild(PointFour);
            turretObject.AddChild(turretSprite);
            tankObject.AddChild(TankSprite);
            tankObject.AddChild(turretObject);

            WallObject.AddChild(WallPointOne);
            WallObject.AddChild(WallPointTwo);
            WallObject.AddChild(WallPointThree);
            WallObject.AddChild(WallPointFour);
            WallObject.AddChild(WallSprite);

            BulletObject.AddChild(BulletSprite);
            BulletObject.AddChild(BPointOne);
            BulletObject.AddChild(BPointTwo);
            BulletObject.AddChild(BPointThree);
            BulletObject.AddChild(BPointFour);

            WallObject.SetPosition(GetScreenWidth() / 4.0f, GetScreenHeight() / 2.0f);
            tankObject.SetPosition(GetScreenWidth() / 2.0f, GetScreenHeight() / 2.0f);
        }

        public void Shutdown()
        {

        }



        public void Update()
        {
            currentTime = stopwatch.ElapsedMilliseconds;
            deltaTime = (currentTime - lastTime) / 1000.0f;

            timer += deltaTime;
            if (timer >= 1)
            {
                fps = frames;
                frames = 0;
                timer -= 1;
            }
            frames++;
            //gets the coners of the hitbox
            tmp[0] = new MathClasses.Vector3(PointOne.GlobalTransform.m7, PointOne.GlobalTransform.m8, 0);
            tmp[1] = new MathClasses.Vector3(PointTwo.GlobalTransform.m7, PointTwo.GlobalTransform.m8, 0);
            tmp[2] = new MathClasses.Vector3(PointThree.GlobalTransform.m7, PointThree.GlobalTransform.m8, 0);
            tmp[3] = new MathClasses.Vector3(PointFour.GlobalTransform.m7, PointFour.GlobalTransform.m8, 0);
            myAABB.Fit(tmp);

            Walltmp[0] = new MathClasses.Vector3(WallPointOne.GlobalTransform.m7, WallPointOne.GlobalTransform.m8, 0);
            Walltmp[1] = new MathClasses.Vector3(WallPointTwo.GlobalTransform.m7, WallPointTwo.GlobalTransform.m8, 0);
            Walltmp[2] = new MathClasses.Vector3(WallPointThree.GlobalTransform.m7, WallPointThree.GlobalTransform.m8, 0);
            Walltmp[3] = new MathClasses.Vector3(WallPointFour.GlobalTransform.m7, WallPointFour.GlobalTransform.m8, 0);
            waAABB.Fit(Walltmp);

            Butmp[0] = new MathClasses.Vector3(BPointOne.GlobalTransform.m7, BPointOne.GlobalTransform.m8, 0);
            Butmp[1] = new MathClasses.Vector3(BPointTwo.GlobalTransform.m7, BPointTwo.GlobalTransform.m8, 0);
            Butmp[2] = new MathClasses.Vector3(BPointThree.GlobalTransform.m7, BPointThree.GlobalTransform.m8, 0);
            Butmp[3] = new MathClasses.Vector3(BPointFour.GlobalTransform.m7, BPointFour.GlobalTransform.m8, 0);
            buAABB.Fit(Butmp);

            if (myAABB.Overlaps(waAABB) && BrokenWall == 0)
            {
                Draw();
            }
            else
            {
                //Controls
                if(IsKeyPressed(KeyboardKey.KEY_L) && BulletCooldown <= 0)
                {
                    BulletObject.SetPosition(turretObject.GlobalTransform.m7, turretObject.GlobalTransform.m8);

                    BPointOne.SetPosition(BulletSprite.Width / 2.0f, BulletSprite.Height / 2.0f);
                    BPointTwo.SetPosition(BulletSprite.Width / 2.0f, -BulletSprite.Height / 2.0f);
                    BPointThree.SetPosition(-BulletSprite.Width / 2.0f, -BulletSprite.Height / 2.0f);
                    BPointFour.SetPosition(-BulletSprite.Width / 2.0f, BulletSprite.Height / 2.0f);

                    BulletDirection.x = turretObject.GlobalTransform.m1;
                    BulletDirection.y = turretObject.GlobalTransform.m2;
                    BulletCooldown = 2;
                }
                if (IsKeyDown(KeyboardKey.KEY_A))
                {
                    tankObject.Rotate(-deltaTime);
                }
                if (IsKeyDown(KeyboardKey.KEY_D))
                {
                    tankObject.Rotate(deltaTime);
                }
                if (IsKeyDown(KeyboardKey.KEY_W))
                {
                    MathClasses.Vector3 facing = new MathClasses.Vector3(tankObject.LocalTransform.m1, tankObject.LocalTransform.m2, 1) * deltaTime * 100;
                    tankObject.Translate(facing.x, facing.y);
                }
                if (IsKeyDown(KeyboardKey.KEY_S))
                {
                    MathClasses.Vector3 facing = new MathClasses.Vector3(tankObject.LocalTransform.m1, tankObject.LocalTransform.m2, 1) * deltaTime * -100;
                    tankObject.Translate(facing.x, facing.y);
                }
                if (IsKeyDown(KeyboardKey.KEY_Q))
                {
                    turretObject.Rotate(-deltaTime);
                }
                if (IsKeyDown(KeyboardKey.KEY_E))
                {
                    turretObject.Rotate(deltaTime);
                }
            }
           if(BulletCooldown > 0)
           {
                BulletObject.Translate(BulletDirection.x * 10, BulletDirection.y * 10);
                BulletCooldown -= deltaTime;
                BulletObject.Draw();
             
           }
           if(buAABB.Overlaps(waAABB))
           {
                BrokenWall = 1;
           }


            tankObject.Update(deltaTime);

            lastTime = currentTime;
        }

        public void Draw()
        {
            BeginDrawing();

            ClearBackground(Color.WHITE);
            DrawText(fps.ToString(), 10, 10, 12, Color.RED);

            
           
            if(myAABB.Overlaps(waAABB) && BrokenWall == 0)
            {
                DrawText("The Tank Has Crashed", 150, 100, 50, Color.ORANGE);
            }
            //draws a circle at each corner of the tank and the min and max of the hitbox
            tankObject.Draw();
            DrawCircle(Convert.ToInt32(PointOne.GlobalTransform.m7), Convert.ToInt32(PointOne.GlobalTransform.m8), 5, Color.GREEN);
            DrawCircle(Convert.ToInt32(PointTwo.GlobalTransform.m7), Convert.ToInt32(PointTwo.GlobalTransform.m8), 5, Color.GREEN);
            DrawCircle(Convert.ToInt32(PointThree.GlobalTransform.m7), Convert.ToInt32(PointThree.GlobalTransform.m8), 5, Color.GREEN);
            DrawCircle(Convert.ToInt32(PointFour.GlobalTransform.m7), Convert.ToInt32(PointFour.GlobalTransform.m8), 5, Color.GREEN);

            DrawCircle(Convert.ToInt32(myAABB.min.x), Convert.ToInt32(myAABB.min.y), 5 , Color.GRAY);
            DrawCircle(Convert.ToInt32(myAABB.max.x), Convert.ToInt32(myAABB.max.y), 5, Color.BLACK);
            //if the wall wall is not destroyed it will draw a circle at each corner of the box
            if (BrokenWall == 1)
            {
                DrawText("The Wall is Destroyed", 150, 200, 50, Color.BLUE);
            }
            else
            {
                WallObject.Draw();
                DrawCircle(Convert.ToInt32(WallPointOne.GlobalTransform.m7), Convert.ToInt32(WallPointOne.GlobalTransform.m8), 5, Color.GREEN);
                DrawCircle(Convert.ToInt32(WallPointTwo.GlobalTransform.m7), Convert.ToInt32(WallPointTwo.GlobalTransform.m8), 5, Color.GREEN);
                DrawCircle(Convert.ToInt32(WallPointThree.GlobalTransform.m7), Convert.ToInt32(WallPointThree.GlobalTransform.m8), 5, Color.GREEN);
                DrawCircle(Convert.ToInt32(WallPointFour.GlobalTransform.m7), Convert.ToInt32(WallPointFour.GlobalTransform.m8), 5, Color.GREEN);
            }
            //draws circles at the corners of the bullets
            DrawCircle(Convert.ToInt32(BPointOne.GlobalTransform.m7), Convert.ToInt32(BPointOne.GlobalTransform.m8), 5, Color.GREEN);
            DrawCircle(Convert.ToInt32(BPointTwo.GlobalTransform.m7), Convert.ToInt32(BPointTwo.GlobalTransform.m8), 5, Color.GREEN);
            DrawCircle(Convert.ToInt32(BPointThree.GlobalTransform.m7), Convert.ToInt32(BPointThree.GlobalTransform.m8), 5, Color.GREEN);
            DrawCircle(Convert.ToInt32(BPointFour.GlobalTransform.m7), Convert.ToInt32(BPointFour.GlobalTransform.m8), 5, Color.GREEN);

            EndDrawing();
        }
    }
}
