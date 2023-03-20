# Vertices Engine Tempaltes

<a href="https://twitter.com/intent/follow?screen_name=virtexedge">
        <img src="https://img.shields.io/twitter/follow/virtexedge.svg?style=social&logo=twitter" alt="follow on Twitter"></a>

The Vertices Engine is the in-house developed game engine from @VirtexEdgeDesign & @rtroe based in and built from the ground up in C#. It uses @MonoGame as it's back end allowing it to run cross platform on PC, OSX, Linux, iOS and Android.

There are recently released Nuget packages which can be found through the badge links below so that users can play around with Vertices on their own machines.
           

# Status

| Platform | Build Status                   | Nuget Package|
|----------|--------------------------------|--|
| DesktopGL| ![alt text][buildOGL] |[![nugetOGL](https://img.shields.io/badge/nuget-released-green.svg)](https://www.nuget.org/packages/Virtex.Lib.Vrtc.GL/)|
| Android  | ![alt text][buildAdr] |[![nugetOGL](https://img.shields.io/badge/nuget-beta-blue.svg)](https://www.nuget.org/packages/Virtex.Lib.Vrtc.Android/)|
| iOS      | ![alt text][buildIOS] |[![nugetOGL](https://img.shields.io/badge/nuget-tbd-orange.svg)](#)|
| DirectX  | ![alt text][buildDrX] |[![nugetOGL](https://img.shields.io/badge/nuget-tbd-orange.svg)](#)|


[buildxna]: https://img.shields.io/badge/build-depreciated-lightgray.svg
[buildDrX]: https://img.shields.io/badge/build-tbd-orange.svg
[buildOGL]: https://img.shields.io/badge/build-passing-green.svg
[buildAdr]: https://img.shields.io/badge/build-passing-green.svg
[buildIOS]: https://img.shields.io/badge/build-passing-green.svg

[nugetSuccess]: https://img.shields.io/badge/nuget-released-green.svg
[nugetbeta]: https://img.shields.io/badge/nuget-beta-blue.svg
[nugetTBD]: https://img.shields.io/badge/nuget-comingsoon-orange.svg
[nugetNA]: https://img.shields.io/badge/nuget-deprecetated-lightgray.svg

# Games
A number of currently released and up coming games use vertices, you can find an outline below:

## The Chaotic Workshop
[![cws](https://rtroe.github.io/img/headers/header_crtn.png)](http://games.virtexedge.com/TheChaoticWorkshop/)

The Chaotic Workshop is a mobile Rube Goldberg style puzzle game involving you to use your creativity and problem solving along with the toolbox of over 70 items to solve 50 backwards, mind bending puzzles in the game.

The Chaotic Workshop is available on <a href="http://games.virtexedge.com/TheChaoticWorkshop/">Android and iOS here</a>.

## Space Esc8bit
[![cws](https://rtroe.github.io/img/headers/header_spcescbt.png)](http://games.virtexedge.com/SpaceEsc8bit/)

Space Esc8bit is a fast paced, 8-bit themed mobile missile run game, with the goal to both get as high an altitude as possible by out-maneuvering as many missiles as possible.

It's out on <a href="http://games.virtexedge.com/SpaceEsc8bit/">Android and iOS now</a>.

## Metric Racer
[![cws](https://rtroe.github.io/img/headers/header_metric.png)](http://games.virtexedge.com/MetricRacer/)

Metric Racer is a fast paced futuristic racer. Multiple tracks, local multiplayer, and a built-in track editor, your speed and creativity are your only limits.

Metric will be launching late 2018 on PC, OSX, Linux, as well as a mobile port on iOS and Android in 2019.

# Features

## Cross Platform
Vertices runs on PC, OSX, Linux, iOS and Android and is coming to consoles soon.

<img src="https://i.imgur.com/cvRHBXV.png" width="500" height="100" />

## In-game Sandbox for Rapid Level Design
A number of Vertices games are sandbox based, and so the engine is built an integrated in-game editor to add, modify and change a game on the fly.

<img src="https://i.imgur.com/wnCtNGX.png" width="500" height="300" />

## Integrated Terrain Editor
<img src="https://i.imgur.com/sPTtg9W.png" width="500" height="300" />

## Customizable and Sinkable GUI system
Vertices supports a number of GUI elements from basic buttons and toolbars to more advanced Ribbon Bars and Property Grib controls.

<img src="https://virtexedgedesign.github.io/VerticesEngine/imgs/features/sandbox.png" width="500" height="300" />

## Integrated Debug System
To help with game development and profiling, Vertices comes with a number of debug profilers and tools.

<img src="https://i.imgur.com/VYCcuGF.png" width="500" height="300" />

## Farseer and BEPU physics library support
Vertices comes with physics support for BEPU and Farseer.

## Networking
Vertices uses Lidgren to handle it's netcode back end, and is integrated into the engine, allowing for fast prototyping of networked games.

# Rendering Pipeline
Vertices comes with a extensible and plug-and-play renderering pipeline. There is a more indepth look at the renderering pipeline over at Virtex's main site here.

## Deferred Renderer

<img src="https://virtexedgedesign.github.io/VerticesEngine/imgs/renderpipeline/deferred.png" width="500" height="300" />

## Cascade Shadow Mapping

<img src="https://virtexedgedesign.github.io/VerticesEngine/imgs/renderpipeline/shados.png" width="500" height="300" />

## Crepuscular Rays(God Rays)

<img src="https://virtexedgedesign.github.io/VerticesEngine/imgs/renderpipeline/godrays.png" width="500" height="300" />

## Screen Space Reflections

<img src="https://virtexedgedesign.github.io/VerticesEngine/imgs/renderpipeline/ssr.png" width="500" height="300" />

## SSAO

<img src="https://virtexedgedesign.github.io/VerticesEngine/imgs/renderpipeline/ssao.png" width="500" height="300" />

## Motion Blur

<img src="https://i.imgur.com/z4PtH79.png" width="500" height="300" />

## Depth of Field

<img src="https://virtexedgedesign.github.io/VerticesEngine/imgs/renderpipeline/depthoffield.png" width="500" height="300" />

## Emissive Materials

<img src="https://i.imgur.com/vJT6gnC.png" width="500" height="300" />

## Renderering Debug 
The renderering pipeline comes with a number of debug options as well, allowing to see what the scene looks like at each stage of the pre and post processing.

<img src="https://i.imgur.com/8YTpuvY.png" width="500" height="300" />

# Support

Verties is in active development currently and therefore may have some bugs. You can open issues here or get in touch with us on Twitter through the links at the top.
