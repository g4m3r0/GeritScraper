## Introduction
GeritScraper is a .NET 8 application designed to scrape and process data from the GERiT website. It is capable of extracting information about both parent and child institutes, organizing this data efficiently into a MongoDB database.

## Prerequisites
Before running GeritScraper, ensure you have the following:
- .NET 8 SDK installed on your system.
- Docker installed if you're using the MongoDB database through Docker.
- Access to the internet to scrape data from the GERiT website.

## MongoDB Database Setup
The application is configured to use a local MongoDB instance. If you are not using the default local MongoDB database setup provided in the Docker Compose script in the ProfJobs repo, you will need to adjust the MongoDB connection string accordingly. The default connection string is:

mongodb://admin:superSecurePassword!@localhost:27017

## How to Use

### Step 1: Compile the Application
First, open the solution or project and compile `GeritScraper.Console`.

### Step 2: Download the GERiT Database
Please navigate to [GERiT's service page](https://www.gerit.org/de/service), download the excel database (`institutionen_gerit.xlsx`), and copy it to the `Input` folder in the startup path of the application.

### Step 3: Run the Application
Run `GeritScraper.Console`. Upon execution, the application will perform the following actions:
- Scrape all parent institutes from GERiT and save the data to a JSON file.
- Extract information about child institutes from the JSON file and add them to the MongoDB database.

## License
This project is licensed under the [LICENSE NAME] - see the `LICENSE.md` file for details.

## Acknowledgements
- GERiT for providing the data source.
- Contributors and maintainers of the GeritScraper project.