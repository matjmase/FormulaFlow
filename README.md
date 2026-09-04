# FormulaFlow

Stock Strategy experimentation platform.

# Architecture

This application utilizes a dual architecture. A standard architecture is used for the data that is not directly related to the Canvas, Card, or Parameter (Repository, Service, Mappers, Controller). The alternate architecture utilizes an intermediate stage in the data flow (Repository, Service, Mapper1, Mapper2, Controller). The Factory allows the user to instantiate the Intermediate model that implements the abstract class `IntermediateCard`. With the implemented methods, the program can easily perform back testing for different Card Networks.

<img src="https://github.com/matjmase/FormulaFlow/blob/main/Screenshots/IntermediateCardPic.jpg" width="600" />

<img src="https://github.com/matjmase/FormulaFlow/blob/main/Screenshots/SplitArchitecture.jpg" width="300" />

# Data Injection

Currently the github project does not have a API integration, so we have to manually injest the data via Frontend.

## Create Stock Symbol

We start by creating the Stock symbol that we want to use and have populated data.

<img src="https://github.com/matjmase/FormulaFlow/blob/main/Screenshots/StockSymbolAddRemove.jpg" width="600" />

<img src="https://github.com/matjmase/FormulaFlow/blob/main/Screenshots/StockSymbolRead.jpg" width="600" />

## Upload the Data

After creating the stock symbol, we will upload the data in a formated manner (very close to how excel extrudes it).

<img src="https://github.com/matjmase/FormulaFlow/blob/main/Screenshots/StockDataUpload.jpg" width="600" />

You can see the data populate in the Read portion of the tab.

<img src="https://github.com/matjmase/FormulaFlow/blob/main/Screenshots/StockDataRead.jpg" width="600" />

# Strategy Manager

You can manage your strategies using the hyperlink in the top right hand corner.

## Pick or Create your Strategy

Now with populated data, we can create our Strategies in the Canvas. Start by selecting or creating a new strategy.

<img src="https://github.com/matjmase/FormulaFlow/blob/main/Screenshots/StrategyList.jpg" width="600" />

## Build your Strategy

Now, you can use the interface to define a network of cards (numerical operators). The lime green button in the lower right hand corner is to download the output of that card at that stage of the network.

<img src="https://github.com/matjmase/FormulaFlow/blob/main/Screenshots/StrategyNetwork.jpg" width="600" />

# Setup

The setup is rather easy as all you really need is a database connection (code first). It also saves the stock data in a .db file, but that should be automatic.

## Database Connection

Update the database connection in the following files to hit your SQL Server.

1. `FormulaFlow\FormulaFlow\FormulaFlow.Data\appsettings.json`
2. `FormulaFlow\FormulaFlow\FormulaFlow.Server\appsettings.json`

After that, the application should run. (Given you have the basic technologies install - .Net, Node.js, Angular CLI)

# TODO - Future Improvements

The application can fail in a multitude of ways during the backtesting portion.

## Custom Exceptions and Frontend notification

I need to make a series of different exceptions that all pertain to different fail instances of the backtesting process. I will then catch the different exceptions and relay the issue with the network fail or the data not lining up to the frontend.

## Clearly Define how the application buffers

I need to make an addition to the user interface that conveys how many data points the card will `look back` at. For example, the Aggregate card calls for the algorithm to `look back` at x - 1 data points (the 1 represents today). Additionally, it would be nice to make the network automatically sum the total data points that will be `looked back at` at that specific stage of the network. This portion of development is unrefined and somewhat nebulous right now, but I will work on it in the future.
