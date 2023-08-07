# BackgroundFix
This repo is dedicated to fixing an annoying issue with Windows 10 desktop background image scaling when a display is connected or disconnected.
======================================================================
===>WHAT IS THIS PROGRAM DOING:
When this program is running, it monitors the number of connected displays (using the WinProc callback function - each time a message is sent to the window, the program checks 
how many displays are detected). When a change is detected, the wallpaper will be changed accordingly. Currently, the maximum count of displays is 2 (since that is what I need 
for my personal usage).

======================================================================
===>HOW TO USE:
!!!Before running, make sure you place the *.exe in its own separate folder because another subdirectory will be created when it's executed 
(If this condition is not met, the program should run with no problems. It may be more of an inconvenience for the user having new subdirectory created wherever executable is located)!!!
1. After running the *.exe for the first time, you will be greeted by the setup window. There are two text boxes labeled "Single display wallpaper" and "Dual display wallpaper" 
with buttons labeled "Explore." Use these buttons to choose the desired wallpaper for each mode.
2. Then, please press the "SAVE" button. Without this step, changes to wallpapers won't be recorded. !!!If a problem with the image arises, the wallpaper will be set to black!!!
By pressing the "SAVE" button, the program will copy the chosen files to its own "res" folder.
2,5. At this point, you can test the correct functionality by pressing the "TEST/INFO" button or simply by turning off one of your displays. If it's not working, then I am sorry, but 
something must have gone wrong :(
3. Check "RUN AT STARTUP" if you want the program to run each time you log in. Once set up, the setup dialog won't appear again.
4. Press the "HIDE" button to hide the setup window. By doing this, the program will run in the background.

===>HOW TO CHANGE SETTINGS (WALLPAPER IMAGES) AGAIN:
If you'd like to make changes, simply rerun the *.exe, and the setup window will appear again.
Alternatively, you can just change images in the "res" folder, but naming must be adhered ("Singlescreen.* | Dualscreen.*").

===>HOW TO STOP THE PROGRAM:
Make the setup window reappear by running the executable again. If you don't want the program to run at startup, uncheck "RUN AT STARTUP." Simply close the window 
by pressing the "X", like any other window in Windows. A popup dialog will show...select "YES." An additional popup dialog will show, asking if you like to revert changes...select as desired.

=====================================================================
===>STORY / WHY I MADE THIS PROGRAM:
> I am running a dual-screen setup but only use both displays when I really need to (to save power). So, the majority of the time, my second display is turned off.
> upgraded my GPU from GeForce GT640 to Radeon RX 6600.
> discovered that the new GPU is smart. When the display is turned off, it won't output at that interface.
> thought to myself: "Wow, that's cool. It's not wasting power rendering something I don't need/see."
> realized that when this happens, Windows doesn't see the display either. So it scales and centers the wallpaper to the middle of the working display.
> felt sad and angry.
> made this program to calm my frustration.
> If there is another simpler solution, I'd rather not know about it, since I spent 4 hours making my own solution.
> If you are having a similar problem, hopefully, my project will help you.

=====================================================================
===>SPECIAL THANKS TO / USED CODE:
Nullfx (Steve) for his article on Single Instance Application:
	http://sanity-free.org/143/csharp_dotnet_single_instance_application.html

Drarig29 (Corentin Girard) for great SilentWallpaper.cs 
	https://gist.github.com/Drarig29/4aa001074826f7da69b5bb73a83ccd39

