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

    print(indexes)

    for name in columns:
        fig = plt.figure()

        data = num_data[name].tolist()

        data_to_use = []
        for index in indexes :
            data_to_use.append(data[index])

        data_to_use.sort()

        std = np.std(data_to_use) 
        print("{0}: {1}".format(name, std))
        mean = np.mean(data_to_use)

        outlierBorder = 0

        prevVal = 0
        for x in data:
            outlierBorder = x
            distance = x - prevVal
            #if we reach a point with distance greater than 10% above standard deviation we reached the outlier threshold 
            if(x > 7 * std):
                outlierBorder = prevVal
                break
            prevVal = x

        sub = fig.add_subplot(211)
        normD = norm.pdf(data_to_use,mean,std)
        plt.plot(data_to_use, normD, '-o')
        plt.hist(data_to_use,normed=True)
 #       plt.axvline(outlierBorder, color="red", linestyle="dashed", marker="8")
 #       plt.text(outlierBorder, sub.get_ylim()[1] * .8, "outliers > {}".format(round(outlierBorder,2)))
        plt.title(name)
        
        sub = fig.add_subplot(212)
        plt.boxplot(data_to_use, vert=False)
 #       plt.axvline(outlierBorder, color="red", linestyle="dashed", marker="8")
 #       plt.text(outlierBorder, sub.get_ylim()[1] * .8, "outliers > {}".format(round(outlierBorder,2)))
 #       plt.text(outlierBorder, -0.1, "test")    

    plt.show()

def plot_method_data():
    columns = ["LOC", "CYCLO", "ATFD", "FDP", "LAA", "MAXNESTING", "NOAV"]
    filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\CodeSniffer.BBN\\TrainingsData\\MethodTrainingSet_2319_17022018.csv"
    classifier = 13 #long method
    plot_data(filename, columns, classifier)

def plot_method_data_featureenvy():
    columns = ["LOC", "CYCLO", "ATFD", "FDP", "LAA", "MAXNESTING", "NOAV"]
    filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\CodeSniffer.BBN\\TrainingsData\\MethodTrainingSet_2319_17022018.csv"
    classifier = 11 #featureenvy
    plot_data(filename, columns, classifier)


def plot_class_data():
    columns = ["LOC", "TCC", "WMC", "ATFD"]
    filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\CodeSniffer.BBN\\TrainingsData\\ClassTrainingSet_2319_17022018.csv"
    classifier = 6
    plot_data(filename, columns, classifier)

def plot_verification_method_data():
    columns = ["LOC", "CYCLO", "ATFD", "FDP", "LAA", "MAXNESTING", "NOAV"]
    filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\CodeSniffer.BBN\\VerificationData\\MethodTrainingSet_1357_18022018.csv"
    classifier = 13 #long method
    plot_data(filename, columns, classifier)

def plot_verification_method_data_featureenvy():
    columns = ["LOC", "CYCLO", "ATFD", "FDP", "LAA", "MAXNESTING", "NOAV"]
    filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\CodeSniffer.BBN\\VerificationData\\MethodTrainingSet_1357_18022018.csv"
    classifier = 11 #long method
    plot_data(filename, columns, classifier)

def plot_verification_class_data():
    columns = ["LOC", "TCC", "WMC", "ATFD"]
    filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\CodeSniffer.BBN\\VerificationData\\ClassTrainingSet_1357_18022018.csv"
    classifier = 6
    plot_data(filename, columns, classifier)


#plot_class_data()
#plot_method_data()
#plot_method_data_featureenvy()

plot_verification_class_data()
plot_verification_method_data()
plot_verification_method_data_featureenvy()
 

