BowlingGame
===========

Kata from http://butunclebob.com/ArticleS.UncleBob.TheBowlingGameKata

Written in C# using Visual Studio 2012

Game class has two main methods: void roll(int pins), and int getScore().

This code is an exercise in executing some tricky logic and in unit testing.  The BowlingGame test class has several unit tests which probe the logic of the roll() and getScore() methods.

The original goals as seen in the link above:
1. Count strike bonuses correctly.  This means strikes count the next two rolls as a bonus.
2. Count spares correctly.  Every spare counts the next roll as a bonus.

Goals I added:
3. Check for valid rolls. Non-negative number of pins.  No more than ten pins in a frame, except last frame.  Last frame allows up to 30 pins with strikes.  Throw exceptions and test for exceptions.
4. Don't count a strike or a spare in the last frame with a bonus.
