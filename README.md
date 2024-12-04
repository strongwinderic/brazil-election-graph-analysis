# brazil-election-graph-analysis
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

## Why question the security of the Brazilian election system?

The security of the Brazilian election system is questioned due to potential vulnerabilities and irregularities in the voting process. The `ChartTools` class in the `BrazilElectionGraphAnalysis` namespace provides methods to generate charts that highlight these concerns.

For more details, refer to the `BrazilElectionGraphAnalysis/ChartTools.cs` file.
