# FormulaFlow

Stock Strategy experimentation platform.

# Architecture

This application utilizes a dual architecture. A standard architecture is used for the data that is not directly related to the Canvas, Card, or Paramters (Repository, Service, Mappers, Controller). The alternate architecture utilizes an intermediate stage in the data flow (Repository, Service, Mapper1, Mapper2, Controller). The Factory allows the user to instantiate the Intermediate model that implements the abstract class `IntermediateCard`. With the implemented methods, the program can easily perform back testing for different Card Networks.

<img src="https://github.com/matjmase/FormulaFlow/blob/main/Screenshots/IntermediateCardPic.jpg" width="600" />

<img src="https://github.com/matjmase/FormulaFlow/blob/main/Screenshots/SplitArchitecture.jpg" width="600" />

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

## Pick or Cretae your Strategy

Now with populated data, we can create our Strategies in the Canvas. Start by selecting or creating a new strategy.

<img src="https://github.com/matjmase/FormulaFlow/blob/main/Screenshots/StrategyList.jpg" width="600" />

## Build your Strategy

Now, you can use the interface to define a network of cards (numerical operators). The lime green button in the lower right hand corner is to download the output of that card at that stage of the network.

<img src="https://github.com/matjmase/FormulaFlow/blob/main/Screenshots/StrategyNetwork.jpg" width="600" />
