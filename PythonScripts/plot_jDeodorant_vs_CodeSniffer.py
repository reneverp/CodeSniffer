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

def plot_class_data(filename):
    my_data = readcsv(filename)

    countAnnotated = 0
    countJdTp = 0
    countJd = 0
    
    countCsTp = 0
    countCs = 0
    
    for idx, row in enumerate(my_data):
        if row[9] == "True":
            countAnnotated += 1 #counts annotated
        if row[8] == "True":
            countJd += 1 #counts jd
        if row[8] == "True" and row[9] == "True":
            countJdTp += 1 #counts jd
        if row[6] == "True" and row[9] == "True":
            countCsTp += 1 #counts ccs
        if row[6] == "True":
            countCs += 1 #counts ccs

    fig = plt.figure()

    plt.bar("Annotated", countAnnotated, align='center', label="Annotated Large Classes")
    plt.bar("JDeodorant", countJd, align='center', label="JD all")
    plt.bar("JDeodorant", countJdTp, align='center', label="JD true positives")
    plt.bar("CodeSniffer", countCs, align='center', label="CS all")
    plt.bar("CodeSniffer", countCsTp, align='center', label="CS true positives")
    
    plt.bar("CodeSniffer", countCs, align='center')
    plt.title("JDeodorant vs CodeSniffer - Large Class")
    plt.legend()

    os.makedirs(os.path.dirname(os.path.realpath(__file__)) + "\\barcharts", exist_ok=True)  
    plt.savefig("barcharts\LargeClass_cs_vs_jd.png")

def plot_longmethod_data(filename):
    my_data = readcsv(filename)

    countAnnotated = 0
    countJdTp = 0
    countJd = 0
    
    countCsTp = 0
    countCs = 0
    
    for idx, row in enumerate(my_data):
        if row[16] == "True":
            countAnnotated += 1 #counts annotated
        if row[18] == "True":
            countJd += 1 #counts jd
        if row[18] == "True" and row[16] == "True":
            countJdTp += 1 #counts jd
        if row[13] == "True" and row[16] == "True":
            countCsTp += 1 #counts ccs
        if row[13] == "True":
            countCs += 1 #counts ccs

    print(countCsTp)

    fig = plt.figure()

    plt.bar("Annotated", countAnnotated, align='center', label="Annotated Long Methods")
    plt.bar("JDeodorant", countJd, align='center', label="JD all")
    plt.bar("JDeodorant", countJdTp, align='center', label="JD true positives")
    plt.bar("CodeSniffer", countCs, align='center', label="CS all")
    plt.bar("CodeSniffer", countCsTp, align='center', label="CS true positives")
    
    plt.bar("CodeSniffer", countCs, align='center')
    plt.title("JDeodorant vs CodeSniffer - Long Method")
    plt.legend()

    os.makedirs(os.path.dirname(os.path.realpath(__file__)) + "\\barcharts", exist_ok=True)  
    plt.savefig("barcharts\LongMethod_cs_vs_jd.png")

def plot_featureenvy_data(filename):
    my_data = readcsv(filename)

    countAnnotated = 0
    countJdTp = 0
    countJd = 0
    
    countCsTp = 0
    countCs = 0
    
    for idx, row in enumerate(my_data):
        if row[15] == "True":
            countAnnotated += 1 #counts annotated
        if row[17] == "True":
            countJd += 1 #counts jd
        if row[17] == "True" and row[15] == "True":
            countJdTp += 1 #counts jd
        if row[11] == "True" and row[15] == "True":
            countCsTp += 1 #counts ccs
        if row[11] == "True":
            countCs += 1 #counts ccs

    print(countCsTp)

    fig = plt.figure()

    plt.bar("Annotated", countAnnotated, align='center', label="Annotated Feature Envy Methods")
    plt.bar("JDeodorant", countJd, align='center', label="JD all")
    plt.bar("JDeodorant", countJdTp, align='center', label="JD true positives")
    plt.bar("CodeSniffer", countCs, align='center', label="CS all")
    plt.bar("CodeSniffer", countCsTp, align='center', label="CS true positives")
    
    plt.bar("CodeSniffer", countCs, align='center')
    plt.title("JDeodorant vs CodeSniffer - Feature Envy")
    plt.legend()

    os.makedirs(os.path.dirname(os.path.realpath(__file__)) + "\\barcharts", exist_ok=True)  
    plt.savefig("barcharts\FeatureEnvy_cs_vs_jd.png")

# def plot_longmethod_data(filename):
#     my_data = readcsv(filename)

#     countAnnotated = 0
#     countJd = 0
#     countCs = 0

#     for idx, row in enumerate(my_data):
#         if row[9] == "True":
#             countAnnotated += 1 #counts annotated
#         if row[8] == "True" and row[9] == "True":
#             countJd += 1 #counts jd
#         if row[6] == "True" and row[9] == "True":
#             countCs += 1 #counts ccs

#     fig = plt.figure()

#     plt.bar("Annotated", countAnnotated, align='center')
#     plt.bar("JDeodorant", countJd, align='center')
#     plt.bar("JDeodorant", countJdTp, align='center')
#     plt.bar("CodeSniffer", countCs, align='center')
#     plt.bar("CodeSniffer", countCsTp, align='center')
#     plt.title("TP - Large Class")

#     os.makedirs(os.path.dirname(os.path.realpath(__file__)) + "\\barcharts", True)  
#     plt.savefig("barcharts\LongMethod_cs_vs_jd.png")



#os.makedirs(outDir, exist_ok=True)

plot_class_data(r"D:\Dropbox\Master Software Engineering\14 Afstuderen\data\04-03-2018\RQ1\Classrun_0.csv")
plot_longmethod_data(r"D:\Dropbox\Master Software Engineering\14 Afstuderen\data\04-03-2018\RQ1\Methodrun_0.csv")
plot_featureenvy_data(r"D:\Dropbox\Master Software Engineering\14 Afstuderen\data\04-03-2018\RQ1\Methodrun_0.csv")
 

