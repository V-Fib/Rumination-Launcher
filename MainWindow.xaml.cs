using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using WinForms = System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Reflection;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SWG_Rumination
{

    enum LauncherStatus
    {
        start,
        install,
        update,
        waiting
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string pathFolder;
        private string filePath = "pathFolder.txt";
        private string versionFile;
        private string versionServer;
        private string treServer;
        private string swgemuServer;
        private string swgemuServerip;
        private string swgemuServerConfig;
        private string swgemuServerPreConfig;
        private string swgemuServerUserConfig;
        private string swgemuServerOptionsConfig;

        private LauncherStatus _status;
        private bool isScanFinish;

        internal LauncherStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                switch (_status)
                {
                    case LauncherStatus.start:
                        //PlayButton.Content = "Choose a folder";
                        BitmapImage StartBitmap = new BitmapImage();
                        StartBitmap.BeginInit();
                        StartBitmap.UriSource = new Uri("Resources/Button_AddGame.png", UriKind.Relative);
                        StartBitmap.EndInit();
                        ButtonImage.Source = StartBitmap;
                        break;
                    case LauncherStatus.install:
                        //PlayButton.Content = "Starting the game";
                        BitmapImage InstallBitmap = new BitmapImage();
                        InstallBitmap.BeginInit();
                        InstallBitmap.UriSource = new Uri("Resources/Button_Play.png", UriKind.Relative);
                        InstallBitmap.EndInit();
                        ButtonImage.Source = InstallBitmap;
                        break;
                    case LauncherStatus.update:
                        //PlayButton.Content = "Update";
                        BitmapImage UpdateBitmap = new BitmapImage();
                        UpdateBitmap.BeginInit();
                        UpdateBitmap.UriSource = new Uri("Resources/Button_Update.png", UriKind.Relative);
                        UpdateBitmap.EndInit();
                        ButtonImage.Source = UpdateBitmap;
                        break;
                    case LauncherStatus.waiting:
                        //PlayButton.Content = "Waiting";
                        BitmapImage WaitingBitmap = new BitmapImage();
                        WaitingBitmap.BeginInit();
                        WaitingBitmap.UriSource = new Uri("Resources/Button_Waiting.png", UriKind.Relative);
                        WaitingBitmap.EndInit();
                        ButtonImage.Source = WaitingBitmap;
                        break;
                    default:
                        break;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            // Menu visibility
            MenuSettings.Dispatcher.Invoke(() =>
            {
                BackgroundSettings.Visibility = Visibility.Hidden;
                MenuSettings.Visibility = Visibility.Hidden;
            });

            // Checks if the game path exists
            if (File.Exists(filePath))
            {
                SettingsButton.Visibility = Visibility.Visible;
                Status = LauncherStatus.install;
                // Read the file
                string fileContents;
                using (StreamReader sr = File.OpenText(filePath))
                {
                    fileContents = sr.ReadToEnd();
                }
                pathFolder = fileContents;
                PathName.Text = pathFolder;
            }
            else
            {
                SettingsButton.Visibility = Visibility.Hidden;
                Status = LauncherStatus.start;
                pathFolder = String.Empty;
                PathName.Text = "...";
            }

            versionFile = System.IO.Path.Combine(pathFolder, "Version.txt");
            versionServer = "/home/rumination/Version.txt"; // Version build location on the server
            treServer = "/home/rumination/Live Patches/Rumination.tre"; // First update file
            swgemuServer = "/home/rumination/Live Patches/swgemu_live.cfg"; // Second update file
            swgemuServerip = "/home/rumination/Live Patches/swgemu_login.cfg"; // Third update file
            swgemuServerConfig = "/home/rumination/Live Patches/swgemu.cfg"; // Fourth update file
            swgemuServerPreConfig = "/home/rumination/Live Patches/swgemu_preload.cfg"; // Fifth update file
            swgemuServerUserConfig = "/home/rumination/Live Patches/user.cfg"; // Sixth update file
            swgemuServerOptionsConfig = "/home/rumination/Live Patches/options.cfg"; // Seventh update file

            CheckForUpdate();
        }

        // Window drag
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        // Close the window
        private void Button_Close(object sender, RoutedEventArgs e) => this.Close();

        // Enlarge window
        private void Button_Maximized(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        // Minimize window
        private void Button_Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        // Choose the path of the game
        private async void Play(object sender, RoutedEventArgs e)
        {
            if (File.Exists(filePath))
            {
                if(Status == LauncherStatus.update)
                {
                    DownloadConfigFiles();
                } else
                {
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "SWGEmu.exe",
                        WorkingDirectory = pathFolder,
                        WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                        UseShellExecute = true,
                    };

                    System.Diagnostics.Process.Start(psi);
                }
            } else
            {
                WinForms.FolderBrowserDialog dialog = new WinForms.FolderBrowserDialog();
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // By default, opens the "My Documents" folder
                WinForms.DialogResult result = dialog.ShowDialog();

                Status = LauncherStatus.waiting;
                await Task.Delay(1000);

                // When a folder has been selected
                if (result == WinForms.DialogResult.OK)
                {
                    DownloadConfigFiles();
                    pathFolder = dialog.SelectedPath;
                    Status = LauncherStatus.install;

                    // Backup to file
                    using (StreamWriter sw = File.CreateText(filePath))
                    {
                        sw.Write(pathFolder);
                    }

                    SettingsButton.Visibility = Visibility.Visible;
                }
                else
                {
                    pathFolder = String.Empty;
                    Status = LauncherStatus.start;
                }
                PathName.Text = pathFolder;

                // Create the version file from the server
                using (SftpClient sftp = new SftpClient("15.204.248.142", "rumination", "Jb09041964@@"))
                {
                    sftp.Connect();
                    using (Stream fileStream = File.OpenWrite(System.IO.Path.Combine(pathFolder, "Version.txt")))
                    {
                        sftp.DownloadFile(versionServer, fileStream);
                    }
                    sftp.Disconnect();
                }
            }
            
        }

        // Settings menu
        private void Open_Settings(object sender, RoutedEventArgs e)
        {

            // Menu visibility
            MenuSettings.Dispatcher.Invoke(() =>
            {
                BackgroundSettings.Visibility = Visibility.Visible;
                MenuSettings.Visibility = Visibility.Visible;
            });
        }

        // Start Scan
        private async void LaunchScan(object sender, RoutedEventArgs e)
        {
            if (!isScanFinish)
            {
                int number1 = new Random().Next(5, 25);
                int number2 = new Random().Next(26, 45);
                int number3 = new Random().Next(46, 70);
                int number4 = new Random().Next(71, 95);
                ScanText.Content = "In progress (0%)";
                await Task.Delay(5000);
                ScanText.Content = "In progress (" + number1 + "%)";
                await Task.Delay(2100);
                DownloadConfigFiles();
                ScanText.Content = "In progress (" + number2 + "%)";
                await Task.Delay(3200);
                ScanText.Content = "In progress (" + number3 + "%)";
                await Task.Delay(4100);
                ScanText.Content = "In progress (" + number4 + "%)";
                await Task.Delay(2500);
                ScanText.Content = "Finish";
                isScanFinish = true;
            }
            else
            {
                // Menu to go invisible after completed scan
                MenuSettings.Dispatcher.Invoke(() =>
                {
                    BackgroundSettings.Visibility = Visibility.Hidden;
                    MenuSettings.Visibility = Visibility.Hidden;
                });
                isScanFinish = false;
            }

        }


        // Change the path
        private void ChangePath(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog dialog = new WinForms.FolderBrowserDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // By default, opens the "My Documents" folder
            WinForms.DialogResult result = dialog.ShowDialog();

            // When a folder has been selected
            if (result == WinForms.DialogResult.OK)
            {
                pathFolder = dialog.SelectedPath;
                PlayButton.Content = "Starting the game";
                Status = LauncherStatus.install;

                // Backup to file
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.Write(pathFolder);
                }
            }
            else
            {
                pathFolder = String.Empty;
                Status = LauncherStatus.start;
            }
            PathName.Text = pathFolder;
        }

        // Checking for updates
        private void CheckForUpdate()
        {
            if (File.Exists(versionFile))
            {
                Version localVersion = new Version(File.ReadAllText(versionFile));
                try
                {
                    using (SftpClient sftp = new SftpClient("15.204.248.142", "rumination", "Jb09041964@@"))
                    {
                        try
                        {
                            sftp.Connect();
                        
                            Version serverVersion = new Version(sftp.ReadAllText(versionServer));

                            if(serverVersion == localVersion)
                            {
                                // These are the same versions
                                Status = LauncherStatus.install;
                            } else
                            {
                                // The versions are different
                                Status = LauncherStatus.update;
                            }

                            sftp.Disconnect();
                        }
                        catch (Exception ex)
                        {
                            // Error
                            MessageBox.Show("Configuration SFTP error : " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        // Downloading config and shift files
        private async void DownloadConfigFiles()
        {
            Status = LauncherStatus.waiting;

            await Task.Delay(1000);

            // Recover files
            using (SftpClient sftp = new SftpClient("15.204.248.142", "rumination", "Jb09041964@@"))
            {
                sftp.Connect();

                try
                {
                    using (Stream fileStream = File.OpenWrite(System.IO.Path.Combine(pathFolder, "Rumination.tre")))
                    {
                        sftp.DownloadFile(treServer, fileStream);
                    }
                    using (Stream fileStream = File.OpenWrite(System.IO.Path.Combine(pathFolder, "swgemu_live.cfg")))
                    {
                        sftp.DownloadFile(swgemuServer, fileStream);
                    }
                    using (Stream fileStream = File.OpenWrite(System.IO.Path.Combine(pathFolder, "swgemu_login.cfg")))
                    {
                        sftp.DownloadFile(swgemuServerip, fileStream);
                    }
                    using (Stream fileStream = File.OpenWrite(System.IO.Path.Combine(pathFolder, "swgemu.cfg")))
                    {
                        sftp.DownloadFile(swgemuServerConfig, fileStream);
                    }
                    using (Stream fileStream = File.OpenWrite(System.IO.Path.Combine(pathFolder, "swgemu_preload.cfg")))
                    {
                        sftp.DownloadFile(swgemuServerPreConfig, fileStream);
                    }
                    using (Stream fileStream = File.OpenWrite(System.IO.Path.Combine(pathFolder, "user.cfg")))
                    {
                        sftp.DownloadFile(swgemuServerUserConfig, fileStream);
                    }
                    using (Stream fileStream = File.OpenWrite(System.IO.Path.Combine(pathFolder, "options.cfg")))
                    {
                        sftp.DownloadFile(swgemuServerOptionsConfig, fileStream);
                    }
                }
                catch (System.IO.IOException)
                {
                    //Error writing the file... do something here (example)
                    MessageBox.Show("Unable to write the game files to disk. Try closing all instances of your games and run this launcher again");
                }

                sftp.Disconnect();
            }

            await Task.Delay(1000);

            Status = LauncherStatus.install;
        }

    }
}
