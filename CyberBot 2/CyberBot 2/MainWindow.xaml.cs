using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CyberBot
{
    public delegate void DisplayMessageDelegate(string text, Brush textColor, bool isMonospaced);
    public partial class MainWindow : Window
    {
        private ChatbotEngine? botEngine;
        private bool isNameRegistered = false;
        public MainWindow()
        {
            InitializeComponent();
            ExecuteStartupSequences();
        }
        private void ExecuteStartupSequences()
        {
            new Voice();

            ascii_logo logoAsset = new ascii_logo();
            LogMessage(logoAsset.GeneratedAsciiText, Brushes.Cyan, true);

            LogMessage("SYSTEM: [Security environment loaded successfully]\n", Brushes.DarkGray, false);
            LogMessage("CyberBot: Welcome user! Let's establish a secure session. Please start by entering your name below.\n", Brushes.PaleGreen, false);
        }
        private void LogMessage(string text, Brush textColor, bool isMonospaced)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new DisplayMessageDelegate(LogMessage), new object[] { text, textColor, isMonospaced });
                return;
            }
            TextBlock messageBlock = new TextBlock
            {
                Text = text,
                Foreground = textColor,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 4, 0, 4),
                FontSize = isMonospaced ? 11 : 13,
                FontFamily = isMonospaced ? new FontFamily("Consolas") : new FontFamily("Segoe UI")
            };

            ChatStack.Children.Add(messageBlock);
            ChatScrollViewer.ScrollToEnd();
        }
        private void HandleInputCycle()
        {
            string rawInput = txtUserInput.Text.Trim();
            if (string.IsNullOrEmpty(rawInput)) return;

            txtUserInput.Clear();

            if (!isNameRegistered)
            {
                LogMessage($"You: {rawInput}", Brushes.DodgerBlue, false);

                botEngine = new ChatbotEngine(rawInput);
                isNameRegistered = true;

                lblStatusHeader.Text = $"Secure Tunnel Session Node: Active [{botEngine.UserName}]";
                LogMessage($"\nCyberBot: Welcome, {botEngine.UserName}! Feel free to inquire about 'passwords', 'scams', 'privacy', or 'phishing'. Let me know if you are feeling worried or frustrated about threats.\n", Brushes.PaleGreen, false);
            }
            else
            {
                LogMessage($"{botEngine!.UserName}: {rawInput}", Brushes.DodgerBlue, false);
                if (rawInput.ToLower() == "exit")
                {
                    LogMessage($"CyberBot: Connection terminated. Goodbye {botEngine.UserName}! Stay alert and protect your system footprint.", Brushes.PaleGreen, false);
                    txtUserInput.IsEnabled = false;
                    btnSend.IsEnabled = false;
                    return;
                }
                string botReplyText = botEngine!.ProcessInput(rawInput);
                LogMessage($"CyberBot: {botReplyText}\n", Brushes.PaleGreen, false);
            }
        }
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            HandleInputCycle();
        }
        private void TxtUserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                HandleInputCycle();
            }
        }
    }
}