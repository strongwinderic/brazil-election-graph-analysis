# brazil-election-graph-analysis

## Requirements

To run this project, you need:

- **.NET 7.0 SDK or later** - Download from [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
- **Operating System**: Windows, Linux, or macOS
- **Dependencies** (automatically restored via NuGet):
  - LiveChartsCore.SkiaSharpView (v2.0.0-beta.516)
  - Newtonsoft.Json (v13.0.1)

## Instructions to run the software and generate the graphs

To run the software and generate the graphs, follow these steps:

1. Ensure you have the required .zip files from https://dados.gov.br/dataset/resultados-2022-boletim-de-urna and copy them to the `BrazilElectionGraphAnalysis/Data/DumpFiles` directory.
2. Build the project using Visual Studio or the .NET CLI.
3. Run the `BrazilElectionGraphAnalysis` project.
4. Follow the on-screen instructions to generate the charts.

For more details, refer to the `BrazilElectionGraphAnalysis/Program.cs` file.

## How the charts were generated

The charts were generated using the `AnalyticsChartsBuilder` class in the `BrazilElectionGraphAnalysis` namespace. This class takes in voting data and generates various charts, including random charts, tendency charts, and stealing votes charts. The charts are saved as PNG files in the specified directory.

For more details, refer to the `BrazilElectionGraphAnalysis/AnalyticsChartsBuilder.cs` file.

## FAQ

### What is the purpose of this project?

The purpose of this project is to analyze the voting data from the Brazilian elections and generate various charts to visualize the data.

### What data is used in this project?

The project uses voting data from the 2022 Brazilian elections, which can be downloaded from https://dados.gov.br/dataset/resultados-2022-boletim-de-urna.

### How are the charts generated?

The charts are generated using the `AnalyticsChartsBuilder` class, which processes the voting data and creates various charts, including random charts, tendency charts, and stealing votes charts.

## About the Analysis and Democratic Concerns in Brazil

This project provides statistical analysis tools to examine voting patterns and data integrity in the 2022 Brazilian elections. The analysis aims to provide transparency and enable independent verification of electoral data in light of significant concerns about institutional oversight, judicial overreach, and democratic processes in Brazil.

### Context: Documented Concerns About Brazilian Democracy

The 2022 Brazilian elections occurred amid serious controversies regarding the role of judicial institutions:

**Censorship and Free Speech Concerns:**
- Supreme Federal Court (STF) Justice Alexandre de Moraes, serving as head of the Superior Electoral Court (TSE), ordered suspensions of social media platforms including Telegram and X (formerly Twitter) during the election period for alleged non-compliance with content removal orders ([Freedom House](https://freedomhouse.org/country/brazil/freedom-net/2022), [Columbia Global Freedom of Expression](https://globalfreedomofexpression.columbia.edu/cases/the-case-of-the-brazil-fake-news-inquiry-2/))
- Freedom House rated Brazil as "Partly Free" in their 2022 Freedom on the Net report, citing censorship concerns and judicial interventions that "sometimes crossed into censorship, targeting right-wing politicians and independent media"
- Brazilian legislators filed complaints with the Inter-American Commission on Human Rights, arguing that platform bans constituted prior restraint and violated freedom of speech protections under the American Convention on Human Rights ([ADF International](https://adfinternational.org/news/brazil-censorship-iachr))

**International Criticism:**
- The United States invoked the Magnitsky Act against Alexandre de Moraes and imposed sanctions, with concerns about "political witch hunt" practices ([NYC Bar Association](https://www.nycbar.org/reports/statement-expressing-concerns-over-actions-undermining-judicial-independence-in-brazil/))
- International observers expressed concerns about the concentration of power and judicial overreach, with debates about whether actions defended democracy or undermined it ([Americas Quarterly](https://www.americasquarterly.org/article/brazils-most-powerful-judge-is-in-the-spotlight-again/))
- Critics argue that aggressive judicial interventions, including detention of political opponents and blocking of social media accounts, have "stifled dissent and risked undermining democracy" even as they aimed to combat disinformation

**Election Integrity Questions:**
- The project allows independent researchers to analyze the official voting data and verify patterns
- Statistical analysis tools help examine whether voting patterns align with natural distributions or suggest anomalies
- Multiple perspectives on data ordering (random, by tendency, simulations) enable comprehensive examination of the electoral data

### What This Tool Provides

The `ChartTools` class in the `BrazilElectionGraphAnalysis` namespace provides methods to generate various charts that visualize voting patterns, including:
- **Tendency charts** that order voting data by different criteria to examine patterns
- **Random sampling charts** that show voting patterns with randomized ballot ordering to establish baseline expectations
- **Vote distribution simulations** that model hypothetical scenarios to understand data behavior

These tools allow researchers, journalists, and citizens to independently examine the electoral data from multiple perspectives and draw their own conclusions about voting pattern behaviors and data integrity.

**Note:** This analysis tool is provided for transparency and independent verification. It presents documented facts about institutional concerns while enabling users to conduct their own statistical analysis of the official electoral data.

## Chart Examples

### Tendency Chart

This chart shows the tendency of votes for Lula and Bolsonaro over time. The data is ordered by the number of votes for Bolsonaro in descending order.

![Tendency Chart](BrazilElectionGraphAnalysis/GeneratedCharts/TendencyChart/tendencyChart.png)

### Stealing Votes Chart

This chart simulates the effect of stealing votes from Lula and giving them to Bolsonaro. The data is ordered randomly, and a certain percentage of votes are stolen from Lula and given to Bolsonaro.

![Stealing Votes Chart](BrazilElectionGraphAnalysis/GeneratedCharts/StealingVotesChart/stealingVotesChart.png)

### Random Charts

These charts show random samples of voting data. Each chart is generated by randomly shuffling the voting data and plotting the votes for Lula and Bolsonaro over time.

![Random Chart 1](BrazilElectionGraphAnalysis/GeneratedCharts/RandomCharts/chart_00001.png)
![Random Chart 2](BrazilElectionGraphAnalysis/GeneratedCharts/RandomCharts/chart_00002.png)
![Random Chart 3](BrazilElectionGraphAnalysis/GeneratedCharts/RandomCharts/chart_00003.png)
![Random Chart 4](BrazilElectionGraphAnalysis/GeneratedCharts/RandomCharts/chart_00004.png)
![Random Chart 5](BrazilElectionGraphAnalysis/GeneratedCharts/RandomCharts/chart_00005.png)
