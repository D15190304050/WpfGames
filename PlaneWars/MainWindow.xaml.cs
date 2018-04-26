using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;

namespace PlaneWars
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer timer;
        private PlayerPlane player;
        private LinkedList<Enemy> enemies;
        private LinkedList<Bullet> bullets;
        private LinkedList<Enemy> enemiesToRemove;
        private LinkedList<Bullet> bulletsToRemove;
        private LinkedList<Enemy> enemiesToDestroy;
        private Random random;
        private int frameCount;
        private bool running;

        public MainWindow()
        {
            timer = new Timer(15);
            timer.Elapsed += ElapsedHandler;
            enemies = new LinkedList<Enemy>();
            bullets = new LinkedList<Bullet>();
            random = new Random();
            bulletsToRemove = new LinkedList<Bullet>();
            enemiesToRemove = new LinkedList<Enemy>();
            enemiesToDestroy = new LinkedList<Enemy>();
            running = true;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            player = new PlayerPlane(playerImage);
            player.BulletKind = BulletKind.Bullet1;

            timer.Start();

            //GenerateEnemy(EnemyKind.LargeEnemy);
        }

        private void ElapsedHandler(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(Update));
        }

        private void Update()
        {
            PlayerMove();
            player.SwitchNormalImage();
            player.Shoot(mainScene, bullets);
            BulletsUpdate();
            EnemiesUpdate();

            frameCount++;
            if (frameCount % Settings.EnemyGenerationInterval1 == 0)
                GenerateEnemy(EnemyKind.SmallEnemy);

            CheckForPauseResume();
            CheckForRestart();
        }

        private void PlayerMove()
        {
            if (Keyboard.IsKeyDown(Key.Up) && Keyboard.IsKeyDown(Key.Left))
            {
                player.MoveUp();
                player.MoveLeft();
            }
            else if (Keyboard.IsKeyDown(Key.Up) && (Keyboard.IsKeyDown(Key.Right)))
            {
                player.MoveUp();
                player.MoveRight();
            }
            else if (Keyboard.IsKeyDown(Key.Down) && (Keyboard.IsKeyDown(Key.Left)))
            {
                player.MoveDown();
                player.MoveLeft();
            }
            else if (Keyboard.IsKeyDown(Key.Down) && (Keyboard.IsKeyDown(Key.Right)))
            {
                player.MoveDown();
                player.MoveRight();
            }
            else if (Keyboard.IsKeyDown(Key.Up))
                player.MoveUp();
            else if (Keyboard.IsKeyDown(Key.Down))
                player.MoveDown();
            else if (Keyboard.IsKeyDown(Key.Left))
                player.MoveLeft();
            else if (Keyboard.IsKeyDown(Key.Right))
                player.MoveRight();
        }

        private void BulletsUpdate()
        {
            BulletMoveUp();
        }

        private void BulletMoveUp()
        {
            LinkedList<Bullet> bulletsToRemove = new LinkedList<Bullet>();

            foreach (Bullet bullet in bullets)
            {
                bullet.MoveUp();

                if (bullet.Top < 0)
                    bulletsToRemove.AddFirst(bullet);
            }

            foreach (Bullet bullet in bulletsToRemove)
            {
                bullets.Remove(bullet);
                mainScene.Children.Remove(bullet.BulletImage);
            }
        }

        private void GenerateEnemy(EnemyKind enemyKind)
        {
            int leftMax = 0;
            switch (enemyKind)
            {
                case EnemyKind.SmallEnemy:
                    leftMax = Settings.SmallEnemyLeftMax;
                    break;
                case EnemyKind.MiddleEnemy:
                    leftMax = Settings.MiddleEnemyLeftMax;
                    break;
                case EnemyKind.LargeEnemy:
                    leftMax = Settings.LargeEnemyLeftMax;
                    break;
            }
            int startX = random.Next(Settings.EnemyLeftMin, leftMax);
            Enemy enemy = new Enemy(enemyKind, startX);
            enemies.AddFirst(enemy);
            mainScene.Children.Add(enemy.EnemyImage);
        }

        private void EnemiesUpdate()
        {
            EnemiesMoveDown();
            EnemiesCollide();
            
            if (enemiesToDestroy.Count > 0)
                EnemiesDestroy();
        }

        private void EnemiesMoveDown()
        {
            LinkedList<Enemy> enemiesToRemove = new LinkedList<Enemy>();

            foreach (Enemy e in enemies)
            {
                e.MoveDown();

                if (e.Top > Settings.EnemyTopMax)
                    enemiesToRemove.AddFirst(e);
            }

            foreach (Enemy e in enemiesToRemove)
            {
                enemies.Remove(e);
                mainScene.Children.Remove(e.EnemyImage);
            }
        }

        private void EnemiesCollide()
        {
            EnemiesCollideWithBullets();
            EnemiesCollideWithPlayer();
        }

        private void EnemiesCollideWithBullets()
        {
            bulletsToRemove.Clear();
            enemiesToRemove.Clear();

            foreach (Enemy enemy in enemies)
            {
                foreach (Bullet bullet in bullets)
                {
                    if (enemy.Collide(bullet.WarheadX, bullet.WarheadY))
                    {
                        enemy.HP--;
                        bulletsToRemove.AddLast(bullet);
                    }
                }

                foreach (Bullet bullet in bulletsToRemove)
                {
                    bullets.Remove(bullet);
                    mainScene.Children.Remove(bullet.BulletImage);
                }

                if (enemy.HP <= 0)
                    enemiesToRemove.AddLast(enemy);
            }

            foreach (Enemy enemy in enemiesToRemove)
            {
                enemies.Remove(enemy);
                enemiesToDestroy.AddLast(enemy);
            }
        }

        private void EnemiesCollideWithPlayer()
        {
            Enemy collidedEnemy = null;
            bool collided = false;

            foreach (Enemy enemy in enemies)
            {
                foreach (Point2D p in player.CollisionPoints)
                {
                    if (enemy.Collide(p.X, p.Y))
                    {
                        player.HP = 0;

                        Rectangle r = new Rectangle();
                        Canvas.SetLeft(r, p.X);
                        Canvas.SetTop(r, p.Y);
                        r.Stroke = Brushes.Red;
                        r.StrokeThickness = 2;
                        r.Width = 20;
                        r.Height = 20;
                        mainScene.Children.Add(r);

                        mainScene.Children.Remove(playerImage);
                        collidedEnemy = enemy;
                        collidedEnemy.HP = 0;

                        collided = true;
                        enemiesToDestroy.AddLast(collidedEnemy);
                        break;
                    }
                }

                if (collided)
                    break;
            }

            enemies.Remove(collidedEnemy);
        }

        private void EnemiesDestroy()
        {
            enemiesToRemove.Clear();
            foreach (Enemy enemy in enemiesToDestroy)
            {
                if (enemy.CanRemove)
                    enemiesToRemove.AddLast(enemy);
                else
                    enemy.Destroy();
            }

            foreach (Enemy enemy in enemiesToRemove)
            {
                enemiesToDestroy.Remove(enemy);
                mainScene.Children.Remove(enemy.EnemyImage);
            }
        }

        private void CheckForPauseResume()
        {
            if (Keyboard.IsKeyDown(Key.P))
            {
                System.Diagnostics.Debug.WriteLine("Pressed");

                if (running)
                {
                    running = false;

                    timer.Stop();
                    timer = new Timer(15);
                    timer.Elapsed += ElapsedHandler;
                }
                else
                {
                    running = true;
                    timer.Start();
                }
            }
        }

        private void Restart()
        {
            timer.Stop();
            timer = new Timer(15);
            timer.Elapsed += ElapsedHandler;

            enemies.Clear();
            bullets.Clear();
            enemiesToRemove.Clear();
            bulletsToRemove.Clear();
            enemiesToDestroy.Clear();
            random = new Random();
            frameCount = 0;
            mainScene.Children.Clear();

            running = true;
            mainScene.Children.Add(playerImage);
            player = new PlayerPlane(playerImage);
            player.BulletKind = BulletKind.Bullet1;

            timer.Start();
        }

        private void CheckForRestart()
        {
            if (player.HP == 0)
                Restart();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer.Stop();
        }
    }
}
