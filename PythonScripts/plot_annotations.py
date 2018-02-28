import sys
import os
import matplotlib.pyplot as plt
from matplotlib.widgets import Slider, Button, RadioButtons
import numpy as np

from scipy.stats import norm
from numpy import genfromtxt

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

def plot_data(filename, columns, classifier):
    my_data = readcsv(filename)
    num_data = genfromtxt(filename, delimiter=',', comments='(', skip_header=False, names=True)

    indexes = []

    for idx, row in enumerate(my_data):
        if row[classifier] == "True":
            indexes.append(idx)

    for name in columns:
        fig = plt.figure()

        data = num_data[name].tolist()

        data_to_use = []
        for index in indexes :
            data_to_use.append(data[index])

        data_to_use.sort()

        sub1 = fig.add_subplot(311)
        #normD = norm.pdf(data_to_use,mean,std)
        #plt.plot(data_to_use, normD, '-o')
        hist = plt.hist(data, color="orange")
 #       plt.axvline(outlierBorder, color="red", linestyle="dashed", marker="8")
 #       plt.text(outlierBorder, sub.get_ylim()[1] * .8, "outliers > {}".format(round(outlierBorder,2)))
        plt.title(name)
        
        sub2 = fig.add_subplot(312)
        plt.hist(data_to_use)
        sub2.set_xlim(sub1.get_xlim())
        

        sub3 = fig.add_subplot(313)
        plt.boxplot(data_to_use, vert=False)
        sub3.set_xlim(sub1.get_xlim())
 #       plt.axvline(outlierBorder, color="red", linestyle="dashed", marker="8")
 #       plt.text(outlierBorder, sub.get_ylim()[1] * .8, "outliers > {}".format(round(outlierBorder,2)))
 #       plt.text(outlierBorder, -0.1, "test")    

        os.makedirs(os.path.dirname(os.path.realpath(__file__)) + "\\annotations\\{}\\{}_{}".format(outDir, os.path.basename(filename)[:-4], classifier), exist_ok=True)
        plt.savefig(os.path.dirname(os.path.realpath(__file__)) + "\\annotations\\{}\\{}_{}\\{}.png".format(outDir, os.path.basename(filename)[:-4], classifier, name))
        plt.close(fig)

def plot_method_data(file):
    columns = ["LOC", "CYCLO", "MAXNESTING", "NOAV"]
    filename = os.path.dirname(os.path.realpath(__file__)) + file
    classifier = 13 #long method
    plot_data(filename, columns, classifier)

def plot_method_data_featureenvy(file):
    columns = ["ATFD", "FDP", "LAA"]
    filename = os.path.dirname(os.path.realpath(__file__)) + file
    classifier = 11 #featureenvy
    plot_data(filename, columns, classifier)


def plot_class_data(file):
    columns = ["LOC", "TCC", "WMC", "ATFD"]
    filename = os.path.dirname(os.path.realpath(__file__)) + file
    classifier = 6
    plot_data(filename, columns, classifier)

outDir = "NoUser"
if(len(sys.argv) == 2):
    outDir = sys.argv[1]
    os.makedirs(sys.argv[1], exist_ok=True)

plot_class_data("\\..\\CodeSniffer.BBN\\TrainingsData\\ClassTrainingSet_2319_17022018.csv")
plot_method_data("\\..\\CodeSniffer.BBN\\TrainingsData\\MethodTrainingSet_2319_17022018.csv")
plot_method_data_featureenvy("\\..\\CodeSniffer.BBN\\TrainingsData\\MethodTrainingSet_2319_17022018.csv")

plot_class_data("\\..\\CodeSniffer.BBN\\VerificationData\\ClassTrainingSet_1357_18022018.csv")
plot_method_data("\\..\\CodeSniffer.BBN\\VerificationData\\MethodTrainingSet_1357_18022018.csv")
plot_method_data_featureenvy("\\..\\CodeSniffer.BBN\\VerificationData\\MethodTrainingSet_1357_18022018.csv")

plot_class_data("\\..\\Bin\\Release\\Classtest.csv")
plot_method_data("\\..\\Bin\\Release\\Methodtest.csv")
plot_method_data_featureenvy("\\..\\Bin\\Release\\Methodtest.csv")
 

