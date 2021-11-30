using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementUnlocker
{
    /*
     * achievementInformed,"CgkIz4GOyIwQEAIQAQ"
achievementBoost,"CgkIz4GOyIwQEAIQBA"
achievementDistance1,"CgkIz4GOyIwQEAIQBQ"
achievementDistance2,"CgkIz4GOyIwQEAIQBg"
achievementDistance3,"CgkIz4GOyIwQEAIQBw"
achievementDistance4,"CgkIz4GOyIwQEAIQCA"
achievementDistance5,"CgkIz4GOyIwQEAIQCQ"
achievementCredits1,"CgkIz4GOyIwQEAIQCg"
achievementCredits2,"CgkIz4GOyIwQEAIQCw"
achievementCredits3,"CgkIz4GOyIwQEAIQDA"
achievementCredits4,"CgkIz4GOyIwQEAIQDQ"
achievementCredits5,"CgkIz4GOyIwQEAIQDg"
achievementSkins1,"CgkIz4GOyIwQEAIQDw"
achievementSkins2,"CgkIz4GOyIwQEAIQEA"
achievementSkins3,"CgkIz4GOyIwQEAIQEQ"
achievementSkins4,"CgkIz4GOyIwQEAIQEg"
achievementSkins5,"CgkIz4GOyIwQEAIQEw"
     */
    
    public static void Unlock(string achievement)
    {
        switch (achievement)
        {
            case "informed":
                Social.ReportProgress("CgkIz4GOyIwQEAIQAQ", 100.0f, b => { });
                break;
            case "boost":
                Social.ReportProgress("CgkIz4GOyIwQEAIQBA", 100.0f, b => { });
                break;
            case "distance1":
                Social.ReportProgress("CgkIz4GOyIwQEAIQBQ", 100.0f, b => { });
                break;
            case "distance2":
                Social.ReportProgress("CgkIz4GOyIwQEAIQBg", 100.0f, b => { });
                break;
            case "distance3":
                Social.ReportProgress("CgkIz4GOyIwQEAIQBw", 100.0f, b => { });
                break;
            case "distance4":
                Social.ReportProgress("CgkIz4GOyIwQEAIQCA", 100.0f, b => { });
                break;
            case "distance5":
                Social.ReportProgress("CgkIz4GOyIwQEAIQCQ", 100.0f, b => { });
                break;
            case "credits1":
                Social.ReportProgress("CgkIz4GOyIwQEAIQCg", 100.0f, b => { });
                break;
            case "credits2":
                Social.ReportProgress("CgkIz4GOyIwQEAIQCw", 100.0f, b => { });
                break;
            case "credits3":
                Social.ReportProgress("CgkIz4GOyIwQEAIQDA", 100.0f, b => { });
                break;
            case "credits4":
                Social.ReportProgress("CgkIz4GOyIwQEAIQDQ", 100.0f, b => { });
                break;
            case "credits5":
                Social.ReportProgress("CgkIz4GOyIwQEAIQDg", 100.0f, b => { });
                break;
            case "skins1":
                Social.ReportProgress("CgkIz4GOyIwQEAIQDw", 100.0f, b => { });
                break;
            case "skins2":
                Social.ReportProgress("CgkIz4GOyIwQEAIQEA", 100.0f, b => { });
                break;
            case "skins3":
                Social.ReportProgress("CgkIz4GOyIwQEAIQEQ", 100.0f, b => { });
                break;
            case "skins4":
                Social.ReportProgress("CgkIz4GOyIwQEAIQEg", 100.0f, b => { });
                break;
            case "skins5":
                Social.ReportProgress("CgkIz4GOyIwQEAIQEw", 100.0f, b => { });
                break;
        }
        
    }
}
