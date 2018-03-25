import sys
import os
import matplotlib.pyplot as plt
from matplotlib.widgets import Slider, Button, RadioButtons
import numpy as np
import ntpath

from scipy.stats import norm
from numpy import genfromtxt

import csv
import time

accuracy_list = []

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

def get_FP_TP_rates(results, verificationResults, column, scoreLabel):
    truePositives = 0
    trueNegatives = 0
    falsePositives = 0
    falseNegatives = 0

    accuracy = 0

    for idx,row in enumerate(results):
        if(row[column] == "True" and verificationResults[idx][column] == "True"):
            #consider true
            truePositives += 1
        
        if(row[column] == "True" and verificationResults[idx][column] == "False"):
            falsePositives += 1

        if(row[column] == "False" and verificationResults[idx][column] == "True"):
            falseNegatives += 1

        if(row[column] == "False" and verificationResults[idx][column] == "False"):
            trueNegatives += 1

    

    accuracy = ((truePositives + trueNegatives) / (truePositives + trueNegatives + falsePositives + falseNegatives)) * 100
    print(accuracy)

    accuracy_list.append(accuracy)
        

def plot_accuracy_curve(filename, results, verificationResults, column, scorelabel):

    get_FP_TP_rates(results, verificationResults, column, scorelabel)

def plot_class_data(inputFilename, verificationFilename):
    columns = {"Large_Class": 6} #large class
    filename = inputFilename
   
    verificationResults = readcsv(verificationFilename)  
    results = readcsv(filename)

    for scoreLabel, col in columns.items():
        plot_accuracy_curve(filename, results, verificationResults, col, scoreLabel)

def plot_method_featureEnvy_data(inputfilename, verificationFilename):
    columns = {"Feature_Envy":11} #large class
    filename = inputfilename
    verificationResults = readcsv(verificationFilename)      
    results = readcsv(filename)

    for scoreLabel, col in columns.items():
        plot_accuracy_curve(filename, results, verificationResults, col, scoreLabel)

def plot_method_longMethod_data(inputfilename, verificationFilename):
    columns = {"Long_Method": 13} #large class
    filename = inputfilename    
    verificationResults = readcsv(verificationFilename)      
    results = readcsv(filename)

    for scoreLabel, col in columns.items():
        plot_accuracy_curve(filename, results, verificationResults, col, scoreLabel)           
    

def plot_verification_class_data():
    a = 0
    
    while(a < 5):
        filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\bin\\Release\\ClassSetOutput_{}.csv".format(a)
        verificationFilename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\bin\\Release\\ClassTestSet_{}.csv".format(a)
        plot_class_data(filename, verificationFilename)
        a += 1
    
    #plt.legend()

    x = [1,2,3,4,5]

    print("Large Class: ")
    print(accuracy_list)
    
    plt.plot(x, accuracy_list) #, marker='o')
    plot = plt.gca()
    plot.set_ylim(80, 100)
    plt.xticks(x)

    accuracy_list.clear()

    #plt.show()
    plt.savefig(outDir + "\\Class.png")
    plt.close()

def plot_verification_longmethod_data():
    a = 0
    while(a < 5):
        filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\bin\\Release\\MethodSetOutput_{}.csv".format(a)
        verificationFilename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\bin\\Release\\MethodTestSet_{}.csv".format(a)        
        plot_method_longMethod_data(filename, verificationFilename)
        a += 1
    
    #plt.legend()

    x = [1,2,3,4,5]

    print("Long Method: ")
    print(accuracy_list)
    plt.plot(x, accuracy_list) #, marker='o')
    plot = plt.gca()
    plot.set_ylim(80, 100)
    plt.xticks(x)

    accuracy_list.clear()

    #plt.show()
    plt.savefig(outDir + "\\LongMethod.png")
    plt.close()

def plot_verification_featureenvy_data():
    a = 0
    while(a < 5):
        filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\bin\\Release\\MethodSetOutput_{}.csv".format(a)
        verificationFilename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\bin\\Release\\MethodTestSet_{}.csv".format(a)                
        plot_method_featureEnvy_data(filename, verificationFilename)
        a += 1
    
    #plt.legend() 
    x = [1,2,3,4,5]

    print("Feature Envy: ")    
    print(accuracy_list)
    plt.plot(x, accuracy_list) #, marker='o')
    plot = plt.gca()
    plot.set_ylim(80, 100)
    plt.xticks(x)

    accuracy_list.clear()

    #plt.show()
    plt.savefig(outDir + "\\FeatureEnvy.png")
    plt.close()


outDir = "accuracyPlotsCrossValidation"
if(len(sys.argv) == 2):
    outDir = sys.argv[1]

os.makedirs(outDir, exist_ok=True)

try:
    plot_verification_class_data()
except:
    print("Unexpected error:", sys.exc_info()[0])

try:
    plot_verification_longmethod_data()
except:
    print("Unexpected error:", sys.exc_info()[0])

try:
    plot_verification_featureenvy_data()
except:
    print("Unexpected error:", sys.exc_info()[0])