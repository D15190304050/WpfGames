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
        private int score;
        private int level;
        private Supply supply;
        private int bullet2Count;
        private bool spaceKeyPressed;
        private MediaPlayer musicPlayerShoot;
        private MediaPlayer musicPlayerGetBomb;
        private MediaPlayer musicPlayerGetBullet;
        private MediaPlayer musicSmallEnemyDestroy;
        private MediaPlayer musicMiddleEnemyDestroy;
        private MediaPlayer musicLargeEnemyDestroy;
        private MediaPlayer musicPlayerUseBomb;
        private MediaPlayer musicPlayerClicked;
        private MediaPlayer musicBGM;

        public MainWindow()
        {
            // Load sounds.
            musicPlayerShoot = new MediaPlayer();
            musicPlayerShoot.Open(new Uri("Sounds/bullet.wav", UriKind.Relative));

            musicPlayerGetBomb = new MediaPlayer();
            musicPlayerGetBomb.Open(new Uri("Sounds/get_bomb.wav", UriKind.Relative));

            musicPlayerGetBullet = new MediaPlayer();
            musicPlayerGetBullet.Open(new Uri("Sounds/get_bullet.wav", UriKind.Relative));

            musicSmallEnemyDestroy = new MediaPlayer();
            musicSmallEnemyDestroy.Open(new Uri("Sounds/enemy1_down.wav", UriKind.Relative));

            musicMiddleEnemyDestroy = new MediaPlayer();
            musicMiddleEnemyDestroy.Open(new Uri("Sounds/enemy2_down.wav", UriKind.Relative));

            musicLargeEnemyDestroy = new MediaPlayer();
            musicLargeEnemyDestroy.Open(new Uri("Sounds/enemy3_down.wav", UriKind.Relative));

            musicPlayerUseBomb = new MediaPlayer();
            musicPlayerUseBomb.Open(new Uri("Sounds/use_bomb.wav", UriKind.Relative));

            musicPlayerClicked = new MediaPlayer();
            musicPlayerClicked.Open(new Uri("Sounds/button.wav", UriKind.Relative));

            musicBGM = new MediaPlayer();
            musicBGM.Open(new Uri("Sounds/game_music.mp3", UriKind.Relative));

            timer = new Timer(15);
            timer.Elapsed += ElapsedHandler;
            enemies = new LinkedList<Enemy>();
            bullets = new LinkedList<Bullet>();
            random = new Random();
            bulletsToRemove = new LinkedList<Bullet>();
            enemiesToRemove = new LinkedList<Enemy>();
            enemiesToDestroy = new LinkedList<Enemy>();
            running = true;
            score = 0;
            level = 0;
            supply = null;
            bullet2Count = 0;
            spaceKeyPressed = false;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            player = new PlayerPlane(playerImage);
            player.BulletKind = BulletKind.Bullet1;

            musicBGM.Play();

            timer.Start();
        }

        private void ElapsedHandler(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(Update));
        }

        private void Update()
        {
            if (!running)
                return;

            PlayBgm();
            frameCount++;
            PlayerMove();
            BulletsUpdate();
            PlayerShoot();
            CheckForBomb();
            EnemiesUpdate();
            CheckForGameOver();
            CheckForPlayerDestroy();
            UpdatePlayerScore();
            UpdateLevel();
            GenerateEnemy();
            SupplyUpdate();
            ShowRemainingLives();
        }

        private void PlayBgm()
        {
            if (musicBGM.Position > TimeSpan.FromSeconds(49))
            {
                musicBGM.Position = TimeSpan.FromSeconds(0);
                musicBGM.Play();
            }
        }

        private void PlayerMove()
        {
            if (player.Destroying)
                return;

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

        private void PlayerShoot()
        {
            if (frameCount % Settings.PlayerShootInterval == 0)
            {
                player.Shoot(mainScene, bullets);

                if (player.BulletKind == BulletKind.Bullet2)
                {
                    bullet2Count++;
                    if (bullet2Count % Settings.Bullet2Total == 0)
                    {
                        bullet2Count = 0;
                        player.BulletKind = BulletKind.Bullet1;
                    }
                }

                musicPlayerShoot.Play();
                musicPlayerShoot.Position = TimeSpan.FromSeconds(0);
            }
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

        private void GenerateEnemy()
        {
            if (frameCount % Settings.GenerationIntervals[level].SmallEnemyGenerationInterval == 0)
                GenerateEnemy(EnemyKind.SmallEnemy);

            if (frameCount % Settings.GenerationIntervals[level].MiddleEnemyGenerationInterval == 0)
                GenerateEnemy(EnemyKind.MiddleEnemy);

            if (frameCount % Settings.GenerationIntervals[level].LargeEnemyGenerationInterval == 0)
                GenerateEnemy(EnemyKind.LargeEnemy);
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
            Enemy enemy = new Enemy(enemyKind, startX, level);
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
                        player.HP--;
                        player.Destroying = true;
                        
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
                score += enemy.Score;

                switch (enemy.EnemyKind)
                {
                    case EnemyKind.SmallEnemy:
                        musicSmallEnemyDestroy.Play();
                        musicSmallEnemyDestroy.Position = TimeSpan.FromSeconds(0);
                        break;
                    case EnemyKind.MiddleEnemy:
                        musicMiddleEnemyDestroy.Play();
                        musicMiddleEnemyDestroy.Position = TimeSpan.FromSeconds(0);
                        break;
                    case EnemyKind.LargeEnemy:
                        musicLargeEnemyDestroy.Play();
                        musicLargeEnemyDestroy.Position = TimeSpan.FromSeconds(0);
                        break;
                }
            }
        }

        private void UpdatePlayerScore()
        {
            txtScore.Text = "Score: " + score;
        }

        private void UpdateLevel()
        {
            for (int i = 0; i < Settings.LevelScores.Length; i++)
            {
                if (score >= Settings.LevelScores[i])
                    level = i + 1;
            }
        }

        private void SupplyUpdate()
        {
            if (frameCount % Settings.SupplyInterval == 0)
                GenerateSupply();

            if (supply != null)
            {
                supply.MoveDown();
                SupplyCollide();
            }
        }

        private void GenerateSupply()
        {
            double r = random.NextDouble();
            SupplyKind nextSupplyKind = r < 0.5 ? SupplyKind.BulletSupply : SupplyKind.BombSupply;

            int startX = random.Next(Settings.SupplyLeftMin, Settings.SupplyLeftMax);
            supply = new Supply(nextSupplyKind, startX);
            mainScene.Children.Add(supply.SupplyImage);
        }

        private void SupplyCollide()
        {
            foreach (Point2D p in player.CollisionPoints)
            {
                if (supply.Collide(p.X, p.Y))
                {
                    if (supply.SupplyKind == SupplyKind.BulletSupply)
                    {
                        player.BulletKind = BulletKind.Bullet2;
                        musicPlayerGetBullet.Play();
                        musicPlayerGetBullet.Position = TimeSpan.FromSeconds(0);
                    }
                    else
                    {
                        if (player.BombCount < 3)
                        {
                            player.BombCount++;
                            txtBombCount.Text = player.BombCount.ToString();

                            musicPlayerGetBomb.Play();
                            musicPlayerGetBomb.Position = TimeSpan.FromSeconds(0);
                        }
                    }

                    mainScene.Children.Remove(supply.SupplyImage);
                    supply = null;

                    break;
                }
            }
        }

        private void CheckForBomb()
        {
            if (spaceKeyPressed &&
                Keyboard.IsKeyUp(Key.Space) &&
                (player.BombCount > 0))
            {
                spaceKeyPressed = false;

                musicPlayerUseBomb.Play();
                musicPlayerUseBomb.Position = TimeSpan.FromSeconds(0);

                foreach (Enemy enemy in enemies)
                    enemiesToDestroy.AddLast(enemy);
                enemies.Clear();
                player.BombCount--;
                txtBombCount.Text = player.BombCount.ToString();
            }
        }

        private void CheckForPlayerDestroy()
        {
            if (player.Destroying)
                player.Destroy();
        }

        private void ShowRemainingLives()
        {
            switch (player.HP)
            {
                // In fact, the branch case 0 will never be accessed.
                case 0:
                    imgLife1.Visibility = Visibility.Collapsed;
                    imgLife2.Visibility = Visibility.Collapsed;
                    imgLife3.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    imgLife1.Visibility = Visibility.Visible;
                    imgLife2.Visibility = Visibility.Collapsed;
                    imgLife3.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    imgLife1.Visibility = Visibility.Visible;
                    imgLife2.Visibility = Visibility.Visible;
                    imgLife3.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    imgLife1.Visibility = Visibility.Visible;
                    imgLife2.Visibility = Visibility.Visible;
                    imgLife3.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void Restart()
        {
            running = true;

            enemies.Clear();
            bullets.Clear();
            enemiesToRemove.Clear();
            bulletsToRemove.Clear();
            enemiesToDestroy.Clear();
            random = new Random();
            frameCount = 0;
            score = 0;
            level = 0;
            bullet2Count = 0;
            supply = null;
            spaceKeyPressed = false;
            mainScene.Children.Clear();
            imgLife1.Visibility = Visibility.Visible;
            imgLife2.Visibility = Visibility.Visible;
            imgLife3.Visibility = Visibility.Visible;

            mainScene.Children.Add(playerImage);
            player = new PlayerPlane(playerImage);
            player.BulletKind = BulletKind.Bullet1;
            mainScene.Children.Add(imgBomb);
            mainScene.Children.Add(imgLife1);
            mainScene.Children.Add(imgLife2);
            mainScene.Children.Add(imgLife3);
            mainScene.Children.Add(txtBombCount);
            mainScene.Children.Add(txtScore);

            txtBombCount.Text = player.BombCount.ToString();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer.Stop();
        }

        private void cmdPause_Click(object sender, RoutedEventArgs e)
        {
            running = false;
            cmdPause.Visibility = Visibility.Collapsed;
            cmdResume.Visibility = Visibility.Visible;
        }

        private void cmdResume_Click(object sender, RoutedEventArgs e)
        {
            running = true;
            cmdResume.Visibility = Visibility.Collapsed;
            cmdPause.Visibility = Visibility.Visible;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                spaceKeyPressed = true;
        }

        private void CheckForGameOver()
        {
            if (player.HP == 0)
            {
                mainScene.Visibility = Visibility.Collapsed;
                gameOverScene.Visibility = Visibility.Visible;
                running = false;
            }
        }

        private void cmdGameOver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cmdRestart_Click(object sender, RoutedEventArgs e)
        {
            musicPlayerClicked.Play();
            musicPlayerClicked.Position = TimeSpan.FromSeconds(0);

            mainScene.Visibility = Visibility.Visible;
            gameOverScene.Visibility = Visibility.Collapsed;
            Restart();
        }
    }
}
