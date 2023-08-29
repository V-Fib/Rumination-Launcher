﻿using System;
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

        private LauncherStatus _status;

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

            // Visibilité du menu
            MenuSettings.Dispatcher.Invoke(() =>
            {
                BackgroundSettings.Visibility = Visibility.Hidden;
                MenuSettings.Visibility = Visibility.Hidden;
            });

            // Vérifie si le fichier de sauvegarde du chemin du jeu existe
            if (File.Exists(filePath))
            {
                SettingsButton.Visibility = Visibility.Visible;
                Status = LauncherStatus.install;
                // Lecture du fichier
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
            versionServer = "/home/rumination/Version.txt"; // Emplacement du build de version sur le serveur
            treServer = "/home/rumination/Live Patches/Rumination.tre"; // Premier fichier de mise à jour
            swgemuServer = "/home/rumination/Live Patches/swgemu_live.cfg"; // Second fichier de mise à jour

            CheckForUpdate();
        }

        // Drag de la fenêtre
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        // Fermer la fenêtre
        private void Button_Close(object sender, RoutedEventArgs e) => this.Close();

        // Agrandir la fenêtre
        private void Button_Maximized(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        // Réduire fenêtre
        private void Button_Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        // Choisir le chemin du jeu
        private async void Play(object sender, RoutedEventArgs e)
        {
            if (File.Exists(filePath))
            {
                if(Status == LauncherStatus.update)
                {
                    DownloadConfigFiles();
                } else
                {
                    System.Diagnostics.Process.Start(pathFolder + "/SWGEmu.exe");
                }
            } else
            {
                WinForms.FolderBrowserDialog dialog = new WinForms.FolderBrowserDialog();
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Par défaut, ouvre le dossier "Mes documents"
                WinForms.DialogResult result = dialog.ShowDialog();

                Status = LauncherStatus.waiting;
                await Task.Delay(1000);

                // Lorsque un dossier à été sélectionné
                if (result == WinForms.DialogResult.OK)
                {
                    DownloadConfigFiles();
                    pathFolder = dialog.SelectedPath;
                    Status = LauncherStatus.install;

                    // Sauvegarde sur le fichier
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

                // Créer le fichier de version depuis le serveur
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

        // Menu paramètres
        private void Open_Settings(object sender, RoutedEventArgs e)
        {

            // Visibilité du menu
            MenuSettings.Dispatcher.Invoke(() =>
            {
                BackgroundSettings.Visibility = Visibility.Visible;
                MenuSettings.Visibility = Visibility.Visible;
            });
        }

        // Lancer le scan
        private async void LaunchScan(object sender, RoutedEventArgs e)
        {
            int number1 = new Random().Next(5, 25);
            int number2 = new Random().Next(26, 45);
            int number3 = new Random().Next(46, 70);
            int number4 = new Random().Next(71, 95);
            ScanText.Content = "In progress (0%)";
            await Task.Delay(5000);
            ScanText.Content = "In progress ("+ number1 + "%)";
            await Task.Delay(2100);
            DownloadConfigFiles();
            ScanText.Content = "In progress ("+ number2 + "%)";
            await Task.Delay(3200);
            ScanText.Content = "In progress ("+ number3 + "%)";
            await Task.Delay(4100);
            ScanText.Content = "In progress ("+ number4 + "%)";
            await Task.Delay(2500);
            ScanText.Content = "Finish";
        }

        // Changer le chemin
        private void ChangePath(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog dialog = new WinForms.FolderBrowserDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Par défaut, ouvre le dossier "Mes documents"
            WinForms.DialogResult result = dialog.ShowDialog();

            // Lorsque un dossier à été sélectionné
            if (result == WinForms.DialogResult.OK)
            {
                pathFolder = dialog.SelectedPath;
                PlayButton.Content = "Starting the game";
                Status = LauncherStatus.install;

                // Sauvegarde sur le fichier
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

        // Vérification des mises à jour
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
                                // Ce sont les mêmes versions
                                Status = LauncherStatus.install;
                            } else
                            {
                                // Les versions sont différentes
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

        // Téléchargement des fichiers de config et maj
        private async void DownloadConfigFiles()
        {
            Status = LauncherStatus.waiting;

            await Task.Delay(1000);

            // Récupérer les fichiers
            using (SftpClient sftp = new SftpClient("15.204.248.142", "rumination", "Jb09041964@@"))
            {
                sftp.Connect();
                using (Stream fileStream = File.OpenWrite(System.IO.Path.Combine(pathFolder, "Rumination.tre")))
                {
                    sftp.DownloadFile(treServer, fileStream);
                }
                using (Stream fileStream = File.OpenWrite(System.IO.Path.Combine(pathFolder, "swgemu_live.cfg")))
                {
                    sftp.DownloadFile(swgemuServer, fileStream);
                }
                sftp.Disconnect();
            }

            await Task.Delay(1000);

            Status = LauncherStatus.install;
        }

    }
}