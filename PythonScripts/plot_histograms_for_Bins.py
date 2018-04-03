import sys
import os
import matplotlib.pyplot as plt
from matplotlib.widgets import Slider, Button, RadioButtons
import numpy as np

from scipy.stats import norm
from numpy import genfromtxt


def plot_bin_histogram(filename, metricname):
    num_data = genfromtxt(filename, delimiter=',', comments='(', skip_header=False, names=True)

    fig = plt.figure(figsize=(5,4))

    y = []
    x = []

    i = 0
    for n in num_data.dtype.names:
        x.append(i)
        y.append(num_data[n])
        i = i +1

    ax1 = fig.add_subplot(111)
    ax1.bar(x, y, 1, edgecolor = "black")
    ax1.set_xticks(np.arange(len(x)))
    ax1.margins(0,0)

    plt.title(metricname)
    os.makedirs("barCharts\\{}".format(outDir), exist_ok=True)
    plt.savefig("barCharts\\{}\\{}_{}.png".format(outDir, os.path.basename(filename)[:-4], metricname))
    plt.close(fig)


    #plt.show()


outDir = "NoUser"
if(len(sys.argv) == 2):
    outDir = sys.argv[1]


#os.makedirs(outDir, exist_ok=True)

plot_bin_histogram(r"..\bin\Release\TrainingsData\ClassTrainingSet_2319_17022018_bins_LOC_.csv", "LOC (Class)")
plot_bin_histogram(r"..\bin\Release\TrainingsData\ClassTrainingSet_2319_17022018_bins_ATFD_.csv", "ATFD (Class)")
plot_bin_histogram(r"..\bin\Release\TrainingsData\ClassTrainingSet_2319_17022018_bins_TCC_.csv", "TCC (Class)")
plot_bin_histogram(r"..\bin\Release\TrainingsData\ClassTrainingSet_2319_17022018_bins_WMC_.csv", "WMC (Class)")

plot_bin_histogram(r"..\bin\Release\TrainingsData\MethodTrainingSet_2319_17022018_bins_ATFD_.csv", "ATFD (Method)")
plot_bin_histogram(r"..\bin\Release\TrainingsData\MethodTrainingSet_2319_17022018_bins_CYCLO_.csv", "CYCLO (Method)")
plot_bin_histogram(r"..\bin\Release\TrainingsData\MethodTrainingSet_2319_17022018_bins_FDP_.csv", "FDP (Method)")
plot_bin_histogram(r"..\bin\Release\TrainingsData\MethodTrainingSet_2319_17022018_bins_LAA_.csv", "LAA (Method)")
plot_bin_histogram(r"..\bin\Release\TrainingsData\MethodTrainingSet_2319_17022018_bins_LOC_.csv", "LOC (Method)")
plot_bin_histogram(r"..\bin\Release\TrainingsData\MethodTrainingSet_2319_17022018_bins_MAXNESTING_.csv", "MAXNESTING (Method)")
plot_bin_histogram(r"..\bin\Release\TrainingsData\MethodTrainingSet_2319_17022018_bins_NOAV_.csv", "NOAV (Method)")


 

