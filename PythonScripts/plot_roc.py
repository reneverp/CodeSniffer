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

tpr_list = []
fpr_list = []

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

def get_FP_TP_rates(results, verificationResults, cutoff, column, scoreLabel):
    truePositives = 0
    trueNegatives = 0
    falsePositives = 0
    falseNegatives = 0

    tpr = 0
    fpr = 0

    for idx,row in enumerate(results):
        if(row[scoreLabel] > cutoff and verificationResults[idx][column] == "True"):
            #consider true
            truePositives += 1
        
        if(row[scoreLabel] > cutoff and verificationResults[idx][column] == "False"):
            falsePositives += 1

        if(row[scoreLabel] <= cutoff and verificationResults[idx][column] == "True"):
            falseNegatives += 1

        if(row[scoreLabel] <= cutoff and verificationResults[idx][column] == "False"):
            trueNegatives += 1

    

    tpr = truePositives / (truePositives + falseNegatives)

    fpr = falsePositives / (falsePositives + trueNegatives)

    tpr_list.append(tpr)
    fpr_list.append(fpr)
        

def plot_roc_curve(filename, results, verificationResults, column, scorelabel):

    scores = results[scorelabel].tolist()
    scores.sort()

    tpr_list.append(1)
    fpr_list.append(1)

    scores = np.unique(scores)

    for score in scores:
        get_FP_TP_rates(results, verificationResults, score, column, scorelabel)

    tpr_list.append(0)
    fpr_list.append(0)

    auc = np.trapz(fpr_list, tpr_list) + 1

    plt.plot(fpr_list, tpr_list, label="{} AUC:{}".format(ntpath.basename(filename), auc)) #, marker='o')
    plt.ylim(0,1.1)
    plt.xlim(0,1.1)

    fpr_list.clear()
    tpr_list.clear()



def plot_class_data(inputFilename):
    columns = {"Large_ClassScore": 6} #large class
    filename = inputFilename
    verificationFilename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\bin\Release\ClassTest.csv" #"\\..\\CodeSniffer.BBN\\VerificationData\\ClassTrainingSet_1357_18022018_discretized.csv"

    verificationResults = readcsv(verificationFilename)  
    results = genfromtxt(filename, delimiter=',', comments='(', skip_header=False, names=True)

    for scoreLabel, col in columns.items():
        plot_roc_curve(filename, results, verificationResults, col, scoreLabel)

def plot_method_featureEnvy_data(inputfilename):
    columns = {"Feature_EnvyScore":11} #large class
    filename = inputfilename
    verificationFilename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\bin\Release\Methodtest.csv" #"\\..\\CodeSniffer.BBN\\VerificationData\\MethodTrainingSet_1357_18022018_discretized.csv"

    verificationResults = readcsv(verificationFilename)      
    results = genfromtxt(filename, delimiter=',', comments='(', skip_header=False, names=True)

    for scoreLabel, col in columns.items():
        plot_roc_curve(filename, results, verificationResults, col, scoreLabel)

def plot_method_longMethod_data(inputfilename):
    columns = {"Long_MethodScore": 13} #large class
    filename = inputfilename
    verificationFilename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\bin\Release\Methodtest.csv" #"\\..\\CodeSniffer.BBN\\VerificationData\\MethodTrainingSet_1357_18022018_discretized.csv"
    
    verificationResults = readcsv(verificationFilename)      
    results = genfromtxt(filename, delimiter=',', comments='(', skip_header=False, names=True)

    for scoreLabel, col in columns.items():
        plot_roc_curve(filename, results, verificationResults, col, scoreLabel)           
    

def plot_verification_class_data():
    a = 0
    
    while(a < 10):
        filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\bin\\Release\\Classrun_{}.csv".format(a)
        plot_class_data(filename)
        a += 1
    
    plt.legend()
    plt.savefig(outDir + "\\Class.png")
    plt.close()

def plot_verification_longmethod_data():
    a = 0
    while(a < 10):
        filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\bin\\Release\\Methodrun_{}.csv".format(a)
        plot_method_longMethod_data(filename)
        a += 1
    
    plt.legend()
    plt.savefig(outDir + "\\LongMethod.png")
    plt.close()

def plot_verification_featureenvy_data():
    a = 0
    while(a < 10):
        filename = os.path.dirname(os.path.realpath(__file__)) + "\\..\\bin\\Release\\Methodrun_{}.csv".format(a)
        plot_method_featureEnvy_data(filename)
        a += 1
    
    plt.legend() 
    plt.savefig(outDir + "\\FeatureEnvy.png")
    plt.close()


outDir = "rocPlots"
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