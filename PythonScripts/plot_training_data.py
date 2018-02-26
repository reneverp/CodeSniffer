import os
import matplotlib.pyplot as plt
from matplotlib.widgets import Slider, Button, RadioButtons
import numpy as np

from scipy.stats import norm
from numpy import genfromtxt

def plot_data(filename, columns):
    my_data = genfromtxt(filename, delimiter=',', comments='(', skip_header=False, names=True)

    for name in columns:
        fig = plt.figure()

        data = my_data[name].tolist()

        data.sort()

        std = np.std(data) 
        print("{0}: {1}".format(name, std))
        mean = np.mean(data)

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

        print("outlierBorder: {}".format(outlierBorder))

        sub = fig.add_subplot(211)
        normD = norm.pdf(data,mean,std)
        plt.plot(data, normD, '-o')
        plt.hist(data,normed=True)
        plt.axvline(outlierBorder, color="red", linestyle="dashed", marker="8")
        plt.text(outlierBorder, sub.get_ylim()[1] * .8, "outliers > {}".format(round(outlierBorder,2)))
        plt.title(name)
        
        sub = fig.add_subplot(212)
        plt.boxplot(data, vert=False)
        plt.axvline(outlierBorder, color="red", linestyle="dashed", marker="8")
        plt.text(outlierBorder, sub.get_ylim()[1] * .8, "outliers > {}".format(round(outlierBorder,2)))
        plt.text(outlierBorder, -0.1, "test")    

        os.makedirs("trainingDataResults", exist_ok=True)

        plt.savefig("trainingDataResults\\{}_{}.png".format(name,os.path.basename(filename)))
        plt.close(fig)

def plot_method_data():
    columns = ["LOC", "CYCLO", "ATFD", "FDP", "LAA", "MAXNESTING", "NOAV"]
    filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\CodeSniffer.BBN\\TrainingsData\\MethodTrainingSet_2319_17022018.csv"
    plot_data(filename, columns)


def plot_class_data():
    columns = ["LOC", "TCC", "WMC", "ATFD"]
    filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\CodeSniffer.BBN\\TrainingsData\\ClassTrainingSet_2319_17022018.csv"
    plot_data(filename, columns)

def plot_verification_method_data():
    columns = ["LOC", "CYCLO", "ATFD", "FDP", "LAA", "MAXNESTING", "NOAV"]
    filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\CodeSniffer.BBN\\VerificationData\\MethodTrainingSet_1357_18022018.csv"
    plot_data(filename, columns)

def plot_verification_class_data():
    columns = ["LOC", "TCC", "WMC", "ATFD"]
    filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\CodeSniffer.BBN\\VerificationData\\ClassTrainingSet_1357_18022018.csv"
    plot_data(filename, columns)


plot_class_data()
plot_method_data()

plot_verification_class_data()
plot_verification_method_data()
 

