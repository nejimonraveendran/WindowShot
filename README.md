# WindowShot
This is a .NET project that does timelapse Windows Desktop screen recording.  This will be useful for those who want to record hours of screen activity (eg. artists doing art works). This can be easily adapted to capture the screen in real time (look through the code).   

The solution mainly uses PrintWindow  Windows API function to capture the screenshots.  AForge.NET library is used to write the screenshots into a video file. 

Since the solution uses PrintWindow API function, whenever there is an overlay window (eg. tooltips etc), it fails and results in a black window being captured.  In order not to include in the final video stream, I use as somewhat crude way of finding out if the captured image contains a black pixel at an XY location where it is unlikely to have a black pixel.
