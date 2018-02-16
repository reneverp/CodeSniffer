import matplotlib.pyplot as plt

from numpy import genfromtxt

my_data = genfromtxt("c:\\temp\\test.csv", delimiter=',', comments='(', skip_header=True)

plt.figure()
plt.pie(my_data[:,1], )


plt.show()
