import matplotlib.pyplot as plt
import numpy as np

from scipy.stats import norm
from numpy import genfromtxt

my_data = genfromtxt("D:\git\\afstuderen\codesniffer\CodeSniffer\CodeSniffer.BBN\TrainingsData\\ClassTrainingSet_042_16022018.csv", delimiter=',', comments='(', skip_header=True)

plt.figure()

data = my_data[:,5].tolist()

data.sort()

print(data)

std = np.std(data) 
mean = np.mean(data) 

plt.plot(data, norm.pdf(data,mean,std), '-o')
plt.hist(data,normed=True)

plt.figure()
plt.boxplot(data)

plt.show() 

