# mt (Modding Terraria codename; 1.0-beta-uwp branch)

My attempt(s) to "reconsruct" some smallest and simplest game mechanics aka Terraria project :)

## Tech. details

:: Terraria 1.0 experimental UWP edition with special "modded" MonoGame ::

:: Min. Win. os build (SDK): 10240 (Astoria compatibility added)

## Screenshot(s)
![](Images/shot1.png)
![](Images/shot2.png)
![](Images/shot3.png)
![](Images/shot4.png)

## Progress of porting to uwp

:: Sound effects fixed :)

:: DB storage ok

:: Some debug mode added (but remerked/commented partially... only "god mode for life autosaving" on at now)))

:: Some modded content added ... but all old terreria 1.0 theme remained, of couse! 

:: DB "auto-deploy" fixed. So, terraria.bd file will automatically copied to AppData\Local\Packages\ME.ModdingTerrariaV1_...\LocalState  at first app start :)

:: The app do its work only at "start mode" (minimum WASD + UpDownLeftRight keyboard control, not touchscreen event yet, sadly...) 

:: No sound (wavebank WMA support not recovered, sadly), but save / restore settings (player/genworld data storage) fixed :)

### Content
This repo contains the decompiled source of the Terraria client binaries, from version 1.0-beta founded at [Terraria-pc-version-archive site](https://archive.org/details/terraria-pc-version-archive). Decompiled with [JetBrains dotPeek](https://www.jetbrains.com/decompiler/). 

### Important
- If you want to fix music themes, find cool utils for modding music content, and use *Src\Contents\Wave Bank.xbw* (unpack it, explore Xbox row RIFF format, recode all music files, pack them...). 
- Use this at your own risk: I am not responsible for any legal consequences that may occur by using this decompiled code, nor will I provide support for it.
I am just providing the decompiled code as a reference and making it easily accessible on GitHub.

## Todo
- Explore all code structures : :
- Try to patch decomp. bugs : :
- Research game mechanics : :
- Add your own game mech
- Fix music : :
- Add touchscreen event handlers : :
- Do something else (deploy to old swwet Lumia phones... why not?) : :

## Add. info / References
- https://onlineblogzone.com/modding-terraria-part-1-getting-started/ Modding Terraria – Part 1 Getting Started (article)
- https://github.com/TheVamp/Terraria-Source-Code  Version 1.2.0.3.1 (decomp/raw)
- https://github.com/UTINKA/Terraria Terraria Version 1.3.5.3 (decomp/raw)
- https://github.com/MikeyIsBaeYT/Terraria-Source-Code Version ??? (decomp/raw)
- https://habr.com/ru/articles/142349/ Terraria: или пишите игры правильно (in Rus.)
- https://habr.com/ru/articles/122839/ Игра «Terraria» и её «хорошая» система шифрования профайлов (in Rus.)

## ..
As is. No support. Educational use/purpose only. DIY

## .
[m][e] 2023
