AccentColors
============

A small WPF program to display and search the available accent colors for the active color scheme on Windows 8.

# Introduction

Windows 8 lets users choose a color scheme to use throughout the system. Each color scheme consists of a huge set of named colors, like *BootBackground* or *StartHighlight*. Officially there is no way to retrieve these colors but by looking at *uxtheme.dll* using IDA and Microsoft's symbol server a bunch of undocumented functions to accomplish exactly that can be found. This project utilizes these functions to display and search all available colors from the active color set.

Developers interested in adjusting the coloring of their own application to the color scheme chosen by the user can look at the provided code to learn how to use the undocumented functions, or just use the [AccentColor class](https://github.com/maxtruxa/AccentColors/blob/master/AccentColors/AccentColor.cs).

# Screenshots

[![AccentColor MainWindow](http://farm6.staticflickr.com/5502/11523053123_089faf77d7.jpg)](http://www.flickr.com/photos/112339154@N04/11523053123/)
