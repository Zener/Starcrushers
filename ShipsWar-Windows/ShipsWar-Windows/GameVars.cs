///////////////////////////////////////////////////////////////////////////////////////////////
// Starcrushers
// Programmed by Carlos Peris
// Barcelona, March-June 2007
///////////////////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Text;

namespace ShipsWar
{
    class GameVars
    {
        #if !XBOX
        public const int MAX_SHIPS_IN_UNIVERSE = 2500;
        #else
        public const int MAX_SHIPS_IN_UNIVERSE = 1500;
        #endif
        public static float soundVolume = 0.5f;
        public static float musicVolume = 0.5f;
        public static int aspectRatio = 0;
        public static bool seeAIShips = true;

        public static float destLogoScale = 0.5f;
        public static bool tutorialMode = false;
        public static bool fullScreenMode = false;



        public static float cameraStartZ = 10.0f;        
        public static float cameraDefaultZ = 95.0f;
        public static float cameraScalableZ = 25.0f;
        public static float cameraPlanetZ = 80.0f;

        public static float planetRangeZ = 15.0f;

        public static int tutorialStep = 0;
        public static String TUTORIAL_TEXT_1 = "Welcome to Starcrushers tutorial. You play with your mothership, which is from the color of your faction. In this game you play with the green faction.";
        public static String TUTORIAL_TEXT_2 = "The number drawed on top of your  mothership indicates the number of fighters inside it. Your mothership is building new fighters constantly. The greater your empire is, the faster new fighters will be produced in your mothership.";

        #if XBOX        
        public static String TUTORIAL_TEXT_3 = "Your cursor is initially placed on your mothership. Try to move your cursor to the planet below your mothership. Use the left analog stick to move the cursor.";
        #else
        public static String TUTORIAL_TEXT_3 = "Your cursor is initially placed on your mothership. Try to move your cursor to the planet below your mothership. Use the left analog stick, mouse or direction keys to move.";        
        #endif

        #if XBOX
        public static String TUTORIAL_TEXT_4 = "Great! Now let's conquer this planet. Send some ships keeping pressed the A button or the right trigger.";
        #else
        public static String TUTORIAL_TEXT_4 = "Great! Now let's conquer this planet. Send some ships keeping pressed the A button, the right trigger, the left mouse button or the return key.";
        #endif

        public static String TUTORIAL_TEXT_5 = "Very well! Now we have a planet conquered. The people of the planet will build also new ships for your empire.";
        public static String TUTORIAL_TEXT_6 = "Now it's time for war. Place your cursor over the red big planet.";
        
        #if XBOX
        public static String TUTORIAL_TEXT_7 = "Let's strike! Keep pressed the A button or the right trigger to send ships to this enemy planet. Attack the planet until the planet turns green.";
        #else
        public static String TUTORIAL_TEXT_7 = "Let's strike! Keep pressed the A button, the right trigger, the left mouse button, or the return key to send ships to this enemy planet. Attack the planet until the planet turns green.";        
        #endif
        
        public static String TUTORIAL_TEXT_8 = "Congratulations! This planet is now under your control!";
        public static String TUTORIAL_TEXT_9 = "We have enough power to attack the enemy motherbase. Place your cursor there.";
        #if XBOX        
        public static String TUTORIAL_TEXT_10 = "This time we will strike with all our forces. Keep pressed the B button or the left trigger to send ships from the motherbase and all planets to the targeted destination.";
        #else
        public static String TUTORIAL_TEXT_10 = "This time we will strike with all our forces. Keep pressed the B button, the left trigger, the right mouse button, or the control key to send ships from the motherbase and all planets to the targeted destination.";        
        #endif
        
        public static String TUTORIAL_TEXT_11 = "You have conquered the enemy mothership! But it's not going to be so easy, the motherships have shields to avoid the invasions.";
        public static String TUTORIAL_TEXT_12 = "Each shield contains 50 fighters. When one of them is destroyed, the mothership gains 50 fighters. After all the shields are destroyed the mothership can be conquered permanently.";
        public static String TUTORIAL_TEXT_13 = "Attack the enemy motherbase until it is without shields and under your control. But before that you can conquer other planets to be more powerfull if you wish to. Good luck!";
        public static String TUTORIAL_TEXT_14 = "Congratulations! The universe is yours. You have successfully completed the tutorial! Wait until the victory screen is shown.";
      
    }
}
