using System;
using System.Collections.Generic;

namespace CyberBot
{
    public class ChatbotEngine
    {
        public string UserName { get; private set; }
        public string FavoriteTopic { get; private set; } = "";
        private bool hasSavedTopic = false;
        private string lastDiscussedTopic = "";
        private Dictionary<string, List<string>> topicResponses = new Dictionary<string, List<string>>();
        private List<string> defaultFallbackResponses = new List<string>();

        public ChatbotEngine(string userName)
        {
            UserName = string.IsNullOrWhiteSpace(userName) ? "User" : userName;
            InitializeResponses();
        }
        private void InitializeResponses()
        {
            topicResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "password", new List<string>
                    {
                        "Make sure to use strong, unique passwords with letters, numbers, and symbols.",
                        "Never reuse the same password across multiple accounts. Try using a secure password manager!",
                        "Change your passwords immediately if you suspect a service you use has leaked data."
                    }
                },
                {
                    "phishing", new List<string>
                    {
                        "Avoid clicking suspicious links or downloading email attachments from unknown senders.",
                        "Phishing emails often create a false sense of urgency. Always double-check the sender's exact address!",
                        "Look closely for subtle spelling mistakes or weird domain extensions in suspicious emails."
                    }
                },
                {
                    "privacy", new List<string>
                    {
                        "Review your social media privacy settings and restrict how much personal info is public.",
                        "Be careful about sharing location data or personal identifiers like ID numbers online.",
                        "Use virtual private networks (VPNs) when connecting to unencrypted public Wi-Fi networks."
                    }
                },
                {
                    "scam", new List<string>
                    {
                        "If an online offer sounds too good to be true, it almost certainly is a scam.",
                        "Never send money or crypto to someone you have only met online or through messaging apps.",
                        "Scammers often impersonate support agents. Official companies will never ask for your PIN or OTP."
                    }
                }
            };
            defaultFallbackResponses = new List<string>
            {
                "I didn't quite catch that. Could you try rephrasing your question?",
                "I'm still learning! Can you ask me about passwords, phishing, privacy, or scams?",
                "I'm not sure how to answer that. Try asking for a 'phishing tip' or 'password safety advice'."
            };
        }
        public string ProcessInput(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
            {
                return "Please type something so I can help you stay safe online!";
            }
            string inputLower = userInput.ToLower();
            string? sentimentResponse = DetectAndRespondToSentiment(inputLower);
            if (!string.IsNullOrEmpty(sentimentResponse))
            {
                return sentimentResponse;
            }
            if ((inputLower.Contains("explain more") || inputLower.Contains("tell me more") || inputLower.Contains("another tip"))
                && !string.IsNullOrEmpty(lastDiscussedTopic))
            {
                return GetRandomResponse(lastDiscussedTopic);
            }
            foreach (var key in topicResponses.Keys)
            {
                if (inputLower.Contains(key))
                {
                    if (!hasSavedTopic)
                    {
                        FavoriteTopic = key;
                        hasSavedTopic = true;
                        lastDiscussedTopic = key;
                        return $"Great! I'll remember that you're interested in {FavoriteTopic}. It's a crucial part of staying safe online.\n\nHere is a tip: {GetRandomResponse(key)}";
                    }

                    lastDiscussedTopic = key;
                    return GetRandomResponse(key);
                }
            }
            if (hasSavedTopic && new Random().Next(0, 4) == 0)
            {
                return $"As someone specifically interested in {FavoriteTopic}, you might also want to review your security settings regularly. Regarding your question: I am not completely sure, could you rephrase?";
            }
            Random rnd = new Random();
            return defaultFallbackResponses[rnd.Next(defaultFallbackResponses.Count)];
        }
        private string GetRandomResponse(string topic)
        {
            if (!topicResponses.ContainsKey(topic)) return "Stay safe online!";
            List<string> responses = topicResponses[topic];
            Random rnd = new Random();
            return responses[rnd.Next(responses.Count)];
        }
        private string? DetectAndRespondToSentiment(string input)
        {
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("anxious"))
            {
                return "It is completely normal to feel worried about online security. Take a deep breath! Following basic rules like using unique passwords helps block up to 99% of common attacks.";
            }
            if (input.Contains("frustrated") || input.Contains("annoyed") || input.Contains("confused"))
            {
                return "Cybersecurity can feel incredibly overwhelming with all its rules and jargon. Don't worry, let's take it one simple step at a time. What topic is confusing you?";
            }
            if (input.Contains("curious") || input.Contains("excited") || input.Contains("interested"))
            {
                return "I love that enthusiasm! Being proactive and curious is the absolute best defense against modern cyber threats. What would you like to explore first?";
            }
            return null;
        }
    }
}