import numpy
import matplotlib
import statistics
from scipy import stats

matplotlib.use('agg')

import matplotlib.pyplot as plots

end_line = '\n'

def collateTimes(file_name, timing_list):
    with open(file_name) as timings:
        for time in timings:
            timing_list.append(float(time))

control = []
expert = []
reinforcement = []

file_to_list_map = [('TimeLogControl.txt', control),
                    ('TimeLogExpert.txt', expert),
                    ('TimeLogTrainedReinforcement.txt', reinforcement)]

median_times = dict()

for pair in file_to_list_map:
    collateTimes(*pair)
    median_times[pair[0]] = statistics.median(pair[1])

ranksum_expert = stats.ranksums(control,expert)
ranksum_reinforcement = stats.ranksums(control,reinforcement)

comparison = [control, expert, reinforcement]

figure_comparison = plots.figure(1,figsize=(5,6))

axes_comparison = figure_comparison.add_subplot(111)

boxplot_comparison = axes_comparison.boxplot(comparison)
axes_comparison.set_xticklabels(['Control','Rule-based','Reinforcement Learning'])
axes_comparison.set_ylabel('Time (s)')
figure_comparison.suptitle('Journey Times Comparison', fontsize=14, fontWeight='bold')

figure_comparison.savefig('comparison.png', bbox_inches='tight')

with open('stat_table.txt','w') as table:
    table.write('Medians:-' + end_line)
    for median in median_times:
        table.write(median + ' = ' + str(median_times[median]) + end_line)
    table.write(end_line)
    table.write('Rank-Sums:-' + end_line)
    table.write('Rule-based = ' + str(ranksum_expert) + end_line)
    table.write('Reinforcement = ' + str(ranksum_reinforcement) + end_line)
