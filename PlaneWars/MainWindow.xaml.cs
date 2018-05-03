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
using MySql.Data.MySqlClient;

namespace PlaneWars
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The timer that update frames in the game window.
        /// </summary>
        private Timer timer;

        /// <summary>
        /// The avatar.
        /// </summary>
        private PlayerPlane player;

        /// <summary>
        /// The linked list that contains the enemies that are alive.
        /// </summary>
        private LinkedList<Enemy> enemies;

        /// <summary>
        /// The linked list that contains the bullets in the game window.
        /// </summary>
        private LinkedList<Bullet> bullets;

        /// <summary>
        /// The linked list that contains enemies that should be removed.
        /// </summary>
        private LinkedList<Enemy> enemiesToRemove;

        /// <summary>
        /// The linked list that contains bullets that should be removed.
        /// </summary>
        private LinkedList<Bullet> bulletsToRemove;

        /// <summary>
        /// The linked list that contains enemies that is destroying.
        /// </summary>
        private LinkedList<Enemy> enemiesToDestroy;

        /// <summary>
        /// A random number generator.
        /// </summary>
        private Random random;

        /// <summary>
        /// A counter that indicates the frame number.
        /// </summary>
        private int frameCount;

        /// <summary>
        /// Inicates whether this game is running.
        /// </summary>
        private bool running;

        /// <summary>
        /// Score of player.
        /// </summary>
        private int score;

        /// <summary>
        /// Difficulty level.
        /// </summary>
        private int level;

        /// <summary>
        /// Supply for player.
        /// </summary>
        private Supply supply;

        /// <summary>
        /// How many times that bullet 2 has been used.
        /// </summary>
        private int bullet2Count;

        /// <summary>
        /// Indicates whether the space key is pressed.
        /// </summary>
        private bool spaceKeyPressed;

        /// <summary>
        /// Name of current player.
        /// </summary>
        private string userName;

        /* Sounds */

        /// <summary>
        /// The sound that indicates that player shoot.
        /// </summary>
        private MediaPlayer musicPlayerShoot;

        /// <summary>
        /// The sound that indicates that player gets bomb supply.
        /// </summary>
        private MediaPlayer musicPlayerGetBomb;

        /// <summary>
        /// The sound that indicates that player gets double-bullet supply.
        /// </summary>
        private MediaPlayer musicPlayerGetBullet;

        /// <summary>
        /// The sound that indicates that a small enemy is destroyed.
        /// </summary>
        private MediaPlayer musicSmallEnemyDestroy;

        /// <summary>
        /// The sound that indicates that a middle enemy is destroyed.
        /// </summary>
        private MediaPlayer musicMiddleEnemyDestroy;

        /// <summary>
        /// The sound that indicates that a large enemy is destroyed.
        /// </summary>
        private MediaPlayer musicLargeEnemyDestroy;

        /// <summary>
        /// The sound that indicates that the bomb explodes.
        /// </summary>
        private MediaPlayer musicPlayerUseBomb;

        /// <summary>
        /// The sound that indicates that player clicks a button.
        /// </summary>
        private MediaPlayer musicPlayerClicked;

        /// <summary>
        /// Background music.
        /// </summary>
        private MediaPlayer musicBGM;

        /// <summary>
        /// Connection to MySql Server.
        /// </summary>
        private MySqlConnection conn;

        /// <summary>
        /// An object which is used to send command text to MySql Server and get results.
        /// </summary>
        private MySqlCommand cmd;

        /// <summary>
        /// Initializes the game plane wars.
        /// </summary>
        public MainWindow()
        {
            // Create a connection object for database connection.
            conn = new MySqlConnection(Settings.ConnectionString);

            // Initialize the MySql command object.
            cmd = new MySqlCommand();
            cmd.Connection = conn;

            /* Load sounds. */
            
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

            // Initialize timer.
            timer = new Timer(15);
            timer.Elapsed += ElapsedHandler;

            // Initialize linked lists.
            enemies = new LinkedList<Enemy>();
            bullets = new LinkedList<Bullet>();
            bulletsToRemove = new LinkedList<Bullet>();
            enemiesToRemove = new LinkedList<Enemy>();
            enemiesToDestroy = new LinkedList<Enemy>();

            // Initialize random number generator.
            random = new Random();

            // This game is running.
            running = false;

            // No supply now.
            supply = null;

            // The space key is not being pressed now.
            spaceKeyPressed = false;

            // Player score is 0 now.
            score = 0;

            // Current difficult level is 0.
            level = 0;
            
            // Double-bullet count starts from 0.
            bullet2Count = 0;
            
            // Initialize components in the window.
            InitializeComponent();
        }

        /// <summary>
        /// Starts game when this window is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize player.
            player = new PlayerPlane(playerImage);
            player.BulletKind = BulletKind.Bullet1;

            // Start to play BGM.
            musicBGM.Play();

            // Game start.
            timer.Start();
        }

        /// <summary>
        /// Updates game objects every frame.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElapsedHandler(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(Update));
        }

        /// <summary>
        /// Updates game objects every frame.
        /// </summary>
        private void Update()
        {
            // Nothing will happen if this game isn't running (i.e. pause or game over).
            if (!running)
                return;

            // Play BGM.
            PlayBgm();

            // Update fram counter.
            frameCount++;

            // Update player's position.
            PlayerMove();

            // Update bullets' position.
            BulletMoveUp();

            // Player shoots.
            PlayerShoot();

            // Check whether player uses bomb, explodes it if conditions is satisfied.
            CheckForBomb();

            // Update enemies.
            EnemiesUpdate();

            // Check if player is dead.
            CheckForPlayerDestroy();

            // Update player's score.
            UpdatePlayerScore();

            // Update difficulty level.
            UpdateLevel();

            // Generate new enemies.
            GenerateEnemy();

            // Update supplies.
            SupplyUpdate();

            // Show how many lives does player still have.
            ShowRemainingLives();

            // Check whether game is over.
            CheckForGameOver();
        }

        /// <summary>
        /// Plays BGM.
        /// </summary>
        private void PlayBgm()
        {
            // This BGM lasts for about 49.1s.
            // Play it from beginning if it played for more than 48s.
            if (musicBGM.Position > TimeSpan.FromSeconds(48))
            {
                musicBGM.Position = TimeSpan.FromSeconds(0);
                musicBGM.Play();
            }
        }

        /// <summary>
        /// Update player's position.
        /// </summary>
        private void PlayerMove()
        {
            // Do nothing if player is destroying.
            if (player.Destroying)
                return;

            // Move player plane according to keys pressed.
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

        /// <summary>
        /// Player shoots.
        /// </summary>
        private void PlayerShoot()
        {
            // Shoot every time frameCount % PlayerShootInterval == 0.
            if (frameCount % Settings.PlayerShootInterval == 0)
            {
                // Add new bullet(s) to the main scene.
                player.Shoot(mainScene, bullets);

                // Check if bullet kind should be changed.
                if (player.BulletKind == BulletKind.Bullet2)
                {
                    // Update bullet 2 counter.
                    bullet2Count++;

                    // Change bullet kind if player uses bullet 2 for Bullet2Total times.
                    if (bullet2Count % Settings.Bullet2Total == 0)
                    {
                        bullet2Count = 0;
                        player.BulletKind = BulletKind.Bullet1;
                    }
                }

                // Play shoot sound.
                musicPlayerShoot.Play();
                musicPlayerShoot.Position = TimeSpan.FromSeconds(0);
            }
        }

        /// <summary>
        /// Makes bullets move up.
        /// </summary>
        private void BulletMoveUp()
        {
            // Clear the linked list.
            bulletsToRemove.Clear();

            // Find out the bullets to remove.
            foreach (Bullet bullet in bullets)
            {
                bullet.MoveUp();

                if (bullet.Top < 0)
                    bulletsToRemove.AddFirst(bullet);
            }

            // Remove bullets that should be.
            foreach (Bullet bullet in bulletsToRemove)
            {
                bullets.Remove(bullet);
                mainScene.Children.Remove(bullet.BulletImage);
            }
        }

        /// <summary>
        /// Generates enemies. Intervals will be determined by current difficulty level.
        /// </summary>
        private void GenerateEnemy()
        {
            if (frameCount % Settings.GenerationIntervals[level].SmallEnemyGenerationInterval == 0)
                GenerateEnemy(EnemyKind.SmallEnemy);

            if (frameCount % Settings.GenerationIntervals[level].MiddleEnemyGenerationInterval == 0)
                GenerateEnemy(EnemyKind.MiddleEnemy);

            if (frameCount % Settings.GenerationIntervals[level].LargeEnemyGenerationInterval == 0)
                GenerateEnemy(EnemyKind.LargeEnemy);
        }

        /// <summary>
        /// Generate a new enemy with specified kind.
        /// </summary>
        /// <param name="enemyKind"></param>
        private void GenerateEnemy(EnemyKind enemyKind)
        {
            // Get left max.
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

            // Random generate a number as its startX.
            int startX = random.Next(Settings.EnemyLeftMin, leftMax);

            // Genetate the enemy.
            Enemy enemy = new Enemy(enemyKind, startX, level);

            // Add it to the linked list that contains all alive enemies.
            enemies.AddFirst(enemy);

            // Add it to the main scene.
            mainScene.Children.Add(enemy.EnemyImage);
        }

        /// <summary>
        /// Updates enemies.
        /// </summary>
        private void EnemiesUpdate()
        {
            // Make all alive enemies move down.
            EnemiesMoveDown();

            // Handle enemy collision events.
            EnemiesCollide();
            
            // Destroy all enemies that need to be.
            if (enemiesToDestroy.Count > 0)
                EnemiesDestroy();
        }

        /// <summary>
        /// Make all alive enemies move down.
        /// </summary>
        private void EnemiesMoveDown()
        {
            // Clear this linked list.
            enemiesToRemove.Clear();

            // Find out the enemies to remove.
            foreach (Enemy enemy in enemies)
            {
                enemy.MoveDown();

                if (enemy.Top > Settings.EnemyTopMax)
                    enemiesToRemove.AddFirst(enemy);
            }

            // Remove enemies.
            foreach (Enemy enemy in enemiesToRemove)
            {
                enemies.Remove(enemy);
                mainScene.Children.Remove(enemy.EnemyImage);
            }
        }

        /// <summary>
        /// Handles enemy collision events.
        /// </summary>
        private void EnemiesCollide()
        {
            // Handle enemy collision events with bullets.
            EnemiesCollideWithBullets();

            // Handle enemy collision events with player.
            EnemiesCollideWithPlayer();
        }

        /// <summary>
        /// Handles enemy collision events with bullets.
        /// </summary>
        private void EnemiesCollideWithBullets()
        {
            // Clear this 2 linked lists.
            bulletsToRemove.Clear();
            enemiesToRemove.Clear();

            // Traverse through each enemy.
            foreach (Enemy enemy in enemies)
            {
                // Traverse through each bullet.
                foreach (Bullet bullet in bullets)
                {
                    // If collide, decrease the enemy's HP and remove the bullet.
                    if (enemy.Collide(bullet.WarheadX, bullet.WarheadY))
                    {
                        enemy.HP--;
                        bulletsToRemove.AddLast(bullet);
                    }
                }

                // Remove the bullet.
                foreach (Bullet bullet in bulletsToRemove)
                {
                    bullets.Remove(bullet);
                    mainScene.Children.Remove(bullet.BulletImage);
                }

                // Enemies whose HP are less than or equal to 0 will be removed.
                if (enemy.HP <= 0)
                    enemiesToRemove.AddLast(enemy);
            }

            // Remove enemies whose HP are less than or equal to 0, and destroy them.
            foreach (Enemy enemy in enemiesToRemove)
            {
                enemies.Remove(enemy);
                enemiesToDestroy.AddLast(enemy);
            }
        }

        /// <summary>
        /// Handles enemy collision events with player.
        /// </summary>
        private void EnemiesCollideWithPlayer()
        {
            // Do nothing if player is destroying.
            if (player.Destroying)
                return;

            // Assume no enemy collides with player.
            Enemy collidedEnemy = null;
            bool collided = false;

            // Traverse through each enemy.
            foreach (Enemy enemy in enemies)
            {
                // Traverse through each collide point of player.
                foreach (Point2D p in player.CollisionPoints)
                {
                    // If collision occured.
                    if (enemy.Collide(p.X, p.Y))
                    {
                        // Decrease player's HP.
                        player.HP--;

                        // Mark player as destroying.
                        player.Destroying = true;
                        
                        // The enemy that collides with player will be removed.
                        collidedEnemy = enemy;
                        collidedEnemy.HP = 0;

                        collided = true;
                        enemiesToDestroy.AddLast(collidedEnemy);

                        // Break the whole loop if collision occurs.
                        break;
                    }
                }

                if (collided)
                    break;
            }

            // Remove the enemy.
            enemies.Remove(collidedEnemy);
        }

        /// <summary>
        /// Handles enemy destroy event.
        /// </summary>
        private void EnemiesDestroy()
        {
            // Clear the linked list.
            enemiesToRemove.Clear();

            // Play destroy images.
            foreach (Enemy enemy in enemiesToDestroy)
            {
                // Remove enemies that finish playing destroy images.
                if (enemy.CanBeRemoved)
                    enemiesToRemove.AddLast(enemy);
                else
                    enemy.Destroy();
            }

            // Remove enemies that finish playing destroy images.
            foreach (Enemy enemy in enemiesToRemove)
            {
                // Remove the enemy from emeies to destroy.
                enemiesToDestroy.Remove(enemy);

                // Remove the enemy from main scene.
                mainScene.Children.Remove(enemy.EnemyImage);

                // Increase player's core.
                score += enemy.Score;

                // Play explosion sound.
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

        /// <summary>
        /// Updates player score shown on the top-left of this window.
        /// </summary>
        private void UpdatePlayerScore()
        {
            txtScore.Text = "Score: " + score;
        }

        /// <summary>
        /// Updates current difficulty level.
        /// </summary>
        private void UpdateLevel()
        {
            // current difficulty level equals the index of the max value that is less than or equal to current score.
            for (int i = 0; i < Settings.LevelScores.Length; i++)
            {
                if (score >= Settings.LevelScores[i])
                    level = i + 1;
            }
        }

        /// <summary>
        /// Updates supplies.
        /// </summary>
        private void SupplyUpdate()
        {
            // Genereate supply every SupplyInterval frames.
            if (frameCount % Settings.SupplyInterval == 0)
                GenerateSupply();

            // If supply is not null, move it down and check for its collision.
            if (supply != null)
            {
                supply.MoveDown();
                SupplyCollide();
            }
        }

        /// <summary>
        /// Generates next supply.
        /// </summary>
        private void GenerateSupply()
        {
            // Determine which kind of supply to generate.
            double r = random.NextDouble();
            SupplyKind nextSupplyKind = r < 0.5 ? SupplyKind.BulletSupply : SupplyKind.BombSupply;

            // Calculate its startX.
            int startX = random.Next(Settings.SupplyLeftMin, Settings.SupplyLeftMax);

            // Generate the supply.
            supply = new Supply(nextSupplyKind, startX);

            // Add it to the main scene.
            mainScene.Children.Add(supply.SupplyImage);
        }

        /// <summary>
        /// Handles supply collision event.
        /// </summary>
        private void SupplyCollide()
        {
            // Traverse through each collision points of player.
            foreach (Point2D p in player.CollisionPoints)
            {
                // Check whether collision occured.
                if (supply.Collide(p.X, p.Y))
                {
                    if (supply.SupplyKind == SupplyKind.BulletSupply)
                    {
                        // Change player's bullet kind.
                        player.BulletKind = BulletKind.Bullet2;

                        // Play sound.
                        musicPlayerGetBullet.Play();
                        musicPlayerGetBullet.Position = TimeSpan.FromSeconds(0);
                    }
                    else
                    {
                        // Player can get an extra bomb if current bomb count less than 3.
                        if (player.BombCount < Settings.PlayerBombMax)
                        {
                            // Increase bomb count.
                            player.BombCount++;

                            // Update text that indicates how many bombs player has now.
                            txtBombCount.Text = player.BombCount.ToString();

                            // Play sound.
                            musicPlayerGetBomb.Play();
                            musicPlayerGetBomb.Position = TimeSpan.FromSeconds(0);
                        }
                    }

                    // Remove the supply.
                    mainScene.Children.Remove(supply.SupplyImage);
                    supply = null;

                    // Break if collision occurs.
                    break;
                }
            }
        }

        /// <summary>
        /// Checks for bomb event.
        /// </summary>
        private void CheckForBomb()
        {
            // If player released the space key and player has bomb, explode it.
            if (spaceKeyPressed &&
                Keyboard.IsKeyUp(Key.Space) &&
                (player.BombCount > 0))
            {
                // The space key is released.
                spaceKeyPressed = false;

                // Play sound.
                musicPlayerUseBomb.Play();
                musicPlayerUseBomb.Position = TimeSpan.FromSeconds(0);

                // Destroy enemies on the screen.
                foreach (Enemy enemy in enemies)
                    enemiesToDestroy.AddLast(enemy);

                // Clear the linked list.
                enemies.Clear();
                
                // Decrease the bomb count.
                player.BombCount--;

                // Update text that indicates how many bombs player has now.
                txtBombCount.Text = player.BombCount.ToString();
            }
        }

        /// <summary>
        /// Plays player destroy image if player is destroying.
        /// </summary>
        private void CheckForPlayerDestroy()
        {
            if (player.Destroying)
                player.Destroy();
        }

        /// <summary>
        /// Show how many lives does player still have.
        /// </summary>
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

        /// <summary>
        /// Restarts this game.
        /// </summary>
        private void Restart()
        {
            running = true;

            musicBGM.Play();

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

        /// <summary>
        /// Terminates the timer when closing this window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer.Stop();
        }

        /// <summary>
        /// Pauses this game when user clicks the pause button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdPause_Click(object sender, RoutedEventArgs e)
        {
            // Mark this game is not running now.
            running = false;

            // Make the pause button disappear.
            cmdPause.Visibility = Visibility.Collapsed;

            // Show the resume button.
            cmdResume.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Resumes this game when user clicks the resume button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdResume_Click(object sender, RoutedEventArgs e)
        {
            // Mark this game is running now.
            running = true;

            // Make the resume button disappear.
            cmdResume.Visibility = Visibility.Collapsed;

            // Show the pause button.
            cmdPause.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Processes the space key down event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                spaceKeyPressed = true;
        }

        /// <summary>
        /// Checks whether this game is over.
        /// </summary>
        private void CheckForGameOver()
        {
            // This game is over if player's HP == 0.
            if (player.HP == 0)
            {
                // Hide the main scene.
                mainScene.Visibility = Visibility.Collapsed;

                // Show the game over scene.
                gameOverScene.Visibility = Visibility.Visible;

                // Mark this game is not running now.
                running = false;

                // Pauses the BGM.
                musicBGM.Pause();

                UpdateBestScore();
            }
        }

        /// <summary>
        /// Closes this window if user clicks the game over button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdGameOver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Restarts a new game if user clicks the restart button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdRestart_Click(object sender, RoutedEventArgs e)
        {
            // Play sound.
            musicPlayerClicked.Play();
            musicPlayerClicked.Position = TimeSpan.FromSeconds(0);

            // Show the main scene.
            mainScene.Visibility = Visibility.Visible;

            // Hide the game over scene.
            gameOverScene.Visibility = Visibility.Collapsed;

            // Restart the game.
            Restart();
        }

        /// <summary>
        /// User login event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdLogin_Click(object sender, RoutedEventArgs e)
        {
            // Get user name.
            string userName = txtUserName.Text;

            // Show a warning if user name is empty.
            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("You must enter your name", "Warning");
                return;
            }

            // Get password.
            string password = txtPassword.Password;

            // Show a warning if password is empty.
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("You must enter your password", "Warning");
                return;
            }

            // Get real password of this player.
            string realPassword = GetRealPassword(userName);

            // Show a warning if no user has such name.
            // Chech password otherwise.
            if (string.IsNullOrEmpty(realPassword))
            {
                MessageBox.Show("No user has the name '" + userName + "'");
                return;
            }
            else if (realPassword != password)
            {
                MessageBox.Show("You entered a wrong password");
                return;
            }
            else
            {
                running = true;
                loginScene.Visibility = Visibility.Collapsed;
                mainScene.Visibility = Visibility.Visible;
                this.userName = userName;
            }
        }

        /// <summary>
        /// Gets the real password of the given user name.
        /// </summary>
        /// <param name="userName">Name of the player.</param>
        /// <returns></returns>
        private string GetRealPassword(string userName)
        {
            // Configure the SQL query command.
            cmd.CommandText = "SELECT Password FROM Users WHERE Name = '" + userName + "';";

            // Try to get the password from database.
            string realPassword = "";
            try
            {
                conn.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                    realPassword = reader[0].ToString();

                reader.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            // Return the password.
            return realPassword;
        }

        /// <summary>
        /// Updates best score of current player.
        /// </summary>
        private void UpdateBestScore()
        {
            // Get history best score.
            int bestScore = GetBestScore();

            // Update and save best score if this user gets a higher score.
            if (bestScore < score)
            {
                bestScore = score;
                SaveBestScore(bestScore);
            }

            // Show the best score.
            txtBestScore.Text = "Your best score: " + bestScore;
        }

        /// <summary>
        /// Gets the best score of current player.
        /// </summary>
        /// <returns>The best score of current player.</returns>
        private int GetBestScore()
        {
            // Configure the SQL query command.
            cmd.CommandText = "SELECT BestScore FROM Users WHERE Name = '" + userName + "';";

            // Try to get the score from database.
            int bestScore = 0;
            try
            {
                conn.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                    bestScore = int.Parse(reader[0].ToString());

                reader.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            // Return the player's best score.
            return bestScore;
        }

        private void SaveBestScore(int bestScore)
        {
            cmd.CommandText = "UPDATE Users SET BestScore = " + bestScore + " WHERE Name = '" + userName + "';";

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Shows the register interface.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdRegister_Click(object sender, RoutedEventArgs e)
        {
            txtLoginRegisterHint.Text = Settings.RegisterHint;
            cmdSubmit.Visibility = Visibility.Visible;
            buttonPanel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// User register submbit event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSubmit_Click(object sender, RoutedEventArgs e)
        {
            // Get user name.
            string userName = txtUserName.Text;

            // Show a warning if user name is empty.
            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("You must enter your name", "Warning");
                return;
            }

            // Get password.
            string password = txtPassword.Password;

            // Show a warning if password is empty.
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("You must enter your password", "Warning");
                return;
            }

            // Try to add this player.
            // If success, get back to login interface.
            if (AddPlayer(userName, password))
            {
                txtLoginRegisterHint.Text = Settings.LoginHint;
                cmdSubmit.Visibility = Visibility.Collapsed;
                buttonPanel.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Tries to add a new user to the database.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">Password of the user.</param>
        /// <returns>True if add successfully, otherwise, false.</returns>
        private bool AddPlayer(string userName, string password)
        {
            // Show a warning if there is a user registered with the same name.
            string realPassword = GetRealPassword(userName);
            if (!string.IsNullOrEmpty(realPassword))
            {
                MessageBox.Show("This name has been used.", "Warning");
                return false;
            }

            // Configure the SQL insert command.
            cmd.CommandText = "INSERT INTO Users VALUE ('" + userName + "', '" + password + "', 0);";

            // Try to add a new player by executing the command configured above.
            // Return true if this command is executed successfully.
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Add Success");
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            // Return false if there is something wrong with the insert command.
            return false;
        }
    }
}
