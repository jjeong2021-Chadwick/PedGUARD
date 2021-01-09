# PedGUARD
Pedestrian's Guiding Utility

PedGUARD a convolutional neural network based mobile application that detects
dangers on the streets and alerts the user through vibration is an extension of my
work on the LaneSensing Detector The system is designed to run in a realtime
environment and is trained to detect a number of objects ie manholes
construction sites the starting point of crosswalk etc that pose possible danger
to pedestrians. 

I used Unity to develop PedGUARD into a mobile app by utilizing the trained weights of
my neural network model and developing user interface running in 
mobile devices. Smartphone screen is thus split into 25 frames
and fed to SimpleNet which returns a list of 25 values with each value denoting whether
danger is detected in a particular frame. User can set a hit ratio using a slider in the app
and alerts the user if percentage of 1s in the list exceeds the hit ratio value.
