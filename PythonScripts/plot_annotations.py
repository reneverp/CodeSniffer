import sys
import os
import matplotlib.pyplot as plt
from matplotlib.widgets import Slider, Button, RadioButtons
import numpy as np

from scipy.stats import norm
from numpy import genfromtxt

from collections import defaultdict

import csv

def readcsv(filename):	
    ifile = open(filename, "rU")
    reader = csv.reader(ifile, delimiter=",")

    rownum = 0	
    a = []

    for row in reader:
        if(rownum > 0):
            a.append (row)
        rownum += 1
    
    ifile.close()

    return a

def plot_data(filename, columns, classifier, bins):
    my_data = readcsv(filename)
    num_data = genfromtxt(filename, delimiter=',', comments='(', skip_header=False, names=True)

    indexes = []

    for idx, row in enumerate(my_data):
        if row[classifier[1]] == "True":
            indexes.append(idx)

    for name in columns:
        fig = plt.figure()

        data = num_data[name].tolist()

        data_to_use = []
        for index in indexes :
            data_to_use.append(data[index])

        data_to_use.sort()

        sub1 = fig.add_subplot(311)
        plt.hist(data, color="orange")
        plt.hist(data_to_use)
        plt.title("{} - {}".format(classifier[0], name))
        
        sub2 = fig.add_subplot(312)
        plt.hist(data_to_use)       

        sub3 = fig.add_subplot(313)
        plt.boxplot(data_to_use, vert=False)
          
        binsize = bins[name][-1] - bins[name][-2]
        upperlimit = bins[name][-1] + binsize
        sub1.set_xlim(0, upperlimit)
        sub2.set_xlim(0, upperlimit)
        sub3.set_xlim(0, upperlimit)
 # 
        for boundary in bins[name]:
            sub1.axvline(boundary, color="red", linestyle="dashed")
            sub1.text(boundary + (sub1.get_xlim()[1] * 0.005), sub1.get_ylim()[1] * 0.85, boundary, style="italic", fontsize=8)
            sub2.axvline(boundary, color="red", linestyle="dashed") 


        
        os.makedirs(os.path.dirname(os.path.realpath(__file__)) + "\\annotations\\{}\\{}_{}".format(outDir, os.path.basename(filename)[:-4], classifier[0]), exist_ok=True)
        plt.savefig(os.path.dirname(os.path.realpath(__file__)) + "\\annotations\\{}\\{}_{}\\{}.png".format(outDir, os.path.basename(filename)[:-4], classifier[0], name))
        plt.close(fig)

def plot_method_data(file):
    columns = ["LOC", "CYCLO", "MAXNESTING", "NOAV"]
    filename = os.path.dirname(os.path.realpath(__file__)) + file
    classifier = ["Long Method", 13] #long method
    bins = defaultdict()
    bins["LOC"        ] = [25, 50, 75, 100, 125, 150, 175 ]
    bins["CYCLO"      ] = [11.25, 22.5, 33.75, 45, 56.25, 67.5, 78.75] 
    bins["MAXNESTING" ] = [1.375, 2.75, 4.125, 5.5, 6.875, 8.25, 9.625]
    bins["NOAV"       ] = [7.875, 15.75, 23.625, 31.5, 39.375, 47.25, 55.125]
    

    plot_data(filename, columns, classifier, bins)

def plot_method_data_featureenvy(file):
    columns = ["ATFD", "FDP", "LAA"]
    bins = defaultdict()
    bins["ATFD"] = [8.25 , 16.5 , 24.75 , 33.0, 41.25 , 49.5 , 57.75 ]
    bins["FDP" ] = [1.875, 3.75 , 5.625 , 7.5 , 9.375 , 11.25, 13.125] 
    bins["LAA" ] = [6.125, 12.25, 18.375, 24.5, 30.625, 36.75, 42.875]

    filename = os.path.dirname(os.path.realpath(__file__)) + file
    classifier = ["Feature Envy", 13] #featureenvy
    plot_data(filename, columns, classifier, bins)


def plot_class_data(file):
    columns = ["LOC", "TCC", "WMC", "ATFD"]
    filename = os.path.dirname(os.path.realpath(__file__)) + file
    classifier = ["Large Class", 6]
    bins = defaultdict()
    bins["LOC" ] = [151.125, 302.25, 453.375, 604.5, 755.625, 906.75, 1057.875]
    bins["TCC" ] = [0.05   , 0.1   , 0.15   , 0.2  , 0.25   , 0.3   , 0.35    ] 
    bins["WMC" ] = [2.25   , 4.5   , 6.75   , 9    , 11.25  , 13.5  , 15.75   ]
    bins["ATFD"] = [10.875 , 21.75 , 32.625 , 43.5 , 54.375 , 65.25 , 76.125  ]

    plot_data(filename, columns, classifier, bins)

outDir = "NoUser"
if(len(sys.argv) == 2):
    outDir = sys.argv[1]


#os.makedirs(outDir, exist_ok=True)

plot_class_data("\\..\\CodeSniffer.BBN\\TrainingsData\\ClassTrainingSet_2319_17022018.csv")
plot_method_data("\\..\\CodeSniffer.BBN\\TrainingsData\\MethodTrainingSet_2319_17022018.csv")
plot_method_data_featureenvy("\\..\\CodeSniffer.BBN\\TrainingsData\\MethodTrainingSet_2319_17022018.csv")

plot_class_data("\\..\\CodeSniffer.BBN\\VerificationData\\ClassTrainingSet_1357_18022018.csv")
plot_method_data("\\..\\CodeSniffer.BBN\\VerificationData\\MethodTrainingSet_1357_18022018.csv")
plot_method_data_featureenvy("\\..\\CodeSniffer.BBN\\VerificationData\\MethodTrainingSet_1357_18022018.csv")

plot_class_data("\\..\\Bin\\Release\\Classtest.csv")
plot_method_data("\\..\\Bin\\Release\\Methodtest.csv")
plot_method_data_featureenvy("\\..\\Bin\\Release\\Methodtest.csv")
 

